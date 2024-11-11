using Entidades.SQLServer;

namespace Negocio.Interfaces
{
    public interface IReservacionLN
    {
        Reservacion Crear(Reservacion entidad);
        Reservacion ProcesarPago(int reservacionId, int usuarioId);
        Reservacion Cancelar(int reservacionId, int usuarioId);
        List<Reservacion> ObtenerPorUsuario(int usuarioId);
        List<Habitacion> ObtenerDisponibles(DateTime fechaEntrada, DateTime fechaSalida);
    }
}