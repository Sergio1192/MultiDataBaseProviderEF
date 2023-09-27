namespace MultiDataBaseProvider.IntegrationTests.Infrastructure;

internal static class GivenExtensions
{
    public static Task<WeatherForecast> AWheatherForecastAsync(this Given given, string prefix = "test")
        => given.AddAsync(GetRandomData(prefix));

    public static Task<WeatherForecast[]> WheatherForecastsAsync(this Given given, int maxCount)
        => given.AddRangeAsync(
            Enumerable.Range(1, maxCount).Select(number => GetRandomData(number.ToString()))
        );

    private static WeatherForecast GetRandomData(string prefix)
        => new()
        {
            City = $"{prefix}_{Guid.NewGuid()}",
            Temperature = Random.Shared.Next(0, 100),
            Date = DateTime.Today.AddDays(Random.Shared.Next(-100, 100))
        };
}
