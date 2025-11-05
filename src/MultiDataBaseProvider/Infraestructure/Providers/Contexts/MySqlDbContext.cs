using Microsoft.EntityFrameworkCore;
using MultiDataBaseProvider.Domain;

namespace MultiDataBaseProvider.Infraestructure.Providers.Contexts;

public class MySqlDbContext(DbContextOptions<MySqlDbContext> options)
    : MyDbContext(options)
{ }
