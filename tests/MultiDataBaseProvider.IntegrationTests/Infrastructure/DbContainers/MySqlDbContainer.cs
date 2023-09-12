using Microsoft.Extensions.DependencyInjection;
using MultiDataBaseProvider.Infraestructure.Providers.Contexts;
using MySql.EntityFrameworkCore.Infrastructure;
using Respawn;
using Testcontainers.MySql;

namespace MultiDataBaseProvider.IntegrationTests.Infrastructure.DbContainers;

public class MySqlDbContainer : DbContainer
{
    private readonly MySqlContainer container = new MySqlBuilder()
        .WithDatabase("InsureTest")
        .Build();

    protected override MySqlContainer GetContainer() => container;

    protected override string GetConnectionString()
        => container.GetConnectionString();

    public override IDbAdapter GetDbAdapter()
        => DbAdapter.MySql;

    public override IServiceCollection AddDbContext(IServiceCollection services)
        => AddDbContext<MySqlDbContext, MySQLDbContextOptionsBuilder>(
            services,
            options => options.UseMySQL,
            builder => builder
                .MigrationsAssembly(typeof(MySqlDbContext).Assembly.FullName)
                .UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)
                .EnableRetryOnFailure()
        );
}
