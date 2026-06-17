using Application.DTOs.Usuarios;
using Domain.Entities;

namespace Application.Mappings;

public static class UsuarioMappings
{
    public static UsuarioDto ToDto(this Usuario e)
        => new(e.Id, e.Name, e.Email, e.Phone, e.CreatedAt, e.UpdatedAt);

    public static Usuario ToEntity(this CreateUsuarioDto dto, string passwordHash)
        => new()
        {
            Name = dto.Name,
            Email = dto.Email,
            PasswordHash = passwordHash,
            Phone = dto.Phone,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

    public static void ApplyTo(this UpdateUsuarioDto dto, Usuario e)
    {
        e.Name = dto.Name;
        e.Phone = dto.Phone;
        e.UpdatedAt = DateTime.UtcNow;
    }
}
