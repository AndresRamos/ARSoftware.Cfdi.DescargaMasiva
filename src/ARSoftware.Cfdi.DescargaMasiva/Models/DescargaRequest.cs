namespace ARSoftware.Cfdi.DescargaMasiva.Models
{
    /// <summary>
    ///     Peticion de descarga.
    /// </summary>
    /// <param name="IdPaquete">Contiene el identificador del paquete que se desea descargar.</param>
    /// <param name="RfcSolicitante">Contiene el RFC del solicitante que genero la petición de solicitud de descarga masiva.</param>
    /// <param name="AccessToken">Token de autorizacion.</param>
    public sealed record DescargaRequest(string IdPaquete, string RfcSolicitante, AccessToken AccessToken);
}
