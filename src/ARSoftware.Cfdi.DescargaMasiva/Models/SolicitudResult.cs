using System;
using System.Net;

namespace ARSoftware.Cfdi.DescargaMasiva.Models
{
    /// <summary>
    ///     Resultado de la peticion de solicitud.
    /// </summary>
    public sealed class SolicitudResult
    {
        private SolicitudResult(string requestId,
                                string requestStatusCode,
                                string requestStatusMessage,
                                HttpStatusCode httpStatusCode,
                                string responseContent)
        {
            RequestStatusCode = requestStatusCode ?? throw new ArgumentNullException(nameof(requestStatusCode));
            RequestId = requestId ?? throw new ArgumentNullException(nameof(requestId));
            RequestStatusMessage = requestStatusMessage ?? throw new ArgumentNullException(nameof(requestStatusMessage));
            HttpStatusCode = httpStatusCode;
            ResponseContent = responseContent ?? throw new ArgumentNullException(nameof(responseContent));
        }

        /// <summary>
        ///     IdSolicitud - Contiene el resultado de la petición con el código de respuesta y los UUID de los CFDIs de los cuales
        ///     se solicitó la descarga, pero se encuentran en espera de una confirmación por parte del receptor.
        /// </summary>
        public string RequestId { get; }

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

        public static SolicitudResult CreateInstance(string requestId,
                                                     string requestStatusCode,
                                                     string requestStatusMessage,
                                                     HttpStatusCode httpStatusCode,
                                                     string responseContent)
        {
            return new SolicitudResult(requestId, requestStatusCode, requestStatusMessage, httpStatusCode, responseContent);
        }
    }
}
