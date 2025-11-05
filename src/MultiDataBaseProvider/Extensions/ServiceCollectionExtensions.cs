using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using MultiDataBaseProvider.Domain;
using MultiDataBaseProvider.Infraestructure.Providers;
using MultiDataBaseProvider.Infraestructure.Providers.Contexts;
using MySql.EntityFrameworkCore.Infrastructure;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;
using Oracle.EntityFrameworkCore.Infrastructure;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        => services
            .AddDbContext(configuration);

    private static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        var provider = configuration.GetValue(nameof(Provider), Provider.SqlServer);

        _ = provider switch
        {
            Provider.SqlServer => services.AddDbContext<SqlServerDbContext, SqlServerDbContextOptionsBuilder>(
                configuration,
                options => options.UseSqlServer,
                builder => builder
                    .MigrationsAssembly(typeof(SqlServerDbContext).Assembly.FullName)
                    .UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)
                    .EnableRetryOnFailure()
            ),
            Provider.MySql => services.AddDbContext<MySqlDbContext, MySQLDbContextOptionsBuilder>(
                configuration,
                options => options.UseMySQL,
                builder => builder
                    .MigrationsAssembly(typeof(MySqlDbContext).Assembly.FullName)
                    .UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)
                    .EnableRetryOnFailure()
            ),
            Provider.Postgres => services.AddDbContext<PostgresDbContext, NpgsqlDbContextOptionsBuilder>(
                configuration,
                options => options.UseNpgsql,
                builder => builder
                    .MigrationsAssembly(typeof(MySqlDbContext).Assembly.FullName)
                    .UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)
                    .EnableRetryOnFailure()
            ),
            Provider.Oracle => services.AddDbContext<OracleDbContext, OracleDbContextOptionsBuilder>(
                configuration,
                options => options.UseOracle,
                builder => builder
                    .MigrationsAssembly(typeof(OracleDbContext).Assembly.FullName)
                    .UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)
            // EnableRetryOnFailure not available in Oracle 
            ),
            _ => throw new InvalidOperationException($"Unsupported provider: {provider}")
        };

        return services;
    }

    private static IServiceCollection AddDbContext<TContextImplementation, TOptionsBuilder>(
        this IServiceCollection services,
        IConfiguration configuration,
        Func<DbContextOptionsBuilder, Func<string, Action<TOptionsBuilder>, DbContextOptionsBuilder>> dbContextOptionsBuilderAction,
        Action<TOptionsBuilder> optionsBuilderAction
    ) where TContextImplementation : MyDbContext
        => services
            .AddDbContext<MyDbContext, TContextImplementation>((serviceProvider, options) =>
            {
                var connectionString = configuration.GetConnectionString("Default")!;
                dbContextOptionsBuilderAction(options)(connectionString, optionsBuilderAction);
            });
}
