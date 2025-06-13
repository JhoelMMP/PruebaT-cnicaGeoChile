using System.Net;
using System.Text.Json;

namespace GeoChile.Application.Middleware;

public class ExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlerMiddleware> _logger;

    public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            // Llama al siguiente middleware en el pipeline
            await _next(context);
        }
        catch (Exception ex)
        {
            // Si ocurre una excepción, la capturamos aquí
            _logger.LogError(ex, "Ha ocurrido una excepción no controlada: {Message}", ex.Message);

            // Preparamos una respuesta de error 500
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            // Creamos un objeto de respuesta de error genérico para no exponer detalles
            var response = new
            {
                StatusCode = context.Response.StatusCode,
                Message = "Ocurrió un error interno en el servidor. Por favor, intente de nuevo más tarde."
            };

            // Escribimos la respuesta en el cuerpo de la petición
            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}