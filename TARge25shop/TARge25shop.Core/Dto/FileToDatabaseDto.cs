using System;
using System.Collections.Generic;
using System.Text;

namespace TARge25shop.Core.Dto
{
    public class FileToDatabaseDto
    {
        public Guid Id  { get; set; }
        public string? ImageTitle { get; set; }
        public byte[]? ImageData { get; set; }
        public Guid? RealEstateId { get; set; }
        { get; set; }
            = new List<FileToDatabaseDto>();
    }
}
