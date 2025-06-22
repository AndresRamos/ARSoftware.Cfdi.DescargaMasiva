using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using ARSoftware.Cfdi.DescargaMasiva.Constants;
using ARSoftware.Cfdi.DescargaMasiva.Exceptions;
using ARSoftware.Cfdi.DescargaMasiva.Helpers;
using ARSoftware.Cfdi.DescargaMasiva.Interfaces;
using ARSoftware.Cfdi.DescargaMasiva.Models;

namespace ARSoftware.Cfdi.DescargaMasiva.Services
{
    public sealed class SolicitudDescargaEmitidosService : ISolicitudDescargaEmitidosService
    {
        private readonly IHttpSoapClient _httpSoapClient;

        public SolicitudDescargaEmitidosService(IHttpSoapClient httpSoapClient)
        {
            _httpSoapClient = httpSoapClient;
        }

        public string GenerateSoapRequestEnvelopeXmlContent(SolicitudDescargaEmitidosRequest solicitudRequest, X509Certificate2 certificate)
        {
            XmlDocument xmlDocument = new();

            XmlElement envelopElement = xmlDocument.CreateElement(CfdiDescargaMasivaNamespaces.S11Prefix, "Envelope",
                CfdiDescargaMasivaNamespaces.S11NamespaceUrl);
            envelopElement.SetAttribute($"xmlns:{CfdiDescargaMasivaNamespaces.S11Prefix}", CfdiDescargaMasivaNamespaces.S11NamespaceUrl);
            envelopElement.SetAttribute($"xmlns:{CfdiDescargaMasivaNamespaces.DesPrefix}", CfdiDescargaMasivaNamespaces.DesNamespaceUrl);
            envelopElement.SetAttribute($"xmlns:{CfdiDescargaMasivaNamespaces.DsPrefix}", CfdiDescargaMasivaNamespaces.DsNamespaceUrl);
            xmlDocument.AppendChild(envelopElement);

            XmlElement headerElement = xmlDocument.CreateElement(CfdiDescargaMasivaNamespaces.S11Prefix, "Header",
                CfdiDescargaMasivaNamespaces.S11NamespaceUrl);
            envelopElement.AppendChild(headerElement);

            XmlElement bodyElement = xmlDocument.CreateElement(CfdiDescargaMasivaNamespaces.S11Prefix, "Body",
                CfdiDescargaMasivaNamespaces.S11NamespaceUrl);
            envelopElement.AppendChild(bodyElement);

            XmlElement solicitaDescargaElement = xmlDocument.CreateElement(CfdiDescargaMasivaNamespaces.DesPrefix,
                "SolicitaDescargaEmitidos", CfdiDescargaMasivaNamespaces.DesNamespaceUrl);
            bodyElement.AppendChild(solicitaDescargaElement);

            XmlElement solicitudElement = xmlDocument.CreateElement(CfdiDescargaMasivaNamespaces.DesPrefix, "solicitud",
                CfdiDescargaMasivaNamespaces.DesNamespaceUrl);

            // Documentación oficial del SAT:
            // Como se puede observar los atributos están ordenados, de acuerdo con el siguiente ejemplo:
            // 2. EstadoComprobante
            // 3. FechaInicial
            // 4. FechaFinal
            // 5. RfcEmisor
            // 7. TipoComprobante
            // 8. TipoSolicitud 

            // Optional
            if (solicitudRequest.HasEstadoComprobante)
                solicitudElement.SetAttribute("EstadoComprobante", solicitudRequest.EstadoComprobante.Value);

            // Obligatorio
            solicitudElement.SetAttribute("FechaInicial", solicitudRequest.FechaInicial.ToSoapStartDateString());

            // Obligatorio
            solicitudElement.SetAttribute("FechaFinal", solicitudRequest.FechaFinal.ToSoapEndDateString());

            // Obligatorio
            solicitudElement.SetAttribute("RfcEmisor", solicitudRequest.RfcEmisor);

            // Optional
            if (solicitudRequest.HasTipoComprobante)
                solicitudElement.SetAttribute("TipoComprobante", solicitudRequest.TipoComprobante.Value);

            // Obligatorio
            solicitudElement.SetAttribute("TipoSolicitud", solicitudRequest.TipoSolicitud.Name);

            // Optional
            if (solicitudRequest.HasRfcSolicitante)
                solicitudElement.SetAttribute("RfcSolicitante", solicitudRequest.RfcSolicitante);

            // Optional
            if (solicitudRequest.HasRfcACuentaTerceros)
                solicitudElement.SetAttribute("RfcACuentaTerceros", solicitudRequest.RfcACuentaTerceros);

            //Optional
            if (solicitudRequest.HasComplemento)
                solicitudElement.SetAttribute("Complemento", solicitudRequest.Complemento);

            XmlElement rfcReceptores = xmlDocument.CreateElement(CfdiDescargaMasivaNamespaces.DesPrefix, "RfcReceptores",
                CfdiDescargaMasivaNamespaces.DesNamespaceUrl);
            foreach (string rfcReceptor in solicitudRequest.RfcReceptores)
            {
                XmlElement rfcReceptorElement = xmlDocument.CreateElement(CfdiDescargaMasivaNamespaces.DesPrefix, "RfcReceptor",
                    CfdiDescargaMasivaNamespaces.DesNamespaceUrl);
                rfcReceptorElement.InnerText = rfcReceptor;
                rfcReceptores.AppendChild(rfcReceptorElement);
            }

            solicitudElement.AppendChild(rfcReceptores);

            XmlElement signatureElement = SignedXmlHelper.SignRequest(solicitudElement, certificate);
            solicitudElement.AppendChild(signatureElement);
            solicitaDescargaElement.AppendChild(solicitudElement);

            return xmlDocument.OuterXml;
        }

        public async Task<SoapRequestResult> SendSoapRequestAsync(string soapRequestContent,
            AccessToken accessToken,
            CancellationToken cancellationToken)
        {
            return await _httpSoapClient.SendRequestAsync(CfdiDescargaMasivaWebServiceUrls.SolicitaDescargaService,
                DescargaMasivaSoapActionUrls.SolicitaDescargaEmitidos, accessToken, soapRequestContent, cancellationToken);
        }

        public async Task<SolicitudDescargaEmitidosResult> SendSoapRequestAsync(SolicitudDescargaEmitidosRequest solicitudRequest,
            X509Certificate2 certificate,
            CancellationToken cancellationToken = default)
        {
            string soapRequestContent = GenerateSoapRequestEnvelopeXmlContent(solicitudRequest, certificate);

            SoapRequestResult soapRequestResult = await _httpSoapClient.SendRequestAsync(
                CfdiDescargaMasivaWebServiceUrls.SolicitaDescargaService, DescargaMasivaSoapActionUrls.SolicitaDescargaEmitidos,
                solicitudRequest.AccessToken, soapRequestContent, cancellationToken);

            return GetSoapResponseResult(soapRequestResult);
        }

        public SolicitudDescargaEmitidosResult GetSoapResponseResult(SoapRequestResult soapRequestResult)
        {
            XmlDocument xmlDocument = new();
            xmlDocument.LoadXml(soapRequestResult.ResponseContent);

            XmlNode? element = xmlDocument.GetElementsByTagName("SolicitaDescargaEmitidosResult")[0];
            if (element is null)
            {
                throw new InvalidResponseContentException("Element SolicitaDescargaEmitidosResult is missing in response.",
                    soapRequestResult.ResponseContent);
            }

            if (element.Attributes is null)
            {
                throw new InvalidResponseContentException("Attributes property of Element SolicitaDescargaEmitidosResult is null.",
                    soapRequestResult.ResponseContent);
            }

            string requestIdSolicitud = element.Attributes.GetNamedItem("IdSolicitud")?.Value ?? string.Empty;
            string requestRfcSolicitante = element.Attributes.GetNamedItem("RfcSolicitante")?.Value ?? string.Empty;
            string requestCodEstatus = element.Attributes.GetNamedItem("CodEstatus")?.Value ?? string.Empty;
            string requestMensaje = element.Attributes.GetNamedItem("Mensaje")?.Value ?? string.Empty;

            return new SolicitudDescargaEmitidosResult
            {
                RfcSolicitante = requestRfcSolicitante,
                IdSolicitud = requestIdSolicitud,
                CodEstatus = requestCodEstatus,
                Mensaje = requestMensaje,
                HttpStatusCode = soapRequestResult.HttpStatusCode,
                ResponseContent = soapRequestResult.ResponseContent
            };
        }
    }
}
