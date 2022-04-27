using Ardalis.SmartEnum;

namespace ARSoftware.Cfdi.DescargaMasiva.Enumerations
{
    public sealed class EstadoSolicitud : SmartEnum<EstadoSolicitud>
    {
        public static readonly EstadoSolicitud Aceptada = new EstadoSolicitud("Aceptada", 1);
        public static readonly EstadoSolicitud EnProceso = new EstadoSolicitud("EnProceso", 2);
        public static readonly EstadoSolicitud Terminada = new EstadoSolicitud("Terminada", 3);
        public static readonly EstadoSolicitud Error = new EstadoSolicitud("Error", 4);
        public static readonly EstadoSolicitud Rechazada = new EstadoSolicitud("Rechazada", 5);
        public static readonly EstadoSolicitud Vencida = new EstadoSolicitud("Vencida", 6);

        private EstadoSolicitud(string name, int value) : base(name, value)
        {
        }
    }
}
