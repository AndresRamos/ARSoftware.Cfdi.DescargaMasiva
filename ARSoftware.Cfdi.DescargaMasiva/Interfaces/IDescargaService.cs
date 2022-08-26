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

        Task<SoapRequestResult> SendSoapRequestAsync(string soapRequestContent, string token, CancellationToken cancellationToken);

        Task<DescargaResult> SendSoapRequestAsync(DescargaRequest descargaRequest,
                                                  X509Certificate2 certificate,
                                                  CancellationToken cancellationToken);
    }
}
