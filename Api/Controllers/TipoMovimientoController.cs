using Api.DTOs.TiposMovimiento;
using Application.Abstractions;
using AutoMapper;
using Domain.Entities;
using Domain.ValueObjects;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public sealed class TipoMovimientoController : BaseApiController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public TipoMovimientoController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    // ✅ GET: api/TipoMovimiento
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TipoMovimientoResponseDto>>> GetAllAsync(CancellationToken ct)
    {
        var tipos = await _unitOfWork.TipoMovimiento.GetAllAsync(ct);
        var result = _mapper.Map<IEnumerable<TipoMovimientoResponseDto>>(tipos);
        return Ok(result);
    }

    // ✅ GET: api/TipoMovimiento/{id}
    [HttpGet("{id:int}")]
    public async Task<ActionResult<TipoMovimientoResponseDto>> GetByIdAsync(int id, CancellationToken ct)
    {
        var tipo = await _unitOfWork.TipoMovimiento.GetByIdAsync(new IdVO(id), ct);
        if (tipo is null)
            return NotFound($"No se encontró el tipo de movimiento con ID {id}.");

        var result = _mapper.Map<TipoMovimientoResponseDto>(tipo);
        return Ok(result);
    }

    // ✅ POST: api/TipoMovimiento
    [HttpPost]
    public async Task<ActionResult<TipoMovimientoResponseDto>> CreateAsync([FromBody] CreateTipoMovimientoDto dto, CancellationToken ct)
    {
        // Validar duplicado
        var existing = await _unitOfWork.TipoMovimiento.GetByNombreAsync(new NombreVO(dto.Nombre), ct);
        if (existing is not null)
            return Conflict($"Ya existe un tipo de movimiento con el nombre '{dto.Nombre}'.");

        var tipo = new TipoMovimiento(
            new IdVO(0),
            new NombreVO(dto.Nombre)
        );

        await _unitOfWork.TipoMovimiento.AddAsync(tipo, ct);
        await _unitOfWork.SaveChanges(ct);

        var result = _mapper.Map<TipoMovimientoResponseDto>(tipo);
        return CreatedAtAction(nameof(GetByIdAsync), new { id = tipo.Id.Value }, result);
    }

    // ✅ PUT: api/TipoMovimiento/{id}
    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateAsync(int id, [FromBody] UpdateTipoMovimientoDto dto, CancellationToken ct)
    {
        var existing = await _unitOfWork.TipoMovimiento.GetByIdAsync(new IdVO(id), ct);
        if (existing is null)
            return NotFound($"No se encontró el tipo de movimiento con ID {id}.");

        existing.Nombre = new NombreVO(dto.Nombre);

        var updated = await _unitOfWork.TipoMovimiento.UpdateAsync(existing, ct);
        if (!updated)
            return BadRequest("No se pudo actualizar el tipo de movimiento.");

        await _unitOfWork.SaveChanges(ct);
        return NoContent();
    }

    // ✅ DELETE: api/TipoMovimiento/{id}
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteAsync(int id, CancellationToken ct)
    {
        var deleted = await _unitOfWork.TipoMovimiento.DeleteAsync(new IdVO(id), ct);
        if (!deleted)
            return NotFound($"No se encontró el tipo de movimiento con ID {id}.");

        await _unitOfWork.SaveChanges(ct);
        return NoContent();
    }
}
