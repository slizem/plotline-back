namespace Plotline.Application.Extensions;

using Microsoft.Extensions.DependencyInjection;
using Plotline.Application.Services;
using Plotline.Core.Interfaces.Services;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Регистрация сервисов.
    /// </summary>
    /// <param name="services">Сервисы.</param>
    /// <returns>Обновленные сервисы.</returns>
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IClientInfoService, ClientInfoService>();

        return services;
    }
}