namespace RestApi.Datos
{
    using System;
    using System.Collections.Generic;          // Permite usar listas (List<T>) para agrupar varios elementos.
    using System.Threading.Tasks;              // Permite usar programación asíncrona (async, await, Task).
    using Microsoft.Data.SqlClient;            // Herramientas oficiales para conectarse y ejecutar comandos en SQL Server.
    using Microsoft.Extensions.Configuration;   // Permite leer archivos de configuración como el 'appsettings.json'.
    using RestApi.Entidades;                   // Conecta con tus modelos propios (como la clase Categoria).

    public class CategoriaRepositorio(IConfiguration config)
    {
        // Obtenemos de forma segura la cadena de conexión a la base de datos desde el appsettings.json
        private readonly string _cn = config.GetConnectionString("Restaurante")
            ?? throw new InvalidOperationException("La cadena de conexión 'Restaurante' no está configurada.");

        // =========================================================================
        // LISTAR: Obtiene todas las categorías ordenadas alfabéticamente
        // =========================================================================
        public async Task<List<Categoria>> ObtenerTodosAsync()
        {
            var lista = new List<Categoria>();

            await using var cn = new SqlConnection(_cn);
            await cn.OpenAsync();

            // CORREGIDO: Apunta a la tabla 'Categorias' en plural
            var cmd = new SqlCommand("SELECT Id, Nombre FROM Categorias ORDER BY Nombre", cn);

            await using var dr = await cmd.ExecuteReaderAsync();
            while (await dr.ReadAsync())
            {
                lista.Add(new Categoria
                {
                    Id = dr.GetInt32(dr.GetOrdinal("Id")),
                    Nombre = dr.GetString(dr.GetOrdinal("Nombre"))
                });
            }
            return lista;
        }

        // =========================================================================
        // OBTENER POR ID: Busca una categoría específica por su número de ID
        // =========================================================================
        public async Task<Categoria?> ObtenerPorIdAsync(int id)
        {
            await using var cn = new SqlConnection(_cn);
            await cn.OpenAsync();

            // CORREGIDO: Apunta a la tabla 'Categorias' en plural
            var cmd = new SqlCommand("SELECT Id, Nombre FROM Categorias WHERE Id = @id", cn);

            cmd.Parameters.AddWithValue("@id", id);

            await using var dr = await cmd.ExecuteReaderAsync();
            if (await dr.ReadAsync())
            {
                return new Categoria
                {
                    Id = dr.GetInt32(dr.GetOrdinal("Id")),
                    Nombre = dr.GetString(dr.GetOrdinal("Nombre"))
                };
            }
            return null;
        }

        // =========================================================================
        // AGREGAR: Inserta una nueva categoría en la base de datos
        // =========================================================================
        public async Task<int> AgregarAsync(Categoria c)
        {
            await using var cn = new SqlConnection(_cn);
            await cn.OpenAsync();

            // CORREGIDO: Apunta a la tabla 'Categorias' en plural
            var cmd = new SqlCommand(
                "INSERT INTO Categorias (Nombre) OUTPUT INSERTED.Id VALUES (@nombre)", cn);

            cmd.Parameters.AddWithValue("@nombre", c.Nombre);

            var result = await cmd.ExecuteScalarAsync();
            return result != null ? (int)result : 0;
        }

        // =========================================================================
        // ACTUALIZAR: Modifica los datos de una categoría existente
        // =========================================================================
        public async Task<bool> ActualizarAsync(Categoria c)
        {
            await using var cn = new SqlConnection(_cn);
            await cn.OpenAsync();

            // CORREGIDO: Apunta a la tabla 'Categorias' en plural
            var cmd = new SqlCommand(
                @"UPDATE Categorias
                  SET Nombre = @nombre
                  WHERE Id = @id", cn);

            cmd.Parameters.AddWithValue("@id", c.Id);
            cmd.Parameters.AddWithValue("@nombre", c.Nombre);

            int filasAfectadas = await cmd.ExecuteNonQueryAsync();
            return filasAfectadas > 0;
        }

        // =========================================================================
        // ELIMINAR (Físico): Elimina el registro por completo de la base de datos
        // =========================================================================
        public async Task<bool> EliminarAsync(int id)
        {
            await using var cn = new SqlConnection(_cn);
            await cn.OpenAsync();

            // CORREGIDO: Apunta a la tabla 'Categorias' en plural
            var cmd = new SqlCommand("DELETE FROM Categorias WHERE Id = @id", cn);
            cmd.Parameters.AddWithValue("@id", id);

            int filasAfectadas = await cmd.ExecuteNonQueryAsync();
            return filasAfectadas > 0;
        }

        // =========================================================================
        // CONTAR PLATOS: Cuenta cuántos platos usan esta categoría (Para validaciones)
        // =========================================================================
        public async Task<int> ContarPlatosAsync(int id)
        {
            await using var cn = new SqlConnection(_cn);
            await cn.OpenAsync();

            var cmd = new SqlCommand(
                "SELECT COUNT(*) FROM Platos WHERE CategoriaId = @id", cn);

            cmd.Parameters.AddWithValue("@id", id);

            var result = await cmd.ExecuteScalarAsync();
            return result != null ? Convert.ToInt32(result) : 0;
        }
    }
}