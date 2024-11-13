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
        private readonly IConfiguration _iConfiguration;

        public ReservacionAD(IConfiguration iConfiguration)
        {
            _iConfiguration = iConfiguration;
        }

        public List<Reservacion> ObtenerTodos()
        {
            using (var conexionSQL = new SqlConnection(_iConfiguration.GetConnectionString("ConexionSQLServer")))
            {
                return (List<Reservacion>)conexionSQL.Query<Reservacion>(
                    "PA_Reservacion_ObtenerTodos",
                    commandType: CommandType.StoredProcedure);
            }
        }

        public bool Agregar(Reservacion reservacion)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add("@UsuarioId", reservacion.UsuarioId, DbType.Int32, ParameterDirection.Input);
            parametros.Add("@HabitacionId", reservacion.HabitacionId, DbType.Int32, ParameterDirection.Input);
            parametros.Add("@FechaEntrada", reservacion.FechaEntrada, DbType.DateTime, ParameterDirection.Input);
            parametros.Add("@FechaSalida", reservacion.FechaSalida, DbType.DateTime, ParameterDirection.Input);
            parametros.Add("@PrecioTotal", reservacion.PrecioTotal, DbType.Decimal, ParameterDirection.Input);
            parametros.Add("@Observaciones", reservacion.Observaciones, DbType.String, ParameterDirection.Input, 500);

            using (var conexionSQL = new SqlConnection(_iConfiguration.GetConnectionString("ConexionSQLServer")))
            {
                return conexionSQL.Execute("PA_Reservacion_Crear", parametros,
                    commandType: CommandType.StoredProcedure) > 0;
            }
        }

        public bool Modificar(Reservacion reservacion)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add("@ReservacionId", reservacion.ReservacionId, DbType.Int32, ParameterDirection.Input);
            parametros.Add("@HabitacionId", reservacion.HabitacionId, DbType.Int32, ParameterDirection.Input);
            parametros.Add("@FechaEntrada", reservacion.FechaEntrada, DbType.DateTime, ParameterDirection.Input);
            parametros.Add("@FechaSalida", reservacion.FechaSalida, DbType.DateTime, ParameterDirection.Input);
            parametros.Add("@EstadoReservacion", reservacion.EstadoReservacion, DbType.String, ParameterDirection.Input, 20);
            parametros.Add("@PrecioTotal", reservacion.PrecioTotal, DbType.Decimal, ParameterDirection.Input);
            parametros.Add("@Observaciones", reservacion.Observaciones, DbType.String, ParameterDirection.Input, 500);

            using (var conexionSQL = new SqlConnection(_iConfiguration.GetConnectionString("ConexionSQLServer")))
            {
                return conexionSQL.Execute("PA_Reservacion_Modificar", parametros,
                    commandType: CommandType.StoredProcedure) > 0;
            }
        }

        public bool Eliminar(int reservacionId)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add("@ReservacionId", reservacionId, DbType.Int32, ParameterDirection.Input);

            using (var conexionSQL = new SqlConnection(_iConfiguration.GetConnectionString("ConexionSQLServer")))
            {
                return conexionSQL.Execute("PA_Reservacion_Eliminar", parametros,
                    commandType: CommandType.StoredProcedure) > 0;
            }
        }

        public bool CancelarReservacion(int reservacionId)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add("@ReservacionId", reservacionId, DbType.Int32, ParameterDirection.Input);

            using (var conexionSQL = new SqlConnection(_iConfiguration.GetConnectionString("ConexionSQLServer")))
            {
                return conexionSQL.Execute("PA_Reservacion_Cancelar", parametros,
                    commandType: CommandType.StoredProcedure) > 0;
            }
        }
    }
}