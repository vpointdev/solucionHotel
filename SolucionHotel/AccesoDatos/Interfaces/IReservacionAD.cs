using Entidades.SQLServer;

namespace AccesoDatos.Interfaces
{
    public interface IReservacionAD
    {
        bool Agregar(Reservacion P_Entidad);
        bool Modificar(Reservacion P_Entidad);
        bool Eliminar(Reservacion P_Entidad);
        List<Reservacion> Consultar(Reservacion P_Entidad);
        bool CancelarReservacion(string pCodigoReservacion);
        List<Reservacion> ConsultarPorUsuario(int pUsuarioId);
        List<Reservacion> ConsultarPorFecha(DateTime pFechaInicio, DateTime pFechaFin);
        decimal CalcularCargoCancelacion(string pCodigoReservacion);
        bool CompletarCheckIn(string pCodigoReservacion);
        bool CompletarCheckOut(string pCodigoReservacion);
    }
}