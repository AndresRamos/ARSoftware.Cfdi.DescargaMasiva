using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using ARSoftware.Cfdi.DescargaMasiva.Models;

namespace ARSoftware.Cfdi.DescargaMasiva.Interfaces
{
    public interface IAutenticacionService
    {
        string GenerateSoapRequestEnvelopeXmlContent(AutenticacionRequest autenticacionRequest, X509Certificate2 certificate);
        AutenticacionResult GetSoapResponseResult(SoapRequestResult soapRequestResult);
        Task<AutenticacionResult> SendSoapRequestAsync(string soapRequestContent, CancellationToken cancellationToken);
    }
}
