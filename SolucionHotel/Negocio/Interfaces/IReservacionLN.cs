using Entidades.SQLServer;

namespace Negocio.Interfaces
{
    public interface IReservacionLN
    {
        List<Reservacion> ObtenerTodos();
        bool Agregar(Reservacion reservacion);
        bool Modificar(Reservacion reservacion);
        bool Eliminar(int reservacionId);
        bool CancelarReservacion(int reservacionId);
    }
}