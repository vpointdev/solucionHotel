using System;

namespace Entidades.SQLServer
{
    public class Perfil
    {
        #region Propiedades
        public int PerfilId { get; set; }          
        public string Nombre { get; set; }  
        public bool Estado { get; set; }          
        #endregion

        #region Constructor
        public Perfil()
        {
            PerfilId = 2;
            Nombre = string.Empty;
            Estado = true;
        }
        #endregion
    }
}