namespace Api.DTOs.Clientes;

public sealed record ClienteDto(
    int Id,
    string Telefono,
    string Direccion,
    bool IsActive,
    int UserId
);
