using AspNetCoreGeneratedDocument;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TARge25shop.ApplicationServices.Services;
using TARge25shop.Core.Dto;
using TARge25shop.Core.ServiceInterface;
using TARge25shop.Data;
using TARge25shop.Models.Realestate;
using TARge25shop.Models.Spaceship;
using static System.Net.Mime.MediaTypeNames;



namespace TARge25shop.Controllers
{
    public class RealestateController : Controller
    {
        private readonly IRealestateServices _realestateServices;
        private readonly TARge25ShopContext _context;

        public RealestateController(IRealestateServices realestateServices,
            TARge25ShopContext context)
        {
            _realestateServices = realestateServices;
            _context = context;
        }

        public IActionResult Index()
        {
            var result = _context.Realestate
                .Select(x => new RealestateIndexViewModel
                {
                    Id = x.Id,
                    Area = x.Area,
                    Location = x.Location,
                    CreatedAt = x.CreatedAt,
                    RoomNumber = x.RoomNumber,
                    BuildingType = x.BuildingType
                });

            return View(result);
        }
        [HttpGet]

        public IActionResult Create()
        {
            RealEstateCreateUpdateViewModel result = new();

            return View("CreateUpdate", result);
        }
        [HttpPost]

        public async Task<IActionResult> Create(RealEstateCreateUpdateViewModel vm)
        {
            var dto = new RealestateDto
            {
                Area = vm.Area,
                Location = vm.Location,
                RoomNumber = vm.RoomNumber,
                BuildingType = vm.BuildingType
            };

            var result = await _realestateServices.Create(dto);

            if (result == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            var realestate = await _realestateServices.DetailAsync(id);

            if (realestate == null)
            {
                return NotFound();
            }

            var vm = new RealEstateCreateUpdateViewModel();

            vm.Id = realestate.Id;
            vm.Area = realestate.Area;
            vm.Location = realestate.Location;
            vm.BuildingType = realestate.BuildingType;
            vm.CreatedAt = realestate.CreatedAt;
            vm.ModifiedAt = realestate.ModifiedAt;

            return View("CreateUpdate", vm);
        }
        [HttpPost]
        public async Task<IActionResult> Update(RealEstateCreateUpdateViewModel vm)
        {
            var dto = new RealestateDto
            {
                Id = vm.Id,
                Area = vm.Area,
                Location = vm.Location,
                RoomNumber = vm.RoomNumber,
                BuildingType = vm.BuildingType
            };
            var result = await _realestateServices.Update(dto);

            if (result == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var realestate = await _realestateServices.DetailAsync(id);

            if (realestate == null)
            {
                return NotFound();
            }
            var vm = new RealestateDeleteViewModel();

            vm.Id = realestate.Id;
            vm.Area = realestate.Area;
            vm.Location = realestate.Location;
            vm.BuildingType = realestate.BuildingType;
            vm.RoomNumber = realestate.RoomNumber;
            vm.CreatedAt = realestate.CreatedAt;
            vm.ModifiedAt = realestate.ModifiedAt;


            return View(vm);
        }
        [HttpPost, ActionName("DeleteConfirmed")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            // Kutsume teenuse välja, et kosmoselaev andmebaasist kustutada
            var realestate = await _realestateServices.Delete(id);

            if (realestate == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var realestate = await _realestateServices.DetailAsync(id);

            if (realestate == null)
            {
                return NotFound();
            }

            var vm = new RealestateDeleteViewModel();

            vm.Id = realestate.Id;
            vm.Area = realestate.Area;
            vm.Location = realestate.Location;
            vm.BuildingType = realestate.BuildingType;
            vm.RoomNumber = realestate.RoomNumber;
            vm.CreatedAt = realestate.CreatedAt;
            vm.ModifiedAt = realestate.ModifiedAt;

            return View(vm);
        }


    }
}
