using Entidades.SQLServer;

namespace Negocio.Interfaces
{
    public interface IHabitacionLN
    {
        bool Agregar(Habitacion P_Entidad);
        bool Modificar(Habitacion P_Entidad);
        bool Eliminar(Habitacion P_Entidad);
        List<Habitacion> Consultar(Habitacion P_Entidad);
        List<Habitacion> ObtenerDisponibles(DateTime pFechaEntrada, DateTime pFechaSalida);
        List<TipoHabitacion> ObtenerTiposHabitacion();
        List<Habitacion> ConsultarOcupacion(DateTime pFechaInicio, DateTime pFechaFin);
        decimal ObtenerIngresos(DateTime pFechaInicio, DateTime pFechaFin);
    }
}