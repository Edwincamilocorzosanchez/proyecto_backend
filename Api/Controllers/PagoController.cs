using Api.DTOs.Pagos;
using Api.Services.Implementations;
using Application.Pagos;
using Domain.Entities;
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
            var pago = new Pago
            {
                Id = IdVO.CreateNew(),
                FacturaId = new IdVO(dto.FacturaId),
                MetodoPagoId = new IdVO(dto.MetodoPagoId),
                EstadoPagoId = new IdVO(dto.EstadoPagoId),
                Monto = new DineroVO(dto.Monto),
                FechaPago = new FechaHistoricaVO(dto.FechaPago)
            };
        
            await _service.AddAsync(pago);
            return Ok("Pago registrado exitosamente.");
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdatePagoDto dto)
        {
            var result = await _service.UpdateAsync(id, dto);

            if (!result)
                return BadRequest("No se pudo actualizar el pago.");

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