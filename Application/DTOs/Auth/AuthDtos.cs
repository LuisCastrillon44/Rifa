using Application.DTOs.Usuarios;

namespace Application.DTOs.Auth;

public record LoginDto(
    string Email,
    string Password);

public record AuthResponseDto(
    string Token,
    DateTime ExpiresAtUtc,
    UsuarioDto User);
