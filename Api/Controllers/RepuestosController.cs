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
//     public class RepuestosController : ControllerBase
//     {
//         private readonly IRepuestoRepository _repository;

//         public RepuestosController(IRepuestoRepository repository)
//         {
//             _repository = repository;
//         }

//         [HttpGet]
//         public async Task<IActionResult> GetAll()
//         {
//             var repuestos = await _repository.GetAllAsync();
//             var result = repuestos.Select(r => new RepuestoDto
//             {
//                 Id = r.Id.Value,
//                 Codigo = r.Codigo.Value,
//                 Descripcion = r.Descripcion.Value,
//                 CantidadStock = r.CantidadStock.Value,
//                 PrecioUnitario = r.PrecioUnitario.Value,
//                 ProveedorId = r.ProveedorId?.Value
//             });
//             return Ok(result);
//         }

//         [HttpGet("{id:guid}")]
//         public async Task<IActionResult> GetById(Guid id)
//         {
//             var repuesto = await _repository.GetByIdAsync(id);
//             if (repuesto == null) return NotFound();

//             return Ok(new RepuestoDto
//             {
//                 Id = repuesto.Id.Value,
//                 Codigo = repuesto.Codigo.Value,
//                 Descripcion = repuesto.Descripcion.Value,
//                 CantidadStock = repuesto.CantidadStock.Value,
//                 PrecioUnitario = repuesto.PrecioUnitario.Value,
//                 ProveedorId = repuesto.ProveedorId?.Value
//             });
//         }

//         [HttpPost]
//         public async Task<IActionResult> Create([FromBody] CreateRepuestoDto dto)
//         {
//             var repuesto = new Repuesto
//             {
//                 Id = new IdVO(Guid.NewGuid()),
//                 Codigo = new CodigoRepuestoVO(dto.Codigo),
//                 Descripcion = new DescripcionVO(dto.Descripcion),
//                 CantidadStock = new CantidadVO(dto.CantidadStock),
//                 PrecioUnitario = new DineroVO(dto.PrecioUnitario),
//                 ProveedorId = dto.ProveedorId.HasValue ? new IdVO(dto.ProveedorId.Value) : null
//             };

//             await _repository.AddAsync(repuesto);
//             return CreatedAtAction(nameof(GetById), new { id = repuesto.Id.Value }, dto);
//         }

//         [HttpPut("{id:guid}")]
//         public async Task<IActionResult> Update(Guid id, [FromBody] UpdateRepuestoDto dto)
//         {
//             var repuesto = await _repository.GetByIdAsync(id);
//             if (repuesto == null) return NotFound();

//             repuesto.Codigo = new CodigoRepuestoVO(dto.Codigo);
//             repuesto.Descripcion = new DescripcionVO(dto.Descripcion);
//             repuesto.CantidadStock = new CantidadVO(dto.CantidadStock);
//             repuesto.PrecioUnitario = new DineroVO(dto.PrecioUnitario);
//             repuesto.ProveedorId = dto.ProveedorId.HasValue ? new IdVO(dto.ProveedorId.Value) : null;

//             await _repository.UpdateAsync(repuesto);
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