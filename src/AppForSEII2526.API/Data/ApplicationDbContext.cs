using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AppForSEII2526.API.Models;


namespace AppForSEII2526.API.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {

        base.OnModelCreating(builder);

        builder.Entity<AlquilerItem>().HasKey(pi => new { pi.CocheId, pi.AlquilerId });
        builder.Entity<ComprarItem>().HasKey(pi => new { pi.CocheId, pi.ComprarId });
        builder.Entity<ReseñarItem>().HasKey(pi => new { pi.CocheId, pi.ReseñarId});
    }

    public DbSet<Modelo> Modelos { get; set; }
    public DbSet<Coche> Coches { get; set; }
    public DbSet<Alquiler> Alquileres { get; set; }
    public DbSet<ApplicationUser> ApplicationUsers { get; set; }

    public DbSet<Comprar> Compras { get; set; }
    public DbSet<Reseñar> Reseñas { get; set; }
}
