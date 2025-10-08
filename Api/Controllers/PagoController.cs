using Api.DTOs.Pagos;
using Api.Services.Implementations;
using Application.Pagos;
using Domain.ValueObjects;
using Microsoft.AspNetCore.Mvc;


namespace Api.Controllers
 {
      [ApiController]
     [Route("api/[controller]")]
     public class PagoController : ControllerBase
     {
         private readonly PagoService _service;

         public PagoController(PagoService service)
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
         public async Task<IActionResult> GetById(int id)
         {
             var result = await _service.GetByIdAsync(new IdVO(id));
             return result is null ? NotFound() : Ok(result);
         }
         [HttpPost]
         public async Task<IActionResult> Create([FromBody] CreatePagoDto dto)
         {
             await _service.AddAsync(dto);
             return Ok("Pago registrado exitosamente.");
         }

         [HttpPut("{id}")]
         public async Task<IActionResult> Update(Guid id, [FromBody] UpdatePagoDto dto)
         {
             await _service.UpdateAsync(id, dto);
             return Ok("Pago actualizado correctamente.");
         }

         [HttpDelete("{id}")]
         public async Task<IActionResult> Delete(int id)
         {
             await _service.DeleteAsync(new IdVO(id));
             return Ok("Pago eliminado.");
         }
     }
 }