using System.Net;

namespace ARSoftware.Cfdi.DescargaMasiva.Models
{
    /// <summary>
    ///     Resultado de la peticion de descarga.
    /// </summary>
    /// <param name="Paquete">Representa el paquete que se desea descargar.</param>
    /// <param name="CodEstatus">Código de estatus de la solicitud.</param>
    /// <param name="Mensaje">Pequeña descripción del código estatus.</param>
    /// <param name="HttpStatusCode">Codigo de estatus de la respuesta HTTP.</param>
    /// <param name="ResponseContent">Contenido del mensage de la respuesta HTTP.</param>
    public sealed record DescargaResult(
        string Paquete,
        string CodEstatus,
        string Mensaje,
        HttpStatusCode HttpStatusCode,
        string ResponseContent);
}
