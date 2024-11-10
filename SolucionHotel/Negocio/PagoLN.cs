using AccesoDatos.Interfaces;
using Entidades.SQLServer;
using Negocio.Interfaces;
using System.Transactions;

namespace Negocio
{
    public class PagoLN : IPagoLN
    {
        #region Atributos
        private readonly IPagoAD _iPagoAD;
        #endregion

        #region Propiedades
        public TransactionOptions TransacionOpciones { get; set; }
        #endregion

        #region Constructor
        public PagoLN(IPagoAD iPagoAD)
        {
            _iPagoAD = iPagoAD;
            TransacionOpciones = new TransactionOptions
            {
                Timeout = TransactionManager.DefaultTimeout,
                IsolationLevel = IsolationLevel.ReadUncommitted
            };
        }
        #endregion

        #region Métodos Públicos
        public bool Agregar(Pago P_Entidad)
        {
            return _iPagoAD.Agregar(P_Entidad);
        }

        public bool Modificar(Pago P_Entidad)
        {
            return _iPagoAD.Modificar(P_Entidad);
        }

        public bool Eliminar(Pago P_Entidad)
        {
            return _iPagoAD.Eliminar(P_Entidad);
        }

        public List<Pago> Consultar(Pago P_Entidad)
        {
            return _iPagoAD.Consultar(P_Entidad);
        }

        public List<Pago> ObtenerPorReservacion(string pCodigoReservacion)
        {
            return _iPagoAD.ObtenerPorReservacion(pCodigoReservacion);
        }

        public List<TipoPago> ObtenerTiposPago()
        {
            return _iPagoAD.ObtenerTiposPago();
        }

        public decimal ObtenerTotalPorReservacion(string pCodigoReservacion)
        {
            return _iPagoAD.ObtenerTotalPorReservacion(pCodigoReservacion);
        }
        #endregion
    }
}