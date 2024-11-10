using Entidades.SQLServer;

namespace Negocio.Interfaces
{
    public interface IPagoLN
    {
        bool Agregar(Pago P_Entidad);
        bool Modificar(Pago P_Entidad);
        bool Eliminar(Pago P_Entidad);
        List<Pago> Consultar(Pago P_Entidad);
        List<Pago> ObtenerPorReservacion(string pCodigoReservacion);
        List<TipoPago> ObtenerTiposPago();
        decimal ObtenerTotalPorReservacion(string pCodigoReservacion);
    }
}