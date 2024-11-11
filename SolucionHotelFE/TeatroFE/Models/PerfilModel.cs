namespace HotelFE.Models
{
    public class PerfilModel
    {
        public int Codigo { get; set; }
        public string Descripcion { get; set; }
        public bool Estado { get; set; }


        public PerfilModel()
        {
            Codigo = 0;
            Descripcion = string.Empty;
            Estado = true;
        }
    }
}
