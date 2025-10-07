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
//     public class OrdenServicioService
//     {
//         private readonly IOrdenServicioRepository _repository;

//         public OrdenServicioService(IOrdenServicioRepository repository)
//         {
//             _repository = repository;
//         }
    
//         public async Task<OrdenServicioResponseDto> CrearAsync(OrdenServicioRequestDto dto)
//         {
//             var orden = new OrdenServicio(
//                 new IdVO(Guid.NewGuid()),
//                 new Vehiculo { Id = new IdVO(dto.VehiculoId) },
//                 new Mecanico { Id = new IdVO(dto.MecanicoId) },
//                 new TipoServicio { Id = new IdVO(dto.TipoServicioId) },
//                 new EstadoOrden { Id = new IdVO(dto.EstadoId) },
//                 new FechaHistoricaVO(dto.FechaIngreso),
//                 new FechaHistoricaVO(dto.FechaEntregaEstimada)
//             );
    
//             await _repository.AddAsync(orden);
    
//             return new OrdenServicioResponseDto
//             {
//                 Id = orden.Id.Value,
//                 VehiculoId = orden.Vehiculo.Id.Value,
//                 MecanicoId = orden.Mecanico.Id.Value,
//                 TipoServicioId = orden.TipoServicio.Id.Value,
//                 EstadoId = orden.Estado.Id.Value,
//                 FechaIngreso = orden.FechaIngreso.Value,
//                 FechaEntregaEstimada = orden.FechaEntregaEstimada.Value
//             };
//         }
    
//         public async Task<IEnumerable<OrdenServicioResponseDto>> ObtenerTodosAsync()
//         {
//             var ordenes = await _repository.GetAllAsync();
    
//             return ordenes.Select(o => new OrdenServicioResponseDto
//             {
//                 Id = o.Id.Value,
//                 VehiculoId = o.Vehiculo.Id.Value,
//                 MecanicoId = o.Mecanico.Id.Value,
//                 TipoServicioId = o.TipoServicio.Id.Value,
//                 EstadoId = o.Estado.Id.Value,
//                 FechaIngreso = o.FechaIngreso.Value,
//                 FechaEntregaEstimada = o.FechaEntregaEstimada.Value,
//                 VehiculoNombre = o.Vehiculo?.ToString(),
//                 MecanicoNombre = o.Mecanico?.ToString(),
//                 TipoServicioNombre = o.TipoServicio?.ToString(),
//                 EstadoNombre = o.Estado?.ToString()
//             });
//         }
    
//         public async Task<OrdenServicioResponseDto?> ObtenerPorIdAsync(Guid id)
//         {
//             var orden = await _repository.GetByIdAsync(id);
//             if (orden == null) return null;
    
//             return new OrdenServicioResponseDto
//             {
//                 Id = orden.Id.Value,
//                 VehiculoId = orden.Vehiculo.Id.Value,
//                 MecanicoId = orden.Mecanico.Id.Value,
//                 TipoServicioId = orden.TipoServicio.Id.Value,
//                 EstadoId = orden.Estado.Id.Value,
//                 FechaIngreso = orden.FechaIngreso.Value,
//                 FechaEntregaEstimada = orden.FechaEntregaEstimada.Value,
//                 VehiculoNombre = orden.Vehiculo?.ToString(),
//                 MecanicoNombre = orden.Mecanico?.ToString(),
//                 TipoServicioNombre = orden.TipoServicio?.ToString(),
//                 EstadoNombre = orden.Estado?.ToString()
//             };
//         }
    
//         public async Task ActualizarAsync(Guid id, OrdenServicioRequestDto dto)
//         {
//             var orden = await _repository.GetByIdAsync(id);
//             if (orden == null)
//                 throw new Exception("Orden de servicio no encontrada.");
    
//             orden.FechaIngreso = new FechaHistoricaVO(dto.FechaIngreso);
//             orden.FechaEntregaEstimada = new FechaHistoricaVO(dto.FechaEntregaEstimada);
    
//             await _repository.UpdateAsync(orden);
//         }
    
//         public async Task EliminarAsync(Guid id)
//         {
//             await _repository.DeleteAsync(id);
//         }
//     }
// }