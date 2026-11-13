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
            grupo.MapGet("/", (CategoriaServicio servicio) =>
                Respuestas.Manejar(async () => Results.Ok(await servicio.ListarTodosAsync())));

            // GET /categorias/{id} → busca una categoría por su ID
            grupo.MapGet("/{id:int}", (int id, CategoriaServicio servicio) =>
                Respuestas.Manejar(async () => Results.Ok(await servicio.ObtenerPorIdAsync(id))));

            // POST /categorias → registra una nueva categoría
            grupo.MapPost("/", (Categoria categoria, CategoriaServicio servicio) =>
                Respuestas.Manejar(async () =>
                {
                    var nuevoId = await servicio.RegistrarCategoriaAsync(categoria);
                    categoria.Id = nuevoId;
                    return Results.Created($"/categorias/{nuevoId}", categoria);
                }));

            // PUT /categorias/{id} → modifica una categoría existente
            grupo.MapPut("/{id:int}", (int id, Categoria categoria, CategoriaServicio servicio) =>
                Respuestas.Manejar(async () =>
                {
                    categoria.Id = id;
                    await servicio.ActualizarAsync(categoria);
                    return Results.NoContent(); // 204
                }));

            // DELETE /categorias/{id} → elimina físicamente una categoría
            grupo.MapDelete("/{id:int}", (int id, CategoriaServicio servicio) =>
                Respuestas.Manejar(async () =>
                {
                    await servicio.EliminarCategoriaAsync(id);
                    return Results.NoContent(); // 204
                }));
        }
    }
}
