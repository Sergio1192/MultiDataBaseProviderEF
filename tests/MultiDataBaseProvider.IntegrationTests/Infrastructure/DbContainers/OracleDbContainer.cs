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
        => container.GetConnectionString();

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

    public override string[] GetSchemasToInclude()
        => new[] { OracleBuilder.DefaultUsername.ToUpper() };
}

