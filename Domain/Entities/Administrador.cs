using System.Text.Json.Serialization.Metadata;
using Domain.Entities.Auth;
using Domain.ValueObjects;

namespace Domain.Entities;

public class Administrador : BaseEntity
{
    public IdVO Id { get; set; } = null!;
    public NombreVO Nombre { get; set; } = null!;
    public TelefonoVO Telefono { get; set; } = null!;
    public NivelAccesoVO NivelAcceso { get; set; } = null!;
    public DescripcionVO AreaResponsabilidad { get; set; } = null!;
    public EstadoVO IsActive { get; set; } = null!;

    // Clave foránea real (int)
    public int UserId { get; set; }
    // Relación con el usuario del sistema (autenticación)
    public UserMember User { get; set; } = null!;
    // constructores 
    public Administrador() { }
    public Administrador(IdVO id, NombreVO nombre, TelefonoVO telefono, NivelAccesoVO nivelAcceso, DescripcionVO areaResponsabilidad, EstadoVO isActive, int userId)
    {
        Id = id;
        Nombre = nombre;
        Telefono = telefono;
        NivelAcceso = nivelAcceso;
        AreaResponsabilidad = areaResponsabilidad;
        IsActive = isActive;
        UserId = userId;
    }
}
