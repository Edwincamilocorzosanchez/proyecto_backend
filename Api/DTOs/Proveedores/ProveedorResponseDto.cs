using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.DTOs
{
    public class ProveedorResponseDto
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string? Telefono { get; set; }
        public string? Correo { get; set; }
        public string? Direccion { get; set; }
        public bool IsActive { get; set; }
        public List<RepuestoDto>? Repuestos { get; set; }
    }
}