namespace ARSoftware.Cfdi.DescargaMasiva.Models
{
    /// <summary>
    ///     Peticion de solicitud de descarga de un CFDI.
    /// </summary>
    /// <param name="AccessToken">Token de autorizacion.</param>
    /// <param name="Folio">Folio Fiscal con formato: XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX. Parámetro obligatorio.</param>
    public sealed record SolicitudDescargaFolioRequest(string Folio, AccessToken AccessToken)
    {
        /// <summary>
        ///     Contiene el RFC del que está realizando la solicitud de descarga.
        /// </summary>
        public string RfcSolicitante { get; init; } = string.Empty;

        public bool HasRfcSolicitante => !string.IsNullOrEmpty(RfcSolicitante);
    }
}
