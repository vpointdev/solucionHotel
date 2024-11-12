namespace HotelFE.Models
{
    public class PerfilModel
    {
        public int PerfilId { get; set; }
        public string Nombre { get; set; }
        public bool Estado { get; set; }


        public PerfilModel()
        {
            PerfilId = 0;
            Nombre = string.Empty;
            Estado = true;
        }
    }
}
