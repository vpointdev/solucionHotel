using AccesoDatos.Interfaces;
using Dapper;
using Entidades.SQLServer;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data;

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
        public Reservacion Crear(Reservacion entidad)
        {
            var parametros = new DynamicParameters();
            parametros.Add("@UsuarioId", entidad.UsuarioId, DbType.Int32);
            parametros.Add("@HabitacionId", entidad.HabitacionId, DbType.Int32);
            parametros.Add("@FechaEntrada", entidad.FechaEntrada, DbType.DateTime);
            parametros.Add("@FechaSalida", entidad.FechaSalida, DbType.DateTime);

            using var connection = new SqlConnection(_iConfiguration.GetConnectionString("ConexionSQLServer"));
            return connection.QuerySingle<Reservacion>("PA_Reservacion_Crear",
                parametros,
                commandType: CommandType.StoredProcedure);
        }

        public Reservacion ProcesarPago(int reservacionId, int usuarioId)
        {
            var parametros = new DynamicParameters();
            parametros.Add("@ReservacionId", reservacionId, DbType.Int32);
            parametros.Add("@UsuarioId", usuarioId, DbType.Int32);

            using var connection = new SqlConnection(_iConfiguration.GetConnectionString("ConexionSQLServer"));
            return connection.QuerySingle<Reservacion>("PA_Reservacion_ProcesarPago",
                parametros,
                commandType: CommandType.StoredProcedure);
        }

        public Reservacion Cancelar(int reservacionId, int usuarioId)
        {
            var parametros = new DynamicParameters();
            parametros.Add("@ReservacionId", reservacionId, DbType.Int32);
            parametros.Add("@UsuarioId", usuarioId, DbType.Int32);

            using var connection = new SqlConnection(_iConfiguration.GetConnectionString("ConexionSQLServer"));
            return connection.QuerySingle<Reservacion>("PA_Reservacion_Cancelar",
                parametros,
                commandType: CommandType.StoredProcedure);
        }

        public List<Reservacion> ObtenerPorUsuario(int usuarioId)
        {
            var parametros = new DynamicParameters();
            parametros.Add("@UsuarioId", usuarioId, DbType.Int32);

            using var connection = new SqlConnection(_iConfiguration.GetConnectionString("ConexionSQLServer"));
            return connection.Query<Reservacion>("PA_Reservacion_ObtenerPorUsuario",
                parametros,
                commandType: CommandType.StoredProcedure).AsList();
        }

        public List<Habitacion> ObtenerDisponibles(DateTime fechaEntrada, DateTime fechaSalida)
        {
            var parametros = new DynamicParameters();
            parametros.Add("@FechaEntrada", fechaEntrada, DbType.DateTime);
            parametros.Add("@FechaSalida", fechaSalida, DbType.DateTime);

            using var connection = new SqlConnection(_iConfiguration.GetConnectionString("ConexionSQLServer"));
            return connection.Query<Habitacion>("PA_Habitacion_ObtenerDisponibles",
                parametros,
                commandType: CommandType.StoredProcedure).AsList();
        }
        #endregion
    }
}