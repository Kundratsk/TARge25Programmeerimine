using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TARge25shop.Core.Domain;

namespace TARge25shop.Data
{
    public class TARge25ShopContext : IdentityDbContext<ApplicationUser>
    {
        public TARge25ShopContext(DbContextOptions<TARge25ShopContext> options)
            : base(options)
        {
        }

        public DbSet<Spaceship> Spaceships { get; set; }
        public DbSet<Kindergarten> Kindergarten { get; set; }
        public DbSet<FileToApi> FileToApis { get; set; }
        public DbSet<Realestate> Realestate { get; set; }
        public DbSet<FileToDatabase> FileToDatabases { get; set; }
    }
}
