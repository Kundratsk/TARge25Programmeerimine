using AspNetCoreGeneratedDocument;
using Microsoft.AspNetCore.Mvc;
using TARge25shop.ApplicationServices.Services;
using TARge25shop.Core.Dto;
using TARge25shop.Core.ServiceInterface;
using TARge25shop.Data;
using TARge25shop.Models.Spaceship;

namespace TARge25shop.Controllers
{
    public class SpaceshipController : Controller
    {
        // (teenuse hoidmiseks)
        private readonly ISpaceshipServices _spaceshipServices;
        private readonly TARge25ShopContext _context;
        // konstruktor (ASP.NET süstib teenuse siitkaudu sisse)
        public SpaceshipController(ISpaceshipServices spaceshipServices, TARge25ShopContext context)
        {
            _spaceshipServices = spaceshipServices;
            _context = context;
        }




        public IActionResult Index()
        {
            //Kutsume teenuse välja, et saada kõik kosmoselaevad.
            //Konstruktoris tuleb välja kutsuda DBContext, et saaksime andmed kätte. Seejärel kutsume teenuse välja
            var result = _context.Spaceships
                .Select(x => new SpaceshipIndexViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    ShipType = x.ShipType,
                    CreatedAt = x.CreatedAt,
                    Crew = x.Crew
                });

            return View(result);
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
