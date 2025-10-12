namespace Api.DTOs.DetallesOrden;

public sealed record DetalleOrdenDto(
    int Id,
    int OrdenServicioId,
    int RepuestoId,
    int Cantidad,
    decimal Costo
);
