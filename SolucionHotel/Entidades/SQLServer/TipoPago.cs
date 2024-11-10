public class TipoPago
{
    #region Propiedades
    public int TipoPagoId { get; set; }
    public string Nombre { get; set; }
    public string Descripcion { get; set; }
    public bool Activo { get; set; }
    public DateTime FechaCreacion { get; set; }
    #endregion

    #region Constructor
    public TipoPago()
    {
        TipoPagoId = 0;
        Nombre = string.Empty;
        Descripcion = string.Empty;
        Activo = true;
        FechaCreacion = DateTime.MinValue;
    }
    #endregion
}
