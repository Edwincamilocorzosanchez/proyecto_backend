namespace Api.DTOs.Administradores;

public sealed record AdministradorResponseDto(
    int Id,
    string Nombre,
    string Telefono,
    string NivelAcceso,
    string AreaResponsabilidad,
    bool IsActive,
    int UserId,
    DateTime CreatedAt,
    DateTime UpdatedAt
);