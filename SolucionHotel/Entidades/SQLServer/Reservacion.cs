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
        public string Estado { get; set; }
        public decimal PrecioTotal { get; set; }
        public bool PagoProcesado { get; set; }
        public decimal? CargoCancelacion { get; set; }
        public decimal? MontoReembolsado { get; set; }
        public decimal? TotalPagado { get; set; }
        public string NumeroHabitacion { get; set; }
        public string TipoHabitacion { get; set; }
        public string Observaciones { get; set; }
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
            Estado = "Pendiente";
            PrecioTotal = 0;
            PagoProcesado = false;
            CargoCancelacion = null;
            MontoReembolsado = null;
            TotalPagado = null;
            NumeroHabitacion = string.Empty;
            TipoHabitacion = string.Empty;
            Observaciones = string.Empty;
        }
        #endregion
    }
}