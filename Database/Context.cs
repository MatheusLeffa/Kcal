using Kcal.Models;
using Microsoft.EntityFrameworkCore;

namespace Kcal.Database;

public class Context(DbContextOptions options) : DbContext(options)
{

    public DbSet<Product> Products { get; set; }
    public DbSet<User> Users{ get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Product>()
            .HasMany(p => p.Users)
            .WithMany(p => p.Products)
            .UsingEntity<ConsumedProducts>();
        
    }
}