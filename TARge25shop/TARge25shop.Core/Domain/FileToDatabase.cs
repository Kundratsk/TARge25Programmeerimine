
using System;

namespace TARge25shop.Core.Domain
{
    public class FileToDatabase
    {
        public Guid Id { get; set; }

        public string? ImageTitle { get; set; }

        public byte[]? ImageData { get; set; }

        public Guid? RealEstateId { get; set; }

        public Guid? KindergartenId { get; set; }

        public Kindergarten? Kindergarten { get; set; }
    }
}
