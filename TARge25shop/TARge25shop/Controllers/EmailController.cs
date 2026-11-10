using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client.NativeInterop;
using TARge25shop.Core.Dto;
using TARge25shop.Core.ServiceInterface;
using TARge25shop.Models.Email;

namespace TARge25shop.Controllers
{
    public class EmailController : Controller
    {

        private readonly IEmailServices _emailServices;

        public EmailController(IEmailServices emailServices)
        {
            _emailServices = emailServices;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        //vaja teha meetod, mis võtab vastu EmailDto objekti ja kutsub välja _emailServices.SendEmail meetodi ning teha instantsi, mis vahendab andmeid EmailDto ja ViewModeli vahel
        public IActionResult SendEmail(EmailViewModel vm)
        {
            // 1. Kontrollime, kas mudeli andmed on korrektsed
            if (!ModelState.IsValid)
            {
                return View("Index", vm);
            }

            // 2. Võtame manused vastu (eelistame ViewModeli listi, vajadusel võtame otse vormist)
            var files = vm.Attachments != null && vm.Attachments.Any()
                ? vm.Attachments
                : (Request.Form.Files.Any() ? Request.Form.Files.ToList() : new List<IFormFile>());

            // 3. Andmete vahendamine ViewModelist DTO-sse
            var dto = new EmailDto
            {
                To = vm.To,
                Subject = vm.Subject,
                Body = vm.Body,
                Attachments = files
            };

            try
            {
                // 4. Kuna SendEmail on 'public void', kutsume selle välja otse ja sünkroonselt
                _emailServices.SendEmail(dto);

                TempData["Teade"] = "E-kiri edukalt teele saadetud!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // 5. PARANDUS: Tagastame "Index" vaate koos mudeliga, et veateade kuvataks ekraanil
                ModelState.AddModelError(string.Empty, "Kirja saatmine ebaõnnestus: " + ex.Message);

                return View("Index", vm);
            }
        }
    }
}
