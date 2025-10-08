namespace Api.DTOs.Citas;

// DTo para crear una cita
public sealed record CrearCitaDto(
    int ClienteId,
    int VehiculoId,
    DateTime FechaCita,
    string? Motivo,
    int EstadoId
);
