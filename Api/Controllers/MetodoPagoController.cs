// using Api.DTOs;
// using Application.DTOs;
// using Microsoft.AspNetCore.Mvc;


// namespace Api.Controllers
// {
//      [ApiController]
//     [Route("api/[controller]")]
//     public class MetodoPagoController : ControllerBase
//     {
//         private readonly MetodoPagoService _service;

//         public MetodoPagoController(MetodoPagoService service)
//         {
//             _service = service;
//         }

//         [HttpGet]
//         public async Task<IActionResult> GetAll()
//         {
//             var result = await _service.GetAllAsync();
//             return Ok(result);
//         }

//         [HttpGet("{id}")]
//         public async Task<IActionResult> GetById(Guid id)
//         {
//             var result = await _service.GetByIdAsync(id);
//             return result is null ? NotFound() : Ok(result);
//         }

//         [HttpPost]
//         public async Task<IActionResult> Create([FromBody] MetodoPagoRequestDto dto)
//         {
//             await _service.AddAsync(dto);
//             return Ok("Método de pago registrado exitosamente.");
//         }

//         [HttpPut("{id}")]
//         public async Task<IActionResult> Update(Guid id, [FromBody] MetodoPagoRequestDto dto)
//         {
//             await _service.UpdateAsync(id, dto);
//             return Ok("Método de pago actualizado correctamente.");
//         }

//         [HttpDelete("{id}")]
//         public async Task<IActionResult> Delete(Guid id)
//         {
//             await _service.DeleteAsync(id);
//             return Ok("Método de pago eliminado.");
//         }
//     }
// }