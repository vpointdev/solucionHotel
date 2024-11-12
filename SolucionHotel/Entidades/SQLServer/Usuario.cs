using System;
using System.Collections.Generic;

namespace Entidades.SQLServer
{
    public class Usuario
    {
        #region Propiedades
        public int UsuarioId { get; set; }
        public string NombreUsuario { get; set; }
        public string Clave { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string CorreoRegistro { get; set; }
        public bool Estado { get; set; }
       
        #endregion

        #region Constructor
        public Usuario()
        {
            UsuarioId = 0;
            NombreUsuario = string.Empty;
            Clave = string.Empty;
            FechaRegistro = DateTime.Now;
            CorreoRegistro = string.Empty;
            Estado = true;
        }
        #endregion
    }
}