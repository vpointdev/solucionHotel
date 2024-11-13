using Entidades.SQLServer;

namespace AccesoDatos.Interfaces
{
    public interface IReservacionAD
    {
        List<Reservacion> ObtenerTodos();
        bool Agregar(Reservacion reservacion);
        bool Modificar(Reservacion reservacion);
        bool Eliminar(int reservacionId);
        bool CancelarReservacion(int reservacionId);
    }
}