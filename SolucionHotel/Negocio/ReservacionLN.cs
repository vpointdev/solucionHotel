using AccesoDatos.Interfaces;
using Entidades;
using Entidades.SQLServer;
using Negocio.Interfaces;
using System.Transactions;

namespace Negocio
{
    public class ReservacionLN : IReservacionLN
    {
        #region Atributos
        private readonly IReservacionAD _iReservacionAD;
        #endregion

        #region Propiedades
        public TransactionOptions TransacionOpciones { get; set; }
        #endregion

        #region Constructor
        public ReservacionLN(IReservacionAD iReservacionAD)
        {
            _iReservacionAD = iReservacionAD;
            TransacionOpciones = new TransactionOptions
            {
                Timeout = TransactionManager.DefaultTimeout,
                IsolationLevel = IsolationLevel.ReadUncommitted
            };
        }
        #endregion

        #region Métodos Públicos
        public bool Agregar(Reservacion P_Entidad)
        {
            return _iReservacionAD.Agregar(P_Entidad);
        }

        public bool Modificar(Reservacion P_Entidad)
        {
            return _iReservacionAD.Modificar(P_Entidad);
        }

        public bool Eliminar(Reservacion P_Entidad)
        {
            return _iReservacionAD.Eliminar(P_Entidad);
        }

        public List<Reservacion> Consultar(Reservacion P_Entidad)
        {
            return _iReservacionAD.Consultar(P_Entidad);
        }

        public bool CancelarReservacion(string pCodigoReservacion)
        {
            return _iReservacionAD.CancelarReservacion(pCodigoReservacion);
        }

        public List<Reservacion> ConsultarPorUsuario(int pUsuarioId)
        {
            return _iReservacionAD.ConsultarPorUsuario(pUsuarioId);
        }

        public List<Reservacion> ConsultarPorFecha(DateTime pFechaInicio, DateTime pFechaFin)
        {
            return _iReservacionAD.ConsultarPorFecha(pFechaInicio, pFechaFin);
        }

        public decimal CalcularCargoCancelacion(string pCodigoReservacion)
        {
            return _iReservacionAD.CalcularCargoCancelacion(pCodigoReservacion);
        }

        public bool CompletarCheckIn(string pCodigoReservacion)
        {
            return _iReservacionAD.CompletarCheckIn(pCodigoReservacion);
        }

        public bool CompletarCheckOut(string pCodigoReservacion)
        {
            return _iReservacionAD.CompletarCheckOut(pCodigoReservacion);
        }
        #endregion
    }
}
