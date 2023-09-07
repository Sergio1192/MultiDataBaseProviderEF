using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MultiDataBaseProvider.Infraestructure.Providers.Contexts;
using Oracle.EntityFrameworkCore.Infrastructure;
using Respawn;
using Testcontainers.Oracle;

namespace MultiDataBaseProvider.IntegrationTests.Infrastructure.DbContainers;

public class OracleDbContainer : DbContainer
{
    private readonly OracleContainer container = new OracleBuilder().Build();

    protected override OracleContainer GetContainer() => container;

    protected override string GetConnectionString()
        => container.GetConnectionString().Replace($"User Id={OracleBuilder.DefaultUsername}", "User Id=system");

    public override IDbAdapter GetDbAdapter()
        => DbAdapter.Oracle;

    public override IServiceCollection AddDbContext(IServiceCollection services)
        => AddDbContext<OracleDbContext, OracleDbContextOptionsBuilder>(
            services,
            options => options.UseOracle,
            builder => builder
                .MigrationsAssembly(typeof(OracleDbContext).Assembly.FullName)
                .UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)
        );
}

