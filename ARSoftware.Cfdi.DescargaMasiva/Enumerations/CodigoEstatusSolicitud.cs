namespace ARSoftware.Cfdi.DescargaMasiva.Enumerations
{
    /// <summary>
    ///     Mensajes recibidos desde la operación VerificaSolicitudDescarga y Descargar.
    /// </summary>
    public class CodigoEstatusSolicitud : Enumeration
    {
        /// <summary>
        ///     Usuario No Válido
        /// </summary>
        public static readonly CodigoEstatusSolicitud _300 = new CodigoEstatusSolicitud(300, "300", "Usuario No Válido", "");

        /// <summary>
        ///     XML Mal Formado
        /// </summary>
        public static readonly CodigoEstatusSolicitud _301 = new CodigoEstatusSolicitud(301, "301", "XML Mal Formado", "Este código de error se regresa cuando el request posee información invalida, ejemplo: un RFC de receptor no valido");

        /// <summary>
        ///     Sello Mal Formado
        /// </summary>
        public static readonly CodigoEstatusSolicitud _302 = new CodigoEstatusSolicitud(302, "302", "Sello Mal Formado", "");

        /// <summary>
        ///     Sello no corresponde con RfcSolicitante
        /// </summary>
        public static readonly CodigoEstatusSolicitud _303 = new CodigoEstatusSolicitud(303, "303", "Sello no corresponde con RfcSolicitante", "");

        /// <summary>
        ///     Certificado Revocado o Caduco
        /// </summary>
        public static readonly CodigoEstatusSolicitud _304 = new CodigoEstatusSolicitud(304, "304", "Certificado Revocado o Caduco", "El certificado fue revocado o bien la fecha de vigencia expiró");

        /// <summary>
        ///     Certificado Inválido
        /// </summary>
        public static readonly CodigoEstatusSolicitud _305 = new CodigoEstatusSolicitud(305, "305", "Certificado Inválido", "El certificado puede ser invalido por múltiples razones como son el tipo, codificación incorrecta, etc.");

        /// <summary>
        ///     Solicitud de descarga recibida con éxito
        /// </summary>
        public static readonly CodigoEstatusSolicitud _5000 = new CodigoEstatusSolicitud(5000, "5000", "Solicitud de descarga recibida con éxito", "");

        /// <summary>
        ///     No se encontró la información
        /// </summary>
        public static readonly CodigoEstatusSolicitud _5004 = new CodigoEstatusSolicitud(5004, "5004", "No se encontró la información", "No se encontró la información del paquete solicitado");

        /// <summary>
        ///     No existe el paquete solicitado
        /// </summary>
        public static readonly CodigoEstatusSolicitud _5007 = new CodigoEstatusSolicitud(5007, "5007", "No existe el paquete solicitado", "Los paquetes solo tienen un periodo de vida de 72hrs");

        /// <summary>
        ///     Máximo de descargas permitidas
        /// </summary>
        public static readonly CodigoEstatusSolicitud _5008 = new CodigoEstatusSolicitud(5008, "5008", "Máximo de descargas permitidas", "Un paquete solo puede descargarse un total de 2 veces, una vez agotadas, ya no se podrá volver a descargar");

        /// <summary>
        ///     Error no Controlado
        /// </summary>
        public static readonly CodigoEstatusSolicitud _404 = new CodigoEstatusSolicitud(404, "404", "Error no Controlado", "Error genérico, en caso de presentarse realizar nuevamente la petición y si persiste el error levantar un RMA.");

        private CodigoEstatusSolicitud(int id, string codigo, string mensaje, string observaciones) : base(id, codigo)
        {
            Mensaje = mensaje;
            Observaciones = observaciones;
        }

        public string Mensaje { get; }
        public string Observaciones { get; }

        public static bool TryParse(string codigoEstatus, out CodigoEstatusSolicitud result)
        {
            if (codigoEstatus == null)
            {
                result = new CodigoEstatusSolicitud(0, "", "Codigo invalido", "El codigo no es un estatus valido");
                return false;
            }

            switch (codigoEstatus)
            {
                case "300":
                    result = _300;
                    return true;
                case "301":
                    result = _301;
                    return true;
                case "302":
                    result = _302;
                    return true;
                case "303":
                    result = _303;
                    return true;
                case "304":
                    result = _304;
                    return true;
                case "305":
                    result = _305;
                    return true;
                case "5000":
                    result = _5000;
                    return true;
                case "5004":
                    result = _5004;
                    return true;
                case "5007":
                    result = _5007;
                    return true;
                case "5008":
                    result = _5008;
                    return true;
                case "404":
                    result = _404;
                    return true;
                default:
                    result = new CodigoEstatusSolicitud(0, codigoEstatus, "Codigo invalido", "El codigo no es un estatus valido");
                    return false;
            }
        }
    }
}