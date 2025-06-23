using System.Net;

namespace ARSoftware.Cfdi.DescargaMasiva.Models
{
    /// <summary>
    ///     Resultado de la peticion de solicitud de descarga de CFDIs emitidos.
    /// </summary>
    /// <param name="IdSolicitud">
    ///     Contiene el resultado de la petición con el código de respuesta y los UUID de los CFDIs de
    ///     los cuales se solicitó la descarga, pero se encuentran en espera de una confirmación por parte del receptor.
    /// </param>
    /// <param name="RfcSolicitante">Contiene el RFC que realizo la solicitud.</param>
    /// <param name="CodEstatus">Código de estatus de la solicitud.</param>
    /// <param name="Mensaje">Pequeña descripción del código estatus.</param>
    /// <param name="HttpStatusCode">Codigo de estatus de la respuesta HTTP.</param>
    /// <param name="ResponseContent">Contenido del mensage de la respuesta HTTP.</param>
    public sealed record SolicitudDescargaEmitidosResult(
        string IdSolicitud,
        string RfcSolicitante,
        string CodEstatus,
        string Mensaje,
        HttpStatusCode HttpStatusCode,
        string ResponseContent);
}
