using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using ARSoftware.Cfdi.DescargaMasiva.Models;

namespace ARSoftware.Cfdi.DescargaMasiva.Services
{
    public class HttpWebRequestSoapService
    {
        private readonly string _soapAction;
        private readonly string _url;

        public HttpWebRequestSoapService(string url, string soapAction)
        {
            _url = url;
            _soapAction = soapAction;
        }

        public SoapRequestResult SendSoapRequest(string soapRequestContent, string authorizationHttpRequestHeader)
        {
            if (soapRequestContent == null)
            {
                throw new ArgumentNullException(nameof(soapRequestContent), "A request can't be sent if its content is empty.");
            }

            var httpWebRequest = CreateHttpWebRequest();

            if (!string.IsNullOrEmpty(authorizationHttpRequestHeader))
            {
                httpWebRequest.Headers.Add(HttpRequestHeader.Authorization, authorizationHttpRequestHeader);
            }

            using (var stream = httpWebRequest.GetRequestStream())
            using (var streamWriter = new StreamWriter(stream))
            {
                streamWriter.Write(soapRequestContent);
            }

            using (var httpWebResponse = (HttpWebResponse) httpWebRequest.GetResponse())
            using (var streamReader = new StreamReader(httpWebResponse.GetResponseStream()))
            {
                return SoapRequestResult.CreateInstance(httpWebResponse.StatusCode, streamReader.ReadToEnd());
            }
        }

        private HttpWebRequest CreateHttpWebRequest()
        {
            var httpWebRequest = (HttpWebRequest) WebRequest.Create(_url);
            httpWebRequest.Method = "POST";
            httpWebRequest.ContentType = "text/xml; charset=utf-8";
            httpWebRequest.Headers.Add("SOAPAction: " + _soapAction);
            return httpWebRequest;
        }

        public async Task<string> SendHttpClient(string xml, string autorization)
        {
            if (xml == null)
            {
                throw new ArgumentNullException(nameof(xml), "El xml no puede ser nulo.");
            }

            var httpClient = new HttpClient();
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri(_url),
                Method = HttpMethod.Post
            };

            request.Headers.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaTypeNames.Text.Xml));
            request.Headers.Add("SOAPAction", _soapAction);

            request.Content = new StringContent(xml, Encoding.UTF8, MediaTypeNames.Text.Xml);
            request.Content.Headers.ContentType = new MediaTypeHeaderValue(MediaTypeNames.Text.Xml);

            var response = await httpClient.SendAsync(request);

            return await response.Content.ReadAsStringAsync();
        }
    }
}