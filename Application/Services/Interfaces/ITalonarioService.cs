using Application.DTOs.Talonarios;

namespace Application.Services.Interfaces;

public interface ITalonarioService
{
    Task<TalonarioDto?> GetByIdAsync(long id, CancellationToken ct = default);
    Task<IReadOnlyList<TalonarioDto>> GetAllAsync(CancellationToken ct = default);
    Task<IReadOnlyList<TalonarioDto>> GetByUserIdAsync(long userId, CancellationToken ct = default);
    Task<TalonarioDto?> GetWithBoletasAsync(long id, CancellationToken ct = default);
    Task<TalonarioDto> CreateAsync(CreateTalonarioDto dto, CancellationToken ct = default);
    Task UpdateAsync(long id, UpdateTalonarioDto dto, CancellationToken ct = default);
    Task DeleteAsync(long id, CancellationToken ct = default);
}
