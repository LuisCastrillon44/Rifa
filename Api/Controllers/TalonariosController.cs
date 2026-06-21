using Application.DTOs.Talonarios;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TalonariosController : ControllerBase
{
    private readonly ITalonarioService _service;

    public TalonariosController(ITalonarioService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<TalonarioDto>>> GetAll(CancellationToken ct)
        => Ok(await _service.GetAllAsync(ct));

    [HttpGet("{id:long}")]
    public async Task<ActionResult<TalonarioDto>> GetById(long id, CancellationToken ct)
    {
        var dto = await _service.GetByIdAsync(id, ct);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpGet("{id:long}/boletas")]
    public async Task<ActionResult<TalonarioDto>> GetWithBoletas(long id, CancellationToken ct)
    {
        var dto = await _service.GetWithBoletasAsync(id, ct);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpGet("by-user/{userId:long}")]
    public async Task<ActionResult<IReadOnlyList<TalonarioDto>>> GetByUser(long userId, CancellationToken ct)
        => Ok(await _service.GetByUserIdAsync(userId, ct));

    [HttpPost]
    public async Task<ActionResult<TalonarioDto>> Create(CreateTalonarioDto dto, CancellationToken ct)
    {
        var created = await _service.CreateAsync(dto, ct);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:long}")]
    public async Task<IActionResult> Update(long id, UpdateTalonarioDto dto, CancellationToken ct)
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
