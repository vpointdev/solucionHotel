using AccesoDatos.Interfaces;
using Entidades.SQLServer;
using Negocio.Interfaces;

namespace Negocio
{
    public class ReservacionLN : IReservacionLN
    {
        private readonly IReservacionAD _reservacionAD;

        public ReservacionLN(IReservacionAD reservacionAD)
        {
            _reservacionAD = reservacionAD;
        }

        public List<Reservacion> ObtenerTodos()
        {
            return _reservacionAD.ObtenerTodos();
        }

        public bool Agregar(Reservacion reservacion)
        {
            return _reservacionAD.Agregar(reservacion);
        }

        public bool Modificar(Reservacion reservacion)
        {
            return _reservacionAD.Modificar(reservacion);
        }

        public bool Eliminar(int reservacionId)
        {
            return _reservacionAD.Eliminar(reservacionId);
        }

        public bool CancelarReservacion(int reservacionId)
        {
            return _reservacionAD.CancelarReservacion(reservacionId);
        }
    }
}