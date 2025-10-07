using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.DTOs
{
    public class HistorialInventarioDto
    {
        public Guid Id { get; set; }
        public Guid RepuestoId { get; set; }
        public Guid? AdminId { get; set; }
        public Guid TipoMovimientoId { get; set; }
        public int Cantidad { get; set; }
        public DateTime FechaMovimiento { get; set; }
        public string? Observaciones { get; set; }
    }
}