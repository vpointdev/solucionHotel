using AccesoDatos.Interfaces;
using Entidades;
using Negocio.Interfaces;
using System.Transactions;

namespace Negocio
{
    public class BitacoraLN : IBitacoraLN
    {
        #region Atributos
        private readonly IBitacoraAD _iBitacoraAD;
        #endregion

        #region Propiedades
        public TransactionOptions TransacionOpciones { get; set; }
        #endregion

        #region Constructor
        public BitacoraLN(IBitacoraAD iBitacoraAD)
        {
            _iBitacoraAD = iBitacoraAD;
            TransacionOpciones = new TransactionOptions
            {
                Timeout = TransactionManager.DefaultTimeout,
                IsolationLevel = IsolationLevel.ReadUncommitted
            };
        }
        #endregion

        #region Métodos Públicos
        public bool Agregar(Bitacora P_Entidad)
        {
            return _iBitacoraAD.Agregar(P_Entidad);
        }

        public bool Modificar(Bitacora P_Entidad)
        {
            return _iBitacoraAD.Modificar(P_Entidad);
        }

        public bool Eliminar(Bitacora P_Entidad)
        {
            return _iBitacoraAD.Eliminar(P_Entidad);
        }

        public List<Bitacora> Listar()
        {
            return _iBitacoraAD.Listar();
        }

        public List<Bitacora> Consultar(Bitacora P_Entidad)
        {
            return _iBitacoraAD.Consultar(P_Entidad);
        }
        #endregion
    }
}