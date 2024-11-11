public interface ITipoHabitacionAD
{
    TipoHabitacion Crear(string nombre, string descripcion, decimal precioBase, int capacidad);
    List<TipoHabitacion> Obtener(int? tipoHabitacionId = null);
    TipoHabitacion Actualizar(int tipoHabitacionId, string nombre, string descripcion, decimal precioBase, int capacidad);
    bool Eliminar(int tipoHabitacionId);
}