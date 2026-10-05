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
                    ModifiedAt = x.ModifiedAt,
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
        [HttpPost]
        public async Task<IActionResult> Create(RealEstateCreateUpdateViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View("CreateUpdate", vm);
            }

            // Kontrollime igaks juhuks, et vm.Image ei oleks null, et vältida NullReferenceExceptionit
            var dto = new RealestateDto
            {
                Area = vm.Area,
                Location = vm.Location,
                RoomNumber = vm.RoomNumber,
                BuildingType = vm.BuildingType,
                Files = vm.Files,
                Image = vm.Image
                    .Select(x => new FileToDatabaseDto
                    {
                        Id = x.ImageId,
                        ImageData = x.ImageData,
                        ImageTitle = x.ImageTitle,
                        RealEstateId = x.RealEstateId
                    }).ToArray()
            }; // Siin lõppeb DTO loomine

            // Teenuse väljakutse on nüüd õigesti meetodi sees
            var result = await _realestateServices.Create(dto);

            if (result == null)
            {
                ModelState.AddModelError("", "Kinnisvara salvestamine ebaõnnestus.");
                return View("CreateUpdate", vm);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Update(RealEstateCreateUpdateViewModel dto)
        {
            if (!ModelState.IsValid)
            {
                return View("CreateUpdate", dto);
            }

            // 1. Otsi andmebaasist üles olemasolev kinnisvara selle Id järgi
            // Siin etapis on 'domain.CreatedAt' sees veel andmebaasi õige aeg
            var domain = await _context.Realestate.FindAsync(dto.Id);

            if (domain == null)
            {
                return NotFound();
            }

            // 2. Kirjuta üle AINULT need väljad, mida kasutaja sai vormil muuta
            domain.Location = dto.Location;
            domain.Area = dto.Area;
            domain.RoomNumber = dto.RoomNumber;
            domain.BuildingType = dto.BuildingType;

            // NB! ME EI KIRJUTA domain.CreatedAt rida siia üldse!
            // Nii jääb andmebaasis olev algne loomise aeg täiesti puutumata.

            // Uuendame ainult muutmise aega praeguse hetke peale
            domain.ModifiedAt = DateTime.Now;

            // 3. Salvesta muudatused
            _context.Realestate.Update(domain);
            await _context.SaveChangesAsync();

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

            var vm = new RealestateDetailsViewModel();

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
