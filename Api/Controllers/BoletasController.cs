using Application.DTOs.Boletas;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
// [Authorize] // TODO: reactivar cuando el front tenga login (JWT ya esta implementado)
public class BoletasController : ControllerBase
{
    private readonly IBoletaService _service;

    public BoletasController(IBoletaService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<BoletaDto>>> GetAll(CancellationToken ct)
        => Ok(await _service.GetAllAsync(ct));

    [HttpGet("{id:long}")]
    public async Task<ActionResult<BoletaDto>> GetById(long id, CancellationToken ct)
    {
        var dto = await _service.GetByIdAsync(id, ct);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpGet("by-talonario/{talonarioId:long}")]
    public async Task<ActionResult<IReadOnlyList<BoletaDto>>> GetByTalonario(long talonarioId, CancellationToken ct)
        => Ok(await _service.GetByTalonarioIdAsync(talonarioId, ct));

    [HttpGet("by-talonario/{talonarioId:long}/number/{number:int}")]
    public async Task<ActionResult<BoletaDto>> GetByNumber(long talonarioId, int number, CancellationToken ct)
    {
        var dto = await _service.GetByNumberAsync(talonarioId, number, ct);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPost]
    public async Task<ActionResult<BoletaDto>> Create(CreateBoletaDto dto, CancellationToken ct)
    {
        var created = await _service.CreateAsync(dto, ct);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:long}")]
    public async Task<IActionResult> Update(long id, UpdateBoletaDto dto, CancellationToken ct)
    {
        await _service.UpdateAsync(id, dto, ct);
        return NoContent();
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Delete(long id, CancellationToken ct)
    {
        await _service.DeleteAsync(id, ct);
        return NoContent();
    }
}
