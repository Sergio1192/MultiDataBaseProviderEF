using Microsoft.EntityFrameworkCore;
using MultiDataBaseProvider.Domain;

namespace MultiDataBaseProvider.Infraestructure.Providers.Contexts;

public class OracleDbContext : MyDbContext
{
    public OracleDbContext(DbContextOptions<MySqlDbContext> options)
        : base(options) { }
}

