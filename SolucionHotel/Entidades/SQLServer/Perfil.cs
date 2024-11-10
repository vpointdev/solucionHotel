namespace Entidades.SQLServer
{
    public class Perfil
    {
        #region Propiedades
        public int PerfilId { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public bool Activo { get; set; }
        #endregion

        #region Constructor
        public Perfil()
        {
            PerfilId = 0;
            Nombre = string.Empty;
            Descripcion = string.Empty;
            Activo = true;
        }
        #endregion
    }
}