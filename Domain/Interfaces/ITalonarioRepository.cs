using Domain.Entities;

namespace Domain.Interfaces;

public interface ITalonarioRepository : IRepository<Talonario>
{
    Task<IReadOnlyList<Talonario>> GetByUserIdAsync(long userId, CancellationToken ct = default);
    Task<Talonario?> GetWithBoletasAsync(long id, CancellationToken ct = default);
}
