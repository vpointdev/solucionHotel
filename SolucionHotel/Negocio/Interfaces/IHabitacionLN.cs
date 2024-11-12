using Entidades.SQLServer;

namespace Negocio.Interfaces
{
    public interface IHabitacionLN
    {
        bool Agregar(Habitacion habitacion);
        bool Modificar(Habitacion habitacion);
        bool Eliminar(int habitacionId);
        List<Habitacion> ObtenerTodos();
        List<TipoHabitacion> ObtenerTiposHabitacion();
    }
}