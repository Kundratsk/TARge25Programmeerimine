using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;
using TARge25shop.Core.Domain;
using TARge25shop.Core.Dto;
using TARge25shop.Core.ServiceInterface;
using TARge25shop.Data;


namespace TARge25shop.ApplicationServices.Services
{
    public class SpaceshipServices : ISpaceshipServices
    {

        private readonly TARge25ShopContext _context;

        public SpaceshipServices(TARge25ShopContext context)
        {
            _context = context;
        }
    
        //see meetod on vaja controlleris esile kutsuda
        //peab lisama interface, et kutsuda see meetod välja
        public async Task<Spaceship> Create(SpaceshipDto dto)
        {
            //Siin on vaheinstans dto ja domain vahel, et andmed liiguvad dto-st domain objekt

            Spaceship spaceShip = new();

            spaceShip.Id = Guid.NewGuid();
            spaceShip.Name = dto.Name;
            spaceShip.ShipType = dto.ShipType;
            spaceShip.Crew = dto.Crew;
            spaceShip.EnginePower = dto.EnginePower;
            spaceShip.CreatedAt = DateTime.Now;
            spaceShip.UpdatedAt = DateTime.Now;

            //andmete salvestamine andmebaasi

            _context.Spaceships.Add(spaceShip);
            await _context.SaveChangesAsync();


            return spaceShip;
        }
        public async Task<Spaceship> Update(SpaceshipDto dto)
        {
            var domain = await _context.Spaceships
                .SingleOrDefaultAsync(x => x.Id == dto.Id);

            if (domain == null)
            {
                return null;
            }

            domain.Name = dto.Name;
            domain.ShipType = dto.ShipType;
            domain.Crew = dto.Crew;
            domain.EnginePower = dto.EnginePower;
            domain.UpdatedAt = DateTime.Now;
        
        _context.Spaceships.Update(domain);
        await _context.SaveChangesAsync();

        return domain;
        }
        public async Task<Spaceship> DetailAsync(Guid id)
        {
            var spaceship = await _context.Spaceships
                .FirstOrDefaultAsync(x => x.Id == id);

            return spaceship;
        }
        public async Task<Spaceship> Delete(Guid id)
        {
            var result = await _context.Spaceships
                .FirstOrDefaultAsync(x => x.Id == id);

            _context.Spaceships.Remove(result);
            await _context.SaveChangesAsync();

            return result;
        }
    }
}
