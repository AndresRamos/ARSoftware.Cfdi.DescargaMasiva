using System.Security.Cryptography.X509Certificates;
using ARSoftware.Cfdi.DescargaMasiva;
using ARSoftware.Cfdi.DescargaMasiva.Enumerations;
using ARSoftware.Cfdi.DescargaMasiva.Helpers;
using ARSoftware.Cfdi.DescargaMasiva.Interfaces;
using ARSoftware.Cfdi.DescargaMasiva.Models;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        // Registrar servicios de descarga masiva con el proveedor de servicios
        services.AddCfdiDescargaMasivaServices();
    })
    .Build();

CancellationTokenSource cancellationTokenSource = new();
CancellationToken cancellationToken = cancellationTokenSource.Token;

await host.StartAsync(cancellationToken);

var logger = host.Services.GetRequiredService<ILogger<Program>>();

logger.LogInformation("Iniciando ejemplo de como utilizar los servicios para descargar los CFDIs recibidos del dia de hoy.");

// Crear X509Certificate2 del certificado PFX
var rutaCertificadoPfx = @"C:\AR Software\CFDI Descarga Masiva\certificado.pfx";
var certificadoPassword = "12345678a";
byte[] certificadoPfx = await File.ReadAllBytesAsync(rutaCertificadoPfx, cancellationToken);

logger.LogInformation("Creando el certificado SAT con el certificado PFX y contrasena.");
X509Certificate2 certificadoSat = X509Certificate2Helper.GetCertificate(certificadoPfx, certificadoPassword);

// Peticion De Autenticacion
logger.LogInformation("Buscando el servicio de autenticacion en el contenedor de servicios (Dependency Injection).");
var autenticacionService = host.Services.GetRequiredService<IAutenticacionService>();

logger.LogInformation("Creando solicitud de autenticacion.");
var autenticacionRequest = AutenticacionRequest.CreateInstance();

logger.LogInformation("Enviando solicitud de autenticacion.");
AutenticacionResult autenticacionResult =
    await autenticacionService.SendSoapRequestAsync(autenticacionRequest, certificadoSat, cancellationToken);

if (!autenticacionResult.AccessToken.IsValid)
{
    logger.LogError("La solicitud de autenticacion no fue exitosa. FaultCode:{0} FaultString:{1}",
        autenticacionResult.FaultCode,
        autenticacionResult.FaultString);
    throw new Exception();
}

logger.LogInformation("La solicitud de autenticacion fue exitosa. AccessToken:{0}", autenticacionResult.AccessToken.DecodedValue);

// Solicitud

// Paremetros para buscar CFDIs recibidos por rango de fecha
DateTime fechaInicio = DateTime.Today;
DateTime fechaFin = DateTime.Today;
TipoSolicitud tipoSolicitud = TipoSolicitud.Cfdi;
var rfcEmisor = "";
var rfcReceptores = new List<string> { "AAA010101AAA" };
var rfcSolicitante = "AAA010101AAA";

logger.LogInformation("Buscando el servicio de solicitud de descarga en el contenedor de servicios (Dependency Injection).");
var solicitudService = host.Services.GetRequiredService<ISolicitudService>();

logger.LogInformation("Creando solicitud de solicitud de descarga.");
var solicitudPorRangoFecha = SolicitudRequest.CreateInstance(fechaInicio,
    fechaFin,
    tipoSolicitud,
    rfcEmisor,
    rfcReceptores,
    rfcSolicitante,
    autenticacionResult.AccessToken);

logger.LogInformation("Enviando solicitud de solicitud de descarga.");
SolicitudResult solicitudResult = await solicitudService.SendSoapRequestAsync(solicitudPorRangoFecha, certificadoSat, cancellationToken);

if (string.IsNullOrEmpty(solicitudResult.RequestId))
{
    logger.LogError("La solicitud de solicitud de descarga no fue exitosa. RequestStatusCode:{0} RequestStatusMessage:{1}",
        solicitudResult.RequestStatusCode,
        solicitudResult.RequestStatusMessage);
    throw new Exception();
}

logger.LogInformation("La solicitud de solicitud de descarga fue exitosa. RequestId:{0}", solicitudResult.RequestId);

// Verificacion
logger.LogInformation("Buscando el servicio de verificacion en el contenedor de servicios (Dependency Injection).");
var verificaSolicitudService = host.Services.GetRequiredService<IVerificacionService>();

logger.LogInformation("Creando solicitud de verificacion.");
var verificacionRequest = VerificacionRequest.CreateInstance(solicitudResult.RequestId, rfcSolicitante, autenticacionResult.AccessToken);

logger.LogInformation("Enviando solicitud de verificacion.");
VerificacionResult verificacionResult = await verificaSolicitudService.SendSoapRequestAsync(verificacionRequest,
    certificadoSat,
    cancellationToken);

if (verificacionResult.DownloadRequestStatusNumber != EstadoSolicitud.Terminada.Value.ToString())
{
    logger.LogError(
        "La solicitud de verificacion no fue exitosa. DownloadRequestStatusNumber:{0} RequestStatusCode:{1} RequestStatusMessage:{2}",
        verificacionResult.DownloadRequestStatusNumber,
        verificacionResult.RequestStatusCode,
        verificacionResult.RequestStatusMessage);

    if (verificacionResult.DownloadRequestStatusNumber == EstadoSolicitud.Aceptada.Value.ToString())
        logger.LogInformation(
            "Es estado de la solicitud es Aceptada. Mandar otra solicitud de verificaion mas tarde para que el servicio web pueda procesar la solicitud.");
    else if (verificacionResult.DownloadRequestStatusNumber == EstadoSolicitud.EnProceso.Value.ToString())
        logger.LogInformation(
            "Es estado de la solicitud es En Proceso. Mandar otra solicitud de verificaion mas tarde para que el servicio web pueda procesar la solicitud.");

    throw new Exception();
}

logger.LogInformation("La solicitud de verificacion fue exitosa.");
foreach (string idsPaquete in verificacionResult.PackageIds)
    logger.LogInformation("PackageId:{0}", idsPaquete);

// Descarga
logger.LogInformation("Buscando el servicio de verificacion en el contenedor de servicios (Dependency Injection).");
var descargarSolicitudService = host.Services.GetRequiredService<IDescargaService>();

foreach (string? idsPaquete in verificacionResult.PackageIds)
{
    logger.LogInformation("Creando solicitud de descarga.");
    var descargaRequest = DescargaRequest.CreateInstace(idsPaquete, rfcSolicitante, autenticacionResult.AccessToken);

    logger.LogInformation("Enviando solicitud de descarga.");
    DescargaResult descargaResult = await descargarSolicitudService.SendSoapRequestAsync(descargaRequest,
        certificadoSat,
        cancellationToken);

    var rutaDescarga = @"C:\AR Software\CFDI Descarga Masiva\CFDIs";

    string fileName = Path.Combine(rutaDescarga, $"{idsPaquete}.zip");
    byte[] paqueteContenido = Convert.FromBase64String(descargaResult.Package);

    logger.LogInformation("Guardando paquete descargado en un archivo .zip en la ruta de descarga.");
    using FileStream fileStream = File.Create(fileName, paqueteContenido.Length);
    await fileStream.WriteAsync(paqueteContenido, 0, paqueteContenido.Length, cancellationToken);
}

await host.StopAsync(cancellationToken);

logger.LogInformation("Proceso terminado.");
