using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Plukliste.Model.Entity;

namespace Plukliste.Data;

public class PluklistDbContext : DbContext
{
    public DbSet<Pluklist> Pluklist { get; set; }
    public DbSet<Item> Item { get; set; }

    public PluklistDbContext(DbContextOptions<PluklistDbContext> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Item>()
                .ToTable("Item")
                .HasKey(x => x.Id);

            modelBuilder.Entity<Pluklist>()
                .ToTable("Pluklist")
                .HasKey(x => x.Id);


            modelBuilder.Entity<Item>()
                .Property(c => c.ItemType)
                .HasConversion<byte>();

            modelBuilder.Entity<Item>()
                .Ignore(x => x.StockAmount);

            base.OnModelCreating(modelBuilder);
        }
}
