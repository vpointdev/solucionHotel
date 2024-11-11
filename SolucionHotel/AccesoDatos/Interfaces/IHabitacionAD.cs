using Entidades.SQLServer;

public interface IHabitacionAD
{
    Habitacion Crear(string numeroHabitacion, int tipoHabitacionId, int piso, string estado = "Disponible", string observaciones = null);
    List<Habitacion> Obtener(int? habitacionId = null, string numeroHabitacion = null, int? tipoHabitacionId = null, string estado = null);
    Habitacion Actualizar(int habitacionId, int tipoHabitacionId, int piso, string estado, string observaciones = null);
    bool Eliminar(int habitacionId);
}