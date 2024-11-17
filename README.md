# Weather API

A C# ASP.NET Core Web API that provides weather data for a list of cities. Uses the OpenWeatherMap API to fetch real-time weather information for a given city.

## Endpoints

### 1. **Get Cities**

Retrieve a paginated list of cities.

**Endpoint**: `GET /api/cities`

**Query Parameters**:

-   `pageNumber` (optional, default = `1`): The page number to retrieve.
-   `pageSize` (optional, default = `50`): The number of cities per page.

**Response**:

```json
{
	"pageNumber": 1,
	"pageSize": 50,
	"totalPages": 10,
	"totalRecords": 500,
	"data": [
		{
			"id": 1,
			"name": "New York",
			"country": "USA"
		},
		{
			"id": 2,
			"name": "London",
			"country": "UK"
		}
	]
}
```

---

### 2. **Get Weather**

Retrieve weather data for a specific city.

**Endpoint**: `GET /api/weather`

**Query Parameters**:

-   `city` (required): The name of the city.

**Response**:

```json
{
	"city": "New York",
	"temperature": "25°C",
	"description": "clear sky",
	"humidity": "60%",
	"windSpeed": "5 m/s"
}
```

---

## Prerequisites

-   **.NET 8 SDK**: [Download .NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
-   **OpenWeatherMap API Key**: Sign up for an API key at [OpenWeatherMap](https://openweathermap.org/api).

---

## Setup Instructions

### 1. Clone the Repository

```bash
git clone https://github.com/AhmadKashmar/Weather-API-Service.git
cd weather-api
```

### 2. Configure the Application

Create a file `appsettings.json`, and add your OpenWeatherMap API key. Below is a sampe configuration:

```json
{
	"Logging": {
		"LogLevel": {
			"Default": "Information",
			"Microsoft.AspNetCore": "Warning"
		}
	},
	"AllowedHosts": "*",
	"OpenWeatherMap": {
		"ApiKey": "YOUR_API_KEY"
	},
	"CityData": {
		"CsvFilePath": "Data/cities.csv"
	}
}
```

### 3. Run the Application

```bash
dotnet run
```

-   Check your logs to find the URL where the application is running (e.g., `http://localhost:5000`).

---

## Technologies Used

-   **ASP.NET Core 8**
-   **CSV Helper**: For parsing CSV files. Should be removed in a future update that integrates a database.
-   **OpenWeatherMap API**: For weather data.
-   **Azure App Services**: For hosting the API.

---

## Project Structure

```plaintext
WeatherAPI/
├── Controllers/
│   ├── CitiesController.cs       # Handles city-related endpoints
│   ├── WeatherController.cs      # Handles weather-related endpoints
├── Models/
│   ├── City.cs                   # Model for city data
│   ├── PaginatedResponse.cs      # Model for paginated API responses
│   ├── CityDataSettings.cs       # Configuration for CSV file path
├── Services/
│   ├── CityService.cs            # Service for managing city data
├── appsettings.json              # Application configuration
├── Program.cs                    # Entry point for the application
├── Data/
│   ├── Data.csv                  # Cities currently available in the API (to be replaced with a database)
```

---

## Contributions

This project was made as a part of a Cloud and Big Data course at the Lebanese University, under the supervision of Dr. Amjad Alhajjar, and with the help of my colleague, Nazih Alhajj.

---

## License

This project is licensed under the Mozilla License. See the [LICENSE](LICENSE) file for details.

---
