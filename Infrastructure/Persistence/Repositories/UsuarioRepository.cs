using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class UsuarioRepository : Repository<Usuario>, IUsuarioRepository
{
    public UsuarioRepository(RifaDbContext context) : base(context)
    {
    }

    public async Task<Usuario?> GetByEmailAsync(string email, CancellationToken ct = default)
        => await Set.AsNoTracking().FirstOrDefaultAsync(u => u.Email == email, ct);
}
