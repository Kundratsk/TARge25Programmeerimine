using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TARge25shop.ApplicationServices.Services;
using TARge25shop.Core.Domain;
using TARge25shop.Core.Dto;
using TARge25shop.Core.ServiceInterface;
using TARge25shop.Data;
using TARge25shop.Models.Realestate;


namespace TARge25Shop.Controllers
{
    public class RealEstateController : Controller
    {
        private readonly IRealestateServices _realestateServices;
        private readonly TARge25ShopContext _context;
        private readonly IFileServices _fileServices;

        public RealEstateController
            (
            IRealestateServices realestateServices,
            TARge25ShopContext context,
            IFileServices fileServices
            )
        {
            _realestateServices = realestateServices;
            _context = context;
            _fileServices = fileServices;
        }

        public IActionResult Index()
        {
            var result = _context.Realestate.Select(x => new RealestateIndexViewModel
            {
                Id = x.Id,
                Area = x.Area,
                Location = x.Location,
                RoomNumber = x.RoomNumber,
                BuildingType = x.BuildingType,
                CreatedAt = x.CreatedAt,
                ModifiedAt = x.ModifiedAt
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
                Id = vm.Id,
                Area = vm.Area,
                Location = vm.Location,
                RoomNumber = vm.RoomNumber,
                BuildingType = vm.BuildingType,
                //failide lisamine
                Files = vm.Files,
                Image = vm.Image
                    .Select(x => new FileToDatabaseDto
                    {
                        Id = x.ImageId,
                        ImageData = x.ImageData,
                        ImageTitle = x.ImageTitle,
                        RealEstateId = x.RealEstateId
                    }).ToArray()
            };

            var result = await _realestateServices.Create(dto);

            if (result == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid Id)
        {
            var realEstate = await _realestateServices.DetailAsync(Id);

            if (realEstate == null)
            {
                return NotFound();
            }

            var image = await FileFromDatabase(Id);

            var vm = new RealEstateCreateUpdateViewModel();

            vm.Id = realEstate.Id;
            vm.Area = realEstate.Area;
            vm.Location = realEstate.Location;
            vm.RoomNumber = realEstate.RoomNumber;
            vm.BuildingType = realEstate.BuildingType;

            vm.Image.AddRange(image);

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
                BuildingType = vm.BuildingType,
                ModifiedAt = vm.ModifiedAt,

                Files = vm.Files,
                Image = vm.Image?
            .Select(x => new FileToDatabaseDto
            {
                Id = x.ImageId,
                ImageData = x.ImageData,
                ImageTitle = x.ImageTitle,
                RealEstateId = x.RealEstateId
            }).ToArray()
            };

            var result = await _realestateServices.Update(dto);
            if (result == null)
            {
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid Id)
        {
            var realEstate = await _realestateServices.DetailAsync(Id);

            if (realEstate == null)
            {
                return NotFound();
            }

            var images = await FileFromDatabase(Id);

            var vm = new RealestateDeleteViewModel();

            vm.Id = realEstate.Id;
            vm.Area = realEstate.Area;
            vm.Location = realEstate.Location;
            vm.RoomNumber = realEstate.RoomNumber;
            vm.BuildingType = realEstate.BuildingType;
            vm.CreatedAt = realEstate.CreatedAt;
            vm.ModifiedAt = realEstate.ModifiedAt;

            vm.Images.AddRange(images);

            return View(vm);
        }

        [HttpPost]
        [ActionName("DeleteConfirmed")]
        public async Task<IActionResult> DeleteConfirmed(Guid Id)
        {
            var realEstate = await _realestateServices.Delete(Id);

            if (realEstate == null)
            {
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid Id)
        {
            var realEstate = await _realestateServices.DetailAsync(Id);

            if (realEstate == null)
            {
                return NotFound();
            }

            var images = await FileFromDatabase(Id);

            var vm = new RealestateDetailsViewModel();

            vm.Id = realEstate.Id;
            vm.Area = realEstate.Area;
            vm.Location = realEstate.Location;
            vm.RoomNumber = realEstate.RoomNumber;
            vm.BuildingType = realEstate.BuildingType;
            vm.CreatedAt = realEstate.CreatedAt;
            vm.ModifiedAt = realEstate.ModifiedAt;
            vm.Images.AddRange(images);

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> RemoveImage(RealEstateImageViewModel vm)
        {
            var dto = new FileToDatabaseDto()
            {
                Id = vm.ImageId
            };

            var image = await _fileServices.RemoveImageFromDatabase(dto);

            if (image == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task<RealEstateImageViewModel[]> FileFromDatabase(Guid id)
        {
            var databaseImages = await _context.FileToDatabases
                .Where(x => x.RealEstateId == id)
                .ToArrayAsync(); // Sulud olid puudu!

            return databaseImages.Select(y => new RealEstateImageViewModel
            {
                ImageId = y.Id,
                ImageTitle = y.ImageTitle,
                ImageData = y.ImageData,
                RealEstateId = y.RealEstateId,
                Image = string.Format("data:image/gif;base64,{0}", Convert.ToBase64String(y.ImageData))
            }).ToArray();
        }
    }
}