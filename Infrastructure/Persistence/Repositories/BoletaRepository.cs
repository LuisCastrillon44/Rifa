using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class BoletaRepository : Repository<Boleta>, IBoletaRepository
{
    public BoletaRepository(RifaDbContext context) : base(context)
    {
    }

    public async Task<IReadOnlyList<Boleta>> GetByTalonarioIdAsync(long talonarioId, CancellationToken ct = default)
        => await Set.AsNoTracking().Where(b => b.TalonarioId == talonarioId).ToListAsync(ct);

    public async Task<Boleta?> GetByNumberAsync(long talonarioId, int number, CancellationToken ct = default)
        => await Set.AsNoTracking().FirstOrDefaultAsync(b => b.TalonarioId == talonarioId && b.Number == number, ct);
}
