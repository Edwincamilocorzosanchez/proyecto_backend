using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.DTOs;
using Application.Abstractions;
using Domain.Entities;
using Domain.ValueObjects;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class ProveedoresController : ControllerBase
    {
        private readonly IProveedorRepository _proveedorRepository;

        public ProveedoresController(IProveedorRepository proveedorRepository)
        {
            _proveedorRepository = proveedorRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProveedorResponseDto>>> GetAll()
        {
            var proveedores = await _proveedorRepository.GetAllAsync();

            var result = proveedores.Select(p => new ProveedorResponseDto
            {
                Id = p.Id.Value,
                Nombre = p.Nombre.Value,
                Telefono = p.Telefono?.Value,
                Correo = p.Correo?.Value,
                Direccion = p.Direccion?.Value,
                IsActive = p.IsActive.Value
            });

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProveedorResponseDto>> GetById(Guid id)
        {
            var proveedor = await _proveedorRepository.GetByIdAsync(id);
            if (proveedor == null) return NotFound();

            return Ok(new ProveedorResponseDto
            {
                Id = proveedor.Id.Value,
                Nombre = proveedor.Nombre.Value,
                Telefono = proveedor.Telefono?.Value,
                Correo = proveedor.Correo?.Value,
                Direccion = proveedor.Direccion?.Value,
                IsActive = proveedor.IsActive.Value
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProveedorRequestDto dto)
        {
            var proveedor = new Proveedor(
                new IdVO(Guid.NewGuid()),
                new NombreVO(dto.Nombre),
                dto.Telefono != null ? new TelefonoVO(dto.Telefono) : null,
                dto.Correo != null ? new CorreoVO(dto.Correo) : null,
                dto.Direccion != null ? new DireccionVO(dto.Direccion) : null,
                new EstadoVO(dto.IsActive)
            );

            await _proveedorRepository.AddAsync(proveedor);
            return CreatedAtAction(nameof(GetById), new { id = proveedor.Id.Value }, dto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] ProveedorRequestDto dto)
        {
            var proveedor = await _proveedorRepository.GetByIdAsync(id);
            if (proveedor == null) return NotFound();

            proveedor.Nombre = new NombreVO(dto.Nombre);
            proveedor.Telefono = dto.Telefono != null ? new TelefonoVO(dto.Telefono) : null;
            proveedor.Correo = dto.Correo != null ? new CorreoVO(dto.Correo) : null;
            proveedor.Direccion = dto.Direccion != null ? new DireccionVO(dto.Direccion) : null;
            proveedor.IsActive = new EstadoVO(dto.IsActive);

            await _proveedorRepository.UpdateAsync(proveedor);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _proveedorRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}