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
            .OrderBy(x => x.Date)
            .ToListAsync();
}