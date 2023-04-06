using System;

namespace ARSoftware.Cfdi.DescargaMasiva.Models
{
    /// <summary>
    ///     Peticion de verificacion.
    /// </summary>
    public sealed class VerificacionRequest
    {
        private VerificacionRequest(string requestId, string requestingRfc, AccessToken accessToken)
        {
            RequestId = requestId ?? throw new ArgumentNullException(nameof(requestId));
            RequestingRfc = requestingRfc ?? throw new ArgumentNullException(nameof(requestingRfc));
            AccessToken = accessToken ?? throw new ArgumentNullException(nameof(accessToken));
        }

        /// <summary>
        ///     IdSolicitud - Contiene el Identificador de la solicitud que se pretende consultar.
        /// </summary>
        public string RequestId { get; }

        /// <summary>
        ///     RfcSolicitante - Contiene el RFC del solicitante que genero la petición de solicitud de descarga masiva.
        /// </summary>
        public string RequestingRfc { get; }

        /// <summary>
        ///     Token de autorizacion.
        /// </summary>
        public AccessToken AccessToken { get; }

        public static VerificacionRequest CreateInstance(string requestId, string requestingRfc, AccessToken accessToken)
        {
            return new VerificacionRequest(requestId, requestingRfc, accessToken);
        }
    }
}
