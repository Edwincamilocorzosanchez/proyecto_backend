using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.DTOs
{
    public class AdministradorResponseDto
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string? Telefono { get; set; }
        public string? NivelAcceso { get; set; }
        public string? AreaResponsabilidad { get; set; }
        public bool IsActive { get; set; }
    }
}