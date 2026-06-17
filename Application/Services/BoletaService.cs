using Application.Common.Exceptions;
using Application.DTOs.Boletas;
using Application.Mappings;
using Application.Services.Interfaces;
using Domain.Interfaces;
using FluentValidation;

namespace Application.Services;

public class BoletaService : IBoletaService
{
    private readonly IBoletaRepository _repo;
    private readonly IUnitOfWork _uow;
    private readonly IValidator<CreateBoletaDto> _createValidator;
    private readonly IValidator<UpdateBoletaDto> _updateValidator;

    public BoletaService(
        IBoletaRepository repo,
        IUnitOfWork uow,
        IValidator<CreateBoletaDto> createValidator,
        IValidator<UpdateBoletaDto> updateValidator)
    {
        _repo = repo;
        _uow = uow;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    public async Task<BoletaDto?> GetByIdAsync(long id, CancellationToken ct = default)
    {
        var e = await _repo.GetByIdAsync(id, ct);
        return e?.ToDto();
    }

    public async Task<IReadOnlyList<BoletaDto>> GetAllAsync(CancellationToken ct = default)
    {
        var list = await _repo.GetAllAsync(ct);
        return list.Select(e => e.ToDto()).ToList();
    }

    public async Task<IReadOnlyList<BoletaDto>> GetByTalonarioIdAsync(long talonarioId, CancellationToken ct = default)
    {
        var list = await _repo.GetByTalonarioIdAsync(talonarioId, ct);
        return list.Select(e => e.ToDto()).ToList();
    }

    public async Task<BoletaDto?> GetByNumberAsync(long talonarioId, int number, CancellationToken ct = default)
    {
        var e = await _repo.GetByNumberAsync(talonarioId, number, ct);
        return e?.ToDto();
    }

    public async Task<BoletaDto> CreateAsync(CreateBoletaDto dto, CancellationToken ct = default)
    {
        await _createValidator.ValidateAndThrowAsync(dto, ct);
        var entity = dto.ToEntity();
        await _repo.AddAsync(entity, ct);
        await _uow.SaveChangesAsync(ct);
        return entity.ToDto();
    }

    public async Task UpdateAsync(long id, UpdateBoletaDto dto, CancellationToken ct = default)
    {
        await _updateValidator.ValidateAndThrowAsync(dto, ct);
        var entity = await _repo.GetByIdAsync(id, ct)
            ?? throw new NotFoundException(nameof(Domain.Entities.Boleta), id);
        dto.ApplyTo(entity);
        _repo.Update(entity);
        await _uow.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(long id, CancellationToken ct = default)
    {
        var entity = await _repo.GetByIdAsync(id, ct)
            ?? throw new NotFoundException(nameof(Domain.Entities.Boleta), id);
        _repo.Delete(entity);
        await _uow.SaveChangesAsync(ct);
    }
}
