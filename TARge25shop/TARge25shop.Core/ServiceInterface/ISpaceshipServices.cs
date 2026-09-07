using TARge25shop.Core.Dto;
using TARge25shop.Core.Domain;

namespace TARge25shop.Core.ServiceInterface
{
    public interface ISpaceshipServices
    {
        Task<Spaceship> Create(SpaceshipDto dto);
    }
}
