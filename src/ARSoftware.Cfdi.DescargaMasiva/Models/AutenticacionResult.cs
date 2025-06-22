using System.Net;

namespace ARSoftware.Cfdi.DescargaMasiva.Models
{
    /// <summary>
    ///     Resultado de la peticion de autenticacion.
    /// </summary>
    /// <param name="AccessToken">Token de autorizacion para autenticar peticiones con el web service.</param>
    /// <param name="FaultCode"></param>
    /// <param name="FaultString"></param>
    /// <param name="HttpStatusCode">Codigo de estatus de la respuesta HTTP.</param>
    /// <param name="ResponseContent">Contenido del mensage de la respuesta HTTP.</param>
    public sealed record AutenticacionResult(
        AccessToken AccessToken,
        string FaultCode,
        string FaultString,
        HttpStatusCode HttpStatusCode,
        string ResponseContent)
    {
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
