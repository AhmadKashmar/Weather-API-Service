using Microsoft.OpenApi.Models;
using WeatherAPI.Services;
using WeatherAPI.Models;
using Serilog;

// UPDATE BEFORE DEPLOYMENT
var DEPLOYED_FRONTEND_URL = "http://127.0.0.1:5500";

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog for logging
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("Logs/log-.log", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

// Add CORS services
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontendOrigin",
        builder =>
        {
            builder
                .WithOrigins(DEPLOYED_FRONTEND_URL)
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
});

// Bind CityData settings
builder.Services.Configure<CityDataSettings>(builder.Configuration.GetSection("CityData"));

// Add services to the container
builder.Services.AddControllers();

// Register CityService with DI and ILogger
builder.Services.AddSingleton<ICityService>(provider =>
{
    var config = provider.GetRequiredService<IConfiguration>();
    var csvFilePath = config.GetSection("CityData")["CsvFilePath"];

    if (string.IsNullOrEmpty(csvFilePath))
    {
        throw new Exception("CSV file path is not configured in appsettings.json under 'CityData:CsvFilePath'.");
    }

    var logger = provider.GetRequiredService<ILogger<CityService>>();
    return new CityService(csvFilePath, logger);
});

// Register IHttpClientFactory
builder.Services.AddHttpClient();

// Configure Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "WeatherAPI", Version = "v1" });
});

var app = builder.Build();

// Apply the CORS policy
app.UseCors("AllowFrontendOrigin");

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "WeatherAPI v1");
});

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
