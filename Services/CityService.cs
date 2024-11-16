using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using WeatherAPI.Models;
using Microsoft.Extensions.Logging;

namespace WeatherAPI.Services
{
    public interface ICityService
    {
        IEnumerable<City> GetCities(int pageNumber, int pageSize);
        int GetTotalCities();
    }

    public class CityService : ICityService
    {
        private readonly List<City> _cities;
        private readonly ILogger<CityService> _logger;

        public CityService(string csvFilePath, ILogger<CityService> logger)
        {
            _logger = logger;
            _cities = LoadCitiesFromCsv(csvFilePath);
        }

        private List<City> LoadCitiesFromCsv(string csvFilePath)
        {
            if (!File.Exists(csvFilePath))
            {
                _logger.LogError($"CSV file not found at path: {csvFilePath}");
                throw new FileNotFoundException($"CSV file not found at path: {csvFilePath}");
            }

            try
            {
                using var reader = new StreamReader(csvFilePath);
                using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

                var records = csv.GetRecords<City>().ToList();
                _logger.LogInformation($"Loaded {records.Count} cities from CSV.");
                return records;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error reading CSV file.");
                throw new Exception($"Error reading CSV file: {ex.Message}", ex);
            }
        }

        public IEnumerable<City> GetCities(int pageNumber, int pageSize)
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 1;

            return _cities
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }

        public int GetTotalCities()
        {
            return _cities.Count;
        }
    }
}
