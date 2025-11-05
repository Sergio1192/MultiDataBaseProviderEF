namespace MultiDataBaseProvider.IntegrationTests.Features.WeatherForecasts;

public class CreateTests(ApiFixture fixture)
    : TestsBase(fixture)
{
    private record Request(string City = "", int Temperature = 0)
    {
        public static Request GetRandom()
            => new(Guid.NewGuid().ToString(), new Random().Next(-20, 50));
    }

    private async Task CreateAsync(Request request)
        => await SendResponseAsync<WeatherForecastController>(
            controller => controller.Create(request.City, request.Temperature)
        );

    [Fact]
    public async Task Create_wheather_forecast_insert_element_in_db()
    {
        var request = Request.GetRandom();

        await CreateAsync(request);

        var elements = await Server.Given().Data(db => db.WeatherForecasts, d => d.ToListAsync());
        elements.Should().NotBeEmpty();
        var element = elements[0];
        element.Temperature.Should().Be(request.Temperature);
        element.City.Should().Be(request.City);
    }
}
