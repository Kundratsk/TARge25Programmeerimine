using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using TARge25Shop.Core.ServiceInterface;
using TARge25Shop.Models.ChuckNorris;

namespace TARge25Shop.Controllers
{
    public class ChuckNorrisController : Controller
    {
        private readonly IChuckNorrisServices _chuckNorrisServices;

        public ChuckNorrisController(IChuckNorrisServices chuckNorrisServices)
        {
            _chuckNorrisServices = chuckNorrisServices;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var joke = await _chuckNorrisServices.ChuckNorrisResultHttpClient();

            if (joke == null)
            {
                return NotFound("API-st ei saadud andmeid.");
            }

            ChuckNorrisViewModel vm = new();

            vm.CreatedAt = joke.CreatedAt;
            vm.IconUrl = joke.IconUrl;
            vm.Id = joke.Id;
            vm.UpdatedAt = joke.UpdatedAt;
            vm.Url = joke.Url;
            vm.Value = joke.Value;

            return View(vm);
        }
    }
}