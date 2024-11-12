namespace HotelFE.Models
{
    public class BitacoraModel
    {
        public string ID { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string UsuarioRegistro { get; set; }
        public string AccesionRealizada { get; set; }
        public string ModuloSistema { get; set; }
        public string Descripcion { get; set; }

        public BitacoraModel() 
        {
            ID = string.Empty;
            FechaRegistro = DateTime.MinValue;
            UsuarioRegistro = string.Empty;
            AccesionRealizada = string.Empty;
            ModuloSistema = string.Empty;
            Descripcion = string.Empty;
        }  


    }
}
