namespace Api.DTOs.HistorialInventario;

public record UpdateHistorialInventarioDto(
    int Cantidad,
    DateTime FechaMovimiento,
    string? Observaciones
);
