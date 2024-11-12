using Entidades.SQLServer;

namespace AccesoDatos.Interfaces
{
    public interface IHabitacionAD
    {
        bool Agregar(Habitacion habitacion);
        bool Modificar(Habitacion habitacion);
        bool Eliminar(int habitacionId);
        List<Habitacion> ObtenerTodos();
        List<TipoHabitacion> ObtenerTiposHabitacion();
    }
}