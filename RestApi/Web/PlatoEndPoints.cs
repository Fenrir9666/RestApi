namespace RestApi.Web
{
    using RestApi.Entidades;
    using RestApi.Negocio;

    public static class PlatoEndpoints
    {
        public static void MapPlatoEndpoints(this Microsoft.AspNetCore.Builder.WebApplication app)
        {
            var grupo = app.MapGroup("/platos");

            // Coincide con ListarDisponiblesAsync() de tu servicio
            grupo.MapGet("/", (PlatoServicio s) =>
                Respuestas.Manejar(async () => Results.Ok(await s.ListarDisponiblesAsync())));

            // Coincide con ObtenerPorCategoriaAsync()
            grupo.MapGet("/categoria/{categoriaId:int}", (int categoriaId, PlatoServicio s) =>
                Respuestas.Manejar(async () => Results.Ok(await s.ObtenerPorCategoriaAsync(categoriaId))));

            // Coincide con ObtenerPorIdAsync()
            grupo.MapGet("/{id:int}", (int id, PlatoServicio s) =>
                Respuestas.Manejar(async () => Results.Ok(await s.ObtenerPorIdAsync(id))));

            // Coincide con RegistrarPlatoAsync()
            grupo.MapPost("/", (Plato p, PlatoServicio s) =>
                Respuestas.Manejar(async () =>
                {
                    var nuevoId = await s.RegistrarPlatoAsync(p);

                    // Buscamos el plato recién creado para que traiga el JOIN con la categoría
                    var platoCreado = await s.ObtenerPorIdAsync(nuevoId);

                    return Results.Created($"/platos/{nuevoId}", platoCreado);
                }));

            // Coincide con ActualizarAsync()
            grupo.MapPut("/{id:int}", (int id, Plato p, PlatoServicio s) =>
                Respuestas.Manejar(async () =>
                {
                    p.Id = id;
                    await s.ActualizarAsync(p);
                    return Results.NoContent();
                }));

            // Coincide con EliminarPlatoAsync()
            grupo.MapDelete("/{id:int}", (int id, PlatoServicio s) =>
                Respuestas.Manejar(async () =>
                {
                    await s.EliminarPlatoAsync(id);
                    return Results.NoContent(); // 204
                }));
        }
    }
}