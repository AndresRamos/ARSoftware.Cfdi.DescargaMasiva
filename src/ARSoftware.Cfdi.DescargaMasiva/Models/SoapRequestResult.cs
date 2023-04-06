using System;
using System.Net;

namespace ARSoftware.Cfdi.DescargaMasiva.Models
{
    /// <summary>
    ///     Resultado de la peticion SOAP
    /// </summary>
    public sealed class SoapRequestResult
    {
        private SoapRequestResult(HttpStatusCode httpStatusCode, string responseContent)
        {
            HttpStatusCode = httpStatusCode;
            ResponseContent = responseContent ?? throw new ArgumentNullException(nameof(responseContent));
        }

        /// <summary>
        ///     Codigo de estatus de la respuesta HTTP.
        /// </summary>
        public HttpStatusCode HttpStatusCode { get; }

        /// <summary>
        ///     Contenido del mensage de la respuesta HTTP.
        /// </summary>
        public string ResponseContent { get; }

        public static SoapRequestResult CreateInstance(HttpStatusCode httpStatusCode, string responseContent)
        {
            return new SoapRequestResult(httpStatusCode, responseContent);
        }
    }
}
