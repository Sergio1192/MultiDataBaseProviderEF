namespace MultiDataBaseProvider.IntegrationTests.Features.WeatherForecasts;

public class UpdateTests : TestsBase
{
    public UpdateTests(ApiFixture fixture)
        : base(fixture) { }

    private record Request(string City = "", int Temperature = 0)
    {
        public static Request GetRandom()
            => new(Guid.NewGuid().ToString(), new Random().Next(-20, 50));
    }

    private Task UpdateAsync(Guid id, Request request)
        => SendResponseAsync<WeatherForecastController>(
            controller => controller.Update(id, request.City, request.Temperature)
        );

    [Fact]
    public async Task Update_wheather_forecast_update_the_element_in_db()
    {
        var data = await Server.Given().AWheatherForecastAsync();
        var request = Request.GetRandom();

        await UpdateAsync(data.Id, request);

        var element = await Server.Given().Data(db => db.WeatherForecasts, d => d.FirstAsync(e => e.Id == data.Id));
        element.Id.Should().Be(data.Id);
        element.Temperature.Should().Be(request.Temperature);
        element.City.Should().Be(request.City);
    }
}
