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
        private readonly IConfiguration _configuration;

        public HabitacionAD(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public bool Agregar(Habitacion habitacion)
        {
            var parametros = new DynamicParameters();
            parametros.Add("@NumeroHabitacion", habitacion.NumeroHabitacion, DbType.String);
            parametros.Add("@TipoHabitacionId", habitacion.TipoHabitacionId, DbType.Int32);
            parametros.Add("@Piso", habitacion.Piso, DbType.Int32);
            parametros.Add("@Observaciones", habitacion.Observaciones, DbType.String);

            using var conexion = new SqlConnection(_configuration.GetConnectionString("ConexionSQLServer"));
            return conexion.Execute("PA_Habitacion_Crear", parametros,
                commandType: CommandType.StoredProcedure) > 0;
        }

        public bool Modificar(Habitacion habitacion)
        {
            var parametros = new DynamicParameters();
            parametros.Add("@HabitacionId", habitacion.HabitacionId, DbType.Int32);
            parametros.Add("@NumeroHabitacion", habitacion.NumeroHabitacion, DbType.String);
            parametros.Add("@TipoHabitacionId", habitacion.TipoHabitacionId, DbType.Int32);
            parametros.Add("@Piso", habitacion.Piso, DbType.Int32);
            parametros.Add("@Estado", habitacion.Estado, DbType.String);
            parametros.Add("@Observaciones", habitacion.Observaciones, DbType.String);
            parametros.Add("@Activo", habitacion.Activo, DbType.Boolean);

            using var conexion = new SqlConnection(_configuration.GetConnectionString("ConexionSQLServer"));
            return conexion.Execute("PA_Habitacion_Actualizar", parametros,
                commandType: CommandType.StoredProcedure) > 0;
        }

        public bool Eliminar(int habitacionId)
        {
            DynamicParameters parametros = new DynamicParameters();
            parametros.Add("@HabitacionId", habitacionId, DbType.Int32, ParameterDirection.Input);

            using (var conexionSQL = new SqlConnection(_configuration.GetConnectionString("ConexionSQLServer")))
            {
                var result = conexionSQL.Execute("PA_Habitacion_Eliminar", parametros,
                    commandType: CommandType.StoredProcedure);
                return result >= 0; // Return true if execution was successful
            }
        }

        public List<Habitacion> ObtenerTodos()
        {
            using var conexion = new SqlConnection(_configuration.GetConnectionString("ConexionSQLServer"));
            return conexion.Query<Habitacion>("PA_Habitacion_ObtenerTodos",
                commandType: CommandType.StoredProcedure).ToList();
        }

        public List<TipoHabitacion> ObtenerTiposHabitacion()
        {
            using var conexion = new SqlConnection(_configuration.GetConnectionString("ConexionSQLServer"));
            return conexion.Query<TipoHabitacion>("PA_TipoHabitacion_ObtenerTodos",
                commandType: CommandType.StoredProcedure).ToList();
        }
    }
}