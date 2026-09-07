using Microsoft.EntityFrameworkCore;
using TARge25shop.Core.Domain;

namespace TARge25shop.Data
{
    //Nimetasime classi TARge25ShopContext, mis prärib DbContext klassi
    public class TARge25ShopContext : DbContext
    {
        //All on konstruktor, mis pärib DbContext klassi
        public TARge25ShopContext (DbContextOptions<TARge25ShopContext> options) 
            : base(options){ }


        //vaja lisada dbSet, mis on seotud meie domain klassiga SpaceShip
        public DbSet<Spaceship> Spaceships { get; set; }
    }
}
