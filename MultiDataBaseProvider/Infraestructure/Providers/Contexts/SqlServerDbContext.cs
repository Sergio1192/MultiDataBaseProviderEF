using Microsoft.EntityFrameworkCore;
using MultiDataBaseProvider.Domain;

namespace MultiDataBaseProvider.Infraestructure.Providers.Contexts;

public class SqlServerDbContext : MyDbContext
{
    public SqlServerDbContext(DbContextOptions<MySqlDbContext> options)
        : base(options) { }
}

