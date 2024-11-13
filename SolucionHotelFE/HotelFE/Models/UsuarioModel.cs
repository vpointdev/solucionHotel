namespace HotelFE.Models
{
    public class UsuarioModel
    {
        public int UsuarioId { get; set; }

        public string NombreUsuario { get; set; }


        public string? Clave { get; set; }


        public string CorreoRegistro { get; set; }


        public DateTime FechaRegistro { get; set; }

        public bool Estado { get; set; }

        public UsuarioModel()
        {
            FechaRegistro = DateTime.Now;
            Estado = true;
        }
    }
}
