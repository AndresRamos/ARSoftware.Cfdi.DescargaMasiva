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
    public sealed class SolicitudDescargaFolioService : ISolicitudDescargaFolioService
    {
        private readonly IHttpSoapClient _httpSoapClient;

        public SolicitudDescargaFolioService(IHttpSoapClient httpSoapClient)
        {
            _httpSoapClient = httpSoapClient;
        }

        public string GenerateSoapRequestEnvelopeXmlContent(SolicitudDescargaFolioRequest solicitudRequest, X509Certificate2 certificate)
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

            XmlElement solicitaDescargaElement = xmlDocument.CreateElement(CfdiDescargaMasivaNamespaces.DesPrefix, "SolicitaDescargaFolio",
                CfdiDescargaMasivaNamespaces.DesNamespaceUrl);
            bodyElement.AppendChild(solicitaDescargaElement);

            XmlElement solicitudElement = xmlDocument.CreateElement(CfdiDescargaMasivaNamespaces.DesPrefix, "solicitud",
                CfdiDescargaMasivaNamespaces.DesNamespaceUrl);

            // Obligatorio
            solicitudElement.SetAttribute("Folio", solicitudRequest.Folio);

            // Optional
            if (solicitudRequest.HasRfcSolicitante)
                solicitudElement.SetAttribute("RfcSolicitante", solicitudRequest.RfcSolicitante);

            XmlElement signatureElement = SignedXmlHelper.SignRequest(solicitudElement, certificate);
            solicitudElement.AppendChild(signatureElement);
            solicitaDescargaElement.AppendChild(solicitudElement);

            return xmlDocument.OuterXml;
        }

        public async Task<SolicitudDescargaFolioResult> SendSoapRequestAsync(SolicitudDescargaFolioRequest solicitudRequest,
            X509Certificate2 certificate,
            CancellationToken cancellationToken = default)
        {
            string soapRequestContent = GenerateSoapRequestEnvelopeXmlContent(solicitudRequest, certificate);

            SoapRequestResult soapRequestResult = await _httpSoapClient.SendRequestAsync(
                CfdiDescargaMasivaWebServiceUrls.SolicitaDescargaService, DescargaMasivaSoapActionUrls.Solicitud,
                solicitudRequest.AccessToken, soapRequestContent, cancellationToken);

            return GetSoapResponseResult(soapRequestResult);
        }

        public SolicitudDescargaFolioResult GetSoapResponseResult(SoapRequestResult soapRequestResult)
        {
            XmlDocument xmlDocument = new();
            xmlDocument.LoadXml(soapRequestResult.ResponseContent);

            XmlNode element = xmlDocument.GetElementsByTagName("SolicitaDescargaFolioResult")[0];
            if (element is null)
            {
                throw new InvalidResponseContentException("Element SolicitaDescargaFolioResult is missing in response.",
                    soapRequestResult.ResponseContent);
            }

            if (element.Attributes is null)
            {
                throw new InvalidResponseContentException("Attributes property of Element SolicitaDescargaFolioResult is null.",
                    soapRequestResult.ResponseContent);
            }

            string requestIdSolicitud = element.Attributes.GetNamedItem("IdSolicitud")?.Value ?? string.Empty;
            string requestRfcSolicitante = element.Attributes.GetNamedItem("RfcSolicitante")?.Value ?? string.Empty;
            string requestCodEstatus = element.Attributes.GetNamedItem("CodEstatus")?.Value ?? string.Empty;
            string requestMensaje = element.Attributes.GetNamedItem("Mensaje")?.Value ?? string.Empty;

            return new SolicitudDescargaFolioResult
            {
                IdSolicitud = requestIdSolicitud,
                RfcSolicitante = requestRfcSolicitante,
                CodEstatus = requestCodEstatus,
                Mensaje = requestMensaje,
                HttpStatusCode = soapRequestResult.HttpStatusCode,
                ResponseContent = soapRequestResult.ResponseContent
            };
        }
    }
}
