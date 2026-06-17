using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class RifaDbContext : DbContext
{
    public RifaDbContext(DbContextOptions<RifaDbContext> options) : base(options)
    {
    }

    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Talonario> Talonarios => Set<Talonario>();
    public DbSet<Boleta> Boletas => Set<Boleta>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(RifaDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
