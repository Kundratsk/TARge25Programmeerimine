using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using System;
using System.Collections.Generic;
using System.Text;


namespace TARge25shop.Core.Domain
{
    public class ApplicationUser : IdentityUser
    {
        public string City { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }
}
