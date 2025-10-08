using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.DTOs
{
    public class PagoResponseDto
    {
        public Guid Id { get; set; }
        public Guid FacturaId { get; set; }
        public Guid MetodoPagoId { get; set; }
        public Guid EstadoPagoId { get; set; }
        public decimal Monto { get; set; }
        public DateTime FechaPago { get; set; }
    }
}