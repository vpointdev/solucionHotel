using Entidades;

namespace AccesoDatos.Interfaces
{
    public interface IBitacoraAD
    {
        bool Agregar(Bitacora P_Entidad);
        bool Modificar(Bitacora P_Entidad);
        bool Eliminar(Bitacora P_Entidad);
        List<Bitacora> Listar();
        List<Bitacora> Consultar(Bitacora P_Entidad);
    }
}