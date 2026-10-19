
namespace TARge25shop.Models.Cocktail
{
    public class CocktailViewModel
    {
        public string SearchTerm { get; set; } = string.Empty;

        public List<CocktailDto> Cocktails { get; set; } = new();

        public string? ErrorMessage { get; set; }
    }
}
