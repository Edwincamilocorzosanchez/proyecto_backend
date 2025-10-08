 using System;
 using System.Collections.Generic;
 using System.Linq;
 using System.Threading.Tasks;
 using Api.DTOs;
 using Application.DTOs;
 using Application.Services;
 using Microsoft.AspNetCore.Mvc;

 namespace Api.Controllers
 {
     [ApiController]
     [Route("api/[controller]")]
     public class TipoMovimientoController : ControllerBase
     {
         private readonly TipoMovimientoService _service;

         public TipoMovimientoController(TipoMovimientoService service)
         {
             _service = service;
         }

         [HttpGet]
         public async Task<IActionResult> GetAll()
         {
             var result = await _service.GetAllAsync();
             return Ok(result);
         }

         [HttpGet("{id}")]
         public async Task<IActionResult> GetById(Guid id)
         {
             var result = await _service.GetByIdAsync(id);
             if (result == null) return NotFound();
             return Ok(result);
         }

         [HttpPost]
         public async Task<IActionResult> Create(TipoMovimientoRequestDto dto)
         {
             var result = await _service.CreateAsync(dto);
             return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
         }

         [HttpPut("{id}")]
         public async Task<IActionResult> Update(Guid id, TipoMovimientoRequestDto dto)
         {
             var updated = await _service.UpdateAsync(id, dto);
             if (!updated) return NotFound();
             return NoContent();
         }

         [HttpDelete("{id}")]
         public async Task<IActionResult> Delete(Guid id)
         {
             var deleted = await _service.DeleteAsync(id);
             if (!deleted) return NotFound();
             return NoContent();
         }
     }
 }