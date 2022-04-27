using Ardalis.SmartEnum;

namespace ARSoftware.Cfdi.DescargaMasiva.Enumerations
{
    public sealed class TipoSolicitud : SmartEnum<TipoSolicitud>
    {
        public static readonly TipoSolicitud Cfdi = new TipoSolicitud("CFDI", 0);
        public static readonly TipoSolicitud Metadata = new TipoSolicitud("Metadata", 1);

        private TipoSolicitud(string name, int value) : base(name, value)
        {
        }
    }
}
