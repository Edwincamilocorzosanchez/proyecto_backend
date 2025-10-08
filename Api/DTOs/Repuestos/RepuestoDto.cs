using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.DTOs
{
    public class RepuestoDto
    {
        public Guid Id { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public int CantidadStock { get; set; }
        public decimal PrecioUnitario { get; set; }
        public Guid? ProveedorId { get; set; }
    }
}