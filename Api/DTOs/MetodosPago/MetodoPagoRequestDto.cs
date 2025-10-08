using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.DTOs
{
    public class MetodoPagoRequestDto
    {
        public string Nombre { get; set; } = null!;
        public bool Activo { get; set; }
    }
}