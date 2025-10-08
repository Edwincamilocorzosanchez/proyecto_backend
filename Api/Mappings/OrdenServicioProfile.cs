using Api.DTOs.OrdenesServicio;
using Api.DTOs.Vehiculos;
using Api.DTOs.Mecanicos;
using Api.DTOs.TiposServicio;
using Api.DTOs.EstadosOrden;
using Api.DTOs.DetallesOrden;
using Api.DTOs.Facturas;
using AutoMapper;
using Domain.Entities;
using Domain.ValueObjects;

namespace Api.Mappings;

public sealed class OrdenServicioProfile : Profile
{
    public OrdenServicioProfile()
    {
        // Entidad -> DTO simple
        CreateMap<OrdenServicio, OrdenServicioDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.Value))
            .ForMember(dest => dest.VehiculoId, opt => opt.MapFrom(src => src.VehiculoId.Value))
            .ForMember(dest => dest.MecanicoId, opt => opt.MapFrom(src => src.MecanicoId.Value))
            .ForMember(dest => dest.TipoServicioId, opt => opt.MapFrom(src => src.TipoServicioId.Value))
            .ForMember(dest => dest.EstadoId, opt => opt.MapFrom(src => src.EstadoId.Value))
            .ForMember(dest => dest.FechaIngreso, opt => opt.MapFrom(src => src.FechaIngreso.Value))
            .ForMember(dest => dest.FechaEntregaEstimada, opt => opt.MapFrom(src => src.FechaEntregaEstimada.Value));

        // Entidad -> DTO detallado
        CreateMap<OrdenServicio, OrdenServicioDetailDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.Value))
            .ForMember(dest => dest.FechaIngreso, opt => opt.MapFrom(src => src.FechaIngreso.Value))
            .ForMember(dest => dest.FechaEntregaEstimada, opt => opt.MapFrom(src => src.FechaEntregaEstimada.Value))
            .ForMember(dest => dest.Vehiculo, opt => opt.MapFrom(src => src.Vehiculo))
            .ForMember(dest => dest.Mecanico, opt => opt.MapFrom(src => src.Mecanico))
            .ForMember(dest => dest.TipoServicio, opt => opt.MapFrom(src => src.TipoServicio))
            .ForMember(dest => dest.Estado, opt => opt.MapFrom(src => src.Estado))
            // Mapeo de colecciones relacionadas usando los DTOs de cada entidad
            .ForMember(dest => dest.Detalles, opt => opt.MapFrom(src => src.Detalles))
            .ForMember(dest => dest.Facturas, opt => opt.MapFrom(src => src.Facturas));

        // Crear DTO -> Entidad
        CreateMap<CreateOrdenServicioDto, OrdenServicio>()
            .AfterMap((src, dest) =>
            {
                dest.Id = new IdVO(0); // generado automáticamente
                dest.VehiculoId = new IdVO(src.VehiculoId);
                dest.MecanicoId = new IdVO(src.MecanicoId);
                dest.TipoServicioId = new IdVO(src.TipoServicioId);
                dest.EstadoId = new IdVO(src.EstadoId);
                dest.FechaIngreso = new FechaHistoricaVO(src.FechaIngreso);
                dest.FechaEntregaEstimada = new FechaHistoricaVO(src.FechaEntregaEstimada);
            });

        // Actualizar DTO -> Entidad
        CreateMap<UpdateOrdenServicioDto, OrdenServicio>()
            .AfterMap((src, dest) =>
            {
                dest.VehiculoId = new IdVO(src.VehiculoId);
                dest.MecanicoId = new IdVO(src.MecanicoId);
                dest.TipoServicioId = new IdVO(src.TipoServicioId);
                dest.EstadoId = new IdVO(src.EstadoId);
                dest.FechaIngreso = new FechaHistoricaVO(src.FechaIngreso);
                dest.FechaEntregaEstimada = new FechaHistoricaVO(src.FechaEntregaEstimada);
            });
    }
}
