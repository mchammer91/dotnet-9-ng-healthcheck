using Microsoft.AspNetCore.Mvc;

namespace HealthCheck.Server.Controllers
{
    /*
     * Could be better to create BaseApiController and have controller inherit from that 
     * like recommended here: https://stackoverflow.com/a/78762495 to provide functionality like default route prefix
     */
    [ApiController]
    [Route("api/[controller]")] 
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IConfiguration _configuration;

        public WeatherForecastController(ILogger<WeatherForecastController> logger,
            
            // Injecting configuration dependency
            IConfiguration configuration
            )
        {
            _logger = logger;
            _configuration = configuration;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            // Demonstrates retrieving value from injected configuration (from appsettings.json)
            // Would use appsettings.Development.json if ASPNETCORE_ENVIRONMENT (set in launchSettings.json by default) were set to "Development"
            // Follows pattern appsettings.<environment name>.json. Ex: appsettings.Stage.json => ASPNETCORE_ENVIRONMENT=Stage 
            var defaultLogging = _configuration["Logging:LogLevel:Default"];
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
