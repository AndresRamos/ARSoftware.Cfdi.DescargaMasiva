using System;
using System.Net;

namespace ARSoftware.Cfdi.DescargaMasiva.Models
{
    /// <summary>
    ///     Resultado de la peticion de autenticacion.
    /// </summary>
    public sealed class AutenticacionResult
    {
        private AutenticacionResult(AccessToken accessToken,
                                    string faultCode,
                                    string faultString,
                                    HttpStatusCode httpStatusCode,
                                    string responseContent)
        {
            AccessToken = accessToken ?? throw new ArgumentNullException(nameof(accessToken));
            FaultCode = faultCode ?? throw new ArgumentNullException(nameof(faultCode));
            FaultString = faultString ?? throw new ArgumentNullException(nameof(faultString));
            HttpStatusCode = httpStatusCode;
            ResponseContent = responseContent ?? throw new ArgumentNullException(nameof(responseContent));
        }

        /// <summary>
        ///     Token de autorizacion para autenticar peticiones con el web service.
        /// </summary>
        public AccessToken AccessToken { get; }

        public string FaultCode { get; }

        public string FaultString { get; }

        /// <summary>
        ///     Codigo de estatus de la respuesta HTTP.
        /// </summary>
        public HttpStatusCode HttpStatusCode { get; }

        /// <summary>
        ///     Contenido del mensage de la respuesta HTTP.
        /// </summary>
        public string ResponseContent { get; }

        public static AutenticacionResult CreateInstance(AccessToken accessToken,
                                                         string faultCode,
                                                         string faultString,
                                                         HttpStatusCode httpStatusCode,
                                                         string responseContent)
        {
            return new AutenticacionResult(accessToken, faultCode, faultString, httpStatusCode, responseContent);
        }

        public static AutenticacionResult CreateSuccess(AccessToken accessToken, HttpStatusCode httpStatusCode, string responseContent)
        {
            return new AutenticacionResult(accessToken, string.Empty, string.Empty, httpStatusCode, responseContent);
        }

        public static AutenticacionResult CreateFailure(string faultCode,
                                                        string faultString,
                                                        HttpStatusCode httpStatusCode,
                                                        string responseContent)
        {
            return new AutenticacionResult(AccessToken.CreateEmpty(), faultCode, faultString, httpStatusCode, responseContent);
        }
    }
}
