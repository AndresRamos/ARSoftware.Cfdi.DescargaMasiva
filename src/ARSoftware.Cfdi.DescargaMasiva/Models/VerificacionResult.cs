using System;
using System.Collections.Generic;
using System.Net;

namespace ARSoftware.Cfdi.DescargaMasiva.Models
{
    /// <summary>
    ///     Resultado de la peticion de verificacion.
    /// </summary>
    public class VerificacionResult
    {
        private VerificacionResult(List<string> packageIds,
                                   string downloadRequestStatusNumber,
                                   string downloadRequestStatusCode,
                                   string numberOfCfdis,
                                   string requestStatusCode,
                                   string requestStatusMessage,
                                   HttpStatusCode httpStatusCode,
                                   string responseContent)
        {
            RequestStatusCode = requestStatusCode ?? throw new ArgumentNullException(nameof(requestStatusCode));
            DownloadRequestStatusCode = downloadRequestStatusCode ?? throw new ArgumentNullException(nameof(downloadRequestStatusCode));
            DownloadRequestStatusNumber =
                downloadRequestStatusNumber ?? throw new ArgumentNullException(nameof(downloadRequestStatusNumber));
            NumberOfCfdis = numberOfCfdis ?? throw new ArgumentNullException(nameof(numberOfCfdis));
            RequestStatusMessage = requestStatusMessage ?? throw new ArgumentNullException(nameof(requestStatusMessage));
            PackageIds = packageIds ?? throw new ArgumentNullException(nameof(packageIds));
            HttpStatusCode = httpStatusCode;
            ResponseContent = responseContent ?? throw new ArgumentNullException(nameof(responseContent));
        }

        /// <summary>
        ///     IdsPaquetes - Contiene los identificadores de los paquetes que componen la solicitud de descarga masiva. Solo se
        ///     devuelve cuando la solicitud posee un estatus de finalizado.
        /// </summary>
        public List<string> PackageIds { get; }

        /// <summary>
        ///     EstadoSolicitud - Contiene el número correspondiente al estado de la solicitud de descarga, Estados de la
        ///     solicitud: [Aceptada=1, EnProceso=2, Terminada=3, Error=4, Rechazada=5, Vencida=6]
        /// </summary>
        public string DownloadRequestStatusNumber { get; }

        /// <summary>
        ///     CodigoEstadoSolicitud - Contiene el código de estado de la solicitud de descarga, los cuales pueden ser
        ///     5000,5002,5003,5004 o 5005 para más información revisar la tabla “Códigos Solicitud Descarga Masiva”.
        /// </summary>
        public string DownloadRequestStatusCode { get; }

        /// <summary>
        ///     NumeroCFDIs - Número de CFDIs que conforman la solicitud de descarga consultada.
        /// </summary>
        public string NumberOfCfdis { get; }

        /// <summary>
        ///     CodEstatus - Código de estatus de la petición de verificación.
        /// </summary>
        public string RequestStatusCode { get; }

        /// <summary>
        ///     Mensaje- Pequeña descripción del código estatus correspondiente a la petición de verificación.
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

        public static VerificacionResult CreateInstance(List<string> packageIds,
                                                        string downloadRequestStatusNumber,
                                                        string downloadRequestStatusCode,
                                                        string numberOfCfdis,
                                                        string requestStatusCode,
                                                        string requestStatusMessage,
                                                        HttpStatusCode httpStatusCode,
                                                        string responseContent)
        {
            return new VerificacionResult(packageIds,
                downloadRequestStatusNumber,
                downloadRequestStatusCode,
                numberOfCfdis,
                requestStatusCode,
                requestStatusMessage,
                httpStatusCode,
                responseContent);
        }
    }
}
