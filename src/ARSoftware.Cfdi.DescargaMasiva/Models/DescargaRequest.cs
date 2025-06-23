namespace ARSoftware.Cfdi.DescargaMasiva.Models
{
    /// <summary>
    ///     Peticion de descarga.
    /// </summary>
    /// <param name="PackageId">IdPaquete - Contiene el identificador del paquete que se desea descargar.</param>
    /// <param name="RequestingRfc">
    ///     RfcSolicitante - Contiene el RFC del solicitante que genero la petición de solicitud de
    ///     descarga masiva.
    /// </param>
    /// <param name="AccessToken">Token de autorizacion.</param>
    public record DescargaRequest(string PackageId, string RequestingRfc, AccessToken AccessToken);
}
