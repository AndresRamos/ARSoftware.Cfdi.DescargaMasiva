using System;
using System.Net;

namespace ARSoftware.Cfdi.DescargaMasiva.Models
{
    /// <summary>
    ///     Resultado de la peticion de descarga.
    /// </summary>
    public class DescargaResult
    {
        private DescargaResult(string package,
                               string requestStatusCode,
                               string requestStatusMessage,
                               HttpStatusCode httpStatusCode,
                               string responseContent)
        {
            RequestStatusCode = requestStatusCode ?? throw new ArgumentNullException(nameof(requestStatusCode));
            RequestStatusMessage = requestStatusMessage ?? throw new ArgumentNullException(nameof(requestStatusMessage));
            Package = package ?? throw new ArgumentNullException(nameof(package));
            HttpStatusCode = httpStatusCode;
            ResponseContent = responseContent ?? throw new ArgumentNullException(nameof(responseContent));
        }

        /// <summary>
        ///     Paquete - Representa el paquete que se desea descargar.
        /// </summary>
        public string Package { get; }

        /// <summary>
        ///     CodEstatus - Código de estatus de la solicitud.
        /// </summary>
        public string RequestStatusCode { get; }

        /// <summary>
        ///     Mensaje - Pequeña descripción del código estatus.
        /// </summary>
        public string RequestStatusMessage { get; }

        /// <summary>
        ///     Codigo de estatus de la respuesta HTTP.
        /// </summary>
        public HttpStatusCode HttpStatusCode { get; }

        /// <summary>
        ///     Contenido del mensage de la respuesta HTTP.
        /// </summary>
        public string ResponseContent { get; }

        public static DescargaResult CreateInstance(string package,
                                                    string requestStatusCode,
                                                    string requestStatusMessage,
                                                    HttpStatusCode httpStatusCode,
                                                    string responseContent)
        {
            return new DescargaResult(package, requestStatusCode, requestStatusMessage, httpStatusCode, responseContent);
        }
    }
}
