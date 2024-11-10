using System;
using System.Collections.Generic;

namespace Entidades.SQLServer
{
    public class Pago
    {
        #region Propiedades
        public int PagoId { get; set; }
        public int ReservacionId { get; set; }
        public int TipoPagoId { get; set; }
        public decimal Monto { get; set; }
        public DateTime FechaPago { get; set; }
        public string NumeroTransaccion { get; set; }
        public string Estado { get; set; }
        public string Observaciones { get; set; }
        public int UsuarioCreacionId { get; set; }
        public DateTime FechaCreacion { get; set; }
        #endregion

        #region Constructor
        public Pago()
        {
            PagoId = 0;
            ReservacionId = 0;
            TipoPagoId = 0;
            Monto = 0;
            FechaPago = DateTime.MinValue;
            NumeroTransaccion = string.Empty;
            Estado = "Pendiente";  // Estados posibles: Pendiente, Completado, Cancelado
            Observaciones = string.Empty;
            UsuarioCreacionId = 0;
            FechaCreacion = DateTime.MinValue;
        }
        #endregion
    }

}