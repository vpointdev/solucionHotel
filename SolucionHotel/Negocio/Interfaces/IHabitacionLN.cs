using Entidades.SQLServer;

namespace Negocio.Interfaces
{
    public interface IHabitacionLN
    {
        Habitacion Crear(Habitacion entidad);
        List<Habitacion> Obtener(int? habitacionId = null, string numeroHabitacion = null, int? tipoHabitacionId = null, string estado = null);
        Habitacion Actualizar(Habitacion entidad);
        bool Eliminar(int habitacionId);
    }
}