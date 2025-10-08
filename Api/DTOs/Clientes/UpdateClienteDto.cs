namespace Api.DTOs.Clientes;

public sealed record UpdateClienteDto(
    string Telefono,
    string Direccion,
    bool IsActive
);
