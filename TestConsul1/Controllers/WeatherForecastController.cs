using Microsoft.AspNetCore.Mvc;
using System.Net.Sockets;

namespace TestConsul1.Controllers
{
    //docker build
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray(); 
        }
        [HttpGet]
        public IActionResult Get([FromServices] IConfiguration configuration)
        {
            var result = new
            {
                msg = $"{nameof(TestConsul1)}:{DateTime.Now}",
                ip = Request.HttpContext.Connection?.LocalIpAddress?.ToString(),
                port = configuration["Consul:Port"]
            };

            return Ok(result);
        }
    }
}