using Domain.Entities;

namespace Domain.Interfaces;

public interface IBoletaRepository : IRepository<Boleta>
{
    Task<IReadOnlyList<Boleta>> GetByTalonarioIdAsync(long talonarioId, CancellationToken ct = default);
    Task<Boleta?> GetByNumberAsync(long talonarioId, int number, CancellationToken ct = default);
}
