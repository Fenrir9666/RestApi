namespace RestApi.Web
{
    using RestApi.Entidades;
    using RestApi.Negocio;

    public static class CategoriaEndpoints
    {
        public static void MapCategoriaEndpoints(this Microsoft.AspNetCore.Builder.WebApplication app)
        {
            var grupo = app.MapGroup("/categorias");

            // GET /categorias → lista de categorías
            grupo.MapGet("/", async (CategoriaServicio servicio) =>
            {
                var categorias = await servicio.ListarTodosAsync();
                return Results.Ok(categorias);
            });

            // GET /categorias/{id} → busca una categoría por su ID
            grupo.MapGet("/{id:int}", async (int id, CategoriaServicio servicio) =>
            {
                try
                {
                    var categoria = await servicio.ObtenerPorIdAsync(id);
                    return Results.Ok(categoria);
                }
                catch (KeyNotFoundException ex)
                {
                    return Results.NotFound(ex.Message);
                }
                catch (Exception)
                {
                    return Results.Problem("Ocurrió un error interno en el servidor.", statusCode: 500);
                }
            });

            // POST /categorias → registra una nueva categoría
            grupo.MapPost("/", async (Categoria categoria, CategoriaServicio servicio) =>
            {
                try
                {
                    var nuevoId = await servicio.RegistrarCategoriaAsync(categoria);
                    categoria.Id = nuevoId;
                    return Results.Created($"/categorias/{nuevoId}", categoria);
                }
                catch (ArgumentException ex)
                {
                    return Results.BadRequest(ex.Message);
                }
                catch (Exception)
                {
                    return Results.Problem("Ocurrió un error interno en el servidor.", statusCode: 500);
                }
            });

            // PUT /categorias/{id} → modifica una categoría existente
            grupo.MapPut("/{id:int}", async (int id, Categoria categoria, CategoriaServicio servicio) =>
            {
                categoria.Id = id;
                try
                {
                    await servicio.ActualizarAsync(categoria);
                    return Results.NoContent();
                }
                catch (ArgumentException ex)
                {
                    return Results.BadRequest(ex.Message);
                }
                catch (KeyNotFoundException ex)
                {
                    return Results.NotFound(ex.Message);
                }
                catch (Exception)
                {
                    return Results.Problem("Ocurrió un error interno en el servidor.", statusCode: 500);
                }
            });

            // DELETE /categorias/{id} → elimina físicamente una categoría
            grupo.MapDelete("/{id:int}", async (int id, CategoriaServicio servicio) =>
            {
                try
                {
                    await servicio.EliminarCategoriaAsync(id);
                    return Results.NoContent();
                }
                catch (KeyNotFoundException ex)
                {
                    return Results.NotFound(ex.Message);
                }
                catch (Exception)
                {
                    return Results.Problem("Ocurrió un error interno en el servidor.", statusCode: 500);
                }
            });
        }
    }
}