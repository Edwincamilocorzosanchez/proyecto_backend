using Api.DTOs.TiposMovimiento;
using AutoMapper;
using Domain.Entities;
using Domain.ValueObjects;

namespace Api.Mappings;

public sealed class TipoMovimientoProfile : Profile
{
    public TipoMovimientoProfile()
    {
        // Entidad -> DTO de respuesta
        CreateMap<TipoMovimiento, TipoMovimientoDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.Value))
            .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.Nombre.Value));

        // Crear DTO -> Entidad
        CreateMap<CreateTipoMovimientoDto, TipoMovimiento>()
            .AfterMap((src, dest) =>
            {
                dest.Id = new IdVO(0); // se generará automáticamente
                dest.Nombre = new NombreVO(src.Nombre);
            });

        // Actualizar DTO -> Entidad
        CreateMap<UpdateTipoMovimientoDto, TipoMovimiento>()
            .AfterMap((src, dest) =>
            {
                dest.Nombre = new NombreVO(src.Nombre);
            });
    }
}
