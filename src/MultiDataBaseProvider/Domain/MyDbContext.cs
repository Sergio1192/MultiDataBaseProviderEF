using Microsoft.EntityFrameworkCore;
using MultiDataBaseProvider.Domain.Entities;

namespace MultiDataBaseProvider.Domain;

public abstract class MyDbContext(DbContextOptions options)
    : DbContext(options)
{
    public DbSet<WeatherForecast> WeatherForecasts { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MyDbContext).Assembly);
    }
}
