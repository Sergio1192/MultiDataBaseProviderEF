using Microsoft.Extensions.DependencyInjection;
using MultiDataBaseProvider.Infraestructure.Providers.Contexts;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;
using Respawn;
using Testcontainers.PostgreSql;

namespace MultiDataBaseProvider.IntegrationTests.Infrastructure.DbContainers;

public class PostgresDbContainer : DbContainer
{
    private readonly PostgreSqlContainer container = new PostgreSqlBuilder().Build();

    protected override PostgreSqlContainer GetContainer() => container;

    protected override string GetConnectionString()
        => container.GetConnectionString();

    public override IDbAdapter GetDbAdapter()
        => DbAdapter.Postgres;

    public override IServiceCollection AddDbContext(IServiceCollection services)
        => AddDbContext<PostgresDbContext, NpgsqlDbContextOptionsBuilder>(
            services,
            options => options.UseNpgsql,
            builder => builder
                .MigrationsAssembly(typeof(PostgresDbContext).Assembly.FullName)
                .UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)
                .EnableRetryOnFailure()
        );
}
