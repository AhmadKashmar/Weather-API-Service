using Microsoft.AspNetCore.Mvc;
using WeatherAPI.Models;
using WeatherAPI.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace WeatherAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CitiesController : ControllerBase
    {
        private readonly ICityService _cityService;
        private readonly ILogger<CitiesController> _logger;

        public CitiesController(ICityService cityService, ILogger<CitiesController> logger)
        {
            _cityService = cityService;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetCities([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 50)
        {
            try
            {
                var totalRecords = _cityService.GetTotalCities();

                if (totalRecords == 0)
                {
                    var emptyResponse = new PaginatedResponse<City>
                    {
                        PageNumber = 1,
                        PageSize = pageSize,
                        TotalPages = 0,
                        TotalRecords = 0,
                        Data = new List<City>()
                    };
                    return Ok(emptyResponse);
                }

                var totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);

                if (pageNumber < 1) pageNumber = 1;
                if (pageNumber > totalPages) pageNumber = totalPages;

                var cities = _cityService.GetCities(pageNumber, pageSize);

                var response = new PaginatedResponse<City>
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalPages = totalPages,
                    TotalRecords = totalRecords,
                    Data = cities
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching cities.");
                return StatusCode(500, "An error occurred while fetching cities.");
            }
        }
    }
}
