namespace Api.DTOs.Proveedores;

public record UpdateProveedorDto(
    string Nombre,
    string? Telefono,
    string? Correo,
    string? Direccion,
    bool IsActive,
    int UserId
);
