# ARSoftware.Cfdi.DescargaMasiva
Clases y servicios para descargar los CFDI de forma masiva utilizando el web service del SAT.

Esta libreria permite hacer uso del web service proporcionado por el SAT para descargar los CFDI de forma masiva.
Esta libreria esta desarrollada en .Net Standard 2.0 lo que la hace compatible para implementarse en desarrollos en .Net Framework y .Net Core.

## Ejemplo
```csharp
public static void Main(string[] args)
{
    // Parameters
    var certificadoPfx = new byte[0];
    var certificadoPassword = "";
    var fechaInicio = DateTime.Today;
    var fechaFin = DateTime.Today;
    var tipoSolicitud = TipoSolicitud.Cfdi;
    var rfcEmisor = "";
    var rfcReceptor = "";
    var rfcSolicitante = "";

    var certificadoSat = X509Certificate2Helper.GetCertificate(certificadoPfx, certificadoPassword);

    // Autenticacion
    var autenticacionService = new AutenticacionService(CfdiDescargaMasivaWebServiceUrls.AutenticacionUrl, CfdiDescargaMasivaWebServiceUrls.AutenticacionSoapActionUrl);
    var autenticacionRequest = AutenticacionRequest.CreateInstance();
    var soapRequestEnvelopeXml = autenticacionService.GenerateSoapRequestEnvelopeXmlContent(autenticacionRequest, certificadoSat);
    var autenticacionResult = autenticacionService.SendSoapRequest(soapRequestEnvelopeXml);
    var authorizationHttpRequestHeader = $@"WRAP access_token=""{HttpUtility.UrlDecode(autenticacionResult.Token)}""";
    
    // Solicitud
    var solicitudService = new SolicitudService(CfdiDescargaMasivaWebServiceUrls.SolicitudUrl, CfdiDescargaMasivaWebServiceUrls.SolicitudSoapActionUrl);
    var solicitudRequest = SolicitudRequest.CreateInstance(
        fechaInicio,
        fechaFin,
        tipoSolicitud,
        rfcEmisor,
        rfcReceptor,
        rfcSolicitante);
    soapRequestEnvelopeXml = SolicitudService.GenerateSoapRequestEnvelopeXmlContent(solicitudRequest, certificadoSat);
    var solicitudResult = solicitudService.SendSoapRequest(soapRequestEnvelopeXml, authorizationHttpRequestHeader);

    // Verificacion
    var verificaSolicitudService = new VerificacionService(CfdiDescargaMasivaWebServiceUrls.VerificacionUrl, CfdiDescargaMasivaWebServiceUrls.VerificacionSoapActionUrl);
    var verificacionRequest = VerificacionRequest.CreateInstance(solicitudResult.IdSolicitud, rfcSolicitante);
    soapRequestEnvelopeXml = verificaSolicitudService.GenerateSoapRequestEnvelopeXmlContent(verificacionRequest, certificadoSat);
    var verificacionResult = verificaSolicitudService.SendSoapRequest(soapRequestEnvelopeXml, authorizationHttpRequestHeader);

    // Descarga
    var descargarSolicitudService = new DescargaService(CfdiDescargaMasivaWebServiceUrls.DescargaUrl, CfdiDescargaMasivaWebServiceUrls.DescargaSoapActionUrl);
    foreach (var idsPaquete in verificacionResult.IdsPaquetes)
    {
        var descargaRequest = DescargaRequest.CreateInstace(idsPaquete, rfcSolicitante);
        soapRequestEnvelopeXml = descargarSolicitudService.GenerateSoapRequestEnvelopeXmlContent(descargaRequest, certificadoSat);
        var descargaResult = descargarSolicitudService.SendSoapRequest(soapRequestEnvelopeXml, authorizationHttpRequestHeader);

        var fileName = Path.Combine(@"C:\CFDIS", $"{idsPaquete}.zip");
        var paqueteContenido = Convert.FromBase64String(descargaResult.Paquete);

        using (var fileStream = File.Create(fileName, paqueteContenido.Length))
        {
            fileStream.Write(paqueteContenido, 0, paqueteContenido.Length);
        }
    }
}

```
## Que Sigue?
- [x] Publicar prjecto
- [x] Publicar en Nuget
- [ ] Crear mejor documentacion
- [ ] Crear ejemplos de uso
- [ ] Cambiar el envio de soap a utilizar de HTTP Web Request a HTTP Client
- [ ] Mejorar la lectura del web response para capturar errores

Nuget: ```Install-Package ARSoftware.Cfdi.DescargaMasiva```

Email: andres@arsoft.net

Pagina Web: https://www.arsoft.net/

Facebook: https://www.facebook.com/AndresRamosSoftware/

Twitter: https://twitter.com/ar_software
