using System.Net;

namespace ARSoftware.Cfdi.DescargaMasiva.Models
{
    public class SoapRequestResult
    {
        private SoapRequestResult()
        {
        }

        public HttpStatusCode HttpStatusCode { get; private set; }
        public string WebResponse { get; private set; }

        public static SoapRequestResult CreateInstance(HttpStatusCode httpStatusCode, string webResponse)
        {
            return new SoapRequestResult
            {
                HttpStatusCode = httpStatusCode,
                WebResponse = webResponse
            };
        }
    }
}