using AccesoDatos.Interfaces;
using Entidades;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace AccesoDatos
{
    public class BitacoraAD : IBitacoraAD
    {
        #region Atributos
        private readonly IConfiguration _iConfiguration;
        private string CadenaConexion = string.Empty;
        private MongoClient InstanciaBD;
        private IMongoDatabase BaseDatos;
        private const string NombreBD = "Seguridad";
        #endregion

        #region Constructor
        public BitacoraAD(IConfiguration iConfiguration)
        {
            _iConfiguration = iConfiguration;
            CadenaConexion = _iConfiguration.GetConnectionString("ConexionMongoDB");
        }
        #endregion

        #region Métodos Privados
        private void EstablecerConexion()
        {
            try
            {
                // Inicializar las variables de conexión
                InstanciaBD = new MongoClient(CadenaConexion);
                BaseDatos = InstanciaBD.GetDatabase(NombreBD);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Métodos Públicos
        public bool Agregar(Bitacora P_Entidad)
        {
            bool resultado = false;

            try
            {
                EstablecerConexion();
                var Coleccion = BaseDatos.GetCollection<Bitacora>("Bitacora");
                Coleccion.InsertOne(P_Entidad);
                resultado = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (InstanciaBD != null)
                    InstanciaBD = null;
                if (BaseDatos != null)
                    BaseDatos = null;
            }

            return resultado;
        }

        public List<Bitacora> Listar()
        {
            List<Bitacora> resultado = new List<Bitacora>();

            try
            {
                EstablecerConexion();
                var Coleccion = BaseDatos.GetCollection<Bitacora>("Bitacora");
                resultado = Coleccion.Find(doc => true).ToList().OrderBy(orden => orden.AccionRealizada).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (InstanciaBD != null)
                    InstanciaBD = null;
                if (BaseDatos != null)
                    BaseDatos = null;
            }

            return resultado;
        }

        public List<Bitacora> Consultar(Bitacora P_Entidad)
        {
            List<Bitacora> resultado = new List<Bitacora>();

            try
            {
                EstablecerConexion();
                var Coleccion = BaseDatos.GetCollection<Bitacora>("Bitacora");

                if (!string.IsNullOrEmpty(P_Entidad.ID))
                    resultado = Coleccion.Find(doc => doc.ID.Equals(P_Entidad.ID))
                        .ToList()
                        .OrderBy(orden => orden.ID)
                        .ToList();
                else if (!string.IsNullOrEmpty(P_Entidad.UsuarioRegistro))
                    resultado = Coleccion.Find(doc => doc.UsuarioRegistro.Equals(P_Entidad.UsuarioRegistro))
                        .ToList()
                        .OrderBy(orden => orden.UsuarioRegistro)
                        .ToList();
                else if (!string.IsNullOrEmpty(P_Entidad.AccionRealizada))
                    resultado = Coleccion.Find(doc => doc.AccionRealizada.Equals(P_Entidad.AccionRealizada))
                        .ToList()
                        .OrderBy(orden => orden.AccionRealizada)
                        .ToList();
                else
                    resultado = Coleccion.Find(doc => true)
                        .ToList()
                        .OrderBy(orden => orden.AccionRealizada)
                        .ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (InstanciaBD != null)
                    InstanciaBD = null;
                if (BaseDatos != null)
                    BaseDatos = null;
            }

            return resultado;
        }

        public bool Modificar(Bitacora P_Entidad)
        {
            bool resultado = false;

            try
            {
                EstablecerConexion();
                var Coleccion = BaseDatos.GetCollection<Bitacora>("Bitacora");
                Coleccion.ReplaceOne(documento => documento.ID.Equals(P_Entidad.ID), P_Entidad);
                resultado = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (InstanciaBD != null)
                    InstanciaBD = null;
                if (BaseDatos != null)
                    BaseDatos = null;
            }

            return resultado;
        }

        public bool Eliminar(Bitacora P_Entidad)
        {
            bool resultado = false;

            try
            {
                EstablecerConexion();
                var Coleccion = BaseDatos.GetCollection<Bitacora>("Bitacora");
                Coleccion.DeleteOne(documento => documento.ID.Equals(P_Entidad.ID));
                resultado = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (InstanciaBD != null)
                    InstanciaBD = null;
                if (BaseDatos != null)
                    BaseDatos = null;
            }

            return resultado;
        }
        #endregion
    }
}