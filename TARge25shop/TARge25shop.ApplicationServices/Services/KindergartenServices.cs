using Microsoft.EntityFrameworkCore;
using TARge25shop.Core.Domain;
using TARge25shop.Core.Dto;
using TARge25shop.Core.ServiceInterface;
using TARge25shop.Data;

namespace TARge25shop.ApplicationServices.Services
{
    public class KindergartenServices : IKindergartenServices
    {
        private readonly TARge25ShopContext _context;
        private readonly IFileServices _fileServices;   // UUS

        public KindergartenServices(
            TARge25ShopContext context,
            IFileServices fileServices)                  // UUS
        {
            _context = context;
            _fileServices = fileServices;                // UUS
        }

        public async Task<Kindergarten> Create(KindergartenDto dto)
        {
            Kindergarten kindergarten = new();

            kindergarten.Id = Guid.NewGuid();
            kindergarten.GroupName = dto.GroupName;
            kindergarten.KindergartenName = dto.KindergartenName;
            kindergarten.ChildrenCount = dto.ChildrenCount;
            kindergarten.TeacherName = dto.TeacherName;
            kindergarten.CreatedAt = DateTime.Now;
            kindergarten.UpdatedAt = DateTime.Now;

            _context.Kindergarten.Add(kindergarten);
            await _context.SaveChangesAsync();

            // UUS: pildid andmebaasi
            await _fileServices.UploadFilesToDatabase(dto, kindergarten);

            return kindergarten;
        }

        public async Task<Kindergarten> Update(KindergartenDto dto)
        {
            var domain = await _context.Kindergarten
                .SingleOrDefaultAsync(x => x.Id == dto.Id);

            if (domain == null)
            {
                return null;
            }

            domain.GroupName = dto.GroupName;
            domain.KindergartenName = dto.KindergartenName;
            domain.ChildrenCount = dto.ChildrenCount;
            domain.TeacherName = dto.TeacherName;
            domain.UpdatedAt = DateTime.Now;

            _context.Kindergarten.Update(domain);
            await _context.SaveChangesAsync();

            // UUS: uued pildid andmebaasi
            await _fileServices.UploadFilesToDatabase(dto, domain);

            return domain;
        }

        public async Task<Kindergarten> DetailAsync(Guid id)
        {
            var kindergarten = await _context.Kindergarten
                .FirstOrDefaultAsync(x => x.Id == id);

            return kindergarten;
        }

        public async Task<Kindergarten> Delete(Guid id)
        {
            var result = await _context.Kindergarten
                .FirstOrDefaultAsync(x => x.Id == id);

            if (result == null)
            {
                return null;
            }

            // UUS: kõigepealt selle lasteaia pildid
            var images = await _context.FileToDatabases
                .Where(x => x.KindergartenId == id)
                .Select(x => new FileToDatabaseDto
                {
                    Id = x.Id,
                    KindergartenId = x.KindergartenId
                })
                .ToArrayAsync();

            if (images.Length > 0)
            {
                await _fileServices.RemoveImagesFromDatabase(images);
            }

            _context.Kindergarten.Remove(result);
            await _context.SaveChangesAsync();

            return result;
        }
    }
}