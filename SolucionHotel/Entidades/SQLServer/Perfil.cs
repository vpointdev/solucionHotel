using System;

namespace Entidades.SQLServer
{
    public class Perfil
    {
        #region Propiedades
        public int Codigo { get; set; }          
        public string Descripcion { get; set; }  
        public bool Estado { get; set; }          
        #endregion

        #region Constructor
        public Perfil()
        {
            Codigo = 2;
            Descripcion = string.Empty;
            Estado = true;
        }
        #endregion
    }
}