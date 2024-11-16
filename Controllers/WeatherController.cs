using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using System;

namespace WeatherAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public WeatherController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> GetWeather([FromQuery] string city)
        {
            if (string.IsNullOrWhiteSpace(city))
            {
                return BadRequest("City name must be provided.");
            }

            var apiKey = _configuration["OpenWeatherMap:ApiKey"];
            if (string.IsNullOrEmpty(apiKey))
            {
                return StatusCode(500, "OpenWeatherMap API key is not configured.");
            }

            var httpClient = _httpClientFactory.CreateClient();

            var requestUrl = $"https://api.openweathermap.org/data/2.5/weather?q={Uri.EscapeDataString(city)}&appid={apiKey}&units=metric";

            try
            {
                var response = await httpClient.GetAsync(requestUrl);

                if (!response.IsSuccessStatusCode)
                {
                    return StatusCode((int)response.StatusCode, "Failed to retrieve weather data.");
                }

                var responseContent = await response.Content.ReadAsStringAsync();

                using var jsonDoc = JsonDocument.Parse(responseContent);
                var root = jsonDoc.RootElement;

                var weatherData = new
                {
                    city = root.GetProperty("name").GetString(),
                    temperature = $"{root.GetProperty("main").GetProperty("temp").GetDouble()}°C",
                    description = root.GetProperty("weather")[0].GetProperty("description").GetString(),
                    humidity = $"{root.GetProperty("main").GetProperty("humidity").GetInt32()}%",
                    windSpeed = $"{root.GetProperty("wind").GetProperty("speed").GetDouble()} m/s"
                };

                return Ok(weatherData);
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(500, $"An error occurred while fetching weather data. {ex.Message}");
            }
            catch (JsonException ex)
            {
                return StatusCode(500, $"An error occurred while processing weather data. {ex.Message}");
            }
        }
    }
}
