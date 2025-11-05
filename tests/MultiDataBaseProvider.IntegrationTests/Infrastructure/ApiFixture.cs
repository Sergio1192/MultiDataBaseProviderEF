using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MultiDataBaseProvider.Domain;
using MultiDataBaseProvider.Extensions;
using MultiDataBaseProvider.Infraestructure.Providers;
using MultiDataBaseProvider.IntegrationTests.Infrastructure.DbContainers;
using Respawn;
using Respawn.Graph;

namespace MultiDataBaseProvider.IntegrationTests.Infrastructure;

public class ApiFixture : WebApplicationFactory<Program>, IAsyncLifetime
{
    private DbContainer DbContainer => Services.GetRequiredService<DbContainer>();
    public Respawner Respawner { get; private set; } = default!;

    private readonly Table[] TablesToIgnore = [
        "__EFMigrationsHistory"
    ];

    protected override IWebHostBuilder CreateWebHostBuilder()
        => WebHost
            .CreateDefaultBuilder()
            .ConfigureAppConfiguration((context, config) =>
            {
                var provider = Environment.GetEnvironmentVariable(nameof(Provider));
                if (provider is null)
                {
                    config.AddInMemoryCollection(new Dictionary<string, string?>
                    {
                        [nameof(Provider)] = TestConstants.DEFAULT_PROVIDER
                    });
                }
            })
            .UseEnvironment("Testing")
            .UseTestServer()
            .Configure(app => app
                .UseRouting()
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                })
            )
            .ConfigureServices((context, services) =>
            {
                var dbContainerAux = DbContainer.Create(context.Configuration);
                dbContainerAux.AddDbContext(services);

                services
                    .AddSingleton(dbContainerAux)
                    .AddControllers()
                    .AddApplicationPart(typeof(Program).Assembly);
            });

    public async Task InitializeAsync()
    {
        await DbContainer.StartAsync();
        await Services.MigrateDbContextAsync();

        using var scope = Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
        using var context = scope.ServiceProvider.GetRequiredService<MyDbContext>();

        await context.Database.OpenConnectionAsync();
        using var connection = context.Database.GetDbConnection();

        Respawner = await Respawner.CreateAsync(connection, new RespawnerOptions
        {
            TablesToIgnore = TablesToIgnore,
            SchemasToInclude = DbContainer.GetSchemasToInclude(),
            DbAdapter = DbContainer.GetDbAdapter()
        });

        await context.Database.CloseConnectionAsync();
    }

    Task IAsyncLifetime.DisposeAsync()
        => DbContainer.StopAsync();
}
