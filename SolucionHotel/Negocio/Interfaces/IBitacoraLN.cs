using Entidades;

namespace Negocio.Interfaces
{
    public interface IBitacoraLN
    {
        bool Agregar(Bitacora P_Entidad);
        bool Modificar(Bitacora P_Entidad);
        bool Eliminar(Bitacora P_Entidad);
        List<Bitacora> Listar();
        List<Bitacora> Consultar(Bitacora P_Entidad);
    }
}