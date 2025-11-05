using Microsoft.EntityFrameworkCore;
using MultiDataBaseProvider.Domain;

namespace MultiDataBaseProvider.Infraestructure.Providers.Contexts;

public class SqlServerDbContext(DbContextOptions<SqlServerDbContext> options)
    : MyDbContext(options)
{ }

