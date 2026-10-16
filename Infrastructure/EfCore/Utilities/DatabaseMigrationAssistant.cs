using Microsoft.EntityFrameworkCore;

namespace TaskAggregator.Infrastructure.EfCore.Utilities;

public static class DatabaseMigrationAssistant
{
    public static async Task InitDatabaseAsync(DbContext dbContext, CancellationToken cancellationToken)
    {
        await dbContext.Database.MigrateAsync(cancellationToken: cancellationToken);
    }
}
