using Kcal.App.Models;
using Microsoft.EntityFrameworkCore;

namespace Kcal.App.Database;

public class Context(DbContextOptions options) : DbContext(options)
{
    public DbSet<Product> Products { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<ConsumedProducts> ConsumedProducts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Product>()
            .HasMany(product => product.Users)
            .WithMany(users => users.Products)
            .UsingEntity<ConsumedProducts>();

        User user = new()
        {
            Id = Guid.NewGuid(),
            Name = "Matheus",
            DataNascimento = DateTime.UtcNow,
            Sexo = "M",
            Altura = 180,
            Peso = 80
        };

        Product product1 = new()
        {
            Id = Guid.NewGuid(),
            Nome = "Laranja",
            Marca = "Natural",
            Gramas = 50,
            Kcal = 20,
            Categoria = "Fruta",
            DataCadastro = DateTime.UtcNow
        };

        Product product2 = new()
        {
            Id = Guid.NewGuid(),
            Nome = "Arroz",
            Marca = "Namorado",
            Gramas = 100,
            Kcal = 80,
            Categoria = "Grãos",
            DataCadastro = DateTime.UtcNow
        };

        modelBuilder.Entity<User>().HasData(user);
        modelBuilder.Entity<Product>().HasData(product1);
        modelBuilder.Entity<Product>().HasData(product2);

        modelBuilder.Entity<ConsumedProducts>()
            .HasData(new ConsumedProducts
            {
                UserId = user.Id,
                ProductId = product1.Id,
                DataConsumo = DateTime.UtcNow,
                Quantidade = 1
            },
            new ConsumedProducts
            {
                UserId = user.Id,
                ProductId = product2.Id,
                DataConsumo = DateTime.UtcNow,
                Quantidade = 2
            });
    }
}