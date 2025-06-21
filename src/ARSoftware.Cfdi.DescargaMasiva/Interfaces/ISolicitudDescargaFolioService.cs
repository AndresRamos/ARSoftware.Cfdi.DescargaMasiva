using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using ARSoftware.Cfdi.DescargaMasiva.Models;

namespace ARSoftware.Cfdi.DescargaMasiva.Interfaces
{
    public interface ISolicitudDescargaFolioService
    {
        /// <summary>
        ///     Genera el contenido para la peticion SOAP enviada al web service
        /// </summary>
        /// <param name="solicitudRequest">Peticion</param>
        /// <param name="certificate">Certificado del SAT (.pfx)</param>
        /// <returns>El contenido para la peticion SOAP</returns>
        string GenerateSoapRequestEnvelopeXmlContent(SolicitudDescargaFolioRequest solicitudRequest, X509Certificate2 certificate);

        /// <summary>
        ///     Envia la peticion al web service de descarga masiva de CFDIs del SAT.
        /// </summary>
        /// <param name="solicitudRequest">Peticion</param>
        /// <param name="certificate">Certificado SAT (.pfx)</param>
        /// <param name="cancellationToken">Token de cancelacion</param>
        /// <returns>El resultado de la peticion.</returns>
        Task<SolicitudDescargaFolioResult> SendSoapRequestAsync(SolicitudDescargaFolioRequest solicitudRequest,
            X509Certificate2 certificate,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     Transforma el resultado de la peticion SOAP en un resultado con los valores asignados al tipo de peticion.
        /// </summary>
        /// <param name="soapRequestResult">Resultado SOAP</param>
        /// <returns>Resultado de la peticion</returns>
        SolicitudDescargaFolioResult GetSoapResponseResult(SoapRequestResult soapRequestResult);
    }
}
