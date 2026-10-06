using TARge25shop.Core.Dto;
using TARge25shop.Core.Domain;

namespace TARge25shop.Core.ServiceInterface
{

    public interface IFileServices
    {
        void FilesToApi(SpaceshipDto dto, Spaceship domain);
        Task<FileToApi> RemoveImageFromApi(FileToApiDto dto);
        Task <List<FileToApi>>RemoveImagesFromApi(FileToApiDto[] dtos);
        Task UploadFilesToDatabase(RealestateDto dto, Realestate domain);
    }    
}
