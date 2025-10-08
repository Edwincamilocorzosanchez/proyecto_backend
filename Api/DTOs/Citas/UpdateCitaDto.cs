namespace Api.DTOs.Citas;

public sealed record ActualizarCitaDto(
    DateTime? FechaCita,
    string? Motivo,
    int? EstadoId
);
