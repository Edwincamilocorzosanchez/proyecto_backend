namespace Api.DTOs.Repuestos;

public record CreateRepuestoDto(
    string Codigo,
    string Descripcion,
    int CantidadStock,
    decimal PrecioUnitario,
    Guid? ProveedorId
);
