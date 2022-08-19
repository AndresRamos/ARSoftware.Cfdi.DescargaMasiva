using System.Threading;
using System.Threading.Tasks;
using ARSoftware.Cfdi.DescargaMasiva.Models;

namespace ARSoftware.Cfdi.DescargaMasiva.Interfaces
{
    public interface ISolicitudService
    {
        SolicitudResult GetSoapResponseResult(SoapRequestResult soapRequestResult);

        Task<SolicitudResult> SendSoapRequest(string soapRequestContent,
                                              string authorizationHttpRequestHeader,
                                              CancellationToken cancellationToken);
    }
}
