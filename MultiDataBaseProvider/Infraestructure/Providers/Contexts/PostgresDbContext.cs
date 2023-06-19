using Microsoft.EntityFrameworkCore;
using MultiDataBaseProvider.Domain;

namespace MultiDataBaseProvider.Infraestructure.Providers.Contexts;

public class PostgresDbContext : MyDbContext
{
    public PostgresDbContext(DbContextOptions<MySqlDbContext> options)
        : base(options) { }
}
