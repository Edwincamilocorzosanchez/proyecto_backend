using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.DTOs
{
    public class OrdenServicioResponseDto
    {
        public Guid Id { get; set; }
        public Guid VehiculoId { get; set; }
        public Guid MecanicoId { get; set; }
        public Guid TipoServicioId { get; set; }
        public Guid EstadoId { get; set; }
        public DateTime FechaIngreso { get; set; }
        public DateTime FechaEntregaEstimada { get; set; }

        public string? VehiculoNombre { get; set; }
        public string? MecanicoNombre { get; set; }
        public string? TipoServicioNombre { get; set; }
        public string? EstadoNombre { get; set; }
    }
}