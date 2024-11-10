namespace Entidades.SQLServer
{
    public class Usuario
    {
        #region Propiedades
        public string NombreUsuario { get; set; }
        public string Clave { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string CorreoRegistro { get; set; }
        public bool Estado { get; set; }
        public List<Perfil> Perfil { get; set; }
        #endregion

        #region Constructor
        public Usuario()
        {
            NombreUsuario = string.Empty;
            Clave = string.Empty;
            FechaRegistro = DateTime.MinValue;
            CorreoRegistro = string.Empty;
            Estado = true;
            Perfil = new List<Perfil>();
        }
        #endregion
    }
}