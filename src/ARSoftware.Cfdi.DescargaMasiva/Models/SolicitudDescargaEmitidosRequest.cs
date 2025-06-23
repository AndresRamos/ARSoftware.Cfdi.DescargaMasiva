using System;
using System.Collections.Generic;
using ARSoftware.Cfdi.DescargaMasiva.Enumerations;

namespace ARSoftware.Cfdi.DescargaMasiva.Models
{
    /// <summary>
    ///     Peticion de solicitud de descarga de CFDIs emitidos.
    /// </summary>
    /// <param name="AccessToken">Token de autorizacion.</param>
    /// <param name="FechaInicial">
    ///     Solo se buscarán CFDI, cuya fecha de emisión sea igual o mayor a la fecha inicial indicada en este parámetro.
    ///     Parámetro obligatorio.
    /// </param>
    /// <param name="FechaFinal">
    ///     Solo se buscarán CFDI, cuya fecha de emisión sea igual o menor a la fecha final indicada en este parámetro.
    ///     Parámetro obligatorio.
    /// </param>
    /// <param name="RfcEmisor">
    ///     Contiene el RFC del emisor del cual se quiere consultar los CFDIs.
    ///     Parámetro obligatorio.
    /// </param>
    /// <param name="TipoSolicitud">
    ///     Define el tipo de descarga: Metadata o CFDI. Parámetro Obligatorio.
    ///     • Metadata
    ///     • CFDI
    ///     Parámetro Obligatorio.
    /// </param>
    public sealed record SolicitudDescargaEmitidosRequest(
        DateTime FechaInicial,
        DateTime FechaFinal,
        string RfcEmisor,
        TipoSolicitud TipoSolicitud,
        AccessToken AccessToken)
    {
        /// <summary>
        ///     Contiene el/los RFC’s receptores de los cuales se quiere consultar los CFDIs.
        ///     Importante: El campo RfcReceptor, únicamente permite la captura de 5 registros como máximo.
        /// </summary>
        public List<string> RfcReceptores { get; init; } = new();

        /// <summary>
        ///     El RFC Solicitante corresponde al contribuyente que está realizando la solicitud de descarga.
        ///     Este parámetro es opcional, pero en caso de proporcionarse debe coincidir con el RFC Emisor.
        ///     Parámetro Opcional.
        /// </summary>
        public string RfcSolicitante { get; init; } = string.Empty;

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
    }
}
