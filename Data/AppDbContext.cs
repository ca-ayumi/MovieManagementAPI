//Movie: Representa a entidade de filmes. Possui uma relação de muitos-para-um com Genre, indicando que um filme pertence a um gênero.
//Genre: Representa a entidade de gêneros. Cada gênero pode estar associado a muitos filmes.
//Rental: Representa a entidade de locações. Possui uma relação de muitos-para-muitos com Movie, indicando que uma locação pode incluir vários filmes e um filme pode ser parte de várias locações.


using Microsoft.EntityFrameworkCore;
using MovieManagementAPI.Models;

namespace MovieManagementAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // DbSets para as entidades
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Rental> Rentals { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Mapeamento para a entidade Movie
             modelBuilder.Entity<Movie>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Active).IsRequired();
                entity.HasOne(e => e.Genre) // Relação de muitos para um com Genre
                    .WithMany(g => g.Movies)
                    .HasForeignKey(e => e.GenreId); // Use GenreId como chave estrangeira
            });

            // Mapeamento para a entidade Genre
            modelBuilder.Entity<Genre>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Active).IsRequired();
            });

            // Mapeamento para a entidade Rental
            modelBuilder.Entity<Rental>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.CustomerCPF).IsRequired().HasMaxLength(14);
                entity.Property(e => e.RentalDate).IsRequired();
                entity.HasMany(e => e.Movies) // Relação de muitos para muitos com Movie
                    .WithMany(m => m.Rentals);
            });
        }
    }
}