using Ardalis.SmartEnum;

namespace ARSoftware.Cfdi.DescargaMasiva.Enumerations;

/// <summary>
///     Define el tipo de comprobante. Valor utilizado en el atributo TipoComprobante de la peticion de solicitud.
/// </summary>
public sealed class TipoComprobante : SmartEnum<TipoComprobante, string>
{
    /// <summary>
    ///     Null = Null
    /// </summary>
    public static readonly TipoComprobante Null = new("Null", "Ninguno");

    /// <summary>
    ///     I = Ingreso
    /// </summary>
    public static readonly TipoComprobante Ingreso = new("Ingreso", "I");

    /// <summary>
    ///     E = Egreso
    /// </summary>
    public static readonly TipoComprobante Egreso = new("Egreso", "E");

    /// <summary>
    ///     T = Traslado
    /// </summary>
    public static readonly TipoComprobante Traslado = new("Traslado", "T");

    /// <summary>
    ///     N = Nomina
    /// </summary>
    public static readonly TipoComprobante Nomina = new("Nomina", "N");

    /// <summary>
    ///     P = Pago
    /// </summary>
    public static readonly TipoComprobante Pago = new("Pago", "P");

    private TipoComprobante(string name, string value) : base(name, value)
    {
    }
}
