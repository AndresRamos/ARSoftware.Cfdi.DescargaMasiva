using Ardalis.SmartEnum;

namespace ARSoftware.Cfdi.DescargaMasiva.Enumerations;

/// <summary>
///     Código de estatus de la solicitud. Mensajes recibidos desde la operación SolicitaDescarga,
///     VerificaSolicitudDescarga y Descargar.
/// </summary>
public sealed class CodigoEstatusSolicitud : SmartEnum<CodigoEstatusSolicitud>
{
    /// <summary>
    ///     300 = Usuario No Válido
    /// </summary>
    public static readonly CodigoEstatusSolicitud _300 = new("300", 300, "Usuario No Válido", "");

    /// <summary>
    ///     301 = XML Mal Formado
    /// </summary>
    public static readonly CodigoEstatusSolicitud _301 = new("301",
        301,
        "XML Mal Formado",
        "Este código de error se regresa cuando el request posee información invalida, ejemplo: un RFC de receptor no valido");

    /// <summary>
    ///     302 = Sello Mal Formado
    /// </summary>
    public static readonly CodigoEstatusSolicitud _302 = new("302", 302, "Sello Mal Formado", "");

    /// <summary>
    ///     303 = Sello no corresponde con RfcSolicitante
    /// </summary>
    public static readonly CodigoEstatusSolicitud _303 = new("303", 303, "Sello no corresponde con RfcSolicitante", "");

    /// <summary>
    ///     304 = Certificado Revocado o Caduco
    /// </summary>
    public static readonly CodigoEstatusSolicitud _304 = new("304",
        304,
        "Certificado Revocado o Caduco",
        "El certificado fue revocado o bien la fecha de vigencia expiró");

    /// <summary>
    ///     305 = Certificado Inválido
    /// </summary>
    public static readonly CodigoEstatusSolicitud _305 = new("305",
        305,
        "Certificado Inválido",
        "El certificado puede ser invalido por múltiples razones como son el tipo, codificación incorrecta, etc.");

    /// <summary>
    ///     404 = Error no Controlado
    /// </summary>
    public static readonly CodigoEstatusSolicitud _404 = new("404",
        404,
        "Error no Controlado",
        "Error genérico, en caso de presentarse realizar nuevamente la petición y si persiste el error levantar un RMA.");

    /// <summary>
    ///     5000 = Solicitud de descarga recibida con éxito
    /// </summary>
    public static readonly CodigoEstatusSolicitud _5000 = new("5000", 5000, "Solicitud de descarga recibida con éxito", "");

    /// <summary>
    ///     5001 = Tercero no autorizado
    /// </summary>
    public static readonly CodigoEstatusSolicitud _5001 = new("5001",
        5001,
        "Tercero no autorizado",
        "El solicitante no tiene autorización de descarga de xml de los contribuyentes");

    /// <summary>
    ///     5002 = Se han agotado las solicitudes de por vida
    /// </summary>
    public static readonly CodigoEstatusSolicitud _5002 = new("5002",
        5002,
        "Se han agotado las solicitudes de por vida",
        "Se ha alcanzado el límite de solicitudes, con el mismo criterio");

    /// <summary>
    ///     5003 = Se han agotado las solicitudes de por vida
    /// </summary>
    public static readonly CodigoEstatusSolicitud _5003 = new("5003",
        5003,
        "Tope máximo",
        "Indica que en base a los parámetros de consulta se está superando el tope máximo de CFDI o Metadata, por solicitud de descarga masiva.");

    /// <summary>
    ///     5004 = No se encontró la información
    /// </summary>
    public static readonly CodigoEstatusSolicitud _5004 = new("5004",
        5004,
        "No se encontró la información",
        "No se encontró la información del paquete solicitado");

    /// <summary>
    ///     5005 = Ya se tiene una solicitud registrada
    /// </summary>
    public static readonly CodigoEstatusSolicitud _5005 = new("5005",
        5005,
        "Ya se tiene una solicitud registrada",
        "Ya existe una solicitud activa con los mismos criterios");

    /// <summary>
    ///     5006 = Error interno en el proceso
    /// </summary>
    public static readonly CodigoEstatusSolicitud _5006 = new("5006", 5006, "Error interno en el proceso", "");

    /// <summary>
    ///     5007 = No existe el paquete solicitado
    /// </summary>
    public static readonly CodigoEstatusSolicitud _5007 = new("5007",
        5007,
        "No existe el paquete solicitado",
        "Los paquetes solo tienen un periodo de vida de 72hrs");

    /// <summary>
    ///     5008 = Máximo de descargas permitidas
    /// </summary>
    public static readonly CodigoEstatusSolicitud _5008 = new("5008",
        5008,
        "Máximo de descargas permitidas",
        "Un paquete solo puede descargarse un total de 2 veces, una vez agotadas, ya no se podrá volver a descargar");

    private CodigoEstatusSolicitud(string name, int value, string mensaje, string observaciones) : base(name, value)
    {
        Mensaje = mensaje;
        Observaciones = observaciones;
    }

    public string Mensaje { get; }
    public string Observaciones { get; }
}
