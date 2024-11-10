using AccesoDatos.Interfaces;
using Dapper;
using Entidades.SQLServer;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace AccesoDatos
{
    public class PagoAD : IPagoAD
    {
        #region Atributos
        private readonly IConfiguration _iConfiguration;
        #endregion

        #region Constructor
        public PagoAD(IConfiguration iConfiguration)
        {
            _iConfiguration = iConfiguration;
        }
        #endregion

        #region Métodos Públicos
        public bool Agregar(Pago P_Entidad)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add("@ReservacionId", P_Entidad.ReservacionId, DbType.Int32, ParameterDirection.Input);
            parametros.Add("@TipoPagoId", P_Entidad.TipoPagoId, DbType.Int32, ParameterDirection.Input);
            parametros.Add("@Monto", P_Entidad.Monto, DbType.Decimal, ParameterDirection.Input);
            parametros.Add("@FechaPago", P_Entidad.FechaPago, DbType.DateTime, ParameterDirection.Input);
            parametros.Add("@NumeroTransaccion", P_Entidad.NumeroTransaccion, DbType.String, ParameterDirection.Input, 50);
            parametros.Add("@Estado", P_Entidad.Estado, DbType.String, ParameterDirection.Input, 20);
            parametros.Add("@Observaciones", P_Entidad.Observaciones, DbType.String, ParameterDirection.Input, 500);
            parametros.Add("@UsuarioCreacionId", P_Entidad.UsuarioCreacionId, DbType.Int32, ParameterDirection.Input);

            using (var conexionSQL = new SqlConnection(_iConfiguration.GetConnectionString("ConexionSQLServer")))
            {
                return conexionSQL.Execute("PA_Pago_Crear", parametros, commandType: CommandType.StoredProcedure) > 0;
            }
        }

        public bool Modificar(Pago P_Entidad)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add("@PagoId", P_Entidad.PagoId, DbType.Int32, ParameterDirection.Input);
            parametros.Add("@Estado", P_Entidad.Estado, DbType.String, ParameterDirection.Input, 20);
            parametros.Add("@Observaciones", P_Entidad.Observaciones, DbType.String, ParameterDirection.Input, 500);

            using (var conexionSQL = new SqlConnection(_iConfiguration.GetConnectionString("ConexionSQLServer")))
            {
                return conexionSQL.Execute("PA_Pago_Actualizar", parametros, commandType: CommandType.StoredProcedure) > 0;
            }
        }

        public bool Eliminar(Pago P_Entidad)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add("@PagoId", P_Entidad.PagoId, DbType.Int32, ParameterDirection.Input);

            using (var conexionSQL = new SqlConnection(_iConfiguration.GetConnectionString("ConexionSQLServer")))
            {
                return conexionSQL.Execute("PA_Pago_Eliminar", parametros, commandType: CommandType.StoredProcedure) > 0;
            }
        }

        public List<Pago> Consultar(Pago P_Entidad)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add("@PagoId", P_Entidad.PagoId, DbType.Int32, ParameterDirection.Input);

            using (var conexionSQL = new SqlConnection(_iConfiguration.GetConnectionString("ConexionSQLServer")))
            {
                return (List<Pago>)conexionSQL.Query<Pago>("PA_Pago_ObtenerPorId", parametros, commandType: CommandType.StoredProcedure);
            }
        }

        public List<Pago> ObtenerPorReservacion(string pCodigoReservacion)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add("@CodigoReservacion", pCodigoReservacion, DbType.String, ParameterDirection.Input, 20);

            using (var conexionSQL = new SqlConnection(_iConfiguration.GetConnectionString("ConexionSQLServer")))
            {
                return (List<Pago>)conexionSQL.Query<Pago>("PA_Pago_ObtenerPorReservacion", parametros, commandType: CommandType.StoredProcedure);
            }
        }

        public List<TipoPago> ObtenerTiposPago()
        {
            using (var conexionSQL = new SqlConnection(_iConfiguration.GetConnectionString("ConexionSQLServer")))
            {
                return (List<TipoPago>)conexionSQL.Query<TipoPago>("PA_TipoPago_ObtenerTodos", commandType: CommandType.StoredProcedure);
            }
        }

        public decimal ObtenerTotalPorReservacion(string pCodigoReservacion)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add("@CodigoReservacion", pCodigoReservacion, DbType.String, ParameterDirection.Input, 20);

            using (var conexionSQL = new SqlConnection(_iConfiguration.GetConnectionString("ConexionSQLServer")))
            {
                return conexionSQL.ExecuteScalar<decimal>("PA_Pago_ObtenerTotalPorReservacion", parametros, commandType: CommandType.StoredProcedure);
            }
        }
        #endregion
    }
}