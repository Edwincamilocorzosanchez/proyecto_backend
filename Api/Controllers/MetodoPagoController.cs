using Api.DTOs.MetodosPago;
using Application.Abstractions;
using Domain.Entities;
using Domain.ValueObjects;
using Microsoft.AspNetCore.Mvc;


namespace Api.Controllers
{
     [ApiController]
    [Route("api/[controller]")]
    public class MetodoPagoController : ControllerBase
    {
        private readonly IMetodoPagoRepository _service;

        public MetodoPagoController(IMetodoPagoRepository service)
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
            return result is null ? NotFound("Método de pago no encontrado.") : Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateMetodoPagoDto dto)
        {
            if (dto == null)
                return BadRequest("Datos inválidos.");

            var metodoPago = new MetodoPago(
                id: IdVO.CreateNew(),
                nombre: new NombreVO(dto.Nombre)
            );

            await _service.AddAsync(metodoPago);
            return Ok("Método de pago registrado exitosamente.");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateMetodoPagoDto dto)
        {
            if (dto == null)
                return BadRequest("Datos inválidos.");

            var existing = await _service.GetByIdAsync(new IdVO(id));
            if (existing is null)
                return NotFound("Método de pago no encontrado.");

            existing.Nombre = new NombreVO(dto.Nombre);

            await _service.UpdateAsync(existing);
            return Ok("Método de pago actualizado correctamente.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(new IdVO(id));
            if (!deleted)
                return NotFound("Método de pago no encontrado.");

            return Ok("Método de pago eliminado correctamente.");
        }
    }
}