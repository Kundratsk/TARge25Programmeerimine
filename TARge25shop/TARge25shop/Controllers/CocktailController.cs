
using Microsoft.AspNetCore.Mvc;
using TARge25shop.Models.Cocktail;
using TARge25shop.ServicesInterface;

namespace TARge25shop.Controllers
{
    public class CocktailController : Controller
    {
        private readonly ICocktailServices _cocktailServices;

        public CocktailController(ICocktailServices cocktailServices)
        {
            _cocktailServices = cocktailServices;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? searchTerm)
        {
            var model = new CocktailViewModel
            {
                SearchTerm = searchTerm ?? string.Empty
            };

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return View(model);
            }

            try
            {
                var result = await _cocktailServices
                    .SearchCocktailsAsync(searchTerm);

                model.Cocktails = result?.Drinks ?? new List<CocktailDto>();
            }
            catch (HttpRequestException)
            {
                model.ErrorMessage =
                    "Kokteilide päring ebaõnnestus. Proovi hiljem uuesti.";
            }
            catch (System.Text.Json.JsonException)
            {
                model.ErrorMessage =
                    "API vastuse töötlemisel tekkis viga.";
            }

            return View(model);
        }
    }
}
