using Api.DTOs.Vehiculos;
using AutoMapper;
using Domain.Entities;
using Domain.ValueObjects;

namespace Api.Mappings;

public sealed class VehiculoProfile : Profile
{
    public VehiculoProfile()
    {
        // Entidad -> Response DTO (VehiculoDto)
        CreateMap<Vehiculo, VehiculoDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.Value))
            .ForMember(dest => dest.ClienteId, opt => opt.MapFrom(src => src.ClienteId.Value))
            .ForMember(dest => dest.Marca, opt => opt.MapFrom(src => src.Marca.Value))
            .ForMember(dest => dest.Modelo, opt => opt.MapFrom(src => src.Modelo.Value))
            .ForMember(dest => dest.Anio, opt => opt.MapFrom(src => src.Anio.Value))
            .ForMember(dest => dest.Vin, opt => opt.MapFrom(src => src.Vin.Value))
            .ForMember(dest => dest.Kilometraje, opt => opt.MapFrom(src => src.Kilometraje.Value))
            .ForMember(dest => dest.ClienteNombre, opt => opt.MapFrom(src => src.Cliente.Nombre.Value))
            .ForMember(dest => dest.ClienteCorreo, opt => opt.MapFrom(src => src.Cliente.Correo.Value));

        // Crear DTO -> Entidad
        CreateMap<CrearVehiculoDto, Vehiculo>()
            .AfterMap((src, dest) =>
            {
                dest.Id = new IdVO(0); // se generará automáticamente
                dest.Cliente = null!;  // se debe asignar después del fetch del cliente
                dest.Marca = new NombreVO(src.Marca);
                dest.Modelo = new NombreVO(src.Modelo);
                dest.Anio = new AnioVehiculoVO(src.Anio);
                dest.Vin = new VinVO(src.Vin);
                dest.Kilometraje = new KilometrajeVO(src.Kilometraje);
            });

        // Actualizar DTO -> Entidad
        CreateMap<UpdateVehiculoDto, Vehiculo>()
            .AfterMap((src, dest) =>
            {
                dest.Marca = new NombreVO(src.Marca);
                dest.Modelo = new NombreVO(src.Modelo);
                dest.Anio = new AnioVehiculoVO(src.Anio);
                dest.Vin = new VinVO(src.Vin);
                dest.Kilometraje = new KilometrajeVO(src.Kilometraje);
            })
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
    }
}
