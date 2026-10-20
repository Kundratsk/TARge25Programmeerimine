
using System.Net.Http;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using TARge25shop.Core.Dto;
using TARge25shop.Core.ServiceInterface;

namespace TARge25shop.ApplicationServices.Services;

public class WeatherService : IWeatherService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public WeatherService(
        HttpClient httpClient,
        IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public async Task<WeatherDto?> GetWeatherAsync(string city)
    {
        var apiKey = _configuration["OpenWeather:ApiKey"];

        if (string.IsNullOrWhiteSpace(apiKey))
        {
            throw new InvalidOperationException(
                "OpenWeather API võti puudub konfiguratsioonist.");
        }

        var url = $"data/2.5/weather?q={Uri.EscapeDataString(city)}" +
                  $"&appid={Uri.EscapeDataString(apiKey)}" +
                  "&units=metric&lang=et";

        using var response = await _httpClient.GetAsync(url);

        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        var json = await response.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<WeatherDto>(
            json,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
    }
}
