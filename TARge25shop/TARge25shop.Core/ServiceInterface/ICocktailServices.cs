
using TARge25shop.Models.Cocktail;

namespace TARge25shop.ServicesInterface
{
    public interface ICocktailServices
    {
        Task<CocktailRootDto?> SearchCocktailsAsync(string name);
    }
}
