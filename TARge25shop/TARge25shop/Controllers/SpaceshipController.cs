using AspNetCoreGeneratedDocument;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
                    EnginePower = x.EnginePower,
                    Crew = x.Crew
                });

            return View(result);
        }

        [HttpGet]
        public IActionResult Create()
        {
            SpaceshipCreateUpdateViewModel result = new();

            return View("CreateUpdate", result);
        }
        [HttpPost]

        public async Task<IActionResult> Create(SpaceshipCreateUpdateViewModel vm)
        {
            var dto = new SpaceshipDto
            {
                Name = vm.Name,
                ShipType = vm.ShipType,
                Crew = vm.Crew,
                EnginePower = vm.EnginePower,
                //failide edasiandmine dto-le
                Files = vm.Files,
                FileToApiDtos = vm.Images
                    .Select(x => new FileToApiDto
                    {
                        Id = x.ImageId,
                        ExistingFilePath = x.FilePath,
                        SpaceshipId = x.SpaceshipId
                    }).ToArray()
            };



            //Nüüd kutsume teenuse välja, et luua uus kosmoselaev. See on asünkroonne tegevus ja kasutame await

            
            var result = await _spaceshipServices.Create(dto);

            if (result == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            // OTSIME laeva andmed (asenda .GetAsync vajadusel oma teenuse otsingumeetodi nimega)
            var spaceship = await _spaceshipServices.DetailAsync(id);

            if (spaceship == null)
            {
                return NotFound();
            }

            var vm = new SpaceshipCreateUpdateViewModel
            {
                Id = spaceship.Id,
                Name = spaceship.Name,
                ShipType = spaceship.ShipType,
                Crew = spaceship.Crew,
                EnginePower = spaceship.EnginePower
            };

            return View("CreateUpdate", vm);
        
        }
        [HttpPost]
        public async Task<IActionResult> Update(SpaceshipCreateUpdateViewModel vm)
        {
            var dto = new SpaceshipDto
            {
                Id = vm.Id,
                Name = vm.Name,
                ShipType = vm.ShipType,
                Crew = vm.Crew,
                EnginePower = vm.EnginePower
            };

            var result = await _spaceshipServices.Update(dto);

            if (result == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var spaceship = await _spaceshipServices.DetailAsync(id);
            
            if (spaceship == null)
            {
                return NotFound();
            }

            var images = await _context.FileToApis
                .Where(x => x.SpaceshipId == id)
                .Select(y => new ImageViewModel
                {
                    FilePath = y.ExistingFilePath,
                    ImageId = y.Id
                }).ToArrayAsync();

            var vm = new SpaceshipDeleteViewModel();

            vm.Id = spaceship.Id;
            vm.Name = spaceship.Name;
            vm.ShipType = spaceship.ShipType;
            vm.Crew = spaceship.Crew;
            vm.EnginePower = spaceship.EnginePower;
            vm.CreatedAt = spaceship.CreatedAt;
            vm.UpdatedAt = spaceship.UpdatedAt;
            vm.Image.AddRange(images);

            return View(vm);
        }
        [HttpPost, ActionName("DeleteConfirmed")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            // Kutsume teenuse välja, et kosmoselaev andmebaasist kustutada
            var spaceship = await _spaceshipServices.Delete(id);

            if (spaceship == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var spaceship = await _spaceshipServices.DetailAsync(id);

            if (spaceship == null)
            {
                return NotFound();
            }
            var images = await _context.FileToApis
                .Where(x => x.SpaceshipId == id)
                .Select(y => new ImageViewModel
                {
                    FilePath = y.ExistingFilePath,
                    ImageId = y.Id
                }).ToArrayAsync();
            // tuleb kasutada AddRange, et saada pildid vm kaasa
            //see on vaheinstants domaini ja vm vahel

            var vm = new SpaceshipDetailsViewModel();

            vm.Id = spaceship.Id;
                vm.Name = spaceship.Name;
                vm.ShipType = spaceship.ShipType;
                vm.Crew = spaceship.Crew;
                vm.EnginePower = spaceship.EnginePower;
                vm.CreatedAt = spaceship.CreatedAt;
                vm.UpdatedAt = spaceship.UpdatedAt;
                vm.Image.AddRange(images);

            return View(vm);
        }
    }
}
