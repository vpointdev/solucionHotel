using AccesoDatos.Interfaces;
using Dapper;
using Entidades.SQLServer;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace AccesoDatos
{
    public class ReservacionAD : IReservacionAD
    {
        #region Atributos
        private readonly IConfiguration _iConfiguration;
        #endregion

        #region Constructor
        public ReservacionAD(IConfiguration iConfiguration)
        {
            _iConfiguration = iConfiguration;
        }
        #endregion

        #region Métodos Públicos
        public bool Agregar(Reservacion P_Entidad)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add("@UsuarioId", P_Entidad.UsuarioId, DbType.Int32, ParameterDirection.Input);
            parametros.Add("@HabitacionId", P_Entidad.HabitacionId, DbType.Int32, ParameterDirection.Input);
            parametros.Add("@FechaEntrada", P_Entidad.FechaEntrada, DbType.DateTime, ParameterDirection.Input);
            parametros.Add("@FechaSalida", P_Entidad.FechaSalida, DbType.DateTime, ParameterDirection.Input);
            parametros.Add("@PrecioTotal", P_Entidad.PrecioTotal, DbType.Decimal, ParameterDirection.Input);
            parametros.Add("@Observaciones", P_Entidad.Observaciones, DbType.String, ParameterDirection.Input, 500);
            parametros.Add("@UsuarioCreacionId", P_Entidad.UsuarioCreacionId, DbType.Int32, ParameterDirection.Input);

            using (var conexionSQL = new SqlConnection(_iConfiguration.GetConnectionString("ConexionSQLServer")))
            {
                return conexionSQL.Execute("PA_Reservacion_Crear", parametros, commandType: CommandType.StoredProcedure) > 0;
            }
        }

        public bool Modificar(Reservacion P_Entidad)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add("@CodigoReservacion", P_Entidad.CodigoReservacion, DbType.String, ParameterDirection.Input, 20);
            parametros.Add("@HabitacionId", P_Entidad.HabitacionId, DbType.Int32, ParameterDirection.Input);
            parametros.Add("@FechaEntrada", P_Entidad.FechaEntrada, DbType.DateTime, ParameterDirection.Input);
            parametros.Add("@FechaSalida", P_Entidad.FechaSalida, DbType.DateTime, ParameterDirection.Input);
            parametros.Add("@EstadoReservacionId", P_Entidad.EstadoReservacionId, DbType.Int32, ParameterDirection.Input);
            parametros.Add("@PrecioTotal", P_Entidad.PrecioTotal, DbType.Decimal, ParameterDirection.Input);
            parametros.Add("@Observaciones", P_Entidad.Observaciones, DbType.String, ParameterDirection.Input, 500);
            parametros.Add("@UsuarioModificacionId", P_Entidad.UsuarioModificacionId, DbType.Int32, ParameterDirection.Input);

            using (var conexionSQL = new SqlConnection(_iConfiguration.GetConnectionString("ConexionSQLServer")))
            {
                return conexionSQL.Execute("PA_Reservacion_Actualizar", parametros, commandType: CommandType.StoredProcedure) > 0;
            }
        }

        public bool Eliminar(Reservacion P_Entidad)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add("@CodigoReservacion", P_Entidad.CodigoReservacion, DbType.String, ParameterDirection.Input, 20);

            using (var conexionSQL = new SqlConnection(_iConfiguration.GetConnectionString("ConexionSQLServer")))
            {
                return conexionSQL.Execute("PA_Reservacion_Eliminar", parametros, commandType: CommandType.StoredProcedure) > 0;
            }
        }

        public List<Reservacion> Consultar(Reservacion P_Entidad)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add("@CodigoReservacion", P_Entidad.CodigoReservacion, DbType.String, ParameterDirection.Input, 20);

            using (var conexionSQL = new SqlConnection(_iConfiguration.GetConnectionString("ConexionSQLServer")))
            {
                return (List<Reservacion>)conexionSQL.Query<Reservacion>("PA_Reservacion_ObtenerPorCodigo", parametros, commandType: CommandType.StoredProcedure);
            }
        }

        public bool CancelarReservacion(string pCodigoReservacion)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add("@CodigoReservacion", pCodigoReservacion, DbType.String, ParameterDirection.Input, 20);

            using (var conexionSQL = new SqlConnection(_iConfiguration.GetConnectionString("ConexionSQLServer")))
            {
                return conexionSQL.Execute("PA_Reservacion_Cancelar", parametros, commandType: CommandType.StoredProcedure) > 0;
            }
        }

        public List<Reservacion> ConsultarPorUsuario(int pUsuarioId)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add("@UsuarioId", pUsuarioId, DbType.Int32, ParameterDirection.Input);

            using (var conexionSQL = new SqlConnection(_iConfiguration.GetConnectionString("ConexionSQLServer")))
            {
                return (List<Reservacion>)conexionSQL.Query<Reservacion>("PA_Reservacion_ObtenerPorUsuario", parametros, commandType: CommandType.StoredProcedure);
            }
        }
        public List<Reservacion> ConsultarPorFecha(DateTime pFechaInicio, DateTime pFechaFin)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add("@FechaInicio", pFechaInicio, DbType.DateTime, ParameterDirection.Input);
            parametros.Add("@FechaFin", pFechaFin, DbType.DateTime, ParameterDirection.Input);

            using (var conexionSQL = new SqlConnection(_iConfiguration.GetConnectionString("ConexionSQLServer")))
            {
                return (List<Reservacion>)conexionSQL.Query<Reservacion>("PA_Reservacion_ObtenerPorRango", parametros, commandType: CommandType.StoredProcedure);
            }
        }

        public decimal CalcularCargoCancelacion(string pCodigoReservacion)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add("@CodigoReservacion", pCodigoReservacion, DbType.String, ParameterDirection.Input, 20);

            using (var conexionSQL = new SqlConnection(_iConfiguration.GetConnectionString("ConexionSQLServer")))
            {
                return conexionSQL.ExecuteScalar<decimal>("PA_Reservacion_CalcularCargoCancelacion", parametros, commandType: CommandType.StoredProcedure);
            }
        }

        public bool CompletarCheckIn(string pCodigoReservacion)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add("@CodigoReservacion", pCodigoReservacion, DbType.String, ParameterDirection.Input, 20);

            using (var conexionSQL = new SqlConnection(_iConfiguration.GetConnectionString("ConexionSQLServer")))
            {
                return conexionSQL.Execute("PA_Reservacion_CompletarCheckIn", parametros, commandType: CommandType.StoredProcedure) > 0;
            }
        }

        public bool CompletarCheckOut(string pCodigoReservacion)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add("@CodigoReservacion", pCodigoReservacion, DbType.String, ParameterDirection.Input, 20);

            using (var conexionSQL = new SqlConnection(_iConfiguration.GetConnectionString("ConexionSQLServer")))
            {
                return conexionSQL.Execute("PA_Reservacion_CompletarCheckOut", parametros, commandType: CommandType.StoredProcedure) > 0;
            }
        }
        #endregion
    }
}
