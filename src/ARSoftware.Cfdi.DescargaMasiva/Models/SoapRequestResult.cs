using System.Net;

namespace ARSoftware.Cfdi.DescargaMasiva.Models
{
    /// <summary>
    ///     Resultado de la peticion SOAP.
    /// </summary>
    /// <param name="HttpStatusCode">Codigo de estatus de la respuesta HTTP.</param>
    /// <param name="ResponseContent">Contenido del mensage de la respuesta HTTP.</param>
    public record SoapRequestResult(HttpStatusCode HttpStatusCode, string ResponseContent);
}
