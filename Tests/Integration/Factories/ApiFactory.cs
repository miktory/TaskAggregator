using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Testcontainers.PostgreSql;
using Xunit;

namespace TaskAggregator.Tests.Integration.Factories;

public sealed class ApiFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _db = new PostgreSqlBuilder("postgres:17-alpine")
        .Build();

    public async ValueTask InitializeAsync()
    {
        await _db.StartAsync();
        _ = Server;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        //builder.UseEnvironment("Testing");
        builder.ConfigureAppConfiguration((_, cfg) =>
            cfg.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["DatabaseConnection:ConnectionString"] = _db.GetConnectionString(),
            }));
    }

    public override async ValueTask DisposeAsync()
    {
        await base.DisposeAsync();
        await _db.DisposeAsync();
    }
}
