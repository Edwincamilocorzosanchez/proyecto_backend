using Api.DTOs.DetallesOrden;
using AutoMapper;
using Domain.Entities;
using Domain.ValueObjects;

namespace Api.Mappings;

public sealed class DetalleOrdenProfile : Profile
{
    public DetalleOrdenProfile()
    {
        // Entidad -> DTO
        CreateMap<DetalleOrden, DetalleOrdenDto>()
            .ForMember(dest => dest.OrdenServicioId, opt => opt.MapFrom(src => src.OrdenServicioId.Value))
            .ForMember(dest => dest.RepuestoId, opt => opt.MapFrom(src => src.RepuestoId.Value))
            .ForMember(dest => dest.Cantidad, opt => opt.MapFrom(src => src.Cantidad.Value))
            .ForMember(dest => dest.Costo, opt => opt.MapFrom(src => src.Costo.Value));

        // Crear DTO -> Entidad
        CreateMap<CreateDetalleOrdenDto, DetalleOrden>()
            .AfterMap((src, dest) =>
            {
                dest.OrdenServicioId = new IdVO(src.OrdenServicioId);
                dest.RepuestoId = new IdVO(src.RepuestoId);
                dest.Cantidad = new CantidadVO(src.Cantidad);
                dest.Costo = new DineroVO(src.Costo);
            });

        // Actualizar DTO -> Entidad
        CreateMap<UpdateDetalleOrdenDto, DetalleOrden>()
            .AfterMap((src, dest) =>
            {
                dest.Cantidad = new CantidadVO(src.Cantidad);
                dest.Costo = new DineroVO(src.Costo);
            });
    }
}
