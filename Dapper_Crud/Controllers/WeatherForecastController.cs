
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dapper_Crud.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
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

        [HttpGet]
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


        [HttpGet("TestAPICreation")]
        public IActionResult GetTest()
        {
            return Ok("Test API is working!");
        }

        [HttpPost("posts")]
        public IActionResult Post()
        {
            return Ok("POST: Test API is working!");
        }

        [HttpPut("update/{id}")]
        public IActionResult Put(int id)
        {
            return Ok($"PUT: Weather forecast with ID {id} updated successfully.");
        }

        [HttpPatch("patch/{id}")]
        public IActionResult Patch(int id)
        {
            return Ok($"PATCH: Weather forecast with ID {id} partially updated successfully.");
        }

        [HttpDelete("delete/{id}")]
        public IActionResult Delete(int id)
        {
            return Ok($"DELETE: Weather forecast with ID {id} deleted successfully.");
        }
    }
}
