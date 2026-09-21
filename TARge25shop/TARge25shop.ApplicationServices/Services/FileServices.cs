using Microsoft.Extensions.Hosting;
using TARge25shop.Core.Domain;
using TARge25shop.Core.Dto;
using TARge25shop.Core.ServiceInterface;
using TARge25shop.Data;


namespace TARge25shop.ApplicationServices.Services
{
    public class FileServices : IFileServices
    {
            private readonly IHostEnvironment _webHost;

            
            

        // Konstruktor, mis küsib Dependency Injection-ilt veebikeskkonna
        public FileServices(IHostEnvironment webHost, TARge25ShopContext context)
            {
                _webHost = webHost;      
                _context = context; 
            
            }

        private readonly TARge25ShopContext _context;
    
            
       
            
            public void FilesToApi(SpaceshipDto dto, Spaceship domain)
        {
            if (dto.Files != null && dto.Files.Count > 0)
            {
                /*kui Directoryt ei ole olemas, siis tee Directory \\wwwroot\\multipleFileUpload\\ 
                 tuleb kasutada webHosti
                if 
                */
                

                // Kui kausta ei ole olemas, siis loome selle
                if (!Directory.Exists(_webHost.ContentRootPath + "\\wwwroot\\multipleFileUpload\\"))
                {
                    Directory.CreateDirectory(_webHost.ContentRootPath + "\\wwwroot\\multipleFileUpload\\");
                }
                foreach (var file in dto.Files)
                {
                    //tuleb teha muutuja, kus on failide asukoht e kuhu hakatakse salvestama 
                    string uploadsFolder = Path.Combine(_webHost.ContentRootPath, "\\wwwroot\\multipleFileUpload");
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                    //Tuleb kaks ülevalpool olevat muutujat kombineerida üheks
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(fileStream);

                        //tuleb Domaini teha class FileToApi
                        //Kus on muutujad Id,ExistingFilePath ja SpaceshipId
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
