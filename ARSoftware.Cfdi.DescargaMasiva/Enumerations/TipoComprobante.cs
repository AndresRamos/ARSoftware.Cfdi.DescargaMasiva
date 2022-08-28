using Ardalis.SmartEnum;

namespace ARSoftware.Cfdi.DescargaMasiva.Enumerations
{
    public sealed class TipoComprobante : SmartEnum<TipoComprobante, string>
    {
        public static readonly TipoComprobante Null = new TipoComprobante("Null", "Ninguno");

        public static readonly TipoComprobante Ingreso = new TipoComprobante("Ingreso", "I");
        public static readonly TipoComprobante Egreso = new TipoComprobante("Egreso", "E");
        public static readonly TipoComprobante Traslado = new TipoComprobante("Traslado", "T");
        public static readonly TipoComprobante Nomina = new TipoComprobante("Nomina", "N");
        public static readonly TipoComprobante Pago = new TipoComprobante("Pago", "P");

        private TipoComprobante(string name, string value) : base(name, value)
        {
        }
    }
}
