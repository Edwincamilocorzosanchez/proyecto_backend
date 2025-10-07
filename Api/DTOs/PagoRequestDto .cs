using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.DTOs
{
    public class PagoRequestDto 
    {
        public Guid FacturaId { get; set; }
        public Guid MetodoPagoId { get; set; }
        public Guid EstadoPagoId { get; set; }
        public decimal Monto { get; set; }
        public DateTime FechaPago { get; set; }
    }
}