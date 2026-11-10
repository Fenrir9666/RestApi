using RestApi.Datos;
using RestApi.Entidades;

namespace RestApi.Negocio
{
    public class ProductoServicio
    {
        private readonly ProductoRepositorio _repo;

        // .NET entrega el repositorio por el constructor
        public ProductoServicio(ProductoRepositorio repo)
        {
            _repo = repo;
        }

        // Regla: al cliente solo se le muestran productos con stock
        public List<Producto> ListarDisponibles() =>
            _repo.ObtenerTodos().Where(p => p.Stock > 0).ToList();

        // Regla: el id debe ser mayor que cero
        public Producto? Buscar(int id)
        {
            if (id <= 0)
                throw new ArgumentException("El id debe ser mayor que cero");

            return _repo.ObtenerPorId(id);
        }
    }





}