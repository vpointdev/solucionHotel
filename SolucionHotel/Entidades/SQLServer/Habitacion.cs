namespace Entidades.SQLServer
{
    public class Habitacion
    {
        #region Propiedades
        public int HabitacionId { get; set; }
        public string NumeroHabitacion { get; set; }
        public int TipoHabitacionId { get; set; }
        public int Piso { get; set; }
        public string Estado { get; set; }
        public string Observaciones { get; set; }
        public bool Activo { get; set; }
        public int UsuarioCreacionId { get; set; }
        public int? UsuarioModificacionId { get; set; }
        #endregion

        #region Constructor
        public Habitacion()
        {
            HabitacionId = 0;
            NumeroHabitacion = string.Empty;
            TipoHabitacionId = 0;
            Piso = 0;
            Estado = "Disponible";
            Observaciones = string.Empty;
            Activo = true;
            UsuarioCreacionId = 0;
            UsuarioModificacionId = null;
        }
        #endregion
    }
}