using System;
using System.Collections.Generic;
using System.Text;
using TARge25shop.Core.Domain;
using TARge25shop.Core.Dto;

namespace TARge25shop.Core.ServiceInterface
{
    public interface IRealestateServices
    {
        Task<Realestate> Create(RealestateDto dto);
        Task<Realestate> Update(RealestateDto dto);
        Task<Realestate> DetailAsync(Guid id);
        Task<Realestate> Delete(Guid id);

        Task<FileToDatabase> RemoveImageFromDatabase(FileToDatabaseDto dto);
    }
}
