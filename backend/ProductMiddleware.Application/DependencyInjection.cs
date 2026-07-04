using Microsoft.Extensions.DependencyInjection;
using ProductMiddleware.Application.Interfaces;
using ProductMiddleware.Application.Services;

namespace ProductMiddleware.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ProductQueryService>();
        services.AddScoped<IProductQueryService>(sp => sp.GetRequiredService<ProductQueryService>());
        services.AddScoped<AuthService>();
        services.AddScoped<IAuthService>(sp => sp.GetRequiredService<AuthService>());

        return services;
    }
}
