using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MultiDataBaseProvider.Domain.Entities.Base;

namespace MultiDataBaseProvider.Domain.Entities;

public class WeatherForecast : EntityBase
{
    public required DateOnly Date { get; set; }

    public required string City { get; set; }

    public required int Temperature { get; set; }
}

internal class WeatherForecastEntityTypeConfiguration : EntityBaseTypeConfiguration<WeatherForecast>
{
    public override void Configure(EntityTypeBuilder<WeatherForecast> builder)
    {
        base.Configure(builder);

        builder.ToTable(nameof(MyDbContext.WeatherForecasts));
    }
}