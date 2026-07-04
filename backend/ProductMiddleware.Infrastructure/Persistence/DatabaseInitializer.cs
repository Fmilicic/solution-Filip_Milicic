using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace ProductMiddleware.Infrastructure.Persistence;

public static class DatabaseInitializer
{
    public static void ApplyMigrations(ProductDbContext db, bool isTestingEnvironment)
    {
        if (isTestingEnvironment)
        {
            EnsureTestingSchema(db);
            return;
        }

        db.Database.Migrate();
    }

    private static void EnsureTestingSchema(ProductDbContext db)
    {
        var creator = db.Database.GetService<IRelationalDatabaseCreator>();

        if (!creator.Exists())
        {
            creator.Create();
            return;
        }

        if (!creator.HasTables())
        {
            creator.CreateTables();
        }
    }
}
