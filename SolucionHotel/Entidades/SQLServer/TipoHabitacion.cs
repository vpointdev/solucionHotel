public class TipoHabitacion
{
    #region Propiedades
    public int TipoHabitacionId { get; set; }
    public string Nombre { get; set; }
    public string Descripcion { get; set; }
    public decimal PrecioBase { get; set; }
    public int Capacidad { get; set; }
    public bool Activo { get; set; }
    public DateTime FechaCreacion { get; set; }
    #endregion

    #region Constructor
    public TipoHabitacion()
    {
        TipoHabitacionId = 0;
        Nombre = string.Empty;
        Descripcion = string.Empty;
        PrecioBase = 0;
        Capacidad = 0;
        Activo = true;
        FechaCreacion = DateTime.MinValue;
    }
    #endregion
}