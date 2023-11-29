using WebApi.Middlewares;

namespace WebApi.Extensions;

public static class AppExtensions
{
    /// <summary>
    /// Handling Response Errors.
    /// </summary>
    /// <param name="app">Application Builder</param>
    public static void UseErrorHandleMiddleware(this IApplicationBuilder app)
    {
        app.UseMiddleware<ErrorHandlerMiddleware>();
    }
}