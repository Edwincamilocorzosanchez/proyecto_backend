using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.DTOs;
using Application.Services;
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
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _service.GetByIdAsync(id);
            return result is null ? NotFound() : Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PagoRequestDto dto)
        {
            await _service.AddAsync(dto);
            return Ok("Pago registrado exitosamente.");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] PagoRequestDto dto)
        {
            await _service.UpdateAsync(id, dto);
            return Ok("Pago actualizado correctamente.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _service.DeleteAsync(id);
            return Ok("Pago eliminado.");
        }
    }
}