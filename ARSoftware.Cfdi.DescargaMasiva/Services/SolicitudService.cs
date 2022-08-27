using System;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using ARSoftware.Cfdi.DescargaMasiva.Constants;
using ARSoftware.Cfdi.DescargaMasiva.Helpers;
using ARSoftware.Cfdi.DescargaMasiva.Interfaces;
using ARSoftware.Cfdi.DescargaMasiva.Models;

namespace ARSoftware.Cfdi.DescargaMasiva.Services
{
    public class SolicitudService : ISolicitudService
    {
        private readonly IHttpSoapClient _httpSoapClient;

        public SolicitudService(IHttpSoapClient httpSoapClient)
        {
            _httpSoapClient = httpSoapClient;
        }

        public string GenerateSoapRequestEnvelopeXmlContent(SolicitudRequest solicitudRequest, X509Certificate2 certificate)
        {
            var xmlDocument = new XmlDocument();

            XmlElement envelopElement = xmlDocument.CreateElement(CfdiDescargaMasivaNamespaces.S11Prefix,
                "Envelope",
                CfdiDescargaMasivaNamespaces.S11NamespaceUrl);
            envelopElement.SetAttribute($"xmlns:{CfdiDescargaMasivaNamespaces.S11Prefix}", CfdiDescargaMasivaNamespaces.S11NamespaceUrl);
            envelopElement.SetAttribute($"xmlns:{CfdiDescargaMasivaNamespaces.DesPrefix}", CfdiDescargaMasivaNamespaces.DesNamespaceUrl);
            envelopElement.SetAttribute($"xmlns:{CfdiDescargaMasivaNamespaces.DsPrefix}", CfdiDescargaMasivaNamespaces.DsNamespaceUrl);
            xmlDocument.AppendChild(envelopElement);

            XmlElement headerElement = xmlDocument.CreateElement(CfdiDescargaMasivaNamespaces.S11Prefix,
                "Header",
                CfdiDescargaMasivaNamespaces.S11NamespaceUrl);
            envelopElement.AppendChild(headerElement);

            XmlElement bodyElement = xmlDocument.CreateElement(CfdiDescargaMasivaNamespaces.S11Prefix,
                "Body",
                CfdiDescargaMasivaNamespaces.S11NamespaceUrl);
            envelopElement.AppendChild(bodyElement);

            XmlElement solicitaDescargaElement = xmlDocument.CreateElement(CfdiDescargaMasivaNamespaces.DesPrefix,
                "SolicitaDescarga",
                CfdiDescargaMasivaNamespaces.DesNamespaceUrl);
            bodyElement.AppendChild(solicitaDescargaElement);

            XmlElement solicitudElement = xmlDocument.CreateElement(CfdiDescargaMasivaNamespaces.DesPrefix,
                "solicitud",
                CfdiDescargaMasivaNamespaces.DesNamespaceUrl);
            solicitudElement.SetAttribute("FechaInicial", solicitudRequest.StartDate.ToSoapStartDateString());
            solicitudElement.SetAttribute("FechaFinal", solicitudRequest.EndDate.ToSoapEndDateString());
            solicitudElement.SetAttribute("RfcEmisor", solicitudRequest.SenderRfc);
            solicitudElement.SetAttribute("RfcSolicitante", solicitudRequest.RequestingRfc);
            solicitudElement.SetAttribute("TipoSolicitud", solicitudRequest.RequestType.Name);
            solicitudElement.SetAttribute("RfcACuentaTerceros", solicitudRequest.ThirdPartyRfc);

            // Optional
            if (solicitudRequest.HasDocumentType)
            {
                solicitudElement.SetAttribute("TipoComprobante", solicitudRequest.DocumentType.Value);
            }

            // Optional
            if (solicitudRequest.HasDocumentStatus)
            {
                solicitudElement.SetAttribute("EstadoComprobante", solicitudRequest.DocumentStatus.Value.ToString());
            }

            // Optional
            if (solicitudRequest.HasComplement)
            {
                solicitudElement.SetAttribute("Complemento", solicitudRequest.Complement);
            }

            // Optional
            if (solicitudRequest.HasUuid)
            {
                solicitudElement.SetAttribute("UUID", solicitudRequest.Uuid);
            }

            XmlElement rfcReceptores = xmlDocument.CreateElement(CfdiDescargaMasivaNamespaces.DesPrefix,
                "RfcReceptores",
                CfdiDescargaMasivaNamespaces.DesNamespaceUrl);
            foreach (string item in solicitudRequest.RecipientsRfcs)
            {
                XmlElement rfcReceptorElement = xmlDocument.CreateElement(CfdiDescargaMasivaNamespaces.DesPrefix,
                    "RfcReceptor",
                    CfdiDescargaMasivaNamespaces.DesNamespaceUrl);
                rfcReceptorElement.InnerText = item;
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
                                                                  CancellationToken cancellationToken = default)
        {
            return await _httpSoapClient.SendRequestAsync(CfdiDescargaMasivaWebServiceUrls.SolicitudUrl,
                CfdiDescargaMasivaWebServiceUrls.SolicitudSoapActionUrl,
                accessToken,
                soapRequestContent,
                cancellationToken);
        }

        public async Task<SolicitudResult> SendSoapRequestAsync(SolicitudRequest solicitudRequest,
                                                                X509Certificate2 certificate,
                                                                CancellationToken cancellationToken = default)
        {
            string soapRequestContent = GenerateSoapRequestEnvelopeXmlContent(solicitudRequest, certificate);

            SoapRequestResult soapRequestResult = await _httpSoapClient.SendRequestAsync(CfdiDescargaMasivaWebServiceUrls.SolicitudUrl,
                CfdiDescargaMasivaWebServiceUrls.SolicitudSoapActionUrl,
                solicitudRequest.AccessToken,
                soapRequestContent,
                cancellationToken);

            return GetSoapResponseResult(soapRequestResult);
        }

        public SolicitudResult GetSoapResponseResult(SoapRequestResult soapRequestResult)
        {
            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(soapRequestResult.ResponseContent);

            XmlNode element = xmlDocument.GetElementsByTagName("SolicitaDescargaResult")[0];
            if (element is null)
            {
                throw new ArgumentException("El resultado no estan en un formato valido.", nameof(soapRequestResult.ResponseContent));
            }

            if (element.Attributes is null)
            {
                throw new ArgumentException("El nodo no tiene atributos.", nameof(soapRequestResult.ResponseContent));
            }

            string codEstatus = element.Attributes.GetNamedItem("CodEstatus")?.Value;
            string mensaje = element.Attributes.GetNamedItem("Mensaje")?.Value;
            string idSolicitud = element.Attributes.GetNamedItem("IdSolicitud")?.Value;
            return SolicitudResult.CreateInstance(codEstatus, idSolicitud, mensaje, soapRequestResult.ResponseContent);
        }
    }
}
