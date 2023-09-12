namespace MultiDataBaseProvider.IntegrationTests.Infrastructure;

internal static class GivenExtensions
{
    public static Task<WeatherForecast> AWheatherForecastAsync(this Given given)
        => given.AddAsync(GetRandomData());

    public static Task<WeatherForecast[]> WheatherForecastsAsync(this Given given, int maxCount)
        => given.AddRangeAsync(
            Enumerable.Repeat(0, maxCount).Select(_ => GetRandomData())
        );

    private static WeatherForecast GetRandomData()
        => new()
        {
            City = Guid.NewGuid().ToString(),
            Temperature = Random.Shared.Next(0, 100),
            Date = DateTime.Today.AddDays(Random.Shared.Next(-100, 100))
        };
}
