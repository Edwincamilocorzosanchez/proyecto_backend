using Api.DTOs.DetallesOrden;
using Api.Services.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.ValueObjects;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class DetalleOrdenController : BaseApiController
{
    private readonly IDetalleOrdenService _detalleOrdenService;
    private readonly IMapper _mapper;

    public DetalleOrdenController(IDetalleOrdenService detalleOrdenService, IMapper mapper)
    {
        _detalleOrdenService = detalleOrdenService;
        _mapper = mapper;
    }

    // ============================================================
    // GET /api/detalleorden/orden/{ordenServicioId}
    // ============================================================
    [HttpGet("orden/{ordenServicioId:int}")]
    public async Task<ActionResult<IEnumerable<DetalleOrdenDto>>> GetByOrdenServicio(int ordenServicioId, CancellationToken ct)
    {
        var detalles = await _detalleOrdenService.GetByOrdenIdAsync(new IdVO(ordenServicioId), ct);
        var result = _mapper.Map<IEnumerable<DetalleOrdenDto>>(detalles);
        return Ok(result);
    }

    // ============================================================
    // GET /api/detalleorden/{ordenServicioId}/{repuestoId}
    // ============================================================
    [HttpGet("{ordenServicioId:int}/{repuestoId:int}")]
    public async Task<ActionResult<DetalleOrdenDto>> GetByIds(int ordenServicioId, int repuestoId, CancellationToken ct)
    {
        var detalle = await _detalleOrdenService.GetByIdAsync(new IdVO(ordenServicioId), new IdVO(repuestoId), ct);

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
        await _detalleOrdenService.AddAsync(detalle, ct);

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
        var existing = await _detalleOrdenService.GetByIdAsync(new IdVO(ordenServicioId), new IdVO(repuestoId), ct);
        if (existing is null)
            return NotFound($"No se encontró el detalle con OrdenServicioId={ordenServicioId} y RepuestoId={repuestoId}");

        _mapper.Map(dto, existing);
        var updated = await _detalleOrdenService.UpdateAsync(existing, ct);

        if (!updated)
            return BadRequest("No se pudo actualizar el detalle.");

        return NoContent();
    }

    // ============================================================
    // DELETE /api/detalleorden/{ordenServicioId}/{repuestoId}
    // ============================================================
    [HttpDelete("{ordenServicioId:int}/{repuestoId:int}")]
    public async Task<ActionResult> Delete(int ordenServicioId, int repuestoId, CancellationToken ct)
    {
        var deleted = await _detalleOrdenService.DeleteAsync(new IdVO(ordenServicioId), new IdVO(repuestoId), ct);

        if (!deleted)
            return NotFound($"No se encontró el detalle con OrdenServicioId={ordenServicioId} y RepuestoId={repuestoId}");

        return NoContent();
    }
}
