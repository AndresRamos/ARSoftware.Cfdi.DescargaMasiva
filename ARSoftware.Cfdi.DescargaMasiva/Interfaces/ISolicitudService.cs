using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using ARSoftware.Cfdi.DescargaMasiva.Models;

namespace ARSoftware.Cfdi.DescargaMasiva.Interfaces
{
    public interface ISolicitudService
    {
        string GenerateSoapRequestEnvelopeXmlContent(SolicitudRequest solicitudRequest, X509Certificate2 certificate);

        Task<SoapRequestResult> SendSoapRequestAsync(string soapRequestContent,
                                                     AccessToken accessToken,
                                                     CancellationToken cancellationToken);

        Task<SolicitudResult> SendSoapRequestAsync(SolicitudRequest solicitudRequest,
                                                   X509Certificate2 certificate,
                                                   CancellationToken cancellationToken);

        SolicitudResult GetSoapResponseResult(SoapRequestResult soapRequestResult);
    }
}
