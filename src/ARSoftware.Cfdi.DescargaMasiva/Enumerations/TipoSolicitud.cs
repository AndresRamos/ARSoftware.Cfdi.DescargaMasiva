using Ardalis.SmartEnum;

namespace ARSoftware.Cfdi.DescargaMasiva.Enumerations;

/// <summary>
///     Define el tipo de descarga. Valor utilizado en el atributo TipoSolicitud de la peticion de solicitud.
/// </summary>
public sealed class TipoSolicitud : SmartEnum<TipoSolicitud>
{
    /// <summary>
    ///     0 = CFDI
    /// </summary>
    public static readonly TipoSolicitud Cfdi = new("CFDI", 0);

    /// <summary>
    ///     1 = Metadata
    /// </summary>
    public static readonly TipoSolicitud Metadata = new("Metadata", 1);

    private TipoSolicitud(string name, int value) : base(name, value)
    {
    }
}
