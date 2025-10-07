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
    public class MetodoPagoMappingProfile : Profile
    {
        public MetodoPagoMappingProfile()
        {
            CreateMap<MetodoPago, MetodoPagoResponseDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.Value))
                .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.Nombre.Value));

            CreateMap<MetodoPagoRequestDto, MetodoPago>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(_ => new IdVO.CreateNew()))
                .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => new NombreVO(src.Nombre)));
        }
    }
}