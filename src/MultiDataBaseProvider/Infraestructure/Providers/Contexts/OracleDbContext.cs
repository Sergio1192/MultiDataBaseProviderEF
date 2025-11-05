using Microsoft.EntityFrameworkCore;
using MultiDataBaseProvider.Domain;

namespace MultiDataBaseProvider.Infraestructure.Providers.Contexts;

public class OracleDbContext(DbContextOptions<OracleDbContext> options)
    : MyDbContext(options)
{ }

