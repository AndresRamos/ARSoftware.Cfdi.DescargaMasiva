namespace ARSoftware.Cfdi.DescargaMasiva.Models
{
    /// <summary>
    ///     Peticion de solicitud de descarga de un CFDI.
    /// </summary>
    public record SolicitudDescargaFolioRequest
    {
        /// <summary>
        ///     Token de autorizacion.
        /// </summary>
        public required AccessToken AccessToken { get; init; }

        /// <summary>
        ///     Contiene el RFC del que está realizando la solicitud de descarga.
        /// </summary>
        public string RfcSolicitante { get; init; }

        /// <summary>
        ///     Folio Fiscal con formato:
        ///     XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX
        ///     Parámetro obligatorio.
        /// </summary>
        public required string Folio { get; init; }

        public bool HasRfcSolicitante => !string.IsNullOrEmpty(RfcSolicitante);
    }
}
