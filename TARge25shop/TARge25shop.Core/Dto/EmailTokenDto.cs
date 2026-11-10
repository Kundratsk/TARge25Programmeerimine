using System;
using System.Collections.Generic;
using System.Text;

namespace TARge25shop.Core.Dto
{
    public class EmailTokenDto: EmailDto
    {
        public string Token { get; set; } = string.Empty;
    }
}
