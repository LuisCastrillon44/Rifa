using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class TalonarioRepository : Repository<Talonario>, ITalonarioRepository
{
    public TalonarioRepository(RifaDbContext context) : base(context)
    {
    }

    public async Task<IReadOnlyList<Talonario>> GetByUserIdAsync(long userId, CancellationToken ct = default)
        => await Set.AsNoTracking().Where(t => t.UserId == userId).ToListAsync(ct);

    public async Task<Talonario?> GetWithBoletasAsync(long id, CancellationToken ct = default)
        => await Set.Include(t => t.Boletas).FirstOrDefaultAsync(t => t.Id == id, ct);
}
