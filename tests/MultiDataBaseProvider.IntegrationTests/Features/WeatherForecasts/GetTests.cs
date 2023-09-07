using MultiDataBaseProvider.Controllers;
using MultiDataBaseProvider.Domain.Entities;

namespace MultiDataBaseProvider.IntegrationTests.Features.WeatherForecasts;

public class GetTests : TestsBase
{
    public GetTests(ApiFixture fixture)
        : base(fixture) { }

    private Task<IEnumerable<WeatherForecast>> GetAsync()
        => GetResultAsync<WeatherForecastController, IEnumerable<WeatherForecast>>(
            controller => controller.Get()
        );

    [Fact]
    public async Task Get_wheather_forecast_return_the_elements_in_db()
    {
        var data = await Server.Given().WheatherForecastsAsync(5);

        var response = await GetAsync();
        response.Should().HaveCount(data.Length);
        response.Select(r => r.Id).Should().BeEquivalentTo(data.Select(d => d.Id));
    }
}