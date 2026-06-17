using Application.DTOs.Boletas;

namespace Application.Services.Interfaces;

public interface IBoletaService
{
    Task<BoletaDto?> GetByIdAsync(long id, CancellationToken ct = default);
    Task<IReadOnlyList<BoletaDto>> GetAllAsync(CancellationToken ct = default);
    Task<IReadOnlyList<BoletaDto>> GetByTalonarioIdAsync(long talonarioId, CancellationToken ct = default);
    Task<BoletaDto?> GetByNumberAsync(long talonarioId, int number, CancellationToken ct = default);
    Task<BoletaDto> CreateAsync(CreateBoletaDto dto, CancellationToken ct = default);
    Task UpdateAsync(long id, UpdateBoletaDto dto, CancellationToken ct = default);
    Task DeleteAsync(long id, CancellationToken ct = default);
}
