using AspNetCoreGeneratedDocument;
using Microsoft.AspNetCore.Mvc;
using TARge25shop.ApplicationServices.Services;
using TARge25shop.Core.Dto;
using TARge25shop.Core.ServiceInterface;
using TARge25shop.Models.Spaceship;

namespace TARge25shop.Controllers
{
    public class SpaceshipController : Controller
    {
        // (teenuse hoidmiseks)
        private readonly ISpaceshipServices _spaceshipServices;

        // konstruktor (ASP.NET süstib teenuse siitkaudu sisse)
        public SpaceshipController(ISpaceshipServices spaceshipServices)
        {
            _spaceshipServices = spaceshipServices;
        }




        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]

        public async Task<IActionResult> Create(SpaceshipCreateViewModel vm)
        {
            var dto = new SpaceshipDto
            {
                Name = vm.Name,
                ShipType = vm.ShipType,
                Crew = vm.Crew,
                EnginePower = vm.EnginePower
            };

            //Nüüd kutsume teenuse välja, et luua uus kosmoselaev. See on asünkroonne tegevus ja kasutame await

            
            var result = await _spaceshipServices.Create(dto);

            if (result == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
