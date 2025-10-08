namespace Api.DTOs.Vehiculos;

// GET /api/vehiculos
// GET /api/vehiculos/{id}
// GET /api/vehiculos/vin/{vin}
// GET /api/vehiculos/cliente/{clienteId}
public sealed record VehiculoDto(
    Guid Id, 
    Guid ClienteId, 
    string Marca, 
    string Modelo,  
    int Anio, 
    string Vin,  
    int Kilometraje, 
    // Información resumida del cliente
    string? ClienteNombre, 
    string? ClienteCorreo
);