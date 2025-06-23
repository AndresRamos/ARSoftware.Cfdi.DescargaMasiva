namespace ARSoftware.Cfdi.DescargaMasiva.Models
{
    /// <summary>
    ///     Peticion de verificacion.
    /// </summary>
    /// <param name="RequestId">IdSolicitud - Contiene el Identificador de la solicitud que se pretende consultar.</param>
    /// <param name="RequestingRfc">
    ///     RfcSolicitante - Contiene el RFC del solicitante que genero la petición de solicitud de
    ///     descarga masiva.
    /// </param>
    /// <param name="AccessToken">Token de autorizacion.</param>
    public sealed record VerificacionRequest(string RequestId, string RequestingRfc, AccessToken AccessToken);
}
