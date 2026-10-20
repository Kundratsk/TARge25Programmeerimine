
using TARge25shop.Core.Dto;

namespace TARge25shop.Core.ServiceInterface;

public interface IWeatherService
{
    Task<WeatherDto?> GetWeatherAsync(string city);
}
