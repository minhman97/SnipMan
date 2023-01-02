using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestSharp;

namespace SnippetManagement.Api.Controllers;

[ApiController]
[Route("[controller]")]
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

    [HttpGet(Name = "GetWeatherForecast")]
    [Authorize]
    public IEnumerable<WeatherForecast> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        
        //eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCIsImtpZCI6IndDYXNVSEZORi1WUzZnSC15Wkc5cyJ9.eyJpc3MiOiJodHRwczovL2Rldi1ucXdjZnIzYXVmZjhxOHdnLnVzLmF1dGgwLmNvbS8iLCJzdWIiOiJiWE1kQ05ac1JITlNQYVo2Y2k1SFB2TzhVUGx5NGI3R0BjbGllbnRzIiwiYXVkIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NDQzOTUiLCJpYXQiOjE2NzI2NzQzOTUsImV4cCI6MTY3Mjc2MDc5NSwiYXpwIjoiYlhNZENOWnNSSE5TUGFaNmNpNUhQdk84VVBseTRiN0ciLCJndHkiOiJjbGllbnQtY3JlZGVudGlhbHMifQ.qz0XYgSx_hyRxszyQeFIfrFzlEJYA5nsg9rhlnUMur5OWd5tMdeNe8YG5r_J5Wq7UBwltT685oP-t_ZCjK3bQkt0zaUnY_Gk9cgXroB8oxCgT0__uyhVUqYsGN7oeuPobLPZt9MqQ1C-5AwCpAfVgGtmU4Ys0ZnH_PPLQ0p7ygaFlg087tmcWcciM0l_vKHng88BCFGCxTbQEtpGXCDXMfmG41e0gI1XUc-v0VM3a3aYneNgEEQ3tjl8Uu2r1tubEfeF_E-j3u15ssCvuJonLQ-FW_QLYf6c9_Ic2z_XWup_t3e3KJyyUOWeLOZmU6LQcW_pvm5LVtAZ2iSOIzbU-A
    }
}