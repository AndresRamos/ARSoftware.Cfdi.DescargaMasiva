using System.Security.Cryptography.X509Certificates;
using ARSoftware.Cfdi.DescargaMasiva;
using ARSoftware.Cfdi.DescargaMasiva.Enumerations;
using ARSoftware.Cfdi.DescargaMasiva.Helpers;
using ARSoftware.Cfdi.DescargaMasiva.Interfaces;
using ARSoftware.Cfdi.DescargaMasiva.Models;

namespace ConsoleDemo;

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
