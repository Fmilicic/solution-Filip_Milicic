using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ProductMiddleware.Infrastructure.Persistence;

namespace ProductMiddleware.Tests.Integration;

public class IntegrationWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly SqliteConnection _connection = new("DataSource=:memory:");

    public IntegrationWebApplicationFactory()
    {
        _connection.Open();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureServices(services =>
        {
            services.RemoveAll<DbContextOptions<ProductDbContext>>();
            services.RemoveAll<ProductDbContext>();

            services.AddSingleton(_connection);
            services.AddDbContext<ProductDbContext>((serviceProvider, options) =>
            {
                var connection = serviceProvider.GetRequiredService<SqliteConnection>();
                options.UseSqlite(connection);
            });
        });
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _connection.Dispose();
        }

        base.Dispose(disposing);
    }
}

[CollectionDefinition("Integration")]
public sealed class IntegrationCollection : ICollectionFixture<IntegrationWebApplicationFactory>;
