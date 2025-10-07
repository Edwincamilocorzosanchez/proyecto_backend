using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.DTOs;
using Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdministradorController : ControllerBase
    {
        private readonly AdministradorService _service;
    
        public AdministradorController(AdministradorService service)
        {
            _service = service;
        }
    
        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] AdministradorRequestDto dto)
        {
            var result = await _service.CrearAsync(dto);
            return Ok(result);
        }
    
        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var result = await _service.ObtenerTodosAsync();
            return Ok(result);
        }
    
        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId(Guid id)
        {
            var result = await _service.ObtenerPorIdAsync(id);
            if (result == null)
                return NotFound();
    
            return Ok(result);
        }
    
        [HttpPut("{id}")]
        public async Task<IActionResult> Actualizar(Guid id, [FromBody] AdministradorRequestDto dto)
        {
            await _service.ActualizarAsync(id, dto);
            return NoContent();
        }
    
        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(Guid id)
        {
            await _service.EliminarAsync(id);
            return NoContent();
        }
    }

}