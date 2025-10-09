using Api.DTOs.EstadosCita;
using Application.Abstractions;
using AutoMapper;
using Domain.Entities;
using Domain.ValueObjects;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

// se coloca seales para que no se puedan extender, los controladores de entidades no deben extenderse
public sealed class EstadoCitaController : BaseApiController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public EstadoCitaController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    // ✅ GET: api/EstadoCita
    [HttpGet]
    public async Task<ActionResult<IEnumerable<EstadoCitaDto>>> GetAllAsync(CancellationToken ct)
    {
        var estados = await _unitOfWork.EstadoCita.GetAllAsync(ct);
        var result = _mapper.Map<IEnumerable<EstadoCitaDto>>(estados);
        return Ok(result);
    }

    // ✅ GET: api/EstadoCita/{id}
    [HttpGet("{id:int}")]
    public async Task<ActionResult<EstadoCitaDto>> GetByIdAsync(int id, CancellationToken ct)
    {
        var estado = await _unitOfWork.EstadoCita.GetByIdAsync(new IdVO(id), ct);
        if (estado is null)
            return NotFound($"No se encontró el estado con ID {id}.");

        var result = _mapper.Map<EstadoCitaDto>(estado);
        return Ok(result);
    }

    // ✅ POST: api/EstadoCita
    [HttpPost]
    public async Task<ActionResult<EstadoCitaDto>> CreateAsync([FromBody] CreateEstadoCitaDto dto, CancellationToken ct)
    {
        // Validar duplicado
        var existing = await _unitOfWork.EstadoCita.GetByNombreAsync(new NombreVO(dto.Nombre), ct);
        if (existing is not null)
            return Conflict($"Ya existe un estado de orden con el nombre '{dto.Nombre}'.");

        var estado = new EstadoCita(
            new IdVO(0),
            new NombreVO(dto.Nombre)
        );

        await _unitOfWork.EstadoCita.AddAsync(estado, ct);
        await _unitOfWork.SaveChanges(ct);

        var result = _mapper.Map<EstadoCitaDto>(estado);
        return CreatedAtAction(nameof(GetByIdAsync), new { id = estado.Id.Value }, result);
    }

    // ✅ PUT: api/EstadoCita/{id}
    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateAsync(int id, [FromBody] UpdateEstadoCitaDto dto, CancellationToken ct)
    {
        var existing = await _unitOfWork.EstadoCita.GetByIdAsync(new IdVO(id), ct);
        if (existing is null)
            return NotFound($"No se encontró el estado con ID {id}.");

        existing.Nombre = new NombreVO(dto.Nombre);

        var updated = await _unitOfWork.EstadoCita.UpdateAsync(existing, ct);
        if (!updated)
            return BadRequest("No se pudo actualizar el estado.");

        await _unitOfWork.SaveChanges(ct);
        return NoContent();
    }

    // ✅ DELETE: api/EstadoCita/{id}
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteAsync(int id, CancellationToken ct)
    {
        var deleted = await _unitOfWork.EstadoCita.DeleteAsync(new IdVO(id), ct);
        if (!deleted)
            return NotFound($"No se encontró el estado con ID {id}.");

        await _unitOfWork.SaveChanges(ct);
        return NoContent();
    }
}
