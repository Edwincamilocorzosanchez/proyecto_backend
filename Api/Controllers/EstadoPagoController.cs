using Api.DTOs.EstadosPago;
using Api.Services.Implementations;
using Application.Abstractions;
using Domain.Entities;
using Domain.ValueObjects;
using Microsoft.AspNetCore.Mvc;
namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EstadoPagoController : ControllerBase
    {
        private readonly IEstadoPagoRepository _repository;

        public EstadoPagoController(IEstadoPagoRepository repository)
        {
            _repository = repository;
        }

        // ✅ GET: api/EstadoPago
        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {
            var estados = await _repository.GetAllAsync(ct);
            return Ok(estados.Select(e => new EstadoPagoDto(
                e.Id.Value,
                e.Nombre.Value
            )));
        }

        // ✅ GET: api/EstadoPago/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id, CancellationToken ct)
        {
            var estado = await _repository.GetByIdAsync(new IdVO(id), ct);
            if (estado == null)
                return NotFound($"No se encontró el estado con ID {id}.");

            return Ok(new EstadoPagoDto(
                estado.Id.Value,
                estado.Nombre.Value
            ));
        }

        // ✅ POST: api/EstadoPago
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateEstadoPagoDto dto, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(dto.Nombre))
                return BadRequest("El nombre del estado es obligatorio.");

            var existente = await _repository.GetByNombreAsync(new NombreVO(dto.Nombre), ct);
            if (existente != null)
                return Conflict("Ya existe un estado con ese nombre.");

            var nuevoEstado = new EstadoPago
            {
                Id = IdVO.CreateNew(),
                Nombre = new NombreVO(dto.Nombre)
            };

            var idGenerado = await _repository.AddAsync(nuevoEstado, ct);
            return CreatedAtAction(nameof(GetById), new { id = idGenerado }, dto);
        }

        // ✅ PUT: api/EstadoPago/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateEstadoPagoDto dto, CancellationToken ct)
        {
            var estado = await _repository.GetByIdAsync(new IdVO(id), ct);
            if (estado == null)
                return NotFound($"No se encontró el estado con ID {id}.");

            estado.Nombre = new NombreVO(dto.Nombre);

            var actualizado = await _repository.UpdateAsync(estado, ct);
            if (!actualizado)
                return StatusCode(500, "Error al actualizar el estado de pago.");

            return Ok("Estado de pago actualizado correctamente.");
        }

        // ✅ DELETE: api/EstadoPago/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var eliminado = await _repository.DeleteAsync(new IdVO(id), ct);
            if (!eliminado)
                return NotFound($"No se encontró el estado con ID {id}.");

            return Ok("Estado de pago eliminado correctamente.");
        }
    }
}