namespace MultiDataBaseProvider.IntegrationTests.Features.WeatherForecasts;

public class GetByIdTests : TestsBase
{
    public GetByIdTests(ApiFixture fixture)
        : base(fixture) { }

    private Task<WeatherForecast> GetAsync(Guid id)
        => GetResultAsync<WeatherForecastController, WeatherForecast>(
            controller => controller.Get(id)
        );

    [Fact]
    public async Task Get_wheather_forecast_return_the_element_in_db()
    {
        var data = await Server.Given().AWheatherForecastAsync();

        var response = await GetAsync(data.Id);
        response.Id.Should().Be(data.Id);
        response.Temperature.Should().Be(data.Temperature);
        response.City.Should().Be(data.City);
        response.Date.Should().Be(data.Date);
    }
}