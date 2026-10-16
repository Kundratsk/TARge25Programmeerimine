using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using TARge25shop.Core.Domain;
using TARge25shop.Core.Dto;
using TARge25shop.Core.ServiceInterface;
using TARge25shop.Data;

namespace TARge25shop.ApplicationServices.Services
{
    public class RealestateServices: IRealestateServices
    {
        private readonly TARge25ShopContext _context;
        private readonly IFileServices _fileServices;

        public RealestateServices(TARge25ShopContext context, IFileServices fileServices)
        {
            _context = context;
            _fileServices = fileServices;
        }

        public async Task<Realestate> Create(RealestateDto dto)
        {
            Realestate realEstate = new();

            realEstate.Id = Guid.NewGuid();
            realEstate.Area = dto.Area;
            realEstate.Location = dto.Location;
            realEstate.RoomNumber = dto.RoomNumber;
            realEstate.BuildingType = dto.BuildingType;
            realEstate.CreatedAt = DateTime.Now;
            realEstate.ModifiedAt = DateTime.Now;

            if (dto.Files != null)
            {
                 await _fileServices.UploadFilesToDatabase(dto, realEstate);
            }

            _context.Realestate.Add(realEstate);
            await _context.SaveChangesAsync();


            return realEstate;

        }

        public async Task<Realestate> Update(RealestateDto dto)
        {
            var domain = await _context.Realestate
                .SingleOrDefaultAsync(x => x.Id == dto.Id);

            if (domain == null)
            {
                return null;
            }

            domain.Area = dto.Area;
            domain.Location = dto.Location;
            domain.RoomNumber = dto.RoomNumber;
            domain.BuildingType = dto.BuildingType;
            domain.ModifiedAt = DateTime.Now;

            if (dto.Files != null)
            {

                await _fileServices.UploadFilesToDatabase(dto, domain);
            }

            _context.Realestate.Update(domain);
            await _context.SaveChangesAsync();

            return domain;
        }

        public async Task<Realestate> DetailAsync(Guid id)
        {
            var realestate = await _context.Realestate
                .FirstOrDefaultAsync(x => x.Id == id);

            return realestate;
        }

        public async Task<Realestate> Delete(Guid id)
        {
            var result = await _context.Realestate
                .FirstOrDefaultAsync(x => x.Id == id);

            var images = await _context.FileToDatabases
                .Where(x => x.RealEstateId == id)
                .Select(y => new FileToDatabaseDto
                {
                    Id = y.Id
                }).ToArrayAsync();

            foreach(var image in images) { 
            await _fileServices.RemoveImageFromDatabase(image);
            }
            _context.Realestate.Remove(result);
            await _context.SaveChangesAsync();

            return result;
        }
        public async Task<FileToDatabase> RemoveImageFromDatabase(FileToDatabaseDto dto)
        {
            // Kutsub otse sinu FileServices failis olevat kustutamismeetodit
            var result = await _fileServices.RemoveImageFromDatabase(dto);
            return result;
        }

    }
}
