namespace MultiDataBaseProvider.IntegrationTests.Features.WeatherForecasts;

public class DeleteTests : TestsBase
{
    public DeleteTests(ApiFixture fixture)
        : base(fixture) { }

    private Task DeleteAsync(Guid id)
        => SendResponseAsync<WeatherForecastController>(
            controller => controller.Delete(id)
        );

    [Fact]
    public async Task Delete_wheather_forecast_remove_the_element_from_db()
    {
        var data = await Server.Given().AWheatherForecastAsync();

        await DeleteAsync(data.Id);

        var elements = await Server.Given().Data(db => db.WeatherForecasts, d => d.Select(e => e.Id).ToListAsync());
        elements.Should().BeEmpty();
    }
}