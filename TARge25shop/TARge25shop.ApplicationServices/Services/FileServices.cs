using Microsoft.Extensions.Hosting; // Kasutame algset viidet
using TARge25shop.Core.Domain;
using TARge25shop.Core.Dto;
using TARge25shop.Core.ServiceInterface;
using TARge25shop.Data;
using Microsoft.EntityFrameworkCore;
using System.IO.Enumeration;
using System.Net.Mime;


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

        public async Task<FileToApi>RemoveImageFromApi(FileToApiDto dto)
        {
            var imageId = await _context.FileToApis
                .FirstOrDefaultAsync(x => x.Id == dto.Id);

            var filePath = _webHost.ContentRootPath + "\\wwwroot\\multipleFileUpload\\"
                + imageId.ExistingFilePath;

            //kui fail asub selles kaustas, siis kustuta

            if (File.Exists(filePath)) {
                File.Delete(filePath);
            }

            _context.FileToApis.Remove(imageId);
            await _context.SaveChangesAsync();

            return null;
        }

        // <List<FileToApi>> List lisati sellepärast, et tegemist on listiga ehk pilte on mitu
        public async Task<List<FileToApi>> RemoveImagesFromApi(FileToApiDto[] dtos)
        {

            foreach(var dto in dtos)
            {
                var imageId = await _context.FileToApis
                            .FirstOrDefaultAsync(x => x.Id == dto.Id);

                var filePath = _webHost.ContentRootPath + "\\wwwroot\\multipleFileUpload\\"
                + imageId.ExistingFilePath;

                        //kui fail asub selles kaustas, siis kustuta

                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }

                _context.FileToApis.Remove(imageId);
                
            }
            await _context.SaveChangesAsync();

            return null;
        }

        public async Task UploadFilesToDatabase(RealestateDto dto, Realestate domain)
        {
            if (dto.Files != null && dto.Files.Count > 0)
            {
                foreach (var file in dto.Files)
                {
                    using (var target = new MemoryStream())
                    {
                        FileToDatabase files = new FileToDatabase()
                        {
                            Id = Guid.NewGuid(),
                            ImageTitle = file.FileName,
                            RealEstateId = domain.Id
                        };

                        file.CopyTo(target);
                        files.ImageData = target.ToArray();

                        // Kasutame asünkroonset lisamist õigesti koos await-iga
                        await _context.FileToDatabases.AddAsync(files);
                    }
                }

                // PÄRAST TSÜKLIT: Salvestame muudatused asünkroonselt andmebaasi
                await _context.SaveChangesAsync();
            }
        }

    }
}
