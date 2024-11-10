using AccesoDatos.Interfaces;
using Entidades;
using Entidades.SQLServer;
using Negocio.Interfaces;
using System.Transactions;

namespace Negocio
{
    public class HabitacionLN : IHabitacionLN
    {
        #region Atributos
        private readonly IHabitacionAD _iHabitacionAD;
        #endregion

        #region Propiedades
        public TransactionOptions TransacionOpciones { get; set; }
        #endregion

        #region Constructor
        public HabitacionLN(IHabitacionAD iHabitacionAD)
        {
            _iHabitacionAD = iHabitacionAD;
            TransacionOpciones = new TransactionOptions
            {
                Timeout = TransactionManager.DefaultTimeout,
                IsolationLevel = IsolationLevel.ReadUncommitted
            };
        }
        #endregion

        #region Métodos Públicos
        public bool Agregar(Habitacion P_Entidad)
        {
            return _iHabitacionAD.Agregar(P_Entidad);
        }

        public bool Modificar(Habitacion P_Entidad)
        {
            return _iHabitacionAD.Modificar(P_Entidad);
        }

        public bool Eliminar(Habitacion P_Entidad)
        {
            return _iHabitacionAD.Eliminar(P_Entidad);
        }

        public List<Habitacion> Consultar(Habitacion P_Entidad)
        {
            return _iHabitacionAD.Consultar(P_Entidad);
        }

        public List<Habitacion> ObtenerDisponibles(DateTime pFechaEntrada, DateTime pFechaSalida)
        {
            return _iHabitacionAD.ObtenerDisponibles(pFechaEntrada, pFechaSalida);
        }

        public List<TipoHabitacion> ObtenerTiposHabitacion()
        {
            return _iHabitacionAD.ObtenerTiposHabitacion();
        }

        public List<Habitacion> ConsultarOcupacion(DateTime pFechaInicio, DateTime pFechaFin)
        {
            return _iHabitacionAD.ConsultarOcupacion(pFechaInicio, pFechaFin);
        }

        public decimal ObtenerIngresos(DateTime pFechaInicio, DateTime pFechaFin)
        {
            return _iHabitacionAD.ObtenerIngresos(pFechaInicio, pFechaFin);
        }
        #endregion
    }
}