using RestApi.Datos;
using RestApi.Negocio;
using RestApi.Web;

var builder = WebApplication.CreateBuilder(args);

// =========================================================================
// 1. REGISTRO DE SERVICIOS (INYECCIÓN DE DEPENDENCIAS)
// =========================================================================

// Servicios de PRODUCTOS
builder.Services.AddScoped<ProductoRepositorio>();
builder.Services.AddScoped<ProductoServicio>();

// Servicios de PLATOS
builder.Services.AddScoped<IPlatoRepositorio, PlatoRepositorio>();
builder.Services.AddScoped<PlatoServicio>();

// Servicios de CATEGORÍAS
builder.Services.AddScoped<ICategoriaRepositorio, CategoriaRepositorio>();
builder.Services.AddScoped<CategoriaServicio>();

// CREACIÓN DE LA APLICACIÓN
var app = builder.Build();

// =========================================================================
// 2. MAPEO DE ENDPOINTS
// =========================================================================

// Mapeamos los endpoints de platos organizados en su propia clase
app.MapPlatoEndpoints();

// Mapeamos los endpoints de categorías organizados en su propia clase
app.MapCategoriaEndpoints();

// (Opcional si mantienes productos de prueba sueltos, o puedes moverlos también)
app.MapGet("/productos", (ProductoServicio servicio) =>
{
    var productosDisponibles = servicio.ListarDisponibles();
    return Results.Ok(productosDisponibles);
});

app.Run();