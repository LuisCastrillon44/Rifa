namespace Application.DTOs.Usuarios;

public record UsuarioDto(
    long Id,
    string? Name,
    string Email,
    string? Phone,
    DateTime CreatedAt,
    DateTime UpdatedAt);

public record CreateUsuarioDto(
    string? Name,
    string Email,
    string Password,
    string? Phone);

public record UpdateUsuarioDto(
    string? Name,
    string? Phone);
