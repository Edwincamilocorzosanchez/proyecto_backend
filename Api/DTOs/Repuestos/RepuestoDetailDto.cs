namespace Api.DTOs.Repuestos;

public record RepuestoDetailDto(
    Guid Id,
    string Codigo,
    string Descripcion,
    int CantidadStock,
    decimal PrecioUnitario,
    Guid? ProveedorId,
    string? ProveedorNombre,
    IEnumerable<string>? Historiales,
    IEnumerable<string>? DetallesOrden
);
