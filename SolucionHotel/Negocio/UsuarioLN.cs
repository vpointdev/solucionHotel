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
            using (var scope = new TransactionScope(TransactionScopeOption.Required, TransacionOpciones))
            {
                var result = _iUsuarioAD.Agregar(P_Entidad);
                if (result)
                    scope.Complete();
                return result;
            }
        }

        public bool Modificar(Usuario P_Entidad)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required, TransacionOpciones))
            {
                var result = _iUsuarioAD.Modificar(P_Entidad);
                if (result)
                    scope.Complete();
                return result;
            }
        }

        public bool Eliminar(Usuario P_Entidad)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required, TransacionOpciones))
            {
                var result = _iUsuarioAD.Eliminar(P_Entidad);
                if (result)
                    scope.Complete();
                return result;
            }
        }

        public List<Usuario> Consultar(Usuario P_Entidad)
        {
            return _iUsuarioAD.Consultar(P_Entidad);
        }

        public List<Usuario> ObtenerTodos()
        {
            return _iUsuarioAD.ObtenerTodos();
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