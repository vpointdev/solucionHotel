using AccesoDatos.Interfaces;
using Dapper;
using Entidades.SQLServer;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace AccesoDatos
{
    public class UsuarioAD : IUsuarioAD
    {
        #region Atributos
        private readonly IConfiguration _iConfiguration;
        #endregion

        #region Constructor
        public UsuarioAD(IConfiguration iConfiguration)
        {
            _iConfiguration = iConfiguration;
        }
        #endregion

        #region Métodos Públicos
        public bool Agregar(Usuario P_Entidad)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add("@NombreUsuario", P_Entidad.NombreUsuario, DbType.String, ParameterDirection.Input, 50);
            parametros.Add("@Clave", P_Entidad.Clave, DbType.String, ParameterDirection.Input, 50);
            parametros.Add("@CorreoRegistro", P_Entidad.CorreoRegistro, DbType.String, ParameterDirection.Input, 100);

            using (var conexionSQL = new SqlConnection(_iConfiguration.GetConnectionString("ConexionSQLServer")))
            {
                return conexionSQL.Execute("PA_Usuario_Crear", parametros, commandType: CommandType.StoredProcedure) > 0;
            }
        }

        public bool Modificar(Usuario P_Entidad)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add("@UsuarioId", P_Entidad.UsuarioId, DbType.Int32, ParameterDirection.Input);
            parametros.Add("@NombreUsuario", P_Entidad.NombreUsuario, DbType.String, ParameterDirection.Input, 50);
            parametros.Add("@Clave", P_Entidad.Clave, DbType.String, ParameterDirection.Input, 50);
            parametros.Add("@CorreoRegistro", P_Entidad.CorreoRegistro, DbType.String, ParameterDirection.Input, 100);
            parametros.Add("@Estado", P_Entidad.Estado, DbType.Boolean, ParameterDirection.Input);

            using (var conexionSQL = new SqlConnection(_iConfiguration.GetConnectionString("ConexionSQLServer")))
            {
                return conexionSQL.Execute("PA_Usuario_Actualizar", parametros, commandType: CommandType.StoredProcedure) > 0;
            }
        }

        public bool Eliminar(Usuario P_Entidad)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add("@UsuarioId", P_Entidad.UsuarioId, DbType.Int32, ParameterDirection.Input);

            using (var conexionSQL = new SqlConnection(_iConfiguration.GetConnectionString("ConexionSQLServer")))
            {
                return conexionSQL.Execute("PA_Usuario_Eliminar", parametros, commandType: CommandType.StoredProcedure) > 0;
            }
        }

        public List<Usuario> Consultar(Usuario P_Entidad)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add("@UsuarioId", P_Entidad.UsuarioId, DbType.Int32, ParameterDirection.Input);

            using (var conexionSQL = new SqlConnection(_iConfiguration.GetConnectionString("ConexionSQLServer")))
            {
                return conexionSQL.Query<Usuario>("PA_Usuario_ObtenerPorId", parametros, commandType: CommandType.StoredProcedure).ToList();
            }
        }

        public List<Perfil> PerfilesUsuario(Usuario P_Entidad)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add("@NombreUsuario", P_Entidad.NombreUsuario, DbType.String, ParameterDirection.Input, 50);

            using (var conexionSQL = new SqlConnection(_iConfiguration.GetConnectionString("ConexionSQLServer")))
            {
                var result = conexionSQL.Query<Perfil>("PA_Usuario_ObtenerPerfiles", parametros, commandType: CommandType.StoredProcedure).ToList();
                return result;
            }
        }

        public bool Autenticacion(Usuario P_Entidad)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add("@NombreUsuario", P_Entidad.NombreUsuario, DbType.String, ParameterDirection.Input, 50);
            parametros.Add("@Clave", P_Entidad.Clave, DbType.String, ParameterDirection.Input, 50);

            using (var conexionSQL = new SqlConnection(_iConfiguration.GetConnectionString("ConexionSQLServer")))
            {
                var result = conexionSQL.Query<Usuario>("PA_Usuario_Autenticar", parametros, commandType: CommandType.StoredProcedure).ToList();
                return result.Count > 0;
            }
        }

        public List<Usuario> ObtenerTodos()
        {
            using (var conexionSQL = new SqlConnection(_iConfiguration.GetConnectionString("ConexionSQLServer")))
            {
                return conexionSQL.Query<Usuario>("PA_Usuario_ObtenerTodos", commandType: CommandType.StoredProcedure).ToList();
            }
        }
        #endregion
    }
}