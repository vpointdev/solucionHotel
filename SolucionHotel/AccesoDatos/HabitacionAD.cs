using AccesoDatos.Interfaces;
using Dapper;
using Entidades.SQLServer;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace AccesoDatos
{
    public class HabitacionAD : IHabitacionAD
    {
        private readonly IConfiguration _iConfiguration;

        public HabitacionAD(IConfiguration iConfiguration)
        {
            _iConfiguration = iConfiguration;
        }

        public Habitacion Crear(string numeroHabitacion, int tipoHabitacionId, int piso, string estado = "Disponible", string observaciones = null)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@NumeroHabitacion", numeroHabitacion, DbType.String);
            parameters.Add("@TipoHabitacionId", tipoHabitacionId, DbType.Int32);
            parameters.Add("@Piso", piso, DbType.Int32);
            parameters.Add("@Estado", estado, DbType.String);
            parameters.Add("@Observaciones", observaciones, DbType.String);

            using (var connection = new SqlConnection(_iConfiguration.GetConnectionString("ConexionSQLServer")))
            {
                return connection.QuerySingle<Habitacion>("PA_Habitacion_Crear", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public List<Habitacion> Obtener(int? habitacionId = null, string numeroHabitacion = null, int? tipoHabitacionId = null, string estado = null)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@HabitacionId", habitacionId, DbType.Int32);
            parameters.Add("@NumeroHabitacion", numeroHabitacion, DbType.String);
            parameters.Add("@TipoHabitacionId", tipoHabitacionId, DbType.Int32);
            parameters.Add("@Estado", estado, DbType.String);

            using (var connection = new SqlConnection(_iConfiguration.GetConnectionString("ConexionSQLServer")))
            {
                return connection.Query<Habitacion>("PA_Habitacion_Obtener", parameters, commandType: CommandType.StoredProcedure).ToList();
            }
        }

        public Habitacion Actualizar(int habitacionId, int tipoHabitacionId, int piso, string estado, string observaciones = null)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@HabitacionId", habitacionId, DbType.Int32);
            parameters.Add("@TipoHabitacionId", tipoHabitacionId, DbType.Int32);
            parameters.Add("@Piso", piso, DbType.Int32);
            parameters.Add("@Estado", estado, DbType.String);
            parameters.Add("@Observaciones", observaciones, DbType.String);

            using (var connection = new SqlConnection(_iConfiguration.GetConnectionString("ConexionSQLServer")))
            {
                return connection.QuerySingle<Habitacion>("PA_Habitacion_Actualizar", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public bool Eliminar(int habitacionId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@HabitacionId", habitacionId, DbType.Int32);

            using (var connection = new SqlConnection(_iConfiguration.GetConnectionString("ConexionSQLServer")))
            {
                connection.Execute("PA_Habitacion_Eliminar", parameters, commandType: CommandType.StoredProcedure);
                return true;
            }
        }
    }
}