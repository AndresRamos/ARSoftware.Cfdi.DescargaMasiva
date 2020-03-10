namespace ARSoftware.Cfdi.DescargaMasiva.Enumerations
{
    public class EstadoSolicitud : Enumeration
    {
        public static readonly EstadoSolicitud Aceptada = new EstadoSolicitud(1, "Aceptada");
        public static readonly EstadoSolicitud EnProceso = new EstadoSolicitud(2, "EnProceso");
        public static readonly EstadoSolicitud Terminada = new EstadoSolicitud(3, "Terminada");
        public static readonly EstadoSolicitud Error = new EstadoSolicitud(4, "Error");
        public static readonly EstadoSolicitud Rechazada = new EstadoSolicitud(5, "Rechazada");
        public static readonly EstadoSolicitud Vencida = new EstadoSolicitud(6, "Vencida");

        public EstadoSolicitud(int id, string name) : base(id, name)
        {
        }

        public static bool TryParse(string estadoSolicitud, out EstadoSolicitud result)
        {
            if (estadoSolicitud == null)
            {
                result = new EstadoSolicitud(0, "Estado Invalido");
                return false;
            }

            switch (estadoSolicitud)
            {
                case "1":
                    result = Aceptada;
                    return true;
                case "2":
                    result = EnProceso;
                    return true;
                case "3":
                    result = Terminada;
                    return true;
                case "4":
                    result = Error;
                    return true;
                case "5":
                    result = Rechazada;
                    return true;
                case "6":
                    result = Vencida;
                    return true;
                default:
                    result = new EstadoSolicitud(0, "Estado Invalido");
                    return false;
            }
        }
    }
}