namespace Api.DTOs.Vehiculos;

// POST /api/vehiculos
public sealed record CrearVehiculoDto(
    int ClienteId,
    string Marca,
    string Modelo,
    int Anio,
    string Vin,
    int Kilometraje
);
