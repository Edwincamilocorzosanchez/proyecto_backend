namespace Api.DTOs.Clientes;

public sealed record CreateClienteDto(
    string Telefono,
    string Direccion,
    bool IsActive,
    int UserId
);
