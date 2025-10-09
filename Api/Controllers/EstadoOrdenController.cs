using Api.DTOs.EstadosOrden;
using Application.Abstractions;
using AutoMapper;
using Domain.Entities;
using Domain.ValueObjects;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public sealed class EstadoOrdenController : BaseApiController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public EstadoOrdenController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    // ✅ GET: api/EstadoOrden
    [HttpGet]
    public async Task<ActionResult<IEnumerable<EstadoOrdenDto>>> GetAllAsync(CancellationToken ct)
    {
        var estados = await _unitOfWork.EstadoOrden.GetAllAsync(ct);
        var result = _mapper.Map<IEnumerable<EstadoOrdenDto>>(estados);
        return Ok(result);
    }

    // ✅ GET: api/EstadoOrden/{id}
    [HttpGet("{id:int}")]
    public async Task<ActionResult<EstadoOrdenDto>> GetByIdAsync(int id, CancellationToken ct)
    {
        var estado = await _unitOfWork.EstadoOrden.GetByIdAsync(new IdVO(id), ct);
        if (estado is null)
            return NotFound($"No se encontró el estado con ID {id}.");

        var result = _mapper.Map<EstadoOrdenDto>(estado);
        return Ok(result);
    }

    // ✅ POST: api/EstadoOrden
    [HttpPost]
    public async Task<ActionResult<EstadoOrdenDto>> CreateAsync([FromBody] CreateEstadoOrdenDto dto, CancellationToken ct)
    {
        // Validar duplicado
        var existing = await _unitOfWork.EstadoOrden.GetByNombreAsync(new NombreVO(dto.Nombre), ct);
        if (existing is not null)
            return Conflict($"Ya existe un estado de orden con el nombre '{dto.Nombre}'.");

        var estado = new EstadoOrden(
            new IdVO(0),
            new NombreVO(dto.Nombre)
        );

        await _unitOfWork.EstadoOrden.AddAsync(estado, ct);
        await _unitOfWork.SaveChanges(ct);

        var result = _mapper.Map<EstadoOrdenDto>(estado);
        return CreatedAtAction(nameof(GetByIdAsync), new { id = estado.Id.Value }, result);
    }

    // ✅ PUT: api/EstadoOrden/{id}
    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateAsync(int id, [FromBody] UpdateEstadoOrdenDto dto, CancellationToken ct)
    {
        var existing = await _unitOfWork.EstadoOrden.GetByIdAsync(new IdVO(id), ct);
        if (existing is null)
            return NotFound($"No se encontró el estado con ID {id}.");

        existing.Nombre = new NombreVO(dto.Nombre);

        var updated = await _unitOfWork.EstadoOrden.UpdateAsync(existing, ct);
        if (!updated)
            return BadRequest("No se pudo actualizar el estado.");

        await _unitOfWork.SaveChanges(ct);
        return NoContent();
    }

    // ✅ DELETE: api/EstadoOrden/{id}
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteAsync(int id, CancellationToken ct)
    {
        var deleted = await _unitOfWork.EstadoOrden.DeleteAsync(new IdVO(id), ct);
        if (!deleted)
            return NotFound($"No se encontró el estado con ID {id}.");

        await _unitOfWork.SaveChanges(ct);
        return NoContent();
    }
}
