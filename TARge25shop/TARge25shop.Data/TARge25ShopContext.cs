using Microsoft.EntityFrameworkCore;
using TARge25shop.Core.Domain;

namespace TARge25shop.Data
{
    //Nimetasime classi TARge25ShopContext, mis prärib DbContext klassi
    public class TARge25ShopContext : DbContext
    {
        //All on konstruktor, mis pärib DbContext klassi
        public DbSet<Spaceship> Spaceships { get; set; }

        public DbSet<FileToApi> FileToApis { get; set; }

        public DbSet<Kindergarten> Kindergarten { get; set; }
    }
}
