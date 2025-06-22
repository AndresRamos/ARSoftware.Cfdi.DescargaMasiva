using System;
using ARSoftware.Cfdi.DescargaMasiva.Enumerations;

namespace ARSoftware.Cfdi.DescargaMasiva.Models
{
    /// <summary>
    ///     Peticion de solicitud de descarga de CFDIs recibidos.
    /// </summary>
    public record SolicitudDescargaRecibidosRequest
    {
        /// <summary>
        ///     Token de autorizacion.
        /// </summary>
        public required AccessToken AccessToken { get; init; }

        /// <summary>
        ///     Solo se buscarán CFDI, cuya fecha de emisión sea igual o mayor a la fecha inicial indicada en este parámetro.
        ///     Parámetro obligatorio.
        /// </summary>
        public required DateTime FechaInicial { get; init; }

        /// <summary>
        ///     Solo se buscarán CFDI, cuya fecha de emisión sea igual o menor a la fecha final indicada en este parámetro.
        ///     Parámetro obligatorio.
        /// </summary>
        public required DateTime FechaFinal { get; init; }

        /// <summary>
        ///     Contiene el RFC Receptor el cual corresponde con el contribuyente del cual se requiere la información.
        ///     Parámetro obligatorio.
        /// </summary>
        public required string RfcReceptor { get; init; }

        /// <summary>
        ///     Contiene el RFC del emisor del cual se quiere consultar los CFDIs.
        ///     Parámetro opcional.
        /// </summary>
        public string RfcEmisor { get; init; } = string.Empty;

        /// <summary>
        ///     El RFC Solicitante corresponde al contribuyente que está realizando la solicitud de descarga.
        ///     Este parámetro es opcional, pero en caso de proporcionarse debe coincidir con el RFC Receptor.
        ///     Parámetro Opcional.
        /// </summary>
        public string RfcSolicitante { get; init; } = string.Empty;

        /// <summary>
        ///     Define el tipo de descarga:
        ///     • Metadata
        ///     • CFDI
        ///     Parámetro Obligatorio.
        /// </summary>
        public required TipoSolicitud TipoSolicitud { get; init; }

        /// <summary>
        ///     Define el tipo de comprobante:
        ///     • null
        ///     • I = Ingreso
        ///     • E = Egreso
        ///     • T= Traslado
        ///     • N = Nomina
        ///     • P = Pago
        ///     *null es el valor predeterminado y en caso de no declararse, se obtendrán todos los comprobantes sin importar el
        ///     tipo comprobante. Los valores aceptados son: I, E, T, N y P.
        /// </summary>
        public TipoComprobante TipoComprobante { get; init; } = TipoComprobante.Null;

        /// <summary>
        ///     Dato opcional que define el estado del comprobante:
        ///     • Todos
        ///     • Cancelado.
        ///     • Vigente.
        ///     En caso de que no se proporcione, se considerara Vigente como valor por defecto.
        ///     Regla: Para efectos de la metadata el listado solo incluirá los comprobantes vigentes y cancelados, para efectos de
        ///     la descarga de XML, solo se incluirán los vigentes. Por lo tanto, el servicio no descargará XML cancelados.
        /// </summary>
        public EstadoComprobante EstadoComprobante { get; init; } = EstadoComprobante.Null;

        /// <summary>
        ///     Contiene el RFC del a cuenta a tercero del cual se quiere consultar los CFDIs
        /// </summary>
        public string RfcACuentaTerceros { get; init; } = string.Empty;

        /// <summary>
        ///     Define el complemento de CFDI a descargar:
        /// </summary>
        public string Complemento { get; init; } = string.Empty;

        public bool HasTipoComprobante => TipoComprobante != TipoComprobante.Null;

        public bool HasEstadoComprobante => EstadoComprobante != EstadoComprobante.Null;

        public bool HasComplemento => !string.IsNullOrWhiteSpace(Complemento);

        public bool HasRfcACuentaTerceros => !string.IsNullOrWhiteSpace(RfcACuentaTerceros);

        public bool HasRfcSolicitante => !string.IsNullOrWhiteSpace(RfcSolicitante);

        public bool HasRfcEmisor => !string.IsNullOrWhiteSpace(RfcEmisor);
    }
}
