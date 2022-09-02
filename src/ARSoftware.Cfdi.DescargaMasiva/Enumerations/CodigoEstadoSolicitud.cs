using Ardalis.SmartEnum;

namespace ARSoftware.Cfdi.DescargaMasiva.Enumerations
{
    /// <summary>
    ///     Contiene el código de estado de la solicitud de descarga. Codigos recibidos desde la operación
    ///     VerificaSolicitudDescarga
    /// </summary>
    public sealed class CodigoEstadoSolicitud : SmartEnum<CodigoEstadoSolicitud>
    {
        /// <summary>
        ///     5000 = Solicitud recibida con éxito
        /// </summary>
        public static readonly CodigoEstadoSolicitud _5000 = new CodigoEstadoSolicitud("5000",
            5000,
            "Solicitud recibida con éxito",
            "Indica que la solicitud de descarga que se está verificando fue aceptada.");

        /// <summary>
        ///     5002 = Se agotó las solicitudes de por vida
        /// </summary>
        public static readonly CodigoEstadoSolicitud _5002 = new CodigoEstadoSolicitud("5002",
            5002,
            "Se agotó las solicitudes de por vida",
            "Para el caso de descarga de tipo CFDI, se tiene unlímite máximo para solicitudes con los mismos parámetros(Fecha Inicial, Fecha Final, RfcEmisor, RfcReceptor).");

        /// <summary>
        ///     5003 = Tope máximo
        /// </summary>
        public static readonly CodigoEstadoSolicitud _5003 = new CodigoEstadoSolicitud("5003",
            5003,
            "Tope máximo",
            "Indica que en base a los parámetros de consulta se está superando el tope máximo de CFDI o Metadata, por solicitud de descarga masiva.");

        /// <summary>
        ///     5004 = No se encontró la información
        /// </summary>
        public static readonly CodigoEstadoSolicitud _5004 = new CodigoEstadoSolicitud("5004",
            5004,
            "No se encontró la información",
            "Indica que la solicitud de descarga que se está verificando no generó paquetes por falta de información.");

        /// <summary>
        ///     5005 = Solicitud duplicada
        /// </summary>
        public static readonly CodigoEstadoSolicitud _5005 = new CodigoEstadoSolicitud("5005",
            5005,
            "Solicitud duplicada",
            "En caso de que exista una solicitud vigente con los mismos parámetros (Fecha Inicial, Fecha Final, RfcEmisor, RfcReceptor, TipoSolicitud), no se permitirá generar una nueva solicitud.");

        /// <summary>
        ///     404 = Error no Controlado
        /// </summary>
        public static readonly CodigoEstadoSolicitud _404 = new CodigoEstadoSolicitud("404",
            404,
            "Error no Controlado",
            "Error genérico, en caso de presentarse realizar nuevamente la petición y si persiste el error levantar un RMA.");

        private CodigoEstadoSolicitud(string name, int value, string mensaje, string observaciones) : base(name, value)
        {
            Mensaje = mensaje;
            Observaciones = observaciones;
        }

        public string Mensaje { get; }
        public string Observaciones { get; }
    }
}
