using System;
using System.Security.Cryptography.X509Certificates;
using System.Xml;
using ARSoftware.Cfdi.DescargaMasiva.Constants;
using ARSoftware.Cfdi.DescargaMasiva.Helpers;
using ARSoftware.Cfdi.DescargaMasiva.Models;

namespace ARSoftware.Cfdi.DescargaMasiva.Services
{
    public class SolicitudService
    {
        public readonly string SoapActionUrl;
        public readonly string Url;

        public SolicitudService(string url, string action)
        {
            Url = url;
            SoapActionUrl = action;
        }

        public static string GenerateSoapRequestEnvelopeXmlContent(SolicitudRequest solicitudRequest, X509Certificate2 certificate)
        {
            var xmlDocument = new XmlDocument();

            var envelopElement = xmlDocument.CreateElement(CfdiDescargaMasivaNamespaces.S11Prefix, "Envelope", CfdiDescargaMasivaNamespaces.S11NamespaceUrl);
            envelopElement.SetAttribute($"xmlns:{CfdiDescargaMasivaNamespaces.S11Prefix}", CfdiDescargaMasivaNamespaces.S11NamespaceUrl);
            envelopElement.SetAttribute($"xmlns:{CfdiDescargaMasivaNamespaces.DesPrefix}", CfdiDescargaMasivaNamespaces.DesNamespaceUrl);
            envelopElement.SetAttribute($"xmlns:{CfdiDescargaMasivaNamespaces.DsPrefix}", CfdiDescargaMasivaNamespaces.DsNamespaceUrl);
            xmlDocument.AppendChild(envelopElement);

            var headerElement = xmlDocument.CreateElement(CfdiDescargaMasivaNamespaces.S11Prefix, "Header", CfdiDescargaMasivaNamespaces.S11NamespaceUrl);
            envelopElement.AppendChild(headerElement);

            var bodyElement = xmlDocument.CreateElement(CfdiDescargaMasivaNamespaces.S11Prefix, "Body", CfdiDescargaMasivaNamespaces.S11NamespaceUrl);
            envelopElement.AppendChild(bodyElement);

            var solicitaDescargaElement = xmlDocument.CreateElement(CfdiDescargaMasivaNamespaces.DesPrefix, "SolicitaDescarga", CfdiDescargaMasivaNamespaces.DesNamespaceUrl);
            bodyElement.AppendChild(solicitaDescargaElement);

            var solicitudElement = xmlDocument.CreateElement(CfdiDescargaMasivaNamespaces.DesPrefix, "solicitud", CfdiDescargaMasivaNamespaces.DesNamespaceUrl);
            solicitudElement.SetAttribute("FechaInicial", solicitudRequest.StartDate.ToSoapStartDateString());
            solicitudElement.SetAttribute("FechaFinal", solicitudRequest.EndDate.ToSoapEndDateString());
            solicitudElement.SetAttribute("RfcEmisor", solicitudRequest.SenderRfc);
            solicitudElement.SetAttribute("RfcReceptor", solicitudRequest.RecipientRfc);
            solicitudElement.SetAttribute("RfcSolicitante", solicitudRequest.RequestingRfc);
            solicitudElement.SetAttribute("TipoSolicitud", solicitudRequest.RequestType.Name);

            var signatureElement = SignedXmlHelper.SignRequest(solicitudElement, certificate);
            solicitudElement.AppendChild(signatureElement);
            solicitaDescargaElement.AppendChild(solicitudElement);

            return xmlDocument.OuterXml;
        }

        public SolicitudResult GetSoapResponseResult(SoapRequestResult soapRequestResult)
        {
            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(soapRequestResult.WebResponse);

            var element = xmlDocument.GetElementsByTagName("SolicitaDescargaResult")[0];
            if (element != null)
            {
                var codEstatus = element.Attributes.GetNamedItem("CodEstatus")?.Value;
                var mensaje = element.Attributes.GetNamedItem("Mensaje")?.Value;
                var idSolicitud = element.Attributes.GetNamedItem("IdSolicitud")?.Value;
                return SolicitudResult.CreateInstance(codEstatus, idSolicitud, mensaje, soapRequestResult.WebResponse);
            }

            throw new ArgumentException("El resultado no estan en un formato valido.", nameof(soapRequestResult.WebResponse));
        }

        public SolicitudResult SendSoapRequest(string soapRequestContent, string authorizationHttpRequestHeader)
        {
            var httpWebRequestSoapService = new HttpWebRequestSoapService(Url, SoapActionUrl);
            var soapRequestResult = httpWebRequestSoapService.SendSoapRequest(soapRequestContent, authorizationHttpRequestHeader);
            return GetSoapResponseResult(soapRequestResult);
        }
    }
}