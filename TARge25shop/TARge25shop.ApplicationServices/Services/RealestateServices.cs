using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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

        public RealestateServices(TARge25ShopContext context)
        {
            _context = context;
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
            domain.CreatedAt = DateTime.Now;
            domain.ModifiedAt = DateTime.Now;

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

            _context.Realestate.Remove(result);
            await _context.SaveChangesAsync();

            return result;
        }
    }
}
