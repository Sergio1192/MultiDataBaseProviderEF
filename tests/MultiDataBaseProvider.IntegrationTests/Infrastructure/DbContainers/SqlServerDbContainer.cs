using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using MultiDataBaseProvider.Infraestructure.Providers.Contexts;
using Respawn;
using Testcontainers.MsSql;

namespace MultiDataBaseProvider.IntegrationTests.Infrastructure.DbContainers;

public class SqlServerDbContainer : DbContainer
{
    private readonly MsSqlContainer container = new MsSqlBuilder().Build();

    protected override MsSqlContainer GetContainer() => container;

    protected override string GetConnectionString()
        => container.GetConnectionString();

    public override IDbAdapter GetDbAdapter()
        => DbAdapter.SqlServer;

    public override IServiceCollection AddDbContext(IServiceCollection services)
        => AddDbContext<SqlServerDbContext, SqlServerDbContextOptionsBuilder>(
            services,
            options => options.UseSqlServer,
            builder => builder
                .MigrationsAssembly(typeof(SqlServerDbContext).Assembly.FullName)
                .UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)
                .EnableRetryOnFailure()
        );
}
