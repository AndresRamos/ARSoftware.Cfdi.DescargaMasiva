using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ARSoftware.Cfdi.DescargaMasiva.Helpers;
using ARSoftware.Cfdi.DescargaMasiva.Interfaces;
using ARSoftware.Cfdi.DescargaMasiva.Models;

namespace ARSoftware.Cfdi.DescargaMasiva.Services
{
    public class HttpSoapClient : IHttpSoapClient
    {
        private readonly HttpClient _httpClient;

        public HttpSoapClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<SoapRequestResult> SendRequestAsync(string url,
                                                              string soapAction,
                                                              string token,
                                                              string requestContent,
                                                              CancellationToken cancellationToken)
        {
            if (requestContent == null)
            {
                throw new ArgumentNullException(nameof(requestContent), "El xml no puede ser nulo.");
            }

            _httpClient.DefaultRequestHeaders.Clear();

            var request = new HttpRequestMessage(HttpMethod.Post, new Uri(url));
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaTypeNames.Text.Xml));

            if (!string.IsNullOrWhiteSpace(token))
            {
                request.Headers.Add("Authorization", SoapRequestHelper.CreateHttpAuthorizationHeaderFromToken(token));
            }

            request.Headers.Add("SOAPAction", soapAction);

            request.Content = new StringContent(requestContent, Encoding.UTF8, MediaTypeNames.Text.Xml);

            HttpResponseMessage response = await _httpClient.SendAsync(request, cancellationToken);

            return SoapRequestResult.CreateInstance(response.StatusCode, await response.Content.ReadAsStringAsync());
        }
    }
}
