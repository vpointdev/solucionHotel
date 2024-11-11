using Entidades.SQLServer;

namespace AccesoDatos.Interfaces
{
    public interface IReservacionAD
    {
        Reservacion Crear(Reservacion entidad);
        Reservacion ProcesarPago(int reservacionId, int usuarioId);
        Reservacion Cancelar(int reservacionId, int usuarioId);
        List<Reservacion> ObtenerPorUsuario(int usuarioId);
        List<Habitacion> ObtenerDisponibles(DateTime fechaEntrada, DateTime fechaSalida);
    }
}