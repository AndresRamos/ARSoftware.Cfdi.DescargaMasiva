using Ardalis.SmartEnum;

namespace ARSoftware.Cfdi.DescargaMasiva.Enumerations
{
    /// <summary>
    ///     Estado de la solicitud de verificacion
    /// </summary>
    public sealed class EstadoSolicitud : SmartEnum<EstadoSolicitud>
    {
        /// <summary>
        ///     1 = Aceptada
        /// </summary>
        public static readonly EstadoSolicitud Aceptada = new EstadoSolicitud("Aceptada", 1);

        /// <summary>
        ///     2 = EnProceso
        /// </summary>
        public static readonly EstadoSolicitud EnProceso = new EstadoSolicitud("EnProceso", 2);

        /// <summary>
        ///     3 = Terminada
        /// </summary>
        public static readonly EstadoSolicitud Terminada = new EstadoSolicitud("Terminada", 3);

        /// <summary>
        ///     4 = Error
        /// </summary>
        public static readonly EstadoSolicitud Error = new EstadoSolicitud("Error", 4);

        /// <summary>
        ///     5 = Rechazada
        /// </summary>
        public static readonly EstadoSolicitud Rechazada = new EstadoSolicitud("Rechazada", 5);

        /// <summary>
        ///     6 = Vencida
        /// </summary>
        public static readonly EstadoSolicitud Vencida = new EstadoSolicitud("Vencida", 6);

        private EstadoSolicitud(string name, int value) : base(name, value)
        {
        }
    }
}
