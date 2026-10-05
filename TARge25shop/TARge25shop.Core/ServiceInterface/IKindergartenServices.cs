using TARge25shop.Core.Domain;
using TARge25shop.Core.Dto;

namespace TARge25shop.Core.ServiceInterface
{
    public interface IKindergartenServices
    {
        Task<Kindergarten> Create(KindergartenDto dto);
        Task<Kindergarten> Update(KindergartenDto dto);
        Task<Kindergarten> DetailAsync(Guid id);
        Task<Kindergarten> Delete(Guid id);
    }
}