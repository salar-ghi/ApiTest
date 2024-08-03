using Microsoft.AspNetCore.Mvc;
using MyApiTwo.Models;
using System;
using Microsoft.AspNetCore.Authorization;

namespace MyApiTwo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "User")]
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

        private static readonly List<WeatherForecast> weatherForecasts = new()
        {
            new WeatherForecast { Id = 1, Date = DateTime.Now.AddDays(1), TemperatureC = 10, Summary = Summaries[0] },
            new WeatherForecast { Id = 2, Date = DateTime.Now.AddDays(2), TemperatureC = 15, Summary = Summaries[1] },
            new WeatherForecast { Id = 3, Date = DateTime.Now.AddDays(3), TemperatureC = 20, Summary = Summaries[2] },
            new WeatherForecast { Id = 4, Date = DateTime.Now.AddDays(4), TemperatureC = 25, Summary = Summaries[3] },
            new WeatherForecast { Id = 5, Date = DateTime.Now.AddDays(5), TemperatureC = 30, Summary = Summaries[4] }
        };

        [HttpGet]
        public IEnumerable<WeatherForecast> GetWeatherForecasts()
        {
            return weatherForecasts;
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public ActionResult<WeatherForecast> GetWeatherForecastById(int id)
        {
            var weatherForecast = weatherForecasts.FirstOrDefault(f => f.Id == id);
            if (weatherForecast == null)
            {
                return NotFound();
            }
            return weatherForecast;
        }

        [HttpGet("date/{date}")]
        public IEnumerable<WeatherForecast> GetWeatherForecastsByDate(DateTime date)
        {
            return weatherForecasts.Where(f => f.Date.Date == date.Date);
        }

        [HttpGet("temperature/{minTemp}/{maxTemp}")]
        public IEnumerable<WeatherForecast> GetWeatherForecastsByTemperatureRange(int minTemp, int maxTemp)
        {
            return weatherForecasts.Where(f => f.TemperatureC >= minTemp && f.TemperatureC <= maxTemp);
        }

        [HttpGet("summary/{summary}")]
        public IEnumerable<WeatherForecast> GetWeatherForecastsBySummary(string summary)
        {
            return weatherForecasts.Where(f => f.Summary.Equals(summary, StringComparison.OrdinalIgnoreCase));
        }

        [HttpGet("top/{count}")]
        public IEnumerable<WeatherForecast> GetTopWeatherForecasts(int count)
        {
            return weatherForecasts.OrderByDescending(f => f.TemperatureC).Take(count);
        }

        [HttpGet("bottom/{count}")]
        public IEnumerable<WeatherForecast> GetBottomWeatherForecasts(int count)
        {
            return weatherForecasts.OrderBy(f => f.TemperatureC).Take(count);
        }

        [HttpGet("city/{city}")]
        public IEnumerable<WeatherForecast> GetWeatherForecastsByCity(string city)
        {
            return weatherForecasts.Where(f => f.City == city);
        }

        [HttpGet("city-summary")]
        public IEnumerable<CityWeatherSummary> GetCityWeatherSummaries()
        {
            return weatherForecasts
                .GroupBy(f => f.City)
                .Select(g => new CityWeatherSummary
                {
                    City = g.Key,
                    AverageTemperature = (int)g.Average(f => f.TemperatureC),
                    HighestTemperature = g.Max(f => f.TemperatureC),
                    LowestTemperature = g.Min(f => f.TemperatureC)
                });
        }

        [HttpPost]
        public ActionResult<WeatherForecast> CreateWeatherForecast(WeatherForecast weatherForecast)
        {
            weatherForecast.Id = weatherForecasts.Count + 1;
            weatherForecasts.Add(weatherForecast);
            return CreatedAtAction(nameof(GetWeatherForecastById), new { id = weatherForecast.Id }, weatherForecast);
        }

    }
}
