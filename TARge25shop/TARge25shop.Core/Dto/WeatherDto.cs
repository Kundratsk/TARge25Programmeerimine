
using System.Text.Json.Serialization;

namespace TARge25shop.Core.Dto;

public class WeatherDto
{
    [JsonPropertyName("name")]
    public string City { get; set; } = "";

    [JsonPropertyName("main")]
    public WeatherMainDto Main { get; set; } = new();

    [JsonPropertyName("weather")]
    public List<WeatherDescriptionDto> Weather { get; set; } = new();

    [JsonPropertyName("wind")]
    public WeatherWindDto Wind { get; set; } = new();
}

public class WeatherMainDto
{
    [JsonPropertyName("temp")]
    public double Temperature { get; set; }

    [JsonPropertyName("feels_like")]
    public double FeelsLike { get; set; }

    [JsonPropertyName("humidity")]
    public int Humidity { get; set; }
}

public class WeatherDescriptionDto
{
    [JsonPropertyName("description")]
    public string Description { get; set; } = "";

    [JsonPropertyName("icon")]
    public string Icon { get; set; } = "";
}

public class WeatherWindDto
{
    [JsonPropertyName("speed")]
    public double Speed { get; set; }
}
