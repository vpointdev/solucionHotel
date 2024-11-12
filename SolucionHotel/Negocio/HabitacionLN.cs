using AccesoDatos.Interfaces;
using Entidades.SQLServer;
using Negocio.Interfaces;

namespace Negocio
{
    public class HabitacionLN : IHabitacionLN
    {
        private readonly IHabitacionAD _habitacionAD;

        public HabitacionLN(IHabitacionAD habitacionAD)
        {
            _habitacionAD = habitacionAD;
        }

        public bool Agregar(Habitacion habitacion)
        {
            return _habitacionAD.Agregar(habitacion);
        }

        public bool Modificar(Habitacion habitacion)
        {
            return _habitacionAD.Modificar(habitacion);
        }

        public bool Eliminar(int habitacionId)
        {
            return _habitacionAD.Eliminar(habitacionId);
        }

        public List<Habitacion> ObtenerTodos()
        {
            return _habitacionAD.ObtenerTodos();
        }

        public List<TipoHabitacion> ObtenerTiposHabitacion()
        {
            return _habitacionAD.ObtenerTiposHabitacion();
        }
    }
}