using Microsoft.EntityFrameworkCore;
using MultiDataBaseProvider.Domain;

namespace MultiDataBaseProvider.Infraestructure.Providers.Contexts;

public class MySqlDbContext : MyDbContext
{
    public MySqlDbContext(DbContextOptions<MySqlDbContext> options)
        : base(options) { }
}
