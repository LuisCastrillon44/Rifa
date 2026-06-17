using Application.Common.Exceptions;
using Application.DTOs.Talonarios;
using Application.Mappings;
using Application.Services.Interfaces;
using Domain.Interfaces;
using FluentValidation;

namespace Application.Services;

public class TalonarioService : ITalonarioService
{
    private readonly ITalonarioRepository _repo;
    private readonly IUnitOfWork _uow;
    private readonly IValidator<CreateTalonarioDto> _createValidator;
    private readonly IValidator<UpdateTalonarioDto> _updateValidator;

    public TalonarioService(
        ITalonarioRepository repo,
        IUnitOfWork uow,
        IValidator<CreateTalonarioDto> createValidator,
        IValidator<UpdateTalonarioDto> updateValidator)
    {
        _repo = repo;
        _uow = uow;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    public async Task<TalonarioDto?> GetByIdAsync(long id, CancellationToken ct = default)
    {
        var e = await _repo.GetByIdAsync(id, ct);
        return e?.ToDto();
    }

    public async Task<IReadOnlyList<TalonarioDto>> GetAllAsync(CancellationToken ct = default)
    {
        var list = await _repo.GetAllAsync(ct);
        return list.Select(e => e.ToDto()).ToList();
    }

    public async Task<IReadOnlyList<TalonarioDto>> GetByUserIdAsync(long userId, CancellationToken ct = default)
    {
        var list = await _repo.GetByUserIdAsync(userId, ct);
        return list.Select(e => e.ToDto()).ToList();
    }

    public async Task<TalonarioDto?> GetWithBoletasAsync(long id, CancellationToken ct = default)
    {
        var e = await _repo.GetWithBoletasAsync(id, ct);
        return e?.ToDto();
    }

    public async Task<TalonarioDto> CreateAsync(CreateTalonarioDto dto, CancellationToken ct = default)
    {
        await _createValidator.ValidateAndThrowAsync(dto, ct);
        var entity = dto.ToEntity();
        await _repo.AddAsync(entity, ct);
        await _uow.SaveChangesAsync(ct);
        return entity.ToDto();
    }

    public async Task UpdateAsync(long id, UpdateTalonarioDto dto, CancellationToken ct = default)
    {
        await _updateValidator.ValidateAndThrowAsync(dto, ct);
        var entity = await _repo.GetByIdAsync(id, ct)
            ?? throw new NotFoundException(nameof(Domain.Entities.Talonario), id);
        dto.ApplyTo(entity);
        _repo.Update(entity);
        await _uow.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(long id, CancellationToken ct = default)
    {
        var entity = await _repo.GetByIdAsync(id, ct)
            ?? throw new NotFoundException(nameof(Domain.Entities.Talonario), id);
        _repo.Delete(entity);
        await _uow.SaveChangesAsync(ct);
    }
}
