using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using ARSoftware.Cfdi.DescargaMasiva.Models;

namespace ARSoftware.Cfdi.DescargaMasiva.Interfaces
{
    /// <summary>
    ///     Servicio para mandar peticiones de verificacion al web service de descarga masiva de CFDIs del SAT
    /// </summary>
    public interface IVerificacionService
    {
        /// <summary>
        ///     Genera el contenido para la peticion SOAP enviada al web service
        /// </summary>
        /// <param name="verificacionRequest">Peticion</param>
        /// <param name="certificate">Certificado del SAT (.pfx)</param>
        /// <returns>El contenido para la peticion SOAP</returns>
        string GenerateSoapRequestEnvelopeXmlContent(VerificacionRequest verificacionRequest, X509Certificate2 certificate);

        /// <summary>
        ///     Envia la peticion al web service de descarga masiva de CFDIs del SAT.
        /// </summary>
        /// <param name="soapRequestContent">Contenido para la peticion SOAP generado por GenerateSoapRequestEnvelopeXmlContent</param>
        /// <param name="accessToken">Token de autorizacion que regresa la peticion de Autenticacion</param>
        /// <param name="cancellationToken">Token de cancelacion</param>
        Task<SoapRequestResult> SendSoapRequestAsync(string soapRequestContent,
                                                     AccessToken accessToken,
                                                     CancellationToken cancellationToken);

        /// <summary>
        ///     Envia la peticion al web service de descarga masiva de CFDIs del SAT.
        /// </summary>
        /// <param name="verificacionRequest">Peicion</param>
        /// <param name="certificate">Certificado SAT (.pfx)</param>
        /// <param name="cancellationToken">Token de cancelacion</param>
        /// <returns>El resultado de la peticion.</returns>
        Task<VerificacionResult> SendSoapRequestAsync(VerificacionRequest verificacionRequest,
                                                      X509Certificate2 certificate,
                                                      CancellationToken cancellationToken);

        /// <summary>
        ///     Transforma el resultado de la peticion SOAP en un resultado con los valores asignados al tipo de peticion.
        /// </summary>
        /// <param name="soapRequestResult">Resultado SOAP</param>
        /// <returns>Resultado de la peticion</returns>
        VerificacionResult GetSoapResponseResult(SoapRequestResult soapRequestResult);
    }
}
