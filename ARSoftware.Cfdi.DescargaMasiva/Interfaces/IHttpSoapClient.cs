using System.Threading;
using System.Threading.Tasks;
using ARSoftware.Cfdi.DescargaMasiva.Models;

namespace ARSoftware.Cfdi.DescargaMasiva.Interfaces
{
    public interface IHttpSoapClient
    {
        Task<SoapRequestResult> SendRequestAsync(string url,
                                                 string soapAction,
                                                 string token,
                                                 string requestContent,
                                                 CancellationToken cancellationToken);
    }
}
