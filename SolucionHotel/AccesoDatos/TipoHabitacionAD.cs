using Dapper;
using System.Data;
using System.Data.SqlClient;
using AccesoDatos.Interfaces;
using Entidades.SQLServer;
using Microsoft.Extensions.Configuration;

namespace AccesoDatos
{
    public class TipoHabitacionAD : ITipoHabitacionAD
    {
        #region Atributos
        private readonly IConfiguration _iConfiguration;
        #endregion

        #region Constructor
        public TipoHabitacionAD(IConfiguration iConfiguration)
        {
            _iConfiguration = iConfiguration;
        }
        #endregion

        #region Métodos Públicos
        public TipoHabitacion Crear(string nombre, string descripcion, decimal precioBase, int capacidad)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Nombre", nombre, DbType.String);
            parameters.Add("@Descripcion", descripcion, DbType.String);
            parameters.Add("@PrecioBase", precioBase, DbType.Decimal);
            parameters.Add("@Capacidad", capacidad, DbType.Int32);

            using (var connection = new SqlConnection(_iConfiguration.GetConnectionString("ConexionSQLServer")))
            {
                return connection.QuerySingle<TipoHabitacion>(
                    "PA_TipoHabitacion_Crear",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
            }
        }

        public List<TipoHabitacion> Obtener(int? tipoHabitacionId = null)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@TipoHabitacionId", tipoHabitacionId, DbType.Int32);

            using (var connection = new SqlConnection(_iConfiguration.GetConnectionString("ConexionSQLServer")))
            {
                return connection.Query<TipoHabitacion>(
                    "PA_TipoHabitacion_Obtener",
                    parameters,
                    commandType: CommandType.StoredProcedure
                ).ToList();
            }
        }

        public TipoHabitacion Actualizar(int tipoHabitacionId, string nombre, string descripcion, decimal precioBase, int capacidad)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@TipoHabitacionId", tipoHabitacionId, DbType.Int32);
            parameters.Add("@Nombre", nombre, DbType.String);
            parameters.Add("@Descripcion", descripcion, DbType.String);
            parameters.Add("@PrecioBase", precioBase, DbType.Decimal);
            parameters.Add("@Capacidad", capacidad, DbType.Int32);

            using (var connection = new SqlConnection(_iConfiguration.GetConnectionString("ConexionSQLServer")))
            {
                return connection.QuerySingle<TipoHabitacion>(
                    "PA_TipoHabitacion_Actualizar",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
            }
        }

        public bool Eliminar(int tipoHabitacionId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@TipoHabitacionId", tipoHabitacionId, DbType.Int32);

            using (var connection = new SqlConnection(_iConfiguration.GetConnectionString("ConexionSQLServer")))
            {
                connection.Execute(
                    "PA_TipoHabitacion_Eliminar",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
                return true;
            }
        }
        #endregion
    }
}