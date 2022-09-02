using Ardalis.SmartEnum;

namespace ARSoftware.Cfdi.DescargaMasiva.Enumerations
{
    /// <summary>
    ///     Define el estado del comprobante. Valor utilizado en el atributo EstadoComprobante en la peticion de solicitud.
    /// </summary>
    public sealed class EstadoComprobante : SmartEnum<EstadoComprobante, int>
    {
        /// <summary>
        ///     Null
        /// </summary>
        public static readonly EstadoComprobante Null = new EstadoComprobante("Null", -1);

        /// <summary>
        ///     Cancelado = 0
        /// </summary>
        public static readonly EstadoComprobante Cancelado = new EstadoComprobante("Cancelado", 0);

        /// <summary>
        ///     Vigente = 1
        /// </summary>
        public static readonly EstadoComprobante Vigente = new EstadoComprobante("Vigente", 1);

        private EstadoComprobante(string name, int value) : base(name, value)
        {
        }
    }
}
