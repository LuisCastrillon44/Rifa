using Application.Common.Exceptions;
using Application.DTOs.Usuarios;
using Application.Mappings;
using Application.Services.Interfaces;
using Domain.Interfaces;
using FluentValidation;

namespace Application.Services;

public class UsuarioService : IUsuarioService
{
    private readonly IUsuarioRepository _repo;
    private readonly IUnitOfWork _uow;
    private readonly IPasswordHasher _hasher;
    private readonly IValidator<CreateUsuarioDto> _createValidator;
    private readonly IValidator<UpdateUsuarioDto> _updateValidator;

    public UsuarioService(
        IUsuarioRepository repo,
        IUnitOfWork uow,
        IPasswordHasher hasher,
        IValidator<CreateUsuarioDto> createValidator,
        IValidator<UpdateUsuarioDto> updateValidator)
    {
        _repo = repo;
        _uow = uow;
        _hasher = hasher;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    public async Task<UsuarioDto?> GetByIdAsync(long id, CancellationToken ct = default)
    {
        var e = await _repo.GetByIdAsync(id, ct);
        return e?.ToDto();
    }

    public async Task<IReadOnlyList<UsuarioDto>> GetAllAsync(CancellationToken ct = default)
    {
        var list = await _repo.GetAllAsync(ct);
        return list.Select(e => e.ToDto()).ToList();
    }

    public async Task<UsuarioDto?> GetByEmailAsync(string email, CancellationToken ct = default)
    {
        var e = await _repo.GetByEmailAsync(email, ct);
        return e?.ToDto();
    }

    public async Task<UsuarioDto> CreateAsync(CreateUsuarioDto dto, CancellationToken ct = default)
    {
        await _createValidator.ValidateAndThrowAsync(dto, ct);
        var entity = dto.ToEntity(_hasher.Hash(dto.Password));
        await _repo.AddAsync(entity, ct);
        await _uow.SaveChangesAsync(ct);
        return entity.ToDto();
    }

    public async Task UpdateAsync(long id, UpdateUsuarioDto dto, CancellationToken ct = default)
    {
        await _updateValidator.ValidateAndThrowAsync(dto, ct);
        var entity = await _repo.GetByIdAsync(id, ct)
            ?? throw new NotFoundException(nameof(Domain.Entities.Usuario), id);
        dto.ApplyTo(entity);
        _repo.Update(entity);
        await _uow.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(long id, CancellationToken ct = default)
    {
        var entity = await _repo.GetByIdAsync(id, ct)
            ?? throw new NotFoundException(nameof(Domain.Entities.Usuario), id);
        _repo.Delete(entity);
        await _uow.SaveChangesAsync(ct);
    }
}
