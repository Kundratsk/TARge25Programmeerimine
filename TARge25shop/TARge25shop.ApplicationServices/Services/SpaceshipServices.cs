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
    }
}
