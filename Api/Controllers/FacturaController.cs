// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;
// using Api.DTOs;
// using Application.Abstractions;
// using AutoMapper;
// using Domain.Entities;
// using Domain.ValueObjects;
// using Microsoft.AspNetCore.Mvc;

// namespace Api.Controllers
// {
//     [ApiController]
//     [Route("api/[controller]")]
//     public class FacturaController : ControllerBase
//     {
//         private readonly IFacturaRepository _facturaRepository;
//         private readonly IMapper _mapper;

//         public FacturaController(IFacturaRepository facturaRepository, IMapper mapper)
//         {
//             _facturaRepository = facturaRepository;
//             _mapper = mapper;
//         }

//         [HttpGet]
//         public async Task<ActionResult<IEnumerable<FacturaResponseDto>>> GetAll()
//         {
//             var facturas = await _facturaRepository.GetAllAsync();
//             return Ok(_mapper.Map<IEnumerable<FacturaResponseDto>>(facturas));
//         }

//         [HttpGet("{id:guid}")]
//         public async Task<ActionResult<FacturaResponseDto>> GetById(Guid id)
//         {
//             var factura = await _facturaRepository.GetByIdAsync(id);
//             if (factura == null) return NotFound();
//             return Ok(_mapper.Map<FacturaResponseDto>(factura));
//         }

//         [HttpPost]
//         public async Task<ActionResult> Create([FromBody] FacturaRequestDto dto)
//         {
//             var factura = _mapper.Map<Factura>(dto);
//             await _facturaRepository.AddAsync(factura);
//             return CreatedAtAction(nameof(GetById), new { id = factura.Id.Value }, _mapper.Map<FacturaResponseDto>(factura));
//         }

//         [HttpPut("{id:guid}")]
//         public async Task<ActionResult> Update(Guid id, [FromBody] FacturaRequestDto dto)
//         {
//             var factura = await _facturaRepository.GetByIdAsync(id);
//             if (factura == null) return NotFound();

//             factura.MontoRepuestos = new DineroVO(dto.MontoRepuestos);
//             factura.ManoObra = new DineroVO(dto.ManoObra);
//             factura.Total = new DineroVO(dto.Total);
//             factura.FechaGeneracion = new FechaHistoricaVO(dto.FechaGeneracion);

//             await _facturaRepository.UpdateAsync(factura);
//             return NoContent();
//         }

//         [HttpDelete("{id:guid}")]
//         public async Task<ActionResult> Delete(Guid id)
//         {
//             await _facturaRepository.DeleteAsync(id);
//             return NoContent();
//         }
//     }
// }