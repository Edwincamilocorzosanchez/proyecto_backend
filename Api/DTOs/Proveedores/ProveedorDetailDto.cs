using Api.DTOs.Repuestos;

namespace Api.DTOs.Proveedores;

public record ProveedorDetailDto(
    int Id,
    string Nombre,
    string? Telefono,
    string? Correo,
    string? Direccion,
    bool IsActive,
    int UserId,
    string? UserName,
    IEnumerable<RepuestoDto>? Repuestos
);
