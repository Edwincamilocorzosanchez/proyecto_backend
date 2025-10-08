using Api.DTOs.Mecanicos;
using AutoMapper;
using Domain.Entities;
using Domain.ValueObjects;

namespace Api.Mappings;

public sealed class MecanicoProfile : Profile
{
    public MecanicoProfile()
    {
        // Entidad -> DTO de respuesta
        CreateMap<Mecanico, MecanicoDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.Value))
            .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.Nombre.Value))
            .ForMember(dest => dest.Telefono, opt => opt.MapFrom(src => src.Telefono != null ? src.Telefono.Value : null))
            .ForMember(dest => dest.Especialidad, opt => opt.MapFrom(src => src.Especialidad != null ? src.Especialidad.Value : null))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive.Value))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId));

        // Crear DTO -> Entidad
        CreateMap<CreateMecanicoDto, Mecanico>()
            .AfterMap((src, dest) =>
            {
                dest.Id = new IdVO(0); // se generará automáticamente
                dest.Nombre = new NombreVO(src.Nombre);
                dest.Telefono = src.Telefono != null ? new TelefonoVO(src.Telefono) : null;
                dest.Especialidad = src.Especialidad != null ? new EspecialidadVO(src.Especialidad) : null;
                dest.IsActive = new EstadoVO(src.IsActive);
                dest.UserId = src.UserId;
            });

        // Actualizar DTO -> Entidad
        CreateMap<UpdateMecanicoDto, Mecanico>()
            .AfterMap((src, dest) =>
            {
                if (src.Nombre != null) dest.Nombre = new NombreVO(src.Nombre);
                if (src.Telefono != null) dest.Telefono = new TelefonoVO(src.Telefono);
                if (src.Especialidad != null) dest.Especialidad = new EspecialidadVO(src.Especialidad);
                if (src.IsActive != null) dest.IsActive = new EstadoVO(src.IsActive);
            });
    }
}
