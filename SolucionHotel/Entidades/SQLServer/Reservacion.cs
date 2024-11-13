namespace Entidades.SQLServer
{
    public class Reservacion
    {
        #region Propiedades
        public int ReservacionId { get; set; }
        public string CodigoReservacion { get; set; }
        public int UsuarioId { get; set; }
        public int HabitacionId { get; set; }
        public DateTime FechaEntrada { get; set; }
        public DateTime FechaSalida { get; set; }
        public string EstadoReservacion { get; set; }
        public decimal PrecioTotal { get; set; }
        public string Observaciones { get; set; }

        // Navigation properties
        public string NumeroHabitacion { get; set; }
        public string TipoHabitacionNombre { get; set; }

        // Audit properties
        public int UsuarioCreacionId { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int? UsuarioModificacionId { get; set; }
        public DateTime? FechaModificacion { get; set; }
        #endregion

        #region Constructor
        public Reservacion()
        {
            CodigoReservacion = string.Empty;
            EstadoReservacion = string.Empty;
            Observaciones = string.Empty;
            NumeroHabitacion = string.Empty;
            TipoHabitacionNombre = string.Empty;
            FechaCreacion = DateTime.Now;
        }
        #endregion
    }
}