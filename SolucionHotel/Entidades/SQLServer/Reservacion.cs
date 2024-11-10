namespace Entidades.SQLServer
{    public class Reservacion
    {
        #region Propiedades
        public int ReservacionId { get; set; }
        public string CodigoReservacion { get; set; }
        public int UsuarioId { get; set; }
        public int HabitacionId { get; set; }
        public DateTime FechaEntrada { get; set; }
        public DateTime FechaSalida { get; set; }
        public int EstadoReservacionId { get; set; }
        public decimal PrecioTotal { get; set; }
        public string Observaciones { get; set; }
        public int UsuarioCreacionId { get; set; }
        public int? UsuarioModificacionId { get; set; }
        #endregion

        #region Constructor
        public Reservacion()
        {
            ReservacionId = 0;
            CodigoReservacion = string.Empty;
            UsuarioId = 0;
            HabitacionId = 0;
            FechaEntrada = DateTime.MinValue;
            FechaSalida = DateTime.MinValue;
            EstadoReservacionId = 0;
            PrecioTotal = 0;
            Observaciones = string.Empty;
            UsuarioCreacionId = 0;
            UsuarioModificacionId = null;
        }
        #endregion
    }
}