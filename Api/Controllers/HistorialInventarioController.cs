// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;
// using Api.DTOs;
// using Application.Abstractions;
// using Domain.Entities;
// using Domain.ValueObjects;
// using Microsoft.AspNetCore.Mvc;

// namespace Api.Controllers
// {
//     [ApiController]
//     [Route("api/[controller]")]
//     public class HistorialInventarioController : ControllerBase
//     {
//         private readonly IHistorialInventarioRepository _repository;

//         public HistorialInventarioController(IHistorialInventarioRepository repository)
//         {
//             _repository = repository;
//         }

//         [HttpGet]
//         public async Task<IActionResult> GetAll()
//         {
//             var historial = await _repository.GetAllAsync();
//             var result = historial.Select(h => new HistorialInventarioDto
//             {
//                 Id = h.Id.Value,
//                 RepuestoId = h.RepuestoId.Value,
//                 AdminId = h.AdminId?.Value,
//                 TipoMovimientoId = h.TipoMovimientoId.Value,
//                 Cantidad = h.Cantidad.Value,
//                 FechaMovimiento = h.FechaMovimiento.Value,
//                 Observaciones = h.Observaciones?.Value
//             });
//             return Ok(result);
//         }

//         [HttpGet("{id:guid}")]
//         public async Task<IActionResult> GetById(Guid id)
//         {
//             var historial = await _repository.GetByIdAsync(id);
//             if (historial == null) return NotFound();

//             return Ok(new HistorialInventarioDto
//             {
//                 Id = historial.Id.Value,
//                 RepuestoId = historial.RepuestoId.Value,
//                 AdminId = historial.AdminId?.Value,
//                 TipoMovimientoId = historial.TipoMovimientoId.Value,
//                 Cantidad = historial.Cantidad.Value,
//                 FechaMovimiento = historial.FechaMovimiento.Value,
//                 Observaciones = historial.Observaciones?.Value
//             });
//         }

//         [HttpPost]
//         public async Task<IActionResult> Create([FromBody] CreateHistorialInventarioDto dto)
//         {
//             var historial = new HistorialInventario(
//                 new IdVO(Guid.NewGuid()),
//                 new IdVO(dto.RepuestoId),
//                 dto.AdminId.HasValue ? new IdVO(dto.AdminId.Value) : null,
//                 new IdVO(dto.TipoMovimientoId),
//                 new CantidadVO(dto.Cantidad),
//                 new FechaHistoricaVO(dto.FechaMovimiento),
//                 !string.IsNullOrWhiteSpace(dto.Observaciones) ? new DescripcionVO(dto.Observaciones) : null
//             );

//             await _repository.AddAsync(historial);
//             return CreatedAtAction(nameof(GetById), new { id = historial.Id.Value }, dto);
//         }

//         [HttpPut("{id:guid}")]
//         public async Task<IActionResult> Update(Guid id, [FromBody] UpdateHistorialInventarioDto dto)
//         {
//             var historial = await _repository.GetByIdAsync(id);
//             if (historial == null) return NotFound();

//             historial.Cantidad = new CantidadVO(dto.Cantidad);
//             historial.FechaMovimiento = new FechaHistoricaVO(dto.FechaMovimiento);
//             historial.Observaciones = !string.IsNullOrWhiteSpace(dto.Observaciones)
//                 ? new DescripcionVO(dto.Observaciones)
//                 : null;

//             await _repository.UpdateAsync(historial);
//             return NoContent();
//         }

//         [HttpDelete("{id:guid}")]
//         public async Task<IActionResult> Delete(Guid id)
//         {
//             await _repository.DeleteAsync(id);
//             return NoContent();
//         }

//     }
// }