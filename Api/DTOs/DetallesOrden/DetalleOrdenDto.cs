namespace Api.DTOs.DetallesOrden;

public sealed record DetalleOrdenDto(
    int OrdenServicioId,
    int RepuestoId,
    int Cantidad,
    decimal Costo
);
