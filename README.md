# ARSoftware.Cfdi.DescargaMasiva

[![Nuget](https://img.shields.io/nuget/v/ARSoftware.Cfdi.DescargaMasiva?style=for-the-badge)](https://www.nuget.org/packages/ARSoftware.Cfdi.DescargaMasiva) 
[![GitHub Discussions](https://img.shields.io/github/discussions/AndresRamos/ARSoftware.Cfdi.DescargaMasiva?style=for-the-badge)](https://github.com/AndresRamos/ARSoftware.Cfdi.DescargaMasiva/discussions)

Este proyecto implementa clases y servicios para descargar los CFDI de forma masiva utilizando el web service del SAT. Desarrollado en .Net Standard 2.0, este proyecto es  compatible para implementarse en desarrollos en .Net Framework y .Net Core.

Este proyecto te permite realizar las siguiente peticiones al web service:
1. [Peticion de Autenticacion](https://github.com/AndresRamos/ARSoftware.Cfdi.DescargaMasiva/wiki/Autenticacion)
2. [Peticion de Solicitud](https://github.com/AndresRamos/ARSoftware.Cfdi.DescargaMasiva/wiki/Solicitud)
3. [Peticion de Verificacion](https://github.com/AndresRamos/ARSoftware.Cfdi.DescargaMasiva/wiki/Verificacion)
4. [Peticion de Descarga](https://github.com/AndresRamos/ARSoftware.Cfdi.DescargaMasiva/wiki/Descarga)

## Instalacion 
Puedes instalarlo utilizando [Nuget](https://www.nuget.org/packages/ARSoftware.Cfdi.DescargaMasiva) Package Manager Console
```
Install-Package ARSoftware.Cfdi.DescargaMasiva
```
O utilizando el .NET CLI Console
```
dotnet add package ARSoftware.Cfdi.DescargaMasiva
```

## Ejemplo
El siguiente ejemplo muestra como utilizar los servicios para hacer una solicitud de descarga de los CFDI recibidos el dia de hoy
```csharp
IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        // Registrar servicios de descarga masiva con el contenedor de servicios
        services.AddCfdiDescargaMasivaServices();
    })
    .Build();

CancellationTokenSource cancellationTokenSource = new();
CancellationToken cancellationToken = cancellationTokenSource.Token;

await host.StartAsync(cancellationToken);

var logger = host.Services.GetRequiredService<ILogger<Program>>();

logger.LogInformation("Iniciando ejemplo de como utilizar los servicios para descargar los CFDIs recibidos del dia de hoy.");

// Parametros de ejemplo
var rutaCertificadoPfx = @"C:\DescargaMasiva\certificado.pfx";
byte[] certificadoPfx = await File.ReadAllBytesAsync(rutaCertificadoPfx, cancellationToken);
var certificadoPassword = "12345678a";
DateTime fechaInicio = DateTime.Today;
DateTime fechaFin = DateTime.Today;
TipoSolicitud? tipoSolicitud = TipoSolicitud.Cfdi;
var rfcEmisor = "";
var rfcReceptores = new List<string> { "AAA010101AAAA" };
var rfcSolicitante = "AAA010101AAAA";
var rutaDescarga = @"C:\DescargaMasiva\CFDIs";

logger.LogInformation("Creando el certificado SAT con el certificado PFX y contrasena.");
X509Certificate2 certificadoSat = X509Certificate2Helper.GetCertificate(certificadoPfx, certificadoPassword);

// Autenticacion
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
logger.LogInformation("Buscando el servicio de solicitud de descarga en el contenedor de servicios (Dependency Injection).");
var solicitudService = host.Services.GetRequiredService<ISolicitudService>();

logger.LogInformation("Creando solicitud de solicitud de descarga.");
var solicitudRequest = SolicitudRequest.CreateInstance(fechaInicio,
    fechaFin,
    tipoSolicitud,
    rfcEmisor,
    rfcReceptores,
    rfcSolicitante,
    autenticacionResult.AccessToken);

logger.LogInformation("Enviando solicitud de solicitud de descarga.");
SolicitudResult solicitudResult = await solicitudService.SendSoapRequestAsync(solicitudRequest, certificadoSat, cancellationToken);

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
    {
        logger.LogInformation(
            "Es estado de la solicitud es Aceptada. Mandar otra solicitud de verificaion mas tarde para que el servicio web pueda procesar la solicitud.");
    }
    else if (verificacionResult.DownloadRequestStatusNumber == EstadoSolicitud.EnProceso.Value.ToString())
    {
        logger.LogInformation(
            "Es estado de la solicitud es En Proceso. Mandar otra solicitud de verificaion mas tarde para que el servicio web pueda procesar la solicitud.");
    }

    throw new Exception();
}

logger.LogInformation("La solicitud de verificacion fue exitosa.");
foreach (string idsPaquete in verificacionResult.PackageIds)
{
    logger.LogInformation("PackageId:{0}", idsPaquete);
}

// Descarga
logger.LogInformation("Buscando el servicio de verificacion en el contenedor de servicios (Dependency Injection).");
var descargarSolicitudService = host.Services.GetRequiredService<IDescargaService>();

foreach (string? idsPaquete in verificacionResult.PackageIds)
{
    logger.LogInformation("Creando solicitud de descarga.");
    var descargaRequest = DescargaRequest.CreateInstace(idsPaquete, rfcSolicitante, autenticacionResult.AccessToken);

    logger.LogInformation("Enviando solicitud de descarga.");
    DescargaResult? descargaResult = await descargarSolicitudService.SendSoapRequestAsync(descargaRequest,
        certificadoSat,
        cancellationToken);

    string fileName = Path.Combine(rutaDescarga, $"{idsPaquete}.zip");
    byte[] paqueteContenido = Convert.FromBase64String(descargaResult.Package);

    logger.LogInformation("Guardando paquete descargado en un archivo .zip en la ruta de descarga.");
    using FileStream fileStream = File.Create(fileName, paqueteContenido.Length);
    await fileStream.WriteAsync(paqueteContenido, 0, paqueteContenido.Length, cancellationToken);
}

await host.StopAsync(cancellationToken);

logger.LogInformation("Proceso terminado.");
```

## Aplicacion De Descarga Masiva
Si buscas una aplicacion ya lista para hacer la descarga masiva de CFDIs te recomiendo mi aplicacion [Manejador Documentos CFDI](https://github.com/AndresRamos/ARSoftware.ManejadorDocumentosCfdi).
