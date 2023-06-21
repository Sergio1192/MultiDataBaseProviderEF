using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MultiDataBaseProvider.Domain;
using MultiDataBaseProvider.Domain.Entities;

namespace MultiDataBaseProvider.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly MyDbContext context;

    public WeatherForecastController(MyDbContext context)
    {
        this.context = context;
    }

    [HttpGet]
    public async Task<IEnumerable<WeatherForecast>> Get()
        => await context.WeatherForecasts
                .AsNoTracking()
                .OrderBy(x => x.Date)
                .ToListAsync();

    [HttpGet("{id:Guid}")]
    public async Task<WeatherForecast> Get([FromRoute] Guid id)
        => await context.WeatherForecasts
                .AsNoTracking()
                .FirstAsync(e => e.Id == id);

    [HttpPost]
    public async Task Create(string city, int temperature)
    {
        await context.WeatherForecasts
            .AddAsync(new WeatherForecast() { City = city, Date = DateTime.Today, Temperature = temperature });

        await context.SaveChangesAsync();
    }

    [HttpPut("{id:Guid}")]
    public async Task Create([FromRoute] Guid id, DateTime day, int temperature)
    {
        WeatherForecast entity = await context.WeatherForecasts
            .FirstAsync(e => e.Id == id);
        entity.Date = day;
        entity.Temperature = temperature;

        await context.SaveChangesAsync();
    }

    [HttpDelete("{id:Guid}")]
    public async Task Delete([FromRoute] Guid id)
        => await context.WeatherForecasts
            .Where(e => e.Id == id)
            .ExecuteDeleteAsync();
}