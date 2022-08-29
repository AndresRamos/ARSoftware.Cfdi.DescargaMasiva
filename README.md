# ARSoftware.Cfdi.DescargaMasiva

![Nuget](https://img.shields.io/nuget/v/ARSoftware.Cfdi.DescargaMasiva?style=for-the-badge)

Este proyecto implementa clases y servicios para descargar los CFDI de forma masiva utilizando el web service del SAT. Desarrollado en .Net Standard 2.0, este proyecto es  compatible para implementarse en desarrollos en .Net Framework y .Net Core.

Este proyecto te permite realizar las siguiente peticiones al web service:
1. Peticion de Autenticacion
2. Peticion de Solicitud
3. Peticion de Verificacion
4. Peticion de Descarga

## Instalacion 
Puedes instalarlo utilizando [Nuget](https://www.nuget.org/packages/ARSoftware.Cfdi.DescargaMasiva)
```
Install-Package ARSoftware.Cfdi.DescargaMasiva
```
O utilizando el .Net CLI
```
dotnet add package ARSoftware.Cfdi.DescargaMasiva
```

## Ejemplo
```csharp
public class Program
{
    private static readonly CancellationTokenSource CancellationTokenSource = new();

    public static async Task Main(string[] args)
    {
        CancellationToken cancellationToken = CancellationTokenSource.Token;

        IHost host = Host.CreateDefaultBuilder(args)
            .ConfigureServices(services =>
            {
                // Registrar servicios de descarga masiva
                services.AddCfdiDescargaMasivaServices();
            })
            .Build();

        await host.StartAsync(cancellationToken);

        var certificadoPfx = new byte[0];
        var certificadoPassword = "";
        DateTime fechaInicio = DateTime.Today;
        DateTime fechaFin = DateTime.Today;
        TipoSolicitud? tipoSolicitud = TipoSolicitud.Cfdi;
        var rfcEmisor = "";
        var rfcReceptores = new List<string> { "" };
        var rfcSolicitante = "";
        var rutaDescarga = @"C:\CFDIS";

        X509Certificate2? certificadoSat = X509Certificate2Helper.GetCertificate(certificadoPfx, certificadoPassword);

        // Autenticacion
        var autenticacionService = host.Services.GetRequiredService<IAutenticacionService>();
        var autenticacionRequest = AutenticacionRequest.CreateInstance();
        AutenticacionResult? autenticacionResult =
            await autenticacionService.SendSoapRequestAsync(autenticacionRequest, certificadoSat, cancellationToken);

        // Solicitud
        var solicitudService = host.Services.GetRequiredService<ISolicitudService>();
        var solicitudRequest = SolicitudRequest.CreateInstance(fechaInicio,
            fechaFin,
            tipoSolicitud,
            rfcEmisor,
            rfcReceptores,
            rfcSolicitante,
            autenticacionResult.AccessToken);
        SolicitudResult? solicitudResult = await solicitudService.SendSoapRequestAsync(solicitudRequest, certificadoSat, cancellationToken);

        // Verificacion
        var verificaSolicitudService = host.Services.GetRequiredService<IVerificacionService>();
        var verificacionRequest =
            VerificacionRequest.CreateInstance(solicitudResult.RequestId, rfcSolicitante, autenticacionResult.AccessToken);
        VerificacionResult? verificacionResult = await verificaSolicitudService.SendSoapRequestAsync(verificacionRequest,
            certificadoSat,
            cancellationToken);

        // Descarga
        var descargarSolicitudService = host.Services.GetRequiredService<IDescargaService>();
        foreach (string? idsPaquete in verificacionResult.PackageIds)
        {
            var descargaRequest = DescargaRequest.CreateInstace(idsPaquete, rfcSolicitante, autenticacionResult.AccessToken);
            DescargaResult? descargaResult = await descargarSolicitudService.SendSoapRequestAsync(descargaRequest,
                certificadoSat,
                cancellationToken);

            string fileName = Path.Combine(rutaDescarga, $"{idsPaquete}.zip");
            byte[] paqueteContenido = Convert.FromBase64String(descargaResult.Package);

            using (FileStream fileStream = File.Create(fileName, paqueteContenido.Length))
            {
                await fileStream.WriteAsync(paqueteContenido, 0, paqueteContenido.Length, cancellationToken);
            }
        }

        await host.StopAsync(cancellationToken);
    }
}
```

## Aplicacion De Descarga Masiva
Si buscas una aplicacion ya lista para hacer la descarga masiva de CFDIs te recomiendo mi aplicacion [Manejador Documentos CFDI](https://github.com/AndresRamos/ARSoftware.ManejadorDocumentosCfdi).
