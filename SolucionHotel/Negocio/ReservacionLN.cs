using Negocio.Interfaces;
using AccesoDatos.Interfaces;
using Entidades.SQLServer;
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
        public Reservacion Crear(Reservacion entidad)
        {
            try
            {
                return _iReservacionAD.Crear(entidad);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Reservacion ProcesarPago(int reservacionId, int usuarioId)
        {
            try
            {
                return _iReservacionAD.ProcesarPago(reservacionId, usuarioId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Reservacion Cancelar(int reservacionId, int usuarioId)
        {
            try
            {
                return _iReservacionAD.Cancelar(reservacionId, usuarioId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Reservacion> ObtenerPorUsuario(int usuarioId)
        {
            try
            {
                return _iReservacionAD.ObtenerPorUsuario(usuarioId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Habitacion> ObtenerDisponibles(DateTime fechaEntrada, DateTime fechaSalida)
        {
            try
            {
                return _iReservacionAD.ObtenerDisponibles(fechaEntrada, fechaSalida);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}