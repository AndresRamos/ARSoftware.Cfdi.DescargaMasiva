using System.Collections.Generic;
using System.Net;

namespace ARSoftware.Cfdi.DescargaMasiva.Models
{
    /// <summary>
    ///     Resultado de la peticion de verificacion.
    /// </summary>
    /// <param name="IdsPaquetes">
    ///     Contiene los identificadores de los paquetes que componen la solicitud de descarga masiva. Solo se devuelve cuando
    ///     la solicitud posee un estatus de finalizado.
    /// </param>
    /// <param name="EstadoSolicitud">
    ///     Contiene el número correspondiente al estado de la
    ///     solicitud de descarga, Estados de la solicitud: [Aceptada=1, EnProceso=2, Terminada=3, Error=4, Rechazada=5,
    ///     Vencida=6]
    /// </param>
    /// <param name="CodigoEstadoSolicitud">
    ///     Contiene el código de estado de la solicitud de
    ///     descarga, los cuales pueden ser 5000,5002,5003,5004 o 5005 para más información revisar la tabla “Códigos Solicitud
    ///     Descarga Masiva”.
    /// </param>
    /// <param name="NumeroCFDIs">Número de CFDIs que conforman la solicitud de descarga consultada.</param>
    /// <param name="CodEstatus">Código de estatus de la petición de verificación.</param>
    /// <param name="Mensaje">
    ///     Mensaje- Pequeña descripción del código estatus correspondiente a la petición de
    ///     verificación.
    /// </param>
    /// <param name="HttpStatusCode">Codigo de estatus de la respuesta HTTP.</param>
    /// <param name="ResponseContent">Contenido del mensaje de la respuesta HTTP.</param>
    public sealed record VerificacionResult(
        List<string> IdsPaquetes,
        string EstadoSolicitud,
        string CodigoEstadoSolicitud,
        string NumeroCFDIs,
        string CodEstatus,
        string Mensaje,
        HttpStatusCode HttpStatusCode,
        string ResponseContent);
}
