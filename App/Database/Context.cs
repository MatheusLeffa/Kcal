using Kcal.App.Models;
using Microsoft.EntityFrameworkCore;

namespace Kcal.App.Database;

public class Context(DbContextOptions options) : DbContext(options)
{
    public DbSet<Product> Products { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Product>()
            .HasMany(product => product.Users)
            .WithMany(users => users.Products)
            .UsingEntity<ConsumedProducts>();

        modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();
    }
}