using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TARge25shop.Core.Dto;
using TARge25shop.Core.ServiceInterface;
using TARge25shop.Data;
using TARge25shop.Models.Kindergarten;

namespace TARge25shop.Controllers
{
    public class KindergartenController : Controller
    {
        private readonly IKindergartenServices _kindergartenServices;
        private readonly IFileServices _fileServices;
        private readonly TARge25ShopContext _context;

        public KindergartenController(
            IKindergartenServices kindergartenServices,
            IFileServices fileServices,
            TARge25ShopContext context)
        {
            _kindergartenServices = kindergartenServices;
            _fileServices = fileServices;
            _context = context;
        }

        // GET: Kindergarten
        public IActionResult Index()
        {
            var result = _context.Kindergarten
                .Select(x => new KindergartenIndexViewModel
                {
                    Id = x.Id,
                    GroupName = x.GroupName,
                    KindergartenName = x.KindergartenName,
                    ChildrenCount = x.ChildrenCount,
                    TeacherName = x.TeacherName,
                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt
                });

            return View(result);
        }

        // GET: Kindergarten/Create
        [HttpGet]
        public IActionResult Create()
        {
            KindergartenCreateUpdateViewModel result = new();

            return View("CreateUpdate", result);
        }

        // POST: Kindergarten/Create
        [HttpPost]
        public async Task<IActionResult> Create(KindergartenCreateUpdateViewModel vm)
        {
            var dto = new KindergartenDto
            {
                GroupName = vm.GroupName,
                KindergartenName = vm.KindergartenName,
                ChildrenCount = vm.ChildrenCount,
                TeacherName = vm.TeacherName,
                Files = vm.Files
            };

            await _kindergartenServices.Create(dto);

            return RedirectToAction(nameof(Index));
        }

        // GET: Kindergarten/Update
        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            var kindergarten = await _kindergartenServices.DetailAsync(id);

            if (kindergarten == null)
            {
                return NotFound();
            }

            var vm = new KindergartenCreateUpdateViewModel
            {
                Id = kindergarten.Id,
                GroupName = kindergarten.GroupName,
                KindergartenName = kindergarten.KindergartenName,
                ChildrenCount = kindergarten.ChildrenCount,
                TeacherName = kindergarten.TeacherName,
                Image = await LoadImages(id)
            };

            return View("CreateUpdate", vm);
        }

        // POST: Kindergarten/Update
        [HttpPost]
        public async Task<IActionResult> Update(KindergartenCreateUpdateViewModel vm)
        {
            var dto = new KindergartenDto
            {
                Id = vm.Id,
                GroupName = vm.GroupName,
                KindergartenName = vm.KindergartenName,
                ChildrenCount = vm.ChildrenCount,
                TeacherName = vm.TeacherName,
                Files = vm.Files
            };

            await _kindergartenServices.Update(dto);

            return RedirectToAction(nameof(Index));
        }

        // GET: Kindergarten/Delete
        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var kindergarten = await _kindergartenServices.DetailAsync(id);

            if (kindergarten == null)
            {
                return NotFound();
            }

            var vm = new KindergartenDeleteViewModel
            {
                Id = kindergarten.Id,
                GroupName = kindergarten.GroupName,
                KindergartenName = kindergarten.KindergartenName,
                ChildrenCount = kindergarten.ChildrenCount,
                TeacherName = kindergarten.TeacherName,
                CreatedAt = kindergarten.CreatedAt,
                UpdatedAt = kindergarten.UpdatedAt,
                Image = await LoadImages(id)
            };

            return View(vm);
        }

        // POST: Kindergarten/Delete
        [HttpPost, ActionName("DeleteConfirmed")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _kindergartenServices.Delete(id);

            return RedirectToAction(nameof(Index));
        }

        // GET: Kindergarten/Details
        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var kindergarten = await _kindergartenServices.DetailAsync(id);

            if (kindergarten == null)
            {
                return NotFound();
            }

            var vm = new KindergartenDetailsViewModel
            {
                Id = kindergarten.Id,
                GroupName = kindergarten.GroupName,
                KindergartenName = kindergarten.KindergartenName,
                ChildrenCount = kindergarten.ChildrenCount,
                TeacherName = kindergarten.TeacherName,
                CreatedAt = kindergarten.CreatedAt,
                UpdatedAt = kindergarten.UpdatedAt,
                Image = await LoadImages(id)
            };

            return View(vm);
        }

        // POST: Kindergarten/RemoveImage
        [HttpPost]
        public async Task<IActionResult> RemoveImage(KindergartenImageViewModel vm)
        {
            if (vm.ImageId == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var dto = new FileToDatabaseDto { Id = vm.ImageId.Value };
            await _fileServices.RemoveImageFromDatabase(dto);

            return RedirectToAction(nameof(Update), new { id = vm.KindergartenId });
        }

        private async Task<List<KindergartenImageViewModel>> LoadImages(Guid id)
        {
            var images = await _context.FileToDatabases
                .Where(x => x.KindergartenId == id)
                .Select(x => new KindergartenImageViewModel
                {
                    ImageId = x.Id,
                    KindergartenId = x.KindergartenId,
                    ImageTitle = x.ImageTitle,
                    Image = x.ImageData
                })
                .ToListAsync();

            foreach (var img in images)
            {
                if (img.Image != null)
                {
                    img.ImageData = "data:image/jpeg;base64," + Convert.ToBase64String(img.Image);
                }
            }

            return images;
        }
    }
}