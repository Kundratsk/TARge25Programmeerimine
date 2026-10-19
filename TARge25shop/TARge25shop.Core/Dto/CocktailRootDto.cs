
using System.Text.Json.Serialization;

namespace TARge25shop.Models.Cocktail
{
    public class CocktailRootDto
    {
        [JsonPropertyName("drinks")]
        public List<CocktailDto>? Drinks { get; set; }
    }

    public class CocktailDto
    {
        [JsonPropertyName("idDrink")]
        public string? Id { get; set; }

        [JsonPropertyName("strDrink")]
        public string? Name { get; set; }

        [JsonPropertyName("strDrinkThumb")]
        public string? Image { get; set; }

        [JsonPropertyName("strInstructions")]
        public string? Instructions { get; set; }

        [JsonPropertyName("strCategory")]
        public string? Category { get; set; }

        [JsonPropertyName("strAlcoholic")]
        public string? Alcoholic { get; set; }

        [JsonPropertyName("strGlass")]
        public string? Glass { get; set; }
    }
}
