namespace RestApi.Datos
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using RestApi.Entidades;

    public interface ICategoriaRepositorio
    {
        Task<List<Categoria>> ObtenerTodosAsync();
        Task<Categoria?> ObtenerPorIdAsync(int id);
        Task<int> AgregarAsync(Categoria c);
        Task<bool> ActualizarAsync(Categoria c);
        Task<bool> EliminarAsync(int id);
        Task<int> ContarPlatosAsync(int id);
    }
}
