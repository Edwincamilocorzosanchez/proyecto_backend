using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.DTOs
{
    public class OrdenServicioRequestDto
    {
        public Guid VehiculoId { get; set; }
        public Guid MecanicoId { get; set; }
        public Guid TipoServicioId { get; set; }
        public Guid EstadoId { get; set; }
        public DateTime FechaIngreso { get; set; }
        public DateTime FechaEntregaEstimada { get; set; }
    }
}