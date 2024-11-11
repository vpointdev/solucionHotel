using AccesoDatos.Interfaces;
using Entidades;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace AccesoDatos
{
    public class BitacoraAD : IBitacoraAD
    {
        #region Atributos
        private readonly IMongoDatabase _database;
        private readonly IMongoCollection<Bitacora> _collection;
        private const string COLLECTION_NAME = "Bitacora";
        #endregion

        #region Constructor
        public BitacoraAD(IConfiguration configuration)
        {
            var client = new MongoClient(configuration.GetConnectionString("ConexionMongoDB"));
            _database = client.GetDatabase("Hotel");
            _collection = _database.GetCollection<Bitacora>(COLLECTION_NAME);
        }
        #endregion

        #region Métodos Públicos
        public bool Agregar(Bitacora P_Entidad)
        {
            try
            {
                _collection.InsertOne(P_Entidad);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public List<Bitacora> Listar()
        {
            try
            {
                return _collection.Find(_ => true)
                    .SortBy(x => x.AccesionRealizada)
                    .ToList();
            }
            catch
            {
                return new List<Bitacora>();
            }
        }

        public List<Bitacora> Consultar(Bitacora P_Entidad)
        {
            try
            {
                var builder = Builders<Bitacora>.Filter;
                var filter = builder.Empty;

                if (!string.IsNullOrEmpty(P_Entidad.ID))
                    filter = builder.Eq(x => x.ID, P_Entidad.ID);
                else if (!string.IsNullOrEmpty(P_Entidad.UsuarioRegistro))
                    filter = builder.Eq(x => x.UsuarioRegistro, P_Entidad.UsuarioRegistro);
                else if (!string.IsNullOrEmpty(P_Entidad.AccesionRealizada))
                    filter = builder.Eq(x => x.AccesionRealizada, P_Entidad.AccesionRealizada);

                return _collection.Find(filter)
                    .SortBy(x => x.AccesionRealizada)
                    .ToList();
            }
            catch
            {
                return new List<Bitacora>();
            }
        }

        public bool Modificar(Bitacora P_Entidad)
        {
            try
            {
                var filter = Builders<Bitacora>.Filter.Eq(x => x.ID, P_Entidad.ID);
                var result = _collection.ReplaceOne(filter, P_Entidad);
                return result.ModifiedCount > 0;
            }
            catch
            {
                return false;
            }
        }

        public bool Eliminar(Bitacora P_Entidad)
        {
            try
            {
                var filter = Builders<Bitacora>.Filter.Eq(x => x.ID, P_Entidad.ID);
                var result = _collection.DeleteOne(filter);
                return result.DeletedCount > 0;
            }
            catch
            {
                return false;
            }
        }
        #endregion
    }
}