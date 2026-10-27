
using Microsoft.AspNetCore.Mvc;
using TARge25shop.Core.ServiceInterface;
using TARge25shop.Models;

namespace TARge25shop.Controllers;

public class WeatherController : Controller
{
    private readonly IWeatherService _weatherService;

    public WeatherController(IWeatherService weatherService)
    {
        _weatherService = weatherService;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View(new WeatherViewModel
        {
            City = ""
        });
    }

    [HttpPost]
    public async Task<IActionResult> Index(string city)
    {
        var model = new WeatherViewModel
        {
            City = city?.Trim() ?? ""
        };

        if (string.IsNullOrWhiteSpace(model.City))
        {
            model.ErrorMessage = "Palun sisesta linna nimi.";
            return View(model);
        }

        try
        {
            var weather = await _weatherService.GetWeatherAsync(
                model.City);

            if (weather == null || weather.Weather.Count == 0)
            {
                model.ErrorMessage =
                    "Ilmaandmeid ei leitud. Kontrolli linna nime.";
                return View(model);
            }

            model.City = weather.City;
            model.Temperature = weather.Main.Temperature;
            model.FeelsLike = weather.Main.FeelsLike;
            model.Humidity = weather.Main.Humidity;
            model.Description = weather.Weather[0].Description;
            model.Icon = weather.Weather[0].Icon;
            model.WindSpeed = weather.Wind.Speed;
        }
        catch (HttpRequestException)
        {
            model.ErrorMessage =
                "Ilmateenusega ei saadud ühendust.";
        }
        catch (System.Text.Json.JsonException)
        {
            model.ErrorMessage =
                "Ilmateenuse vastust ei õnnestunud lugeda.";
        }

        return View(model);
    }
}
