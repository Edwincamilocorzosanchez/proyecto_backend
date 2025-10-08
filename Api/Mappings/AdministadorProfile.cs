using Api.DTOs.Administradores;
using AutoMapper;
using Domain.Entities;
using Domain.ValueObjects;

namespace Api.Mappings;

public sealed class AdministradorProfile : Profile
{
    public AdministradorProfile()
    {
        // ==========================================================
        // ✅ Entidad ->  DTO
        // ==========================================================
        CreateMap<Administrador, AdministradorDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.Value))
            .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.Nombre.Value))
            .ForMember(dest => dest.Telefono, opt => opt.MapFrom(src => src.Telefono.Value))
            .ForMember(dest => dest.NivelAcceso, opt => opt.MapFrom(src => src.NivelAcceso.Value))
            .ForMember(dest => dest.AreaResponsabilidad, opt => opt.MapFrom(src => src.AreaResponsabilidad.Value))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive.Value))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt));

        // ==========================================================
        // ✅ Create DTO -> Entidad
        // ==========================================================
        CreateMap<CreateAdministradorDto, Administrador>()
            .ConstructUsing(src => new Administrador(
                new IdVO(0), // se genera automáticamente (por EF o dominio)
                new NombreVO(src.Nombre),
                new TelefonoVO(src.Telefono),
                new NivelAccesoVO(src.NivelAcceso),
                new DescripcionVO(src.AreaResponsabilidad),
                new EstadoVO(src.IsActive),
                src.UserId
            ));

        // ==========================================================
        // ✅ Update DTO -> Entidad (solo para actualizar campos no nulos)
        // ==========================================================
        var updateMap = CreateMap<UpdateAdministradorDto, Administrador>();
        updateMap.ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
        updateMap.AfterMap((src, dest) =>
        {
            if (src.Nombre != null)
                dest.Nombre = new NombreVO(src.Nombre);

            if (src.Telefono != null)
                dest.Telefono = new TelefonoVO(src.Telefono);

            if (src.NivelAcceso != null)
                dest.NivelAcceso = new NivelAccesoVO(src.NivelAcceso);

            if (src.AreaResponsabilidad != null)
                dest.AreaResponsabilidad = new DescripcionVO(src.AreaResponsabilidad);

            if (src.IsActive.HasValue)
                dest.IsActive = new EstadoVO(src.IsActive.Value);
        });
    }
}
