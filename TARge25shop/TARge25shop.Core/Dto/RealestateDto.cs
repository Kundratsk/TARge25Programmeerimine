using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace TARge25shop.Core.Dto
{
    public class RealestateDto
    {
        public Guid? Id { get; set; }
        public double? Area { get; set; }
        public string Location { get; set; }
        public int RoomNumber { get; set; }
        public string BuildingType { get; set; }
        public List<IFormFile> Files { get; set; }
        public IEnumerable<FileToDatabaseDto> Image { get; set; }
            = new List<FileToDatabaseDto>();
        public DateTime? CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }

    }
}
