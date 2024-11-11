using AccesoDatos.Interfaces;
using Entidades;
using Entidades.SQLServer;
using Negocio.Interfaces;
using System.Transactions;

namespace Negocio
{
    public class HabitacionLN : IHabitacionLN
    {
        private readonly IHabitacionAD _habitacionAD;

        public HabitacionLN(IHabitacionAD habitacionAD)
        {
            _habitacionAD = habitacionAD;
        }

        public Habitacion Crear(Habitacion entidad)
        {
            return _habitacionAD.Crear(
                entidad.NumeroHabitacion,
                entidad.TipoHabitacionId,
                entidad.Piso,
                entidad.Estado,
                entidad.Observaciones
            );
        }

        public List<Habitacion> Obtener(int? habitacionId = null, string numeroHabitacion = null, int? tipoHabitacionId = null, string estado = null)
        {
            return _habitacionAD.Obtener(habitacionId, numeroHabitacion, tipoHabitacionId, estado);
        }

        public Habitacion Actualizar(Habitacion entidad)
        {
            return _habitacionAD.Actualizar(
                entidad.HabitacionId,
                entidad.TipoHabitacionId,
                entidad.Piso,
                entidad.Estado,
                entidad.Observaciones
            );
        }

        public bool Eliminar(int habitacionId)
        {
            return _habitacionAD.Eliminar(habitacionId);
        }
    }
}