using Ardalis.SmartEnum;

namespace ARSoftware.Cfdi.DescargaMasiva.Enumerations
{
    public sealed class EstadoComprobante : SmartEnum<EstadoComprobante, int>
    {
        public static readonly EstadoComprobante Null = new EstadoComprobante("Null", -1);

        public static readonly EstadoComprobante Cancelado = new EstadoComprobante("Cancelado", 0);
        public static readonly EstadoComprobante Vigente = new EstadoComprobante("Vigente", 1);

        private EstadoComprobante(string name, int value) : base(name, value)
        {
        }
    }
}
