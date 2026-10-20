
namespace TARge25shop.Models;

public class WeatherViewModel
{
    public string City { get; set; } = "";

    public double Temperature { get; set; }

    public double FeelsLike { get; set; }

    public int Humidity { get; set; }

    public string Description { get; set; } = "";

    public string Icon { get; set; } = "";

    public double WindSpeed { get; set; }

    public string? ErrorMessage { get; set; }
}
