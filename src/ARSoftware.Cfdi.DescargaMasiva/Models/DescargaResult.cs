using System.Net;

namespace ARSoftware.Cfdi.DescargaMasiva.Models
{
    /// <summary>
    ///     Resultado de la peticion de descarga.
    /// </summary>
    /// <param name="Package">Paquete - Representa el paquete que se desea descargar.</param>
    /// <param name="RequestStatusCode">CodEstatus - Código de estatus de la solicitud.</param>
    /// <param name="RequestStatusMessage">Mensaje - Pequeña descripción del código estatus.</param>
    /// <param name="HttpStatusCode">Codigo de estatus de la respuesta HTTP.</param>
    /// <param name="ResponseContent">Contenido del mensage de la respuesta HTTP.</param>
    public record DescargaResult(
        string Package,
        string RequestStatusCode,
        string RequestStatusMessage,
        HttpStatusCode HttpStatusCode,
        string ResponseContent)
    {
    }
}
