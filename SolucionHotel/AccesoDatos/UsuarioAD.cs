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

            parametros.Add("@Usuario", P_Entidad.NombreUsuario, DbType.String, ParameterDirection.Input, 20);
            parametros.Add("@Password", P_Entidad.Clave, DbType.String, ParameterDirection.Input, 15);
            parametros.Add("@Fecha", P_Entidad.FechaRegistro, DbType.DateTime, ParameterDirection.Input);
            parametros.Add("@Correo", P_Entidad.CorreoRegistro, DbType.String, ParameterDirection.Input, 100);
            parametros.Add("@Estado", P_Entidad.Estado, DbType.Boolean, ParameterDirection.Input);

            using (var conexionSQL = new SqlConnection(_iConfiguration.GetConnectionString("ConexionSQLServer")))
            {
                return conexionSQL.Execute("PA_Usuario_Crear", parametros, commandType: CommandType.StoredProcedure) > 0;
            }
        }

        public bool Modificar(Usuario P_Entidad)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add("@Usuario", P_Entidad.NombreUsuario, DbType.String, ParameterDirection.Input, 20);
            parametros.Add("@Pass", P_Entidad.Clave, DbType.String, ParameterDirection.Input, 15);
            parametros.Add("@Fecha", P_Entidad.FechaRegistro, DbType.DateTime, ParameterDirection.Input);
            parametros.Add("@Correo", P_Entidad.CorreoRegistro, DbType.String, ParameterDirection.Input, 100);
            parametros.Add("@Estado", P_Entidad.Estado, DbType.Boolean, ParameterDirection.Input);

            using (var conexionSQL = new SqlConnection(_iConfiguration.GetConnectionString("ConexionSQLServer")))
            {
                return conexionSQL.Execute("PA_Usuario_Actualizar", parametros, commandType: CommandType.StoredProcedure) > 0;
            }
        }

        public bool Eliminar(Usuario P_Entidad)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add("@Usuario", P_Entidad.NombreUsuario, DbType.String, ParameterDirection.Input, 20);

            using (var conexionSQL = new SqlConnection(_iConfiguration.GetConnectionString("ConexionSQLServer")))
            {
                return conexionSQL.Execute("PA_Usuario_Eliminar", parametros, commandType: CommandType.StoredProcedure) > 0;
            }
        }

        public List<Usuario> Consultar(Usuario P_Entidad)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add("@Usuario", P_Entidad.NombreUsuario, DbType.String, ParameterDirection.Input, 20);

            using (var conexionSQL = new SqlConnection(_iConfiguration.GetConnectionString("ConexionSQLServer")))
            {
                return (List<Usuario>)conexionSQL.Query<Usuario>("PA_Usuario_ObtenerPorId", parametros, commandType: CommandType.StoredProcedure);
            }
        }

        public List<Perfil> PerfilesUsuario(Usuario P_Entidad)
        {
            try
            {
                DynamicParameters parametros = new DynamicParameters();
                parametros.Add("@Usuario", P_Entidad.NombreUsuario, DbType.String, ParameterDirection.Input, 20);

                using (var conexionSQL = new SqlConnection(_iConfiguration.GetConnectionString("ConexionSQLServer")))
                {
                    var perfiles = conexionSQL.Query<Perfil>("PA_Usuario_ObtenerPerfiles",
                        parametros,
                        commandType: CommandType.StoredProcedure).ToList();

                                       return perfiles;
                }
            }
            catch (Exception ex)
            {
                // Debug: Capturar cualquier error
                System.Diagnostics.Debug.WriteLine($"Error en PerfilesUsuario: {ex.Message}");
                throw;
            }
        }

        public bool Autenticacion(Usuario P_Entidad)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add("@Usuario", P_Entidad.NombreUsuario, DbType.String, ParameterDirection.Input, 20);
            parametros.Add("@Clave", P_Entidad.Clave, DbType.String, ParameterDirection.Input, 15);

            using (var conexionSQL = new SqlConnection(_iConfiguration.GetConnectionString("ConexionSQLServer")))
            {
                return ((List<Usuario>)conexionSQL.Query<Usuario>("PA_Usuario_Autenticar", parametros, commandType: CommandType.StoredProcedure)).Count > 0;
            }
        }
        #endregion
    }
}