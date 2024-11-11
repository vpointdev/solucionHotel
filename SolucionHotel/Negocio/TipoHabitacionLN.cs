using Negocio.Interfaces;
using AccesoDatos.Interfaces;
using Entidades.SQLServer;
using System.Transactions;

namespace Negocio
{
    public class TipoHabitacionLN : ITipoHabitacionLN
    {
        #region Atributos
        private readonly ITipoHabitacionAD _iTipoHabitacionAD;
        #endregion

        #region Propiedades
        public TransactionOptions TransacionOpciones { get; set; }
        #endregion

        #region Constructor
        public TipoHabitacionLN(ITipoHabitacionAD iTipoHabitacionAD)
        {
            _iTipoHabitacionAD = iTipoHabitacionAD;
            TransacionOpciones = new TransactionOptions
            {
                Timeout = TransactionManager.DefaultTimeout,
                IsolationLevel = IsolationLevel.ReadUncommitted
            };
        }
        #endregion

        #region Métodos Públicos
        public TipoHabitacion Crear(TipoHabitacion entidad)
        {
            return _iTipoHabitacionAD.Crear(
                entidad.Nombre,
                entidad.Descripcion,
                entidad.PrecioBase,
                entidad.Capacidad
            );
        }

        public List<TipoHabitacion> Obtener(int? tipoHabitacionId = null)
        {
            return _iTipoHabitacionAD.Obtener(tipoHabitacionId);
        }

        public TipoHabitacion Actualizar(TipoHabitacion entidad)
        {
            return _iTipoHabitacionAD.Actualizar(
                entidad.TipoHabitacionId,
                entidad.Nombre,
                entidad.Descripcion,
                entidad.PrecioBase,
                entidad.Capacidad
            );
        }

        public bool Eliminar(int tipoHabitacionId)
        {
            return _iTipoHabitacionAD.Eliminar(tipoHabitacionId);
        }
        #endregion
    }
}
