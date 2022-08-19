using System.Net;

namespace ARSoftware.Cfdi.DescargaMasiva.Models
{
    public class SoapRequestResult
    {
        private SoapRequestResult()
        {
        }

        public HttpStatusCode HttpStatusCode { get; private set; }
        public string ResponseContent { get; private set; }

        public static SoapRequestResult CreateInstance(HttpStatusCode httpStatusCode, string responseContent)
        {
            return new SoapRequestResult { HttpStatusCode = httpStatusCode, ResponseContent = responseContent };
        }
    }
}
