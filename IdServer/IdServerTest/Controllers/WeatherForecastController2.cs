using Microsoft.AspNetCore.Mvc;

namespace IdServerTest.Controllers
{
    [ApiController]
    [Route("idservertest/[controller]/[action]")]
    public class WeatherForecastController2 : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController2> _logger;

        public WeatherForecastController2(ILogger<WeatherForecastController2> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast2> GetWeatherForecast2()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast2
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}