using System.Web;
using ARSoftware.Cfdi.DescargaMasiva.Helpers;

namespace ARSoftware.Cfdi.DescargaMasiva.Models
{
    /// <summary>
    ///     Token de autorizacion para autenticar peticiones con el web service de descarga masiva de CFDIs del SAT.
    /// </summary>
    /// <param name="Value">Valor del Token tomado del resultado de la peticion de autenticacion.</param>
    public record AccessToken(string Value)
    {
        public bool IsValid => !string.IsNullOrWhiteSpace(Value);

        /// <summary>
        ///     HttpUtility.UrlDecode(Token);
        /// </summary>
        public string DecodedValue => HttpUtility.UrlDecode(Value);

        /// <summary>
        ///     Token convertido en un HTTP header para Autorizacion. WRAP access_token="Token";
        /// </summary>
        public string HttpAuthorizationHeader => SoapRequestHelper.CreateHttpAuthorizationHeaderFromToken(Value);

        public static AccessToken CreateEmpty()
        {
            return new AccessToken(string.Empty);
        }
    }
}
