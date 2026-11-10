using RestApi.Entidades;

namespace RestApi.Datos
{
    public class ProductoRepositorio
    {
        // Nuestra "base de datos" simulada
        private static readonly List<Producto> _datos = new()
        {
            new Producto { Id = 1, Nombre = "Mouse", Precio = 12.50m, Stock = 20 },
            new Producto { Id = 2, Nombre = "Teclado", Precio = 25m, Stock = 2 },
            new Producto { Id = 3, Nombre = "Monitor", Precio = 180m, Stock = 5 }
        };

        // Método para obtener todos los productos
        public List<Producto> ObtenerTodos() => _datos;

        // Método para obtener un producto por su Id
        public Producto? ObtenerPorId(int id) => _datos.FirstOrDefault(p => p.Id == id);
    }
}