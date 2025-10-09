using Api.DTOs.Mecanicos;
using Application.Abstractions;
using AutoMapper;
using Domain.Entities;
using Domain.ValueObjects;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public sealed class MecanicoController : BaseApiController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public MecanicoController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    // ✅ GET: api/Mecanico
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MecanicoDto>>> GetAllAsync(CancellationToken ct)
    {
        var mecanicos = await _unitOfWork.Mecanicos.GetAllAsync(ct);
        var result = _mapper.Map<IEnumerable<MecanicoDto>>(mecanicos);
        return Ok(result);
    }

    // ✅ GET: api/Mecanico/{id}
    [HttpGet("{id:int}")]
    public async Task<ActionResult<MecanicoDetailDto>> GetByIdAsync(int id, CancellationToken ct)
    {
        var mecanico = await _unitOfWork.Mecanicos.GetByIdAsync(new IdVO(id), ct);
        if (mecanico is null)
            return NotFound($"No se encontró el mecánico con ID {id}.");

        var result = _mapper.Map<MecanicoDetailDto>(mecanico);
        return Ok(result);
    }

    // ✅ POST: api/Mecanico
    [HttpPost]
    public async Task<ActionResult<MecanicoDto>> CreateAsync([FromBody] CreateMecanicoDto dto, CancellationToken ct)
    {
        var existeNombre = await _unitOfWork.Mecanicos.ExistsByNombreAsync(new NombreVO(dto.Nombre), ct);
        if (existeNombre)
            return Conflict($"Ya existe un mecánico con el nombre '{dto.Nombre}'.");

        var mecanico = new Mecanico(
            new IdVO(0),
            new NombreVO(dto.Nombre),
            string.IsNullOrEmpty(dto.Telefono) ? null : new TelefonoVO(dto.Telefono),
            string.IsNullOrEmpty(dto.Especialidad) ? null : new EspecialidadVO(dto.Especialidad),
            new EstadoVO(dto.IsActive),
            dto.UserId
        );

        await _unitOfWork.Mecanicos.AddAsync(mecanico, ct);
        await _unitOfWork.SaveChanges(ct);

        var result = _mapper.Map<MecanicoDto>(mecanico);
        return CreatedAtAction(nameof(GetByIdAsync), new { id = mecanico.Id.Value }, result);
    }

    // ✅ PUT: api/Mecanico/{id}
    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateAsync(int id, [FromBody] UpdateMecanicoDto dto, CancellationToken ct)
    {
        var existing = await _unitOfWork.Mecanicos.GetByIdAsync(new IdVO(id), ct);
        if (existing is null)
            return NotFound($"No se encontró el mecánico con ID {id}.");

        existing.Nombre = new NombreVO(dto.Nombre);
        existing.Telefono = dto.Telefono is null ? null : new TelefonoVO(dto.Telefono);
        existing.Especialidad = dto.Especialidad is null ? null : new EspecialidadVO(dto.Especialidad);
        existing.IsActive = dto.IsActive is null ? existing.IsActive : new EstadoVO(dto.IsActive.Value);

        var updated = await _unitOfWork.Mecanicos.UpdateAsync(existing, ct);
        if (!updated)
            return BadRequest("No se pudo actualizar el mecánico.");

        await _unitOfWork.SaveChanges(ct);
        return NoContent();
    }

    // ✅ DELETE: api/Mecanico/{id}
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteAsync(int id, CancellationToken ct)
    {
        var deleted = await _unitOfWork.Mecanicos.DeleteAsync(new IdVO(id), ct);
        if (!deleted)
            return NotFound($"No se encontró el mecánico con ID {id}.");

        await _unitOfWork.SaveChanges(ct);
        return NoContent();
    }
}
