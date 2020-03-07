using System.Security.Cryptography.X509Certificates;
using System.Xml;
using ARSoftware.Cfdi.DescargaMasiva.Constants;
using ARSoftware.Cfdi.DescargaMasiva.Helpers;
using ARSoftware.Cfdi.DescargaMasiva.Models;

namespace ARSoftware.Cfdi.DescargaMasiva.Services
{
    public class DescargaService
    {
        public readonly string SoapActionUrl;
        public readonly string Url;

        public DescargaService(string url, string action)
        {
            Url = url;
            SoapActionUrl = action;
        }

        public string GenerateSoapRequestEnvelopeXmlContent(DescargaRequest descargaRequest, X509Certificate2 certificate)
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

            var peticionDescargaMasivaTercerosEntradaElement = xmlDocument.CreateElement(CfdiDescargaMasivaNamespaces.DesPrefix, "PeticionDescargaMasivaTercerosEntrada", CfdiDescargaMasivaNamespaces.DesNamespaceUrl);
            bodyElement.AppendChild(peticionDescargaMasivaTercerosEntradaElement);

            var peticionDescargaElement = xmlDocument.CreateElement(CfdiDescargaMasivaNamespaces.DesPrefix, "peticionDescarga", CfdiDescargaMasivaNamespaces.DesNamespaceUrl);
            peticionDescargaElement.SetAttribute("IdPaquete", descargaRequest.PackageId);
            peticionDescargaElement.SetAttribute("RfcSolicitante", descargaRequest.RequestingRfc);

            var signatureElement = SignedXmlHelper.SignRequest(peticionDescargaElement, certificate);
            peticionDescargaElement.AppendChild(signatureElement);
            peticionDescargaMasivaTercerosEntradaElement.AppendChild(peticionDescargaElement);

            return xmlDocument.OuterXml;
        }

        public DescargaResult GetSoapResponseResult(SoapRequestResult soapRequestResult)
        {
            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(soapRequestResult.WebResponse);

            var element = xmlDocument.GetElementsByTagName("h:respuesta")[0];
            var codEstatus = element.Attributes.GetNamedItem("CodEstatus").Value;
            var mensaje = element.Attributes.GetNamedItem("Mensaje").Value;
            var paqete = xmlDocument.GetElementsByTagName("Paquete")[0].InnerXml;

            return DescargaResult.CreateInstance(codEstatus, mensaje, paqete, soapRequestResult.WebResponse);
        }

        public DescargaResult SendSoapRequest(string soapRequestContent, string authorizationHttpRequestHeader)
        {
            var httpWebRequestSoapService = new HttpWebRequestSoapService(Url, SoapActionUrl);
            var soapRequestResult = httpWebRequestSoapService.SendSoapRequest(soapRequestContent, authorizationHttpRequestHeader);
            return GetSoapResponseResult(soapRequestResult);
        }
    }
}