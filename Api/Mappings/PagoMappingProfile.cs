// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;
// using Api.DTOs;
// using AutoMapper;
// using Domain.Entities;
// using Domain.ValueObjects;

// namespace Api.Mappings
// {
//     public class PagoMappingProfile : Profile
//     {
//         public PagoMappingProfile()
//         {
//             CreateMap<Pago, PagoResponseDto>()
//                 .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.Value))
//                 .ForMember(dest => dest.FacturaId, opt => opt.MapFrom(src => src.FacturaId.Value))
//                 .ForMember(dest => dest.MetodoPagoId, opt => opt.MapFrom(src => src.MetodoPagoId.Value))
//                 .ForMember(dest => dest.EstadoPagoId, opt => opt.MapFrom(src => src.EstadoPagoId.Value))
//                 .ForMember(dest => dest.Monto, opt => opt.MapFrom(src => src.Monto.Value))
//                 .ForMember(dest => dest.FechaPago, opt => opt.MapFrom(src => src.FechaPago.Value));

//             CreateMap<PagoRequestDto, Pago>()
//                 .ForMember(dest => dest.Id, opt => opt.MapFrom(_ => new IdVO(Guid.NewGuid())))
//                 .ForMember(dest => dest.FacturaId, opt => opt.MapFrom(src => new IdVO(src.FacturaId)))
//                 .ForMember(dest => dest.MetodoPagoId, opt => opt.MapFrom(src => new IdVO(src.MetodoPagoId)))
//                 .ForMember(dest => dest.EstadoPagoId, opt => opt.MapFrom(src => new IdVO(src.EstadoPagoId)))
//                 .ForMember(dest => dest.Monto, opt => opt.MapFrom(src => new DineroVO(src.Monto)))
//                 .ForMember(dest => dest.FechaPago, opt => opt.MapFrom(src => new FechaHistoricaVO(src.FechaPago)));
//         }
//     }
// }