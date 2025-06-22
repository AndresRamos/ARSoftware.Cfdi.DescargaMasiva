using Ardalis.SmartEnum;

namespace ARSoftware.Cfdi.DescargaMasiva.Enumerations
{
    /// <summary>
    ///     Define el tipo de descarga. Valor utilizado en el atributo TipoSolicitud de la peticion de solicitud.
    /// </summary>
    public sealed class TipoSolicitud : SmartEnum<TipoSolicitud, string>
    {
        /// <summary>
        ///     Metadata
        /// </summary>
        public static readonly TipoSolicitud Metadata = new("Metadata", "Metadata");

        /// <summary>
        ///     CFDI
        /// </summary>
        public static readonly TipoSolicitud Cfdi = new("CFDI", "CFDI");

        private TipoSolicitud(string name, string value) : base(name, value)
        {
        }
    }
}
