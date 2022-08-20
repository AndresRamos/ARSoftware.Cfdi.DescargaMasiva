using System.Threading;
using System.Threading.Tasks;
using ARSoftware.Cfdi.DescargaMasiva.Models;

namespace ARSoftware.Cfdi.DescargaMasiva.Interfaces
{
    public interface ISolicitudService
    {
        SolicitudResult GetSoapResponseResult(SoapRequestResult soapRequestResult);

        Task<SolicitudResult> SendSoapRequestAsync(string soapRequestContent,
                                                   string authorizationHttpRequestHeader,
                                                   CancellationToken cancellationToken);
    }
}
