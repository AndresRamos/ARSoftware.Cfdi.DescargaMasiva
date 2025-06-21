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
    public class SolicitudDescargaRecibidosService : ISolicitudDescargaRecibidosService
    {
        private readonly IHttpSoapClient _httpSoapClient;

        public SolicitudDescargaRecibidosService(IHttpSoapClient httpSoapClient)
        {
            _httpSoapClient = httpSoapClient;
        }

        public string GenerateSoapRequestEnvelopeXmlContent(SolicitudDescargaRecibidosRequest solicitudRequest,
            X509Certificate2 certificate)
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
                "SolicitaDescargaRecibidos", CfdiDescargaMasivaNamespaces.DesNamespaceUrl);
            bodyElement.AppendChild(solicitaDescargaElement);

            XmlElement solicitudElement = xmlDocument.CreateElement(CfdiDescargaMasivaNamespaces.DesPrefix, "solicitud",
                CfdiDescargaMasivaNamespaces.DesNamespaceUrl);

            // Como se puede observar los atributos están ordenados, de acuerdo con el siguiente ejemplo:
            // 2.EstadoComprobante
            // 3.FechaInicial
            // 4.FechaFinal
            // 8.TipoSolicitud
            // 9.RfcReceptor

            // Optional
            if (solicitudRequest.HasEstadoComprobante)
                solicitudElement.SetAttribute("EstadoComprobante", solicitudRequest.EstadoComprobante.Value);

            // Obligatorio
            solicitudElement.SetAttribute("FechaInicial", solicitudRequest.FechaInicial.ToSoapStartDateString());

            // Obligatorio
            solicitudElement.SetAttribute("FechaFinal", solicitudRequest.FechaFinal.ToSoapEndDateString());

            // Obligatorio
            solicitudElement.SetAttribute("TipoSolicitud", solicitudRequest.TipoSolicitud.Name);

            // Obligatorio
            solicitudElement.SetAttribute("RfcReceptor", solicitudRequest.RfcReceptor);

            // Opcional
            if (solicitudRequest.HasRfcEmisor)
                solicitudElement.SetAttribute("RfcEmisor", solicitudRequest.RfcEmisor);

            // Optional
            if (solicitudRequest.HasTipoComprobante)
                solicitudElement.SetAttribute("TipoComprobante", solicitudRequest.TipoComprobante.Value);

            // Optional
            if (solicitudRequest.HasRfcSolicitante)
                solicitudElement.SetAttribute("RfcSolicitante", solicitudRequest.RfcSolicitante);

            // Optional
            if (solicitudRequest.HasRfcACuentaTerceros)
                solicitudElement.SetAttribute("RfcACuentaTerceros", solicitudRequest.RfcACuentaTerceros);

            //Optional
            if (solicitudRequest.HasComplemento)
                solicitudElement.SetAttribute("Complemento", solicitudRequest.Complemento);

            XmlElement signatureElement = SignedXmlHelper.SignRequest(solicitudElement, certificate);
            solicitudElement.AppendChild(signatureElement);
            solicitaDescargaElement.AppendChild(solicitudElement);

            return xmlDocument.OuterXml;
        }

        public async Task<SolicitudDescargaRecibidosResult> SendSoapRequestAsync(SolicitudDescargaRecibidosRequest solicitudRequest,
            X509Certificate2 certificate,
            CancellationToken cancellationToken = default)
        {
            string soapRequestContent = GenerateSoapRequestEnvelopeXmlContent(solicitudRequest, certificate);

            SoapRequestResult soapRequestResult = await _httpSoapClient.SendRequestAsync(
                CfdiDescargaMasivaWebServiceUrls.SolicitaDescargaService, DescargaMasivaSoapActionUrls.Solicitud,
                solicitudRequest.AccessToken, soapRequestContent, cancellationToken);

            return GetSoapResponseResult(soapRequestResult);
        }

        public SolicitudDescargaRecibidosResult GetSoapResponseResult(SoapRequestResult soapRequestResult)
        {
            XmlDocument xmlDocument = new();
            xmlDocument.LoadXml(soapRequestResult.ResponseContent);

            XmlNode element = xmlDocument.GetElementsByTagName("SolicitaDescargaRecibidosResult")[0];
            if (element is null)
            {
                throw new InvalidResponseContentException("Element SolicitaDescargaRecibidosResult is missing in response.",
                    soapRequestResult.ResponseContent);
            }

            if (element.Attributes is null)
            {
                throw new InvalidResponseContentException("Attributes property of Element SolicitaDescargaRecibidosResult is null.",
                    soapRequestResult.ResponseContent);
            }

            string requestIdSolicitud = element.Attributes.GetNamedItem("IdSolicitud")?.Value ?? string.Empty;
            string requestRfcSolicitante = element.Attributes.GetNamedItem("RfcSolicitante")?.Value ?? string.Empty;
            string requestCodEstatus = element.Attributes.GetNamedItem("CodEstatus")?.Value ?? string.Empty;
            string requestMensaje = element.Attributes.GetNamedItem("Mensaje")?.Value ?? string.Empty;

            return new SolicitudDescargaRecibidosResult
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
