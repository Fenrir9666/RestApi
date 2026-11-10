namespace RestApi.Negocio
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using RestApi.Datos;
    using RestApi.Entidades;

    public class PlatoServicio
    {
        private readonly PlatoRepositorio repo;
        private readonly CategoriaServicio categorias; // Inyecta el servicio de categorías

        // Constructor que recibe ambos repositorios/servicios
        public PlatoServicio(PlatoRepositorio repo, CategoriaServicio categorias)
        {
            this.repo = repo;
            this.categorias = categorias;
        }

        // =========================================================================
        // LISTAR: Obtiene todos los platos disponibles
        // =========================================================================
        public Task<List<Plato>> ListarDisponiblesAsync() => repo.ObtenerTodosAsync();

        // =========================================================================
        // OBTENER POR ID: Busca un plato específico por su ID
        // =========================================================================
        public async Task<Plato> ObtenerPorIdAsync(int id)
        {
            var plato = await repo.ObtenerPorIdAsync(id);
            if (plato is null)
                throw new KeyNotFoundException($"No existe el plato {id}."); // 404
            return plato;
        }

        // =========================================================================
        // OBTENER POR CATEGORÍA: Valida la categoría y retorna sus platos
        // =========================================================================
        public async Task<List<Plato>> ObtenerPorCategoriaAsync(int categoriaId)
        {
            // Valida si la categoría existe (lanza KeyNotFoundException / 404 si no)
            await categorias.ObtenerPorIdAsync(categoriaId);

            return await repo.ObtenerPorCategoriaAsync(categoriaId);
        }

        // =========================================================================
        // REGISTRAR: Valida los datos y agrega un nuevo plato
        // =========================================================================
        public async Task<int> RegistrarPlatoAsync(Plato p)
        {
            await ValidarAsync(p);
            return await repo.AgregarAsync(p);
        }

        // =========================================================================
        // ACTUALIZAR: Modifica un plato existente
        // =========================================================================
        public async Task ActualizarAsync(Plato p)
        {
            await ValidarAsync(p);
            var actualizado = await repo.ActualizarAsync(p);
            if (!actualizado)
            {
                throw new KeyNotFoundException($"No existe el plato {p.Id}."); // 404
            }
        }

        // =========================================================================
        // ELIMINAR: Realiza la baja lógica del plato
        // =========================================================================
        public async Task<bool> EliminarPlatoAsync(int id)
        {
            var eliminado = await repo.EliminarAsync(id);
            if (!eliminado)
            {
                throw new KeyNotFoundException($"No existe el plato {id}."); // 404
            }
            return true;
        }

        // =========================================================================
        // VALIDAR: Método privado de validación centralizada (Reglas de negocio)
        // =========================================================================
        private async Task ValidarAsync(Plato p)
        {
            if (string.IsNullOrWhiteSpace(p.Nombre))
                throw new ArgumentException("El nombre del plato es obligatorio."); // 400

            if (p.Nombre.Length > 100)
                throw new ArgumentException("El nombre no puede superar los 100 caracteres."); // 400

            if (p.Precio <= 0)
                throw new ArgumentException("El precio debe ser mayor a 0."); // 400

            if (await categorias.ObtenerPorIdAsync(p.CategoriaId) is null)
                throw new ArgumentException($"La categoría {p.CategoriaId} no existe."); // 400

            p.Nombre = p.Nombre.Trim();
            p.Descripcion = (p.Descripcion ?? "").Trim();
        }
    }
}