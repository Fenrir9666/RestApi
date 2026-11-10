namespace RestApi.Entidades
{
    public class Plato
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; }
        public decimal Precio { get; set; }
        public int CategoriaId { get; set; }

        // solo cambio la categoriaIF pero cambio Categoria, 
        // por ejemplo sopa 
        public string Categoria { get; set; } = "";
        public bool Disponible { get; set; } = true;
    }
}