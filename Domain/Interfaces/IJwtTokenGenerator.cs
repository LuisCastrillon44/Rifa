using Domain.Entities;

namespace Domain.Interfaces;

public record GeneratedToken(string Token, DateTime ExpiresAtUtc);

public interface IJwtTokenGenerator
{
    GeneratedToken Generate(Usuario usuario);
}
