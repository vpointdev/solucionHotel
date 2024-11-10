using AccesoDatos.Interfaces;
using Dapper;
using Entidades.SQLServer;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace AccesoDatos
{
    public class HabitacionAD : IHabitacionAD
    {
        #region Atributos
        private readonly IConfiguration _iConfiguration;
        #endregion

        #region Constructor
        public HabitacionAD(IConfiguration iConfiguration)
        {
            _iConfiguration = iConfiguration;
        }
        #endregion

        #region Métodos Públicos
        public bool Agregar(Habitacion P_Entidad)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add("@NumeroHabitacion", P_Entidad.NumeroHabitacion, DbType.String, ParameterDirection.Input, 10);
            parametros.Add("@TipoHabitacionId", P_Entidad.TipoHabitacionId, DbType.Int32, ParameterDirection.Input);
            parametros.Add("@Piso", P_Entidad.Piso, DbType.Int32, ParameterDirection.Input);
            parametros.Add("@Estado", P_Entidad.Estado, DbType.String, ParameterDirection.Input, 20);
            parametros.Add("@Observaciones", P_Entidad.Observaciones, DbType.String, ParameterDirection.Input, 500);
            parametros.Add("@UsuarioCreacionId", P_Entidad.UsuarioCreacionId, DbType.Int32, ParameterDirection.Input);

            using (var conexionSQL = new SqlConnection(_iConfiguration.GetConnectionString("ConexionSQLServer")))
            {
                return conexionSQL.Execute("PA_Habitacion_Crear", parametros, commandType: CommandType.StoredProcedure) > 0;
            }
        }

        public bool Modificar(Habitacion P_Entidad)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add("@NumeroHabitacion", P_Entidad.NumeroHabitacion, DbType.String, ParameterDirection.Input, 10);
            parametros.Add("@TipoHabitacionId", P_Entidad.TipoHabitacionId, DbType.Int32, ParameterDirection.Input);
            parametros.Add("@Piso", P_Entidad.Piso, DbType.Int32, ParameterDirection.Input);
            parametros.Add("@Estado", P_Entidad.Estado, DbType.String, ParameterDirection.Input, 20);
            parametros.Add("@Observaciones", P_Entidad.Observaciones, DbType.String, ParameterDirection.Input, 500);
            parametros.Add("@UsuarioModificacionId", P_Entidad.UsuarioModificacionId, DbType.Int32, ParameterDirection.Input);

            using (var conexionSQL = new SqlConnection(_iConfiguration.GetConnectionString("ConexionSQLServer")))
            {
                return conexionSQL.Execute("PA_Habitacion_Actualizar", parametros, commandType: CommandType.StoredProcedure) > 0;
            }
        }

        public bool Eliminar(Habitacion P_Entidad)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add("@NumeroHabitacion", P_Entidad.NumeroHabitacion, DbType.String, ParameterDirection.Input, 10);

            using (var conexionSQL = new SqlConnection(_iConfiguration.GetConnectionString("ConexionSQLServer")))
            {
                return conexionSQL.Execute("PA_Habitacion_Eliminar", parametros, commandType: CommandType.StoredProcedure) > 0;
            }
        }

        public List<Habitacion> Consultar(Habitacion P_Entidad)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add("@NumeroHabitacion", P_Entidad.NumeroHabitacion, DbType.String, ParameterDirection.Input, 10);

            using (var conexionSQL = new SqlConnection(_iConfiguration.GetConnectionString("ConexionSQLServer")))
            {
                return (List<Habitacion>)conexionSQL.Query<Habitacion>("PA_Habitacion_ObtenerPorId", parametros, commandType: CommandType.StoredProcedure);
            }
        }

        public List<Habitacion> ObtenerDisponibles(DateTime pFechaEntrada, DateTime pFechaSalida)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add("@FechaEntrada", pFechaEntrada, DbType.DateTime, ParameterDirection.Input);
            parametros.Add("@FechaSalida", pFechaSalida, DbType.DateTime, ParameterDirection.Input);

            using (var conexionSQL = new SqlConnection(_iConfiguration.GetConnectionString("ConexionSQLServer")))
            {
                return (List<Habitacion>)conexionSQL.Query<Habitacion>("PA_Habitacion_ObtenerDisponibles", parametros, commandType: CommandType.StoredProcedure);
            }
        }

        public List<TipoHabitacion> ObtenerTiposHabitacion()
        {
            using (var conexionSQL = new SqlConnection(_iConfiguration.GetConnectionString("ConexionSQLServer")))
            {
                return (List<TipoHabitacion>)conexionSQL.Query<TipoHabitacion>("PA_TipoHabitacion_ObtenerTodos", commandType: CommandType.StoredProcedure);
            }
        }

        public List<Habitacion> ConsultarOcupacion(DateTime pFechaInicio, DateTime pFechaFin)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add("@FechaInicio", pFechaInicio, DbType.DateTime, ParameterDirection.Input);
            parametros.Add("@FechaFin", pFechaFin, DbType.DateTime, ParameterDirection.Input);

            using (var conexionSQL = new SqlConnection(_iConfiguration.GetConnectionString("ConexionSQLServer")))
            {
                return (List<Habitacion>)conexionSQL.Query<Habitacion>("PA_Habitacion_ConsultarOcupacion", parametros, commandType: CommandType.StoredProcedure);
            }
        }

        public decimal ObtenerIngresos(DateTime pFechaInicio, DateTime pFechaFin)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add("@FechaInicio", pFechaInicio, DbType.DateTime, ParameterDirection.Input);
            parametros.Add("@FechaFin", pFechaFin, DbType.DateTime, ParameterDirection.Input);

            using (var conexionSQL = new SqlConnection(_iConfiguration.GetConnectionString("ConexionSQLServer")))
            {
                return conexionSQL.ExecuteScalar<decimal>("PA_Habitacion_ObtenerIngresos", parametros, commandType: CommandType.StoredProcedure);
            }
        }
        #endregion
    }
}