using System.Text.Json;
using Application.Wrappers;

namespace WebApi.Middlewares;

/// <summary>
/// Use for handle error by sending correct HTTP Status Code corespond to Exception.
/// </summary>
public class ErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            var response = context.Response;
            response.ContentType = "application/json";
            var responseModel = new Response<string> { Succeeded = false, Message = ex?.Message };

            switch (ex)
            {
                case Application.Exceptions.ApiException e:
                    response.StatusCode = e.StatusCode ?? StatusCodes.Status400BadRequest;
                    break;
                case Application.Exceptions.ValidationException e:
                    response.StatusCode = StatusCodes.Status400BadRequest;
                    responseModel.Errors = e.Errors;
                    break;
                case KeyNotFoundException:
                    response.StatusCode = StatusCodes.Status404NotFound;
                    break;
                default:
                    response.StatusCode = StatusCodes.Status500InternalServerError;
                    break;
            }
            var jsonResult = JsonSerializer.Serialize(responseModel);
            await response.WriteAsync(jsonResult);
        }
    }
}