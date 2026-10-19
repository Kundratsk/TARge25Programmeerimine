
using System.Text.Json;
using TARge25shop.Models.Cocktail;
using TARge25shop.ServicesInterface;

namespace TARge25shop.Services
{
    public class CocktailServices : ICocktailServices
    {
        private readonly HttpClient _httpClient;

        public CocktailServices(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<CocktailRootDto?> SearchCocktailsAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return new CocktailRootDto
                {
                    Drinks = new List<CocktailDto>()
                };
            }

            var url =
                $"https://www.thecocktaildb.com/api/json/v1/1/search.php?s={Uri.EscapeDataString(name)}";

            using var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            await using var stream =
                await response.Content.ReadAsStreamAsync();

            return await JsonSerializer.DeserializeAsync<CocktailRootDto>(
                stream,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
        }
    }
}
