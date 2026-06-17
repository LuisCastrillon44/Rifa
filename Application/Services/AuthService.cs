using Application.Common.Exceptions;
using Application.DTOs.Auth;
using Application.DTOs.Usuarios;
using Application.Mappings;
using Application.Services.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using FluentValidation;

namespace Application.Services;

public class AuthService : IAuthService
{
    private readonly IUsuarioRepository _repo;
    private readonly IUnitOfWork _uow;
    private readonly IPasswordHasher _hasher;
    private readonly IJwtTokenGenerator _tokenGenerator;
    private readonly IValidator<CreateUsuarioDto> _registerValidator;
    private readonly IValidator<LoginDto> _loginValidator;

    public AuthService(
        IUsuarioRepository repo,
        IUnitOfWork uow,
        IPasswordHasher hasher,
        IJwtTokenGenerator tokenGenerator,
        IValidator<CreateUsuarioDto> registerValidator,
        IValidator<LoginDto> loginValidator)
    {
        _repo = repo;
        _uow = uow;
        _hasher = hasher;
        _tokenGenerator = tokenGenerator;
        _registerValidator = registerValidator;
        _loginValidator = loginValidator;
    }

    public async Task<AuthResponseDto> RegisterAsync(CreateUsuarioDto dto, CancellationToken ct = default)
    {
        await _registerValidator.ValidateAndThrowAsync(dto, ct);

        var existing = await _repo.GetByEmailAsync(dto.Email, ct);
        if (existing is not null)
            throw new ConflictException($"Ya existe un usuario con el email '{dto.Email}'.");

        var entity = dto.ToEntity(_hasher.Hash(dto.Password));
        await _repo.AddAsync(entity, ct);
        await _uow.SaveChangesAsync(ct);

        return BuildResponse(entity);
    }

    public async Task<AuthResponseDto> LoginAsync(LoginDto dto, CancellationToken ct = default)
    {
        await _loginValidator.ValidateAndThrowAsync(dto, ct);

        var user = await _repo.GetByEmailAsync(dto.Email, ct);
        if (user is null || !_hasher.Verify(dto.Password, user.PasswordHash))
            throw new UnauthorizedException("Credenciales invalidas.");

        return BuildResponse(user);
    }

    private AuthResponseDto BuildResponse(Usuario user)
    {
        var token = _tokenGenerator.Generate(user);
        return new AuthResponseDto(token.Token, token.ExpiresAtUtc, user.ToDto());
    }
}
