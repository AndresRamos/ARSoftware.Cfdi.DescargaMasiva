using System.Security.Cryptography.X509Certificates;
using System.Web;
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

        IHost host = Host.CreateDefaultBuilder(args).ConfigureServices(services => { services.AddCfdiDescargaMasivaServices(); }).Build();

        await host.StartAsync(cancellationToken);

        var certificadoPfx = new byte[0];
        var certificadoPassword = "";
        DateTime fechaInicio = DateTime.Today;
        DateTime fechaFin = DateTime.Today;
        TipoSolicitud? tipoSolicitud = TipoSolicitud.Cfdi;
        var rfcEmisor = "";
        var rfcReceptores = new List<string> { "" };
        var rfcSolicitante = "";

        X509Certificate2? certificadoSat = X509Certificate2Helper.GetCertificate(certificadoPfx, certificadoPassword);

        // Autenticacion
        var autenticacionService = host.Services.GetRequiredService<IAutenticacionService>();
        var autenticacionRequest = AutenticacionRequest.CreateInstance();
        string? soapRequestEnvelopeXml = autenticacionService.GenerateSoapRequestEnvelopeXmlContent(autenticacionRequest, certificadoSat);
        AutenticacionResult? autenticacionResult =
            await autenticacionService.SendSoapRequestAsync(soapRequestEnvelopeXml, cancellationToken);
        var authorizationHttpRequestHeader = $@"WRAP access_token=""{HttpUtility.UrlDecode(autenticacionResult.Token)}""";

        // Solicitud
        var solicitudService = host.Services.GetRequiredService<ISolicitudService>();
        var solicitudRequest = SolicitudRequest.CreateInstance(
            fechaInicio,
            fechaFin,
            tipoSolicitud,
            rfcEmisor,
            rfcReceptores,
            rfcSolicitante);
        soapRequestEnvelopeXml = solicitudService.GenerateSoapRequestEnvelopeXmlContent(solicitudRequest, certificadoSat);
        SolicitudResult? solicitudResult =
            await solicitudService.SendSoapRequestAsync(soapRequestEnvelopeXml, authorizationHttpRequestHeader, cancellationToken);

        // Verificacion
        var verificaSolicitudService = host.Services.GetRequiredService<IVerificacionService>();
        var verificacionRequest = VerificacionRequest.CreateInstance(solicitudResult.IdSolicitud, rfcSolicitante);
        soapRequestEnvelopeXml = verificaSolicitudService.GenerateSoapRequestEnvelopeXmlContent(verificacionRequest, certificadoSat);
        VerificacionResult? verificacionResult =
            await verificaSolicitudService.SendSoapRequestAsync(soapRequestEnvelopeXml, authorizationHttpRequestHeader, cancellationToken);

        // Descarga
        var descargarSolicitudService = host.Services.GetRequiredService<IDescargaService>();
        foreach (string? idsPaquete in verificacionResult.IdsPaquetes)
        {
            var descargaRequest = DescargaRequest.CreateInstace(idsPaquete, rfcSolicitante);
            soapRequestEnvelopeXml = descargarSolicitudService.GenerateSoapRequestEnvelopeXmlContent(descargaRequest, certificadoSat);
            DescargaResult? descargaResult =
                await descargarSolicitudService.SendSoapRequestAsync(soapRequestEnvelopeXml,
                    authorizationHttpRequestHeader,
                    cancellationToken);

            string fileName = Path.Combine(@"C:\CFDIS", $"{idsPaquete}.zip");
            byte[] paqueteContenido = Convert.FromBase64String(descargaResult.Paquete);

            using (FileStream fileStream = File.Create(fileName, paqueteContenido.Length))
            {
                await fileStream.WriteAsync(paqueteContenido, 0, paqueteContenido.Length, cancellationToken);
            }
        }

        await host.StopAsync(cancellationToken);
    }
}
