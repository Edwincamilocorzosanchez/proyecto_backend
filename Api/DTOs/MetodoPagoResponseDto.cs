using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.DTOs
{
    public class MetodoPagoResponseDto
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; } = null!;
    }
}