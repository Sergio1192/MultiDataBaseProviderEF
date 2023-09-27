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
                .ToListAsync(HttpContext.RequestAborted);

    [HttpGet("{id:Guid}")]
    public async Task<WeatherForecast> Get([FromRoute] Guid id)
        => await context.WeatherForecasts
                .AsNoTracking()
                .FirstAsync(e => e.Id == id, HttpContext.RequestAborted);

    [HttpGet(nameof(Filtered))]
    public async Task<IEnumerable<WeatherForecast>> Filtered(string city)
        => await context.WeatherForecasts
            .AsNoTracking()
            .Where(x => x.City.Contains(city))
            .OrderBy(x => x.Date)
            .ToListAsync(HttpContext.RequestAborted);

    [HttpPost]
    public async Task Create(string city, int temperature)
    {
        await context.WeatherForecasts
            .AddAsync(new WeatherForecast() { City = city, Date = DateTime.Today, Temperature = temperature }, HttpContext.RequestAborted);

        await context.SaveChangesAsync(HttpContext.RequestAborted);
    }

    [HttpPut("{id:Guid}")]
    public async Task Update([FromRoute] Guid id, string city, int temperature)
    {
        WeatherForecast entity = await context.WeatherForecasts
            .FirstAsync(e => e.Id == id, HttpContext.RequestAborted);
        entity.City = city;
        entity.Temperature = temperature;

        await context.SaveChangesAsync(HttpContext.RequestAborted);
    }

    [HttpDelete("{id:Guid}")]
    public async Task Delete([FromRoute] Guid id)
        => await context.WeatherForecasts
            .Where(e => e.Id == id)
            .ExecuteDeleteAsync(HttpContext.RequestAborted);
}