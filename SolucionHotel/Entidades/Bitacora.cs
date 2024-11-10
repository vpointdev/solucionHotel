using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Entidades
{
    public class Bitacora
    {
        #region Propiedades  
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ID { get; set; }

        [BsonElement("date")]
        public DateTime FechaRegistro { get; set; }

        [BsonElement("user")]
        public string UsuarioRegistro { get; set; }

        [BsonElement("action")]
        public string AccionRealizada { get; set; }

        [BsonElement("module")]
        public string ModuloSistema { get; set; }

        [BsonElement("description")]
        public string Descripcion { get; set; }
        #endregion

        #region Constructor
        public Bitacora()
        {
            ID = string.Empty;
            FechaRegistro = DateTime.MinValue;
            UsuarioRegistro = string.Empty;
            AccionRealizada = string.Empty;
            ModuloSistema = string.Empty;
            Descripcion = string.Empty;
        }
        #endregion
    }
}