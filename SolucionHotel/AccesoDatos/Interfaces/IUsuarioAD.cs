using Entidades.SQLServer;

namespace AccesoDatos.Interfaces
{
    public interface IUsuarioAD
    {
        bool Agregar(Usuario P_Entidad);
        bool Modificar(Usuario P_Entidad);
        bool Eliminar(Usuario P_Entidad);
        List<Usuario> Consultar(Usuario P_Entidad);
        List<Perfil> PerfilesUsuario(Usuario P_Entidad);
        bool Autenticacion(Usuario P_Entidad);
    }
}