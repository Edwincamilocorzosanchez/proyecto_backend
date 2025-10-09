using Api.DTOs.Citas;
using Application.Abstractions;
using AutoMapper;
using Domain.Entities;
using Domain.ValueObjects;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class CitaController : BaseApiController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CitaController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    // ============================================================
    // GET /api/cita
    // ============================================================
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CitaDto>>> GetAll(CancellationToken ct)
    {
        var citas = await _unitOfWork.Citas.GetAllAsync(ct);
        var result = _mapper.Map<IEnumerable<CitaDto>>(citas);
        return Ok(result);
    }

    // ============================================================
    // GET /api/cita/{id}
    // ============================================================
    [HttpGet("{id:int}")]
    public async Task<ActionResult<CitaDto>> GetById(int id, CancellationToken ct)
    {
        var cita = await _unitOfWork.Citas.GetByIdAsync(new IdVO(id), ct);
        if (cita is null)
            return NotFound($"No se encontró la cita con ID {id}");

        var dto = _mapper.Map<CitaDto>(cita);
        return Ok(dto);
    }

    // ============================================================
    // GET /api/cita/cliente/{clienteId}
    // ============================================================
    [HttpGet("cliente/{clienteId:int}")]
    public async Task<ActionResult<IEnumerable<CitaDto>>> GetByCliente(int clienteId, CancellationToken ct)
    {
        var citas = await _unitOfWork.Citas.GetByClienteIdAsync(new IdVO(clienteId), ct);
        var result = _mapper.Map<IEnumerable<CitaDto>>(citas);
        return Ok(result);
    }

    // ============================================================
    // GET /api/cita/vehiculo/{vehiculoId}
    // ============================================================
    [HttpGet("vehiculo/{vehiculoId:int}")]
    public async Task<ActionResult<IEnumerable<CitaDto>>> GetByVehiculo(int vehiculoId, CancellationToken ct)
    {
        var citas = await _unitOfWork.Citas.GetByVehiculoIdAsync(new IdVO(vehiculoId), ct);
        var result = _mapper.Map<IEnumerable<CitaDto>>(citas);
        return Ok(result);
    }

    // ============================================================
    // POST /api/cita
    // ============================================================
    [HttpPost]
    public async Task<ActionResult> Create([FromBody] CreateCitaDto dto, CancellationToken ct)
    {
        var cita = _mapper.Map<Cita>(dto);
        await _unitOfWork.Citas.AddAsync(cita, ct);
        await _unitOfWork.SaveChanges(ct);

        return CreatedAtAction(nameof(GetById), new { id = cita.Id.Value }, new { cita.Id.Value });
    }

    // ============================================================
    // PUT /api/cita/{id}
    // ============================================================
    [HttpPut("{id:int}")]
    public async Task<ActionResult> Update(int id, [FromBody] UpdateCitaDto dto, CancellationToken ct)
    {
        var existing = await _unitOfWork.Citas.GetByIdAsync(new IdVO(id), ct);
        if (existing is null)
            return NotFound($"No se encontró la cita con ID {id}");

        _mapper.Map(dto, existing);
        await _unitOfWork.Citas.UpdateAsync(existing, ct);
        await _unitOfWork.SaveChanges(ct);

        return NoContent();
    }

    // ============================================================
    // DELETE /api/cita/{id}
    // ============================================================
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id, CancellationToken ct)
    {
        var deleted = await _unitOfWork.Citas.DeleteAsync(new IdVO(id), ct);
        if (!deleted)
            return NotFound($"No se encontró la cita con ID {id}");

        await _unitOfWork.SaveChanges(ct);
        return NoContent();
    }
}
