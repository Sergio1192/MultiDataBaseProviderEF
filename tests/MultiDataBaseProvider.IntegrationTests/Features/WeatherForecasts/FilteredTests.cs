namespace MultiDataBaseProvider.IntegrationTests.Features.WeatherForecasts;

public class FilteredTests(ApiFixture fixture)
    : TestsBase(fixture)
{
    private Task<IEnumerable<WeatherForecast>> GetAsync(string city)
        => GetResultAsync<WeatherForecastController, IEnumerable<WeatherForecast>>(
            controller => controller.Filtered(city)
        );

    [Fact]
    public async Task Get_filtered_wheather_forecasts_return_the_elements_in_db()
    {
        await Server.Given().WheatherForecastsAsync(5);
        var data = await Server.Given().AWheatherForecastAsync("prueba");

        var response = await GetAsync("prueba");
        response.Should().HaveCount(1);
        response.First().Id.Should().Be(data.Id);
    }
}