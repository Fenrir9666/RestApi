using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace RestApi.Web
{
    public static class Respuestas
    {
        public static async Task<IResult> Manejar(Func<Task<IResult>> accion)
        {
            try
            {
                return await accion();
            }
            catch (ArgumentException ex) // Datos inválidos
            {
                return Results.BadRequest(new { error = ex.Message }); // 400
            }
            catch (KeyNotFoundException ex) // No existe
            {
                return Results.NotFound(new { error = ex.Message }); // 404   
            }
            catch (InvalidOperationException ex) // Choca con una regla de negocio
            {
                return Results.Conflict(new { error = ex.Message }); // 409
            }
            catch (Exception ex) // Error genérico no controlado
            {
                return Results.InternalServerError(new { error = "Ocurrió un error inesperado.", detalle = ex.Message }); // 500
            }
        }
    }
}
