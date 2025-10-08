using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.DTOs;
using Api.DTOs.HistorialesInventario;
using Application.Abstractions;
using Domain.Entities;
using Domain.ValueObjects;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HistorialInventarioController : ControllerBase
    {
        private readonly IHistorialInventarioRepository _repository;

        public HistorialInventarioController(IHistorialInventarioRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var historial = await _repository.GetAllAsync();
            var result = historial.Select(h => new HistorialInventarioDto
            (
                h.Id.Value,
                h.RepuestoId.Value,
                h.AdminId?.Value,
                h.TipoMovimientoId.Value,
                h.Cantidad.Value,
                h.FechaMovimiento.Value,
                h.Observaciones?.Value
            ));
            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(int id)
        {
            var historial = await _repository.GetByIdAsync (new IdVO(id));
            if (historial == null) return NotFound();

            return Ok(new HistorialInventarioDto
            (
                historial.Id.Value,
                historial.RepuestoId.Value,
                historial.AdminId?.Value,
                historial.TipoMovimientoId.Value,
                historial.Cantidad.Value,
                historial.FechaMovimiento.Value,
                historial.Observaciones?.Value
            ));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateHistorialInventarioDto dto)
        {
            var historial = new HistorialInventario
            {
                Id = IdVO.CreateNew(),
                RepuestoId = new IdVO(dto.RepuestoId),
                AdminId= dto.AdminId.HasValue ? new IdVO(dto.AdminId.Value) : null,
                TipoMovimientoId = new IdVO(dto.TipoMovimientoId),
                Cantidad = new CantidadVO(dto.Cantidad),
                FechaMovimiento = new FechaHistoricaVO(dto.FechaMovimiento),
                Observaciones= !string.IsNullOrWhiteSpace(dto.Observaciones) ? new DescripcionVO(dto.Observaciones) : null
            };

            await _repository.AddAsync(historial);
            return CreatedAtAction(nameof(GetById), new { id = historial.Id.Value }, dto);
        }
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateHistorialInventarioDto dto)
        {
            var historial = await _repository.GetByIdAsync(new IdVO(id));
            if (historial == null) return NotFound();

            historial.Cantidad = new CantidadVO(dto.Cantidad);
            historial.FechaMovimiento = new FechaHistoricaVO(dto.FechaMovimiento);
            historial.Observaciones = !string.IsNullOrWhiteSpace(dto.Observaciones)
                ? new DescripcionVO(dto.Observaciones)
                : null;

            await _repository.UpdateAsync(historial);
            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _repository.DeleteAsync(new IdVO(id));
            return NoContent();
        }

    }
}