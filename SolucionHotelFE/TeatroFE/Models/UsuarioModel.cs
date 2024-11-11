namespace HotelFE.Models
{
    public class UsuarioModel
    {
        public string NombreUsuario { get; set; }
        public string Clave { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string CorreoRegistro { get; set; }
        public bool Estado { get; set; }

        public UsuarioModel()
        {
            NombreUsuario = string.Empty;
            Clave = string.Empty;
            FechaRegistro = DateTime.MinValue;
            CorreoRegistro = string.Empty;
            Estado = true;
        }
    }
}
