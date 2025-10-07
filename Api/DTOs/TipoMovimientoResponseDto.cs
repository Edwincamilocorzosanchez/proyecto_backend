using System;

namespace Application.DTOs
{
    public class TipoMovimientoResponseDto
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
    }
}
