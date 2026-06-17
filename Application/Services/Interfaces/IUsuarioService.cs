using Application.DTOs.Usuarios;

namespace Application.Services.Interfaces;

public interface IUsuarioService
{
    Task<UsuarioDto?> GetByIdAsync(long id, CancellationToken ct = default);
    Task<IReadOnlyList<UsuarioDto>> GetAllAsync(CancellationToken ct = default);
    Task<UsuarioDto?> GetByEmailAsync(string email, CancellationToken ct = default);
    Task<UsuarioDto> CreateAsync(CreateUsuarioDto dto, CancellationToken ct = default);
    Task UpdateAsync(long id, UpdateUsuarioDto dto, CancellationToken ct = default);
    Task DeleteAsync(long id, CancellationToken ct = default);
}
