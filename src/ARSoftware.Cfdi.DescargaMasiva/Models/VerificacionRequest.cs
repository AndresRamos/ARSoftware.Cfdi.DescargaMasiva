namespace ARSoftware.Cfdi.DescargaMasiva.Models
{
    /// <summary>
    ///     Peticion de verificacion.
    /// </summary>
    /// <param name="IdSolicitud">Contiene el Identificador de la solicitud que se pretende consultar.</param>
    /// <param name="RfcSolicitante">Contiene el RFC del solicitante que genero la petición de solicitud de descarga masiva.</param>
    /// <param name="AccessToken">Token de autorizacion.</param>
    public sealed record VerificacionRequest(string IdSolicitud, string RfcSolicitante, AccessToken AccessToken);
}
