namespace RestApi.Negocio
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.Data.SqlClient;
    using RestApi.Datos;
    using RestApi.Entidades;

    public class CategoriaServicio
    {
        private readonly CategoriaRepositorio _repo;

        public CategoriaServicio(CategoriaRepositorio repo)
        {
            _repo = repo;
        }

        // =========================================================================
        // LISTAR: Obtiene todas las categorías
        // =========================================================================
        public async Task<List<Categoria>> ListarTodosAsync()
        {
            // Corregido: Retornaba List<Plato>, ahora retorna List<Categoria>
            return await _repo.ObtenerTodosAsync();
        }

        // =========================================================================
        // OBTENER POR ID: Busca una categoría por su ID o lanza error si no existe
        // =========================================================================
        public async Task<Categoria> ObtenerPorIdAsync(int id)
        {
            // Corregido: Se cambió el nombre de la variable 'plato' a 'categoria'
            var categoria = await _repo.ObtenerPorIdAsync(id);

            if (categoria == null)
                throw new KeyNotFoundException($"No se encontró la categoría con el ID {id}.");

            return categoria;
        }

        // =========================================================================
        // REGISTRAR: Valida los datos básicos antes de insertar la categoría
        // =========================================================================
        public async Task<int> RegistrarCategoriaAsync(Categoria categoria)
        {
            if (string.IsNullOrWhiteSpace(categoria.Nombre))
                throw new ArgumentException("El nombre de la categoría no puede estar vacío.");

            if (categoria.Nombre.Length > 100)
                throw new ArgumentException("El nombre no puede superar los 100 caracteres.");

            categoria.Nombre = categoria.Nombre.Trim();

            return await _repo.AgregarAsync(categoria);
        }

        // =========================================================================
        // ACTUALIZAR: Modifica una categoría existente aplicando validaciones
        // =========================================================================
        public async Task ActualizarAsync(Categoria c)
        {
            // Corregido: Se cambió 'p.Id' por 'c.Id'
            if (c.Id <= 0)
                throw new ArgumentException("Debe indicar una categoría válida.");

            if (string.IsNullOrWhiteSpace(c.Nombre))
                throw new ArgumentException("El nombre de la categoría es obligatorio.");

            if (c.Nombre.Length > 100)
                throw new ArgumentException("El nombre no puede superar los 100 caracteres.");

            c.Nombre = c.Nombre.Trim();

            // Corregido: Se cambió '_repo.ActualizarAsync(p)' por '_repo.ActualizarAsync(c)'
            var actualizado = await _repo.ActualizarAsync(c);

            if (!actualizado)
            {
                throw new KeyNotFoundException($"No existe la categoría con el ID {c.Id}.");
            }
        }

        // =========================================================================
        // ELIMINAR: Realiza el borrado de la categoría
        // =========================================================================
        public async Task<bool> EliminarCategoriaAsync(int id)
        {
            var eliminado = await _repo.EliminarAsync(id);

            if (!eliminado)
                throw new KeyNotFoundException(
                    $"No se pudo eliminar, la categoría con ID {id} no existe."
                );

            return true;
        }
    }
}
