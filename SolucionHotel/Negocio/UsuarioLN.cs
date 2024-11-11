using AccesoDatos.Interfaces;
using Entidades.SQLServer;
using Negocio.Interfaces;
using System.Transactions;

using AccesoDatos.Interfaces;
using Entidades.SQLServer;
using Negocio.Interfaces;
using System.Transactions;

namespace Negocio
{
    public class UsuarioLN : IUsuarioLN
    {
        #region Atributos
        private readonly IUsuarioAD _iUsuarioAD;
        #endregion

        #region Propiedades
        public TransactionOptions TransacionOpciones { get; set; }
        #endregion

        #region Constructor
        public UsuarioLN(IUsuarioAD iUsuarioAD)
        {
            _iUsuarioAD = iUsuarioAD;
            TransacionOpciones = new TransactionOptions
            {
                Timeout = TransactionManager.DefaultTimeout,
                IsolationLevel = IsolationLevel.ReadUncommitted
            };
        }
        #endregion

        #region Métodos Públicos
        public bool Agregar(Usuario P_Entidad)
        {
            return _iUsuarioAD.Agregar(P_Entidad);
        }

        public bool Modificar(Usuario P_Entidad)
        {
            return _iUsuarioAD.Modificar(P_Entidad);
        }

        public bool Eliminar(Usuario P_Entidad)
        {
            return _iUsuarioAD.Eliminar(P_Entidad);
        }

        public List<Usuario> Consultar(Usuario P_Entidad)
        {
            return _iUsuarioAD.Consultar(P_Entidad);
        }

        public List<Perfil> PerfilesUsuario(Usuario P_Entidad)
        {
            return _iUsuarioAD.PerfilesUsuario(P_Entidad);
        }

        public bool Autenticacion(Usuario P_Entidad)
        {
            return _iUsuarioAD.Autenticacion(P_Entidad);
        }
        #endregion
    }
}