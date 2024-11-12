using Entidades.SQLServer;

namespace Negocio.Interfaces
{
    public interface IUsuarioLN
    {
        bool Agregar(Usuario P_Entidad);
        bool Modificar(Usuario P_Entidad);
        bool Eliminar(Usuario P_Entidad);
        List<Usuario> Consultar(Usuario P_Entidad);
        List<Usuario> ObtenerTodos();
        List<Perfil> PerfilesUsuario(Usuario P_Entidad);
        bool Autenticacion(Usuario P_Entidad);
    }
}