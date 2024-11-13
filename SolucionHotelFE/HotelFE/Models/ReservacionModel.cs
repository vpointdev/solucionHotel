namespace HotelFE.Models
{
    public class ReservacionModel
    {
        public int ReservacionId { get; set; }
        public string CodigoReservacion { get; set; }
        public int UsuarioId { get; set; }
        public int HabitacionId { get; set; }
        public DateTime FechaEntrada { get; set; }
        public DateTime FechaSalida { get; set; }
        public string EstadoReservacion { get; set; }
        public decimal PrecioTotal { get; set; }
        public string Observaciones { get; set; }

        // Navigation properties for display purposes
        public string NumeroHabitacion { get; set; }
        public string TipoHabitacionNombre { get; set; }
    }
}