using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using ARSoftware.Cfdi.DescargaMasiva.Models;

namespace ARSoftware.Cfdi.DescargaMasiva.Interfaces
{
    public interface IVerificacionService
    {
        string GenerateSoapRequestEnvelopeXmlContent(VerificacionRequest verificacionRequest, X509Certificate2 certificate);

        VerificacionResult GetSoapResponseResult(SoapRequestResult soapRequestResult);

        Task<VerificacionResult> SendSoapRequestAsync(string soapRequestContent,
                                                      string authorizationHttpRequestHeader,
                                                      CancellationToken cancellationToken);
    }
}
