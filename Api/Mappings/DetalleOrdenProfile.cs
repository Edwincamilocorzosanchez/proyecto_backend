using Api.DTOs.DetallesOrden;
using AutoMapper;
using Domain.Entities;
using Domain.ValueObjects;

namespace Api.Mappings;

public sealed class DetalleOrdenProfile : Profile
{
    public DetalleOrdenProfile()
    {
        // ✅ Entidad -> DTO
        CreateMap<DetalleOrden, DetalleOrdenDto>()
            .ForMember(dest => dest.OrdenServicioId, opt => opt.MapFrom(src => src.OrdenServicioId.Value))
            .ForMember(dest => dest.RepuestoId, opt => opt.MapFrom(src => src.RepuestoId.Value))
            .ForMember(dest => dest.Cantidad, opt => opt.MapFrom(src => src.Cantidad.Value))
            .ForMember(dest => dest.Costo, opt => opt.MapFrom(src => src.Costo.Value));

        // ✅ Crear DTO -> Entidad (nuevo registro)
        CreateMap<CreateDetalleOrdenDto, DetalleOrden>()
            .ConstructUsing(src => new DetalleOrden(
                new IdVO(src.OrdenServicioId),
                new IdVO(src.RepuestoId),
                new CantidadVO(src.Cantidad),
                new DineroVO(src.Costo)
            ));

        // ✅ Actualizar DTO -> Entidad (actualización)
        CreateMap<UpdateDetalleOrdenDto, DetalleOrden>()
            .AfterMap((src, dest) =>
            {
                if (src.Cantidad > 0)
                    dest.Cantidad = new CantidadVO(src.Cantidad);

                if (src.Costo > 0)
                    dest.Costo = new DineroVO(src.Costo);
            });
    }
}
