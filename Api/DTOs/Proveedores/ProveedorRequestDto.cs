namespace Api.DTOs
{
    public class ProveedorRequestDto
    {
        public string Nombre { get; set; } = string.Empty;
        public string? Telefono { get; set; }
        public string? Correo { get; set; }
        public string? Direccion { get; set; }
        public bool IsActive { get; set; }
    }
}
