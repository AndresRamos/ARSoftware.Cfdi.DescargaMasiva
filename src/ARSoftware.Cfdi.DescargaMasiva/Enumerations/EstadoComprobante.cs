using Ardalis.SmartEnum;

namespace ARSoftware.Cfdi.DescargaMasiva.Enumerations
{
    /// <summary>
    ///     Define el estado del comprobante. Valor utilizado en el atributo EstadoComprobante en la peticion de solicitud.
    /// </summary>
    public sealed class EstadoComprobante : SmartEnum<EstadoComprobante, string>
    {
        /// <summary>
        ///     Null
        /// </summary>
        public static readonly EstadoComprobante Null = new("Null", "Null");

        /// <summary>
        ///     Todos
        /// </summary>
        public static readonly EstadoComprobante Todos = new("Todos", "Todos");

        /// <summary>
        ///     Cancelado
        /// </summary>
        public static readonly EstadoComprobante Cancelado = new("Cancelado", "Cancelado");

        /// <summary>
        ///     Vigente
        /// </summary>
        public static readonly EstadoComprobante Vigente = new("Vigente", "Vigente");

        private EstadoComprobante(string name, string value) : base(name, value)
        {
        }
    }
}
