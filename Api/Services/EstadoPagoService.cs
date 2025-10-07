// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;
// using Api.DTOs;
// using Application.Abstractions;
// using Domain.Entities;
// using Domain.ValueObjects;

// namespace Api.Services
// {
// public class EstadoPagoService
// {
//     private readonly IEstadoPagoRepository _repository;

//     public EstadoPagoService(IEstadoPagoRepository repository)
//     {
//         _repository = repository;
//     }

//     public async Task<EstadoPagoResponseDto> CrearAsync(EstadoPagoRequestDto dto)
//     {
//         var estado = new EstadoPago(
//             new IdVO(Guid.NewGuid()),
//             new NombreVO(dto.Nombre)
//         );

//         await _repository.AddAsync(estado);

//         return new EstadoPagoResponseDto
//         {
//             Id = estado.Id.Value,
//             Nombre = estado.Nombre.Value
//         };
//     }

//     public async Task<IEnumerable<EstadoPagoResponseDto>> ObtenerTodosAsync()
//     {
//         var estados = await _repository.GetAllAsync();

//         return estados.Select(e => new EstadoPagoResponseDto
//         {
//             Id = e.Id.Value,
//             Nombre = e.Nombre.Value
//         });
//     }

//     public async Task<EstadoPagoResponseDto?> ObtenerPorIdAsync(Guid id)
//     {
//         var estado = await _repository.GetByIdAsync(id);
//         if (estado == null) return null;

//         return new EstadoPagoResponseDto
//         {
//             Id = estado.Id.Value,
//             Nombre = estado.Nombre.Value
//         };
//     }

//     public async Task ActualizarAsync(Guid id, EstadoPagoRequestDto dto)
//     {
//         var estado = await _repository.GetByIdAsync(id);
//         if (estado == null)
//             throw new Exception("Estado de pago no encontrado.");

//         estado.Nombre = new NombreVO(dto.Nombre);

//         await _repository.UpdateAsync(estado);
//     }

//     public async Task EliminarAsync(Guid id)
//     {
//         await _repository.DeleteAsync(id);
//     }
// }
// }