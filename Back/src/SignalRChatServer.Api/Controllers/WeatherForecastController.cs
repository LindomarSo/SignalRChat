using Microsoft.AspNetCore.Mvc;
using SignalRChatServer.Api.Models.Enums;
using SignalRChatServer.Api.Services;

namespace SignalRChatServer.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;
    private readonly IHubConnectionService _hubConnection;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, IHubConnectionService hubConnection)
    {
        _logger = logger;
        _hubConnection = hubConnection;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public async Task<IEnumerable<WeatherForecast>> Get()
    {
        var user = "API Weather Forecast";

        await _hubConnection.SendAsync(HubMethod.NewMessage, user, $"{DateOnly.FromDateTime(DateTime.Now)}, {Random.Shared.Next(25)}");

        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }

    public record Message(string Text, string UserName);
}
