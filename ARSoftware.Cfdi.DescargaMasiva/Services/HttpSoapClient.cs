﻿using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ARSoftware.Cfdi.DescargaMasiva.Interfaces;
using ARSoftware.Cfdi.DescargaMasiva.Models;

namespace ARSoftware.Cfdi.DescargaMasiva.Services
{
    public class HttpSoapClient : IHttpSoapClient
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public HttpSoapClient(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<SoapRequestResult> SendRequestAsync(string url,
                                                              string soapAction,
                                                              string requestContent,
                                                              string authorizationRequestHeader,
                                                              CancellationToken cancellationToken)
        {
            if (requestContent == null)
            {
                throw new ArgumentNullException(nameof(requestContent), "El xml no puede ser nulo.");
            }

            HttpClient httpClient = _httpClientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Clear();

            var request = new HttpRequestMessage(HttpMethod.Post, new Uri(url));
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaTypeNames.Text.Xml));
            request.Headers.Add("Authorization", authorizationRequestHeader);
            request.Headers.Add("SOAPAction", soapAction);

            request.Content = new StringContent(requestContent, Encoding.UTF8, MediaTypeNames.Text.Xml);

            HttpResponseMessage response = await httpClient.SendAsync(request, cancellationToken);

            return SoapRequestResult.CreateInstance(response.StatusCode, await response.Content.ReadAsStringAsync());
        }
    }
}