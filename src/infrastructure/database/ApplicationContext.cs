using Kcal.src.modules.consumedProducts.domain.models;
using Kcal.src.modules.product.domain.models;
using Kcal.src.modules.user.domain.models;
using Microsoft.EntityFrameworkCore;

namespace Kcal.src.infrastructure.database;

public class ApplicationContext(DbContextOptions options) : DbContext(options)
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
            Peso = 80,
            Email = "matheus@example.com"
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

        // Data initialization
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