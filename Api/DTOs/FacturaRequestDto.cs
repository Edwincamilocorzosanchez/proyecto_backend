using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.DTOs
{
    public class FacturaRequestDto
    {
        public Guid OrdenServicioId { get; set; }
        public decimal MontoRepuestos { get; set; }
        public decimal ManoObra { get; set; }
        public decimal Total { get; set; }
        public DateTime FechaGeneracion { get; set; }
    }
}