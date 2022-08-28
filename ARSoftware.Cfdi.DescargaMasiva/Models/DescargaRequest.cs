using System;

namespace ARSoftware.Cfdi.DescargaMasiva.Models
{
    /// <summary>
    ///     Peticion de descarga.
    /// </summary>
    public class DescargaRequest
    {
        private DescargaRequest(string packageId, string requestingRfc, AccessToken accessToken)
        {
            PackageId = packageId ?? throw new ArgumentNullException(nameof(packageId));
            RequestingRfc = requestingRfc ?? throw new ArgumentNullException(nameof(requestingRfc));
            AccessToken = accessToken ?? throw new ArgumentNullException(nameof(accessToken));
        }

        /// <summary>
        ///     IdPaquete - Contiene el identificador del paquete que se desea descargar.
        /// </summary>
        public string PackageId { get; }

        /// <summary>
        ///     RfcSolicitante - Contiene el RFC del solicitante que genero la petición de solicitud de descarga masiva.
        /// </summary>
        public string RequestingRfc { get; }

        /// <summary>
        ///     Token de autorizacion.
        /// </summary>
        public AccessToken AccessToken { get; }

        public static DescargaRequest CreateInstace(string packageId, string requestingRfc, AccessToken accessToken)
        {
            return new DescargaRequest(packageId, requestingRfc, accessToken);
        }
    }
}
