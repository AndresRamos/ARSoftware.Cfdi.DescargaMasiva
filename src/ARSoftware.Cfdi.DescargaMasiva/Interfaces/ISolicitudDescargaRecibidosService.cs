using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using ARSoftware.Cfdi.DescargaMasiva.Models;

namespace ARSoftware.Cfdi.DescargaMasiva.Interfaces
{
    public interface ISolicitudDescargaRecibidosService
    {
        string GenerateSoapRequestEnvelopeXmlContent(SolicitudDescargaRecibidosRequest solicitudRequest, X509Certificate2 certificate);

        Task<SolicitudDescargaRecibidosResult> SendSoapRequestAsync(SolicitudDescargaRecibidosRequest solicitudRequest,
            X509Certificate2 certificate,
            CancellationToken cancellationToken = default);

        SolicitudDescargaRecibidosResult GetSoapResponseResult(SoapRequestResult soapRequestResult);
    }
}
