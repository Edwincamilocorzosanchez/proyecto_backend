
using Api.DTOs.Citas;
using AutoMapper;
using Domain.Entities;
using Domain.ValueObjects;

namespace Api.Mappings;

public sealed class CitaProfile : Profile
{
    public CitaProfile()
    {
        // Entidad ->  DTO
        CreateMap<Cita, CitaDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.Value))
            .ForMember(dest => dest.ClienteId, opt => opt.MapFrom(src => src.ClienteId.Value))
            .ForMember(dest => dest.ClienteNombre, opt => opt.MapFrom(src => src.Cliente.Nombre.Value))
            .ForMember(dest => dest.VehiculoId, opt => opt.MapFrom(src => src.VehiculoId.Value))
            .ForMember(dest => dest.VehiculoPlaca, opt => opt.MapFrom(src => src.Vehiculo.Vin.Value))
            .ForMember(dest => dest.FechaCita, opt => opt.MapFrom(src => src.FechaCita.Value))
            .ForMember(dest => dest.Motivo, opt => opt.MapFrom(src => src.Motivo != null ? src.Motivo.Value : null))
            .ForMember(dest => dest.EstadoId, opt => opt.MapFrom(src => src.EstadoId.Value))
            .ForMember(dest => dest.EstadoNombre, opt => opt.MapFrom(src => src.Estado.Nombre.Value))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt));

        // Crear DTO -> Entidad
        CreateMap<CrearCitaDto, Cita>()
            .ConstructUsing(src => new Cita(
                new IdVO(0), // se genera automáticamente
                new IdVO(src.ClienteId),
                new IdVO(src.VehiculoId),
                new FechaCitaVO(src.FechaCita),
                src.Motivo != null ? new DescripcionVO(src.Motivo) : null,
                new IdVO(src.EstadoId)
            ));

        // Actualizar DTO -> Entidad
        var actualizarCitaMap = CreateMap<ActualizarCitaDto, Cita>();
        actualizarCitaMap.ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
        actualizarCitaMap.AfterMap((src, dest) =>
        {
            if (src.FechaCita.HasValue)
                dest.FechaCita = new FechaCitaVO(src.FechaCita.Value);

            if (!string.IsNullOrWhiteSpace(src.Motivo))
                dest.Motivo = new DescripcionVO(src.Motivo!);

            if (src.EstadoId.HasValue)
                dest.EstadoId = new IdVO(src.EstadoId.Value);
        });
    }
}
