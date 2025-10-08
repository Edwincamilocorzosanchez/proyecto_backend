namespace Api.DTOs.Proveedores;

public record ProveedorDto(
    int Id,
    string Nombre,
    string? Telefono,
    string? Correo,
    string? Direccion,
    bool IsActive,
    int UserId
);
