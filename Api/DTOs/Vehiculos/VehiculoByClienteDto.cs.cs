namespace Api.DTOs.Vehiculos;

public sealed record VehiculoByClienteDto(
    Guid Id,
    string Marca, 
    string Modelo, 
    int Anio,
    string Vin,
    int Kilometraje
);