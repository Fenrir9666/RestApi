namespace RestApi.Datos
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using RestApi.Entidades;

    public interface IPlatoRepositorio
    {
        Task<List<Plato>> ObtenerTodosAsync();
        Task<Plato?> ObtenerPorIdAsync(int id);
        Task<List<Plato>> ObtenerPorCategoriaAsync(int categoriaId);
        Task<int> AgregarAsync(Plato p);
        Task<bool> ActualizarAsync(Plato p);
        Task<bool> EliminarAsync(int id);
    }
}
