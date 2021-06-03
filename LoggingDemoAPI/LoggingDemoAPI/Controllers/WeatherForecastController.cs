using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoggingDemoAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        //private readonly ILogger<WeatherForecastController> _logger;

        private readonly ILogger _logger;

        public WeatherForecastController(ILoggerFactory factory)
        {
            _logger = factory.CreateLogger("WeatherForecast");
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            _logger.LogInformation("some info");
            _logger.LogDebug("Debug");
            _logger.LogTrace("Tracing....");
            _logger.LogError("APP crashed");
            _logger.LogCritical("something was breached");
            _logger.LogWarning("alert something is probably not right");

            var i = 10;
            var id = 14;
            _logger.LogWarning(id,"whoops server timed out.. {Servertime}ms", i);

            try
            {
                throw new Exception("ERROR ERROR ERROR");
            }
            catch (Exception e)
            {
                _logger.LogError(e, "{time} There was an exception", DateTime.UtcNow);
            }
            

            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
