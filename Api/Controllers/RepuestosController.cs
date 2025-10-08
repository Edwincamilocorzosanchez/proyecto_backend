using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.DTOs;
using Api.DTOs.Repuestos;
using Application.Abstractions;
using Domain.Entities;
using Domain.ValueObjects;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RepuestosController : ControllerBase
        {
        private readonly IRepuestoRepository _repository;

        public RepuestosController(IRepuestoRepository repository)
        {
          _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var repuestos = await _repository.GetAllAsync();
            var result = repuestos.Select(r => new RepuestoDto(
                r.Id.Value,
                r.Codigo.Value,
                r.Descripcion.Value,
                r.CantidadStock.Value,
                r.PrecioUnitario.Value,
                r.ProveedorId?.Value
            ));
        
            return Ok(result);
        }
        
        

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(int id)
        {
            var repuesto = await _repository.GetByIdAsync(new IdVO(id));
            if (repuesto == null) return NotFound();
        
            return Ok(new RepuestoDto(
                repuesto.Id.Value,
                repuesto.Codigo.Value,
                repuesto.Descripcion.Value,
                repuesto.CantidadStock.Value,
                repuesto.PrecioUnitario.Value,
                repuesto.ProveedorId?.Value
            ));
        }
        


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateRepuestoDto dto)
        {
            var repuesto = new Repuesto
            {
                Id = IdVO.CreateNew(), // o simplemente null si EF lo maneja
                Codigo = new CodigoRepuestoVO(dto.Codigo),
                Descripcion = new DescripcionVO(dto.Descripcion),
                CantidadStock = new CantidadVO(dto.CantidadStock),
                PrecioUnitario = new DineroVO(dto.PrecioUnitario),
                ProveedorId = dto.ProveedorId.HasValue ? new IdVO(dto.ProveedorId.Value) : null
            };
        
            await _repository.AddAsync(repuesto);
            return CreatedAtAction(nameof(GetById), new { id = repuesto.Id.Value }, dto);
        }


        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateRepuestoDto dto)
        {
            var repuesto = await _repository.GetByIdAsync(new IdVO(id));
            if (repuesto == null) return NotFound();

            repuesto.Codigo = new CodigoRepuestoVO(dto.Codigo);
            repuesto.Descripcion = new DescripcionVO(dto.Descripcion);
            repuesto.CantidadStock = new CantidadVO(dto.CantidadStock);
            repuesto.PrecioUnitario = new DineroVO(dto.PrecioUnitario);
            repuesto.ProveedorId = dto.ProveedorId.HasValue ? new IdVO(dto.ProveedorId.Value) : null;

            await _repository.UpdateAsync(repuesto);
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