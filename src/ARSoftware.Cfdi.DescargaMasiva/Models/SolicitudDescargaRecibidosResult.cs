using System.Net;

namespace ARSoftware.Cfdi.DescargaMasiva.Models
{
    /// <summary>
    ///     Resultado de la peticion de solicitud de descarga de CFDIs recibidos.
    /// </summary>
    public record SolicitudDescargaRecibidosResult
    {
        /// <summary>
        ///     IdSolicitud - Contiene el resultado de la petición con el código de respuesta y los UUID de los CFDIs de los cuales
        ///     se solicitó
        ///     la descarga, pero se encuentran en espera de una confirmación por parte del receptor.
        /// </summary>
        public required string IdSolicitud { get; init; }

        /// <summary>
        ///     RfcSolicitante - Contiene el RFC que realizo la solicitud.
        /// </summary>
        public required string RfcSolicitante { get; init; }

        /// <summary>
        ///     CodEstatus - Código de estatus de la solicitud.
        /// </summary>
        public required string CodEstatus { get; init; }

        /// <summary>
        ///     Mensaje - Pequeña descripción del código estatus.
        /// </summary>
        public required string Mensaje { get; init; }

        /// <summary>
        ///     Codigo de estatus de la respuesta HTTP.
        /// </summary>
        public required HttpStatusCode HttpStatusCode { get; init; }

        /// <summary>
        ///     Contenido del mensage de la respuesta HTTP.
        /// </summary>
        public required string ResponseContent { get; init; }
    }
}
