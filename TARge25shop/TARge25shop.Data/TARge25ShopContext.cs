using Microsoft.EntityFrameworkCore;
using TARge25shop.Core.Domain;

namespace TARge25shop.Data
{
    // Nimetasime classi TARge25ShopContext, mis pärib DbContext klassi
    public class TARge25ShopContext : DbContext
    {
        // 1. KONSTRUKTOR PEAB ASUMA SIIN (klassi sees)
        public TARge25ShopContext(DbContextOptions<TARge25ShopContext> options)
            : base(options)
        {
        }

        // 2. Seejärel tulevad sinu DbSet tabelid
        public DbSet<Spaceship> Spaceships { get; set; }

        public DbSet<FileToApi> FileToApis { get; set; }

        public DbSet<Kindergarten> Kindergarten { get; set; }
    }
}
