using Microsoft.Extensions.Hosting; // Kasutame algset viidet
using TARge25shop.Core.Domain;
using TARge25shop.Core.Dto;
using TARge25shop.Core.ServiceInterface;
using TARge25shop.Data;

namespace TARge25shop.ApplicationServices.Services
{
    public class FileServices : IFileServices
    {
        private readonly IHostEnvironment _webHost;
        private readonly TARge25ShopContext _context;

        public FileServices(IHostEnvironment webHost, TARge25ShopContext context)
        {
            _webHost = webHost;
            _context = context;
        }

        public void FilesToApi(SpaceshipDto dto, Spaceship domain)
        {
            if (dto.Files != null && dto.Files.Count > 0)
            {
                // MÄRKUS: Eemaldasime algusest veidrad kaldkriipsud. 
                // Nüüd Path.Combine paneb teed õigesti kokku!
                string uploadsFolder = Path.Combine(_webHost.ContentRootPath, "wwwroot", "multipleFileUpload");

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                foreach (var file in dto.Files)
                {
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(fileStream);

                        FileToApi path = new FileToApi
                        {
                            Id = Guid.NewGuid(),
                            ExistingFilePath = uniqueFileName,
                            SpaceshipId = domain.Id
                        };
                        _context.FileToApis.AddAsync(path);
                    }
                }
            }
        }
    }
}
