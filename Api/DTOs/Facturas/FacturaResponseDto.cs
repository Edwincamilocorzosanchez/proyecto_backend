using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.DTOs
{
    public class FacturaResponseDto
    {
        public Guid Id { get; set; }
        public Guid OrdenServicioId { get; set; }
        public decimal MontoRepuestos { get; set; }
        public decimal ManoObra { get; set; }
        public decimal Total { get; set; }
        public DateTime FechaGeneracion { get; set; }

        public List<PagoResponseDto>? Pagos { get; set; }
        public OrdenServicioResponseDto? OrdenServicio { get; set; }
    }
}