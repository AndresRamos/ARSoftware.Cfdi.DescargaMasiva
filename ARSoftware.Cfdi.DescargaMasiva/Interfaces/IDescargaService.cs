using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using ARSoftware.Cfdi.DescargaMasiva.Models;

namespace ARSoftware.Cfdi.DescargaMasiva.Interfaces
{
    public interface IDescargaService
    {
        string GenerateSoapRequestEnvelopeXmlContent(DescargaRequest descargaRequest, X509Certificate2 certificate);
        DescargaResult GetSoapResponseResult(SoapRequestResult soapRequestResult);

        Task<DescargaResult> SendSoapRequestAsync(string soapRequestContent,
                                                  string authorizationHttpRequestHeader,
                                                  CancellationToken cancellationToken);
    }
}
