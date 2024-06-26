using Microsoft.EntityFrameworkCore;
using SistemaHoteis.Models;

namespace SistemaHoteis.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Hotel> Hoteis { get; set; } = null!;
    public DbSet<Hospede> Hospedes { get; set; } = null!;
    public DbSet<CheckIn> Checkins { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Hotel>()
                    .HasMany(c => c.Checkins)
                    .WithOne(h => h.Hotel)
                    .HasForeignKey(fg => fg.HotelId);

        modelBuilder.Entity<Hospede>()
                    .HasMany(c => c.Checkins)
                    .WithOne(h => h.Hospede)
                    .HasForeignKey(fg => fg.HospedeId);

        modelBuilder.Entity<CheckIn>()
                    .HasIndex(c => new { c.Id, c.HotelId })
                    .IsUnique();
    }
}
