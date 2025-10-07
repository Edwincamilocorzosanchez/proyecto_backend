using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.DTOs;
using AutoMapper;
using Domain.Entities;
using Domain.ValueObjects;

namespace Api.Mappings
{
    public class FacturaMappingProfile : Profile
    {
        public FacturaMappingProfile()
        {
            CreateMap<Factura, FacturaResponseDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.Value))
                .ForMember(dest => dest.OrdenServicioId, opt => opt.MapFrom(src => src.OrdenServicioId.Value))
                .ForMember(dest => dest.MontoRepuestos, opt => opt.MapFrom(src => src.MontoRepuestos.Value))
                .ForMember(dest => dest.ManoObra, opt => opt.MapFrom(src => src.ManoObra.Value))
                .ForMember(dest => dest.Total, opt => opt.MapFrom(src => src.Total.Value))
                .ForMember(dest => dest.FechaGeneracion, opt => opt.MapFrom(src => src.FechaGeneracion.Value));

            CreateMap<FacturaRequestDto, Factura>()
                .ConstructUsing(dto => new Factura(
                    new IdVO(Guid.NewGuid()),
                    new IdVO(dto.OrdenServicioId),
                    new DineroVO(dto.MontoRepuestos),
                    new DineroVO(dto.ManoObra),
                    new DineroVO(dto.Total),
                    new FechaHistoricaVO(dto.FechaGeneracion)
                ));
    }
}
}