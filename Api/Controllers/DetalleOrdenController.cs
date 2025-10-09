using Api.DTOs.DetallesOrden;
using Application.Abstractions;
using AutoMapper;
using Domain.Entities;
using Domain.ValueObjects;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class DetalleOrdenController : BaseApiController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public DetalleOrdenController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    // ============================================================
    // GET /api/detalleorden/orden/{ordenServicioId}
    // ============================================================
    [HttpGet("orden/{ordenServicioId:int}")]
    public async Task<ActionResult<IEnumerable<DetalleOrdenDto>>> GetByOrdenServicio(int ordenServicioId, CancellationToken ct)
    {
        var detalles = await _unitOfWork.DetalleOrden
            .GetByOrdenServicioIdAsync(new IdVO(ordenServicioId), ct);

        var result = _mapper.Map<IEnumerable<DetalleOrdenDto>>(detalles);
        return Ok(result);
    }

    // ============================================================
    // GET /api/detalleorden/repuesto/{repuestoId}
    // ============================================================
    [HttpGet("repuesto/{repuestoId:int}")]
    public async Task<ActionResult<IEnumerable<DetalleOrdenDto>>> GetByRepuesto(int repuestoId, CancellationToken ct)
    {
        var detalles = await _unitOfWork.DetalleOrden
            .GetByRepuestoIdAsync(new IdVO(repuestoId), ct);

        var result = _mapper.Map<IEnumerable<DetalleOrdenDto>>(detalles);
        return Ok(result);
    }

    // ============================================================
    // GET /api/detalleorden/{ordenServicioId}/{repuestoId}
    // ============================================================
    [HttpGet("{ordenServicioId:int}/{repuestoId:int}")]
    public async Task<ActionResult<DetalleOrdenDto>> GetByIds(int ordenServicioId, int repuestoId, CancellationToken ct)
    {
        var detalle = await _unitOfWork.DetalleOrden
            .GetByIdsAsync(new IdVO(ordenServicioId), new IdVO(repuestoId), ct);

        if (detalle is null)
            return NotFound($"No se encontró el detalle con OrdenServicioId={ordenServicioId} y RepuestoId={repuestoId}");

        var dto = _mapper.Map<DetalleOrdenDto>(detalle);
        return Ok(dto);
    }

    // ============================================================
    // POST /api/detalleorden
    // ============================================================
    [HttpPost]
    public async Task<ActionResult> Create([FromBody] CreateDetalleOrdenDto dto, CancellationToken ct)
    {
        var detalle = _mapper.Map<DetalleOrden>(dto);
        await _unitOfWork.DetalleOrden.AddAsync(detalle, ct);
        await _unitOfWork.SaveChanges(ct);

        return CreatedAtAction(nameof(GetByIds),
            new { ordenServicioId = dto.OrdenServicioId, repuestoId = dto.RepuestoId },
            new { dto.OrdenServicioId, dto.RepuestoId });
    }

    // ============================================================
    // PUT /api/detalleorden/{ordenServicioId}/{repuestoId}
    // ============================================================
    [HttpPut("{ordenServicioId:int}/{repuestoId:int}")]
    public async Task<ActionResult> Update(int ordenServicioId, int repuestoId, [FromBody] UpdateDetalleOrdenDto dto, CancellationToken ct)
    {
        var existing = await _unitOfWork.DetalleOrden
            .GetByIdsAsync(new IdVO(ordenServicioId), new IdVO(repuestoId), ct);

        if (existing is null)
            return NotFound($"No se encontró el detalle con OrdenServicioId={ordenServicioId} y RepuestoId={repuestoId}");

        _mapper.Map(dto, existing);
        await _unitOfWork.DetalleOrden.UpdateAsync(existing, ct);
        await _unitOfWork.SaveChanges(ct);

        return NoContent();
    }

    // ============================================================
    // DELETE /api/detalleorden/{ordenServicioId}/{repuestoId}
    // ============================================================
    [HttpDelete("{ordenServicioId:int}/{repuestoId:int}")]
    public async Task<ActionResult> Delete(int ordenServicioId, int repuestoId, CancellationToken ct)
    {
        var deleted = await _unitOfWork.DetalleOrden
            .DeleteAsync(new IdVO(ordenServicioId), new IdVO(repuestoId), ct);

        if (!deleted)
            return NotFound($"No se encontró el detalle con OrdenServicioId={ordenServicioId} y RepuestoId={repuestoId}");

        await _unitOfWork.SaveChanges(ct);
        return NoContent();
    }
}
