namespace RestApi.Datos
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Data.SqlClient;
    using Microsoft.Extensions.Configuration;
    using RestApi.Entidades;

    public class PlatoRepositorio(IConfiguration config)
    {
        private readonly string _cn = config.GetConnectionString("Restaurante")
            ?? throw new InvalidOperationException("La cadena de conexión 'Restaurante' no está configurada.");

        // SQL base con JOIN corregido apuntando a la tabla 'Categorias' y usando un alias claro
        private const string SelectConCategoria = @"
            SELECT p.Id, p.Nombre, p.Descripcion, p.Precio, p.CategoriaId, c.Nombre AS NombreCategoria 
            FROM Platos p
            INNER JOIN Categorias c ON c.Id = p.CategoriaId";

        // Método auxiliar privado corregido para leer por nombre de columna/alias de forma segura
        private async Task<List<Plato>> LeerAsync(string sql, params (string nombre, object valor)[] parametros)
        {
            var lista = new List<Plato>();

            await using var cn = new SqlConnection(_cn);
            await cn.OpenAsync();

            await using var cmd = new SqlCommand(sql, cn);
            foreach (var (nombre, valor) in parametros)
            {
                cmd.Parameters.AddWithValue(nombre, valor ?? DBNull.Value);
            }

            await using var dr = await cmd.ExecuteReaderAsync();
            while (await dr.ReadAsync())
            {
                lista.Add(new Plato
                {
                    Id = dr.GetInt32(dr.GetOrdinal("Id")),
                    Nombre = dr.GetString(dr.GetOrdinal("Nombre")),
                    Descripcion = dr.IsDBNull(dr.GetOrdinal("Descripcion")) ? string.Empty : dr.GetString(dr.GetOrdinal("Descripcion")),
                    Precio = dr.GetDecimal(dr.GetOrdinal("Precio")),
                    CategoriaId = dr.GetInt32(dr.GetOrdinal("CategoriaId")),
                    Categoria = dr.IsDBNull(dr.GetOrdinal("NombreCategoria")) ? string.Empty : dr.GetString(dr.GetOrdinal("NombreCategoria"))
                });
            }
            return lista;
        }

        // Obtiene todos los platos disponibles ordenados por categoría y nombre[cite: 4]
        public Task<List<Plato>> ObtenerTodosAsync() =>
            LeerAsync(SelectConCategoria + " WHERE p.Disponible = 1 ORDER BY c.Nombre, p.Nombre");

        // Obtiene un plato específico por su ID[cite: 4]
        public async Task<Plato?> ObtenerPorIdAsync(int id) =>
            (await LeerAsync(SelectConCategoria + " WHERE p.Id = @id", ("@id", id))).FirstOrDefault();

        // Obtiene platos filtrados por categoría[cite: 4]
        public Task<List<Plato>> ObtenerPorCategoriaAsync(int categoriaId) =>
            LeerAsync(SelectConCategoria + " WHERE p.CategoriaId = @cat AND p.Disponible = 1 ORDER BY p.Nombre", ("@cat", categoriaId));

        // AGREGAR: Inserta un nuevo plato
        public async Task<int> AgregarAsync(Plato p)
        {
            await using var cn = new SqlConnection(_cn);
            await cn.OpenAsync();

            var cmd = new SqlCommand(
                "INSERT INTO Platos (Nombre, Descripcion, Precio, CategoriaId) OUTPUT INSERTED.Id VALUES (@nombre, @descripcion, @precio, @cat)", cn);

            cmd.Parameters.AddWithValue("@nombre", p.Nombre);
            cmd.Parameters.AddWithValue("@descripcion", (object?)p.Descripcion ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@precio", p.Precio);
            cmd.Parameters.AddWithValue("@cat", p.CategoriaId);

            var result = await cmd.ExecuteScalarAsync();
            return result != null ? (int)result : 0;
        }

        // ACTUALIZAR: Modifica un plato existente
        public async Task<bool> ActualizarAsync(Plato p)
        {
            await using var cn = new SqlConnection(_cn);
            await cn.OpenAsync();

            var cmd = new SqlCommand(
                @"UPDATE Platos
                  SET Nombre = @nombre,
                      Descripcion = @descripcion,
                      Precio = @precio,
                      CategoriaId = @cat,
                      Disponible = @disponible
                  WHERE Id = @id", cn);

            cmd.Parameters.AddWithValue("@id", p.Id);
            cmd.Parameters.AddWithValue("@nombre", p.Nombre);
            cmd.Parameters.AddWithValue("@descripcion", (object?)p.Descripcion ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@precio", p.Precio);
            cmd.Parameters.AddWithValue("@cat", p.CategoriaId);
            cmd.Parameters.AddWithValue("@disponible", p.Disponible);

            int filasAfectadas = await cmd.ExecuteNonQueryAsync();
            return filasAfectadas > 0;
        }

        // ELIMINAR: Baja lógica cambiando Disponible a 0
        public async Task<bool> EliminarAsync(int id)
        {
            await using var cn = new SqlConnection(_cn);
            await cn.OpenAsync();

            var cmd = new SqlCommand("UPDATE Platos SET Disponible = 0 WHERE Id = @id", cn);
            cmd.Parameters.AddWithValue("@id", id);

            int filasAfectadas = await cmd.ExecuteNonQueryAsync();
            return filasAfectadas > 0;
        }
    }
}