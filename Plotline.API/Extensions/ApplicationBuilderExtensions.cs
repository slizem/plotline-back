using Plotline.API.Middleware;

namespace Plotline.API.Extensions;

public static class ApplicationBuilderExtensions
{
    /// <summary>
    /// Подключение мидлвара.
    /// </summary>
    /// <param name="builder">Билдер.</param>
    /// <returns>Обновленный билдер.</returns>
    public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ExceptionMiddleware>();
    }
}