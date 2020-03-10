namespace ARSoftware.Cfdi.DescargaMasiva.Enumerations
{
    public class CodigoEstadoSolicitud : Enumeration
    {
        /// <summary>
        ///     Solicitud recibida con éxito
        /// </summary>
        public static readonly CodigoEstadoSolicitud _5000 = new CodigoEstadoSolicitud(5000, "5000", "Solicitud recibida con éxito", "Indica que la solicitud de descarga que se está verificando fue aceptada.");

        /// <summary>
        ///     Se agotó las solicitudes de por vida
        /// </summary>
        public static readonly CodigoEstadoSolicitud _5002 = new CodigoEstadoSolicitud(5002, "5002", "Se agotó las solicitudes de por vida", "Para el caso de descarga de tipo CFDI, se tiene unlímite máximo para solicitudes con los mismos parámetros(Fecha Inicial, Fecha Final, RfcEmisor, RfcReceptor).");

        /// <summary>
        ///     Tope máximo
        /// </summary>
        public static readonly CodigoEstadoSolicitud _5003 = new CodigoEstadoSolicitud(5003, "5003", "Tope máximo", "Indica que en base a los parámetros de consulta se está superando el tope máximo de CFDI o Metadata, por solicitud de descarga masiva.");

        /// <summary>
        ///     No se encontró la información
        /// </summary>
        public static readonly CodigoEstadoSolicitud _5004 = new CodigoEstadoSolicitud(5004, "5004", "No se encontró la información", "Indica que la solicitud de descarga que se está verificando no generó paquetes por falta de información.");

        /// <summary>
        ///     Solicitud duplicada
        /// </summary>
        public static readonly CodigoEstadoSolicitud _5005 = new CodigoEstadoSolicitud(5005, "5005", "Solicitud duplicada", "En caso de que exista una solicitud vigente con los mismos parámetros (Fecha Inicial, Fecha Final, RfcEmisor, RfcReceptor, TipoSolicitud), no se permitirá generar una nueva solicitud.");

        /// <summary>
        ///     Error no Controlado
        /// </summary>
        public static readonly CodigoEstadoSolicitud _404 = new CodigoEstadoSolicitud(404, "404", "Error no Controlado", "Error genérico, en caso de presentarse realizar nuevamente la petición y si persiste el error levantar un RMA.");

        private CodigoEstadoSolicitud(int id, string codigo, string mensaje, string observaciones) : base(id, codigo)
        {
            Mensaje = mensaje;
            Observaciones = observaciones;
        }

        public string Mensaje { get; }
        public string Observaciones { get; }

        public static bool TryParse(string cdigoSolicitud, out CodigoEstadoSolicitud result)
        {
            if (cdigoSolicitud == null)
            {
                result = new CodigoEstadoSolicitud(0, "", "Codigo invalido", "El codigo no es un codigo de solicitud valdio.");
                return false;
            }

            switch (cdigoSolicitud)
            {
                case "5000":
                    result = _5000;
                    return true;
                case "5002":
                    result = _5002;
                    return true;
                case "5003":
                    result = _5003;
                    return true;
                case "5004":
                    result = _5004;
                    return true;
                case "5005":
                    result = _5005;
                    return true;
                case "404":
                    result = _404;
                    return true;
                default:
                    result = new CodigoEstadoSolicitud(0, cdigoSolicitud, "Codigo invalido", "El codigo no es un codigo de solicitud valdio.");
                    return false;
            }
        }
    }
}