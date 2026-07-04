using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductMiddleware.Application.Interfaces;
using ProductMiddleware.Infrastructure.DummyJson;
using ProductMiddleware.Infrastructure.Options;

namespace ProductMiddleware.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<DummyJsonOptions>(configuration.GetSection(DummyJsonOptions.SectionName));

        services.AddHttpClient<DummyJsonProductClient>();
        services.AddHttpClient<DummyJsonAuthClient>();

        services.AddScoped<IAuthClient, DummyJsonAuthClient>();
        services.AddScoped<DummyJsonProductSource>();
        services.AddScoped<IProductSource>(sp => sp.GetRequiredService<DummyJsonProductSource>());

        return services;
    }
}
