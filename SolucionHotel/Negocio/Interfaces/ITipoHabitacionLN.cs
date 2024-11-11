using Entidades.SQLServer;

namespace Negocio.Interfaces
{
    public interface ITipoHabitacionLN
    {
        TipoHabitacion Crear(TipoHabitacion entidad);
        List<TipoHabitacion> Obtener(int? tipoHabitacionId = null);
        TipoHabitacion Actualizar(TipoHabitacion entidad);
        bool Eliminar(int tipoHabitacionId);
    }
}