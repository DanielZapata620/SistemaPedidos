using FluentValidation;
using System.Net;

namespace PedidoApi.Helpers;

public class ApiExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ApiExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ValidationException ex)
        {
            await WriteError(context, HttpStatusCode.BadRequest, string.Join(" ", ex.Errors.Select(error => error.ErrorMessage)));
        }
        catch (UnauthorizedAccessException ex)
        {
            await WriteError(context, HttpStatusCode.Unauthorized, ex.Message);
        }
        catch (KeyNotFoundException ex)
        {
            await WriteError(context, HttpStatusCode.NotFound, ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            await WriteError(context, HttpStatusCode.BadRequest, ex.Message);
        }
        catch
        {
            await WriteError(context, HttpStatusCode.InternalServerError, "Ocurrio un error inesperado. Intenta de nuevo.");
        }
    }

    private static async Task WriteError(HttpContext context, HttpStatusCode statusCode, string message)
    {
        context.Response.StatusCode = (int)statusCode;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsJsonAsync(new { message });
    }
}
