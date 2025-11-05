using DotNet.Testcontainers.Containers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MultiDataBaseProvider.Domain;
using MultiDataBaseProvider.Infraestructure.Providers;
using Respawn;
using System.Diagnostics;

namespace MultiDataBaseProvider.IntegrationTests.Infrastructure.DbContainers;

public abstract class DbContainer
{
    protected abstract IContainer GetContainer();
    protected abstract string GetConnectionString();

    public abstract IDbAdapter GetDbAdapter();
    public abstract IServiceCollection AddDbContext(IServiceCollection services);

    public virtual string[] GetSchemasToInclude()
        => [];

    public virtual Task StartAsync(CancellationToken ct = default)
        => GetContainer().StartAsync(ct);

    public virtual async Task StopAsync(CancellationToken ct = default)
    {
        await GetContainer().StopAsync(ct);
        await GetContainer().DisposeAsync().AsTask();
    }

    protected IServiceCollection AddDbContext<TContextImplementation, TOptionsBuilder>(
        IServiceCollection services,
        Func<DbContextOptionsBuilder, Func<string, Action<TOptionsBuilder>, DbContextOptionsBuilder>> dbContextOptionsBuilderAction,
        Action<TOptionsBuilder> optionsBuilderAction)
        where TContextImplementation : MyDbContext
        => services
            .AddDbContext<MyDbContext, TContextImplementation>((serviceProvider, options) =>
                dbContextOptionsBuilderAction(options)(GetConnectionString(), optionsBuilderAction)
            );

    public static DbContainer Create(IConfiguration configuration)
    {
        var provider = configuration.GetValue(nameof(Provider), Provider.SqlServer);
        Debug.WriteLine($"[Database Provider] {provider}");
        Console.WriteLine($"[Database Provider] {provider}");
        DbContainer dbContainer = provider switch
        {
            Provider.SqlServer => new SqlServerDbContainer(),
            Provider.MySql => new MySqlDbContainer(),
            Provider.Postgres => new PostgresDbContainer(),
            Provider.Oracle => new OracleDbContainer(),
            _ => throw new InvalidOperationException($"Unsupported provider: {provider}")
        };

        return dbContainer;
    }
}
