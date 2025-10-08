namespace Api.DTOs.Citas;

public sealed record CitaResponseDto(
    int Id,
    int ClienteId,
    string ClienteNombre,
    int VehiculoId,
    string VehiculoPlaca,
    DateTime FechaCita,
    string? Motivo,
    int EstadoId,
    string EstadoNombre,
    DateTime CreatedAt,
    DateTime UpdatedAt
);
