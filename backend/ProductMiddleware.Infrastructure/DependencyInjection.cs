using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using ProductMiddleware.Application.Interfaces;
using ProductMiddleware.Infrastructure.Auth;
using ProductMiddleware.Infrastructure.DummyJson;
using ProductMiddleware.Infrastructure.Options;
using ProductMiddleware.Infrastructure.Persistence;
using ProductMiddleware.Infrastructure.Sources;

namespace ProductMiddleware.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration,
        string contentRootPath)
    {
        services.Configure<DummyJsonOptions>(configuration.GetSection(DummyJsonOptions.SectionName));
        services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.SectionName));
        services.Configure<CorsOptions>(configuration.GetSection(CorsOptions.SectionName));

        var connectionString = configuration.GetConnectionString("ProductsDb") ?? "Data Source=products.db";
        connectionString = ResolveSqliteConnectionString(connectionString, contentRootPath);

        services.AddDbContext<ProductDbContext>(options => options.UseSqlite(connectionString));

        services.AddHttpClient<DummyJsonProductClient>();
        services.AddHttpClient<DummyJsonAuthClient>();
        services.AddHttpClient<DummyJsonCategoryClient>();

        services.AddScoped<IAuthClient, DummyJsonAuthClient>();
        services.AddScoped<ICategoryClient, DummyJsonCategoryClient>();
        services.AddScoped<ITokenService, JwtTokenService>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<DatabaseProductSource>();
        services.AddScoped<DummyJsonProductSource>();
        services.AddScoped<IProductSource>(provider => new AggregatingProductSource(new IProductSource[]
        {
            provider.GetRequiredService<DatabaseProductSource>(),
            provider.GetRequiredService<DummyJsonProductSource>()
        }));

        return services;
    }

    private static string ResolveSqliteConnectionString(string connectionString, string contentRootPath)
    {
        const string dataSourcePrefix = "Data Source=";
        if (!connectionString.StartsWith(dataSourcePrefix, StringComparison.OrdinalIgnoreCase))
        {
            return connectionString;
        }

        var databasePath = connectionString[dataSourcePrefix.Length..];
        if (Path.IsPathRooted(databasePath))
        {
            return connectionString;
        }

        return $"{dataSourcePrefix}{Path.Combine(contentRootPath, databasePath)}";
    }
}
