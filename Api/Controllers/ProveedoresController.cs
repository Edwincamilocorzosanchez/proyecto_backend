 //using System;
 //using System.Collections.Generic;
 //using System.Linq;
 //using System.Threading.Tasks;
 //using Api.DTOs;
 //using Application.Abstractions;
 //using Domain.Entities;
 //using Domain.ValueObjects;
 //using Microsoft.AspNetCore.Mvc;
//
 //namespace Api.Controllers
 //{
 //    public class ProveedoresController : ControllerBase
 //    {
 //        private readonly IProveedorRepository _proveedorRepository;
//
 //        public ProveedoresController(IProveedorRepository proveedorRepository)
 //        {
 //            _proveedorRepository = proveedorRepository;
 //        }
//
 //        [HttpGet]
 //        public async Task<ActionResult<IEnumerable<ProveedorResponseDto>>> GetAll()
 //        {
 //            var proveedores = await _proveedorRepository.GetAllAsync();
//
 //            var result = proveedores.Select(p => new ProveedorResponseDto
 //            {
 //                Id = p.Id.Value,
 //                Nombre = p.Nombre.Value,
 //                Telefono = p.Telefono?.Value,
 //                Correo = p.Correo?.Value,
 //                Direccion = p.Direccion?.Value,
 //                IsActive = p.IsActive.Value
 //            });
//
 //            return Ok(result);
 //        }
//
 //           [HttpGet("{id}")]
 //           public async Task<ActionResult<ProveedorResponseDto>> GetById(int id)
 //           {
 //               var proveedor = await _proveedorRepository.GetByIdAsync(new IdVO(id));
 //               if (proveedor == null) return NotFound();
//
 //               return Ok(new ProveedorResponseDto
 //               {
 //                   Id = proveedor.Id.Value,
 //                   Nombre = proveedor.Nombre.Value,
 //                   Telefono = proveedor.Telefono?.Value,
 //                   Correo = proveedor.Correo?.Value,
 //                   Direccion = proveedor.Direccion?.Value,
 //                   IsActive = proveedor.IsActive.Value
 //               });
 //           }
//
//
 //        //[HttpPost]
 //        //public async Task<IActionResult> Create([FromBody] ProveedorRequestDto dto)
 //        //{
 //        //    var proveedor = new Proveedor(
 //        //        new IdVO(Guid.NewGuid()),
 //        //        new NombreVO(dto.Nombre),
 //        //        dto.Telefono != null ? new TelefonoVO(dto.Telefono) : null,
 //        //        dto.Correo != null ? new CorreoVO(dto.Correo) : null,
 //        //        dto.Direccion != null ? new DireccionVO(dto.Direccion) : null,
 //        //        new EstadoVO(dto.IsActive),
 //        //        userId
 //        //    );
////
 //        //    await _proveedorRepository.AddAsync(proveedor);
 //        //    return CreatedAtAction(nameof(GetById), new { id = proveedor.Id.Value }, dto);
 //        //}
 //        [HttpPost]
 //        public async Task<IActionResult> Create([FromBody] ProveedorRequestDto dto)
 //        {
 //              int userId = 1; // 🔧 por ahora un valor fijo (luego lo se puede llamar del JWT o contexto)
 //          
 //              var proveedor = new Proveedor(
 //                  IdVO.CreateNew(),
 //                  new NombreVO(dto.Nombre),
 //                  dto.Telefono != null ? new TelefonoVO(dto.Telefono) : null,
 //                  dto.Correo != null ? new CorreoVO(dto.Correo) : null,
 //                  dto.Direccion != null ? new DireccionVO(dto.Direccion) : null,
 //                  new EstadoVO(dto.IsActive),
 //                  userId // ✅ nuevo argumento agregado
 //              );
//
 //               await _proveedorRepository.AddAsync(proveedor);
 //               return CreatedAtAction(nameof(GetById), new { id = proveedor.Id.Value }, dto);
 //           }
//
//
 //        [HttpPut("{id}")]
 //        public async Task<IActionResult> Update(Guid id, [FromBody] ProveedorRequestDto dto)
 //        {
 //            var proveedor = await _proveedorRepository.GetByIdAsync(id);
 //            if (proveedor == null) return NotFound();
//
 //            proveedor.Nombre = new NombreVO(dto.Nombre);
 //            proveedor.Telefono = dto.Telefono != null ? new TelefonoVO(dto.Telefono) : null;
 //            proveedor.Correo = dto.Correo != null ? new CorreoVO(dto.Correo) : null;
 //            proveedor.Direccion = dto.Direccion != null ? new DireccionVO(dto.Direccion) : null;
 //            proveedor.IsActive = new EstadoVO(dto.IsActive);
//
 //            await _proveedorRepository.UpdateAsync(proveedor);
 //            return NoContent();
 //        }
//
 //        [HttpDelete("{id}")]
 //        public async Task<IActionResult> Delete(Guid id)
 //        {
 //            await _proveedorRepository.DeleteAsync(id);
 //            return NoContent();
 //        }
 //    }
 //}
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
    [ApiController]
    [Route("api/[controller]")]
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

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProveedorResponseDto>> GetById(int id)
        {
            var proveedor = await _proveedorRepository.GetByIdAsync(new IdVO(id));
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
                IdVO.CreateNew(), // usa tu método estático para ID temporal
                new NombreVO(dto.Nombre),
                dto.Telefono != null ? new TelefonoVO(dto.Telefono) : null,
                dto.Correo != null ? new CorreoVO(dto.Correo) : null,
                dto.Direccion != null ? new DireccionVO(dto.Direccion) : null,
                new EstadoVO(dto.IsActive),
                0 // si tu constructor requiere userId, pásalo aquí; si no, remuévelo del constructor
            );

            await _proveedorRepository.AddAsync(proveedor);
            return CreatedAtAction(nameof(GetById), new { id = proveedor.Id.Value }, dto);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] ProveedorRequestDto dto)
        {
            var proveedor = await _proveedorRepository.GetByIdAsync(new IdVO(id));
            if (proveedor == null) return NotFound();

            proveedor.Nombre = new NombreVO(dto.Nombre);
            proveedor.Telefono = dto.Telefono != null ? new TelefonoVO(dto.Telefono) : null;
            proveedor.Correo = dto.Correo != null ? new CorreoVO(dto.Correo) : null;
            proveedor.Direccion = dto.Direccion != null ? new DireccionVO(dto.Direccion) : null;
            proveedor.IsActive = new EstadoVO(dto.IsActive);

            var updated = await _proveedorRepository.UpdateAsync(proveedor);
            if (!updated) return StatusCode(500, "No se pudo actualizar el proveedor.");
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _proveedorRepository.DeleteAsync(new IdVO(id));
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
// deje las 2 versiones pq siento que puede cambiar en cualquier momento de guid a int (por si acaso)