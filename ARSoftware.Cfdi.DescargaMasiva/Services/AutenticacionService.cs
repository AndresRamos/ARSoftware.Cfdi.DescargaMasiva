using System;
using System.Security.Cryptography.X509Certificates;
using System.Xml;
using ARSoftware.Cfdi.DescargaMasiva.Constants;
using ARSoftware.Cfdi.DescargaMasiva.Helpers;
using ARSoftware.Cfdi.DescargaMasiva.Models;

namespace ARSoftware.Cfdi.DescargaMasiva.Services
{
    public class AutenticacionService
    {
        public readonly string SoapActionUrl;
        public readonly string Url;

        public AutenticacionService(string url, string action)
        {
            Url = url;
            SoapActionUrl = action;
        }

        public string GenerateSoapRequestEnvelopeXmlContent(AutenticacionRequest autenticacionRequest, X509Certificate2 certificate)
        {
            var xmlDocument = new XmlDocument();

            var envelopElement = xmlDocument.CreateElement(CfdiDescargaMasivaNamespaces.S11Prefix, "Envelope", CfdiDescargaMasivaNamespaces.S11NamespaceUrl);
            envelopElement.SetAttribute($"xmlns:{CfdiDescargaMasivaNamespaces.S11Prefix}", CfdiDescargaMasivaNamespaces.S11NamespaceUrl);
            envelopElement.SetAttribute($"xmlns:{CfdiDescargaMasivaNamespaces.WsuPrefix}", CfdiDescargaMasivaNamespaces.WsuNamespaceUrl);
            xmlDocument.AppendChild(envelopElement);

            var headerElement = xmlDocument.CreateElement(CfdiDescargaMasivaNamespaces.S11Prefix, "Header", CfdiDescargaMasivaNamespaces.S11NamespaceUrl);
            envelopElement.AppendChild(headerElement);

            var securityElement = xmlDocument.CreateElement(CfdiDescargaMasivaNamespaces.WssePrefix, "Security", CfdiDescargaMasivaNamespaces.WsseNamespaceUrl);
            securityElement.SetAttribute("mustUnderstand", CfdiDescargaMasivaNamespaces.S11NamespaceUrl, "1");
            headerElement.AppendChild(securityElement);

            var timestampElement = xmlDocument.CreateElement(CfdiDescargaMasivaNamespaces.WsuPrefix, "Timestamp", CfdiDescargaMasivaNamespaces.WsuNamespaceUrl);
            timestampElement.SetAttribute("Id", CfdiDescargaMasivaNamespaces.WsuNamespaceUrl, "_0");
            securityElement.AppendChild(timestampElement);

            var createdElement = xmlDocument.CreateElement(CfdiDescargaMasivaNamespaces.WsuPrefix, "Created", CfdiDescargaMasivaNamespaces.WsuNamespaceUrl);
            createdElement.InnerText = autenticacionRequest.TokenCreatedDateUtc.ToSoapSecurityTimestampString();
            timestampElement.AppendChild(createdElement);

            var expiresElement = xmlDocument.CreateElement(CfdiDescargaMasivaNamespaces.WsuPrefix, "Expires", CfdiDescargaMasivaNamespaces.WsuNamespaceUrl);
            expiresElement.InnerText = autenticacionRequest.TokenExpiresDateUtc.ToSoapSecurityTimestampString();
            timestampElement.AppendChild(expiresElement);

            var binarySecurityTokenElement = xmlDocument.CreateElement(CfdiDescargaMasivaNamespaces.WssePrefix, "BinarySecurityToken", CfdiDescargaMasivaNamespaces.WsseNamespaceUrl);
            binarySecurityTokenElement.SetAttribute("Id", CfdiDescargaMasivaNamespaces.WsuNamespaceUrl, autenticacionRequest.Uuid.ToBinarySecurityTokenId());
            binarySecurityTokenElement.SetAttribute("ValueType", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-x509-token-profile-1.0#X509v3");
            binarySecurityTokenElement.SetAttribute("EncodingType", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#Base64Binary");
            binarySecurityTokenElement.InnerText = Convert.ToBase64String(certificate.RawData);
            securityElement.AppendChild(binarySecurityTokenElement);

            var securityTokenReferenceElement = xmlDocument.CreateElement(CfdiDescargaMasivaNamespaces.WssePrefix, "SecurityTokenReference", CfdiDescargaMasivaNamespaces.WsseNamespaceUrl);
            var securityTokenReferenceReferenceElement = xmlDocument.CreateElement(CfdiDescargaMasivaNamespaces.WssePrefix, "Reference", CfdiDescargaMasivaNamespaces.WsseNamespaceUrl);
            var valueType = xmlDocument.CreateAttribute("ValueType");
            valueType.Value = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-x509-token-profile-1.0#X509v3";
            securityTokenReferenceReferenceElement.Attributes.Append(valueType);
            var encodingType = xmlDocument.CreateAttribute("URI");
            encodingType.Value = $"#{autenticacionRequest.Uuid.ToBinarySecurityTokenId()}";
            securityTokenReferenceReferenceElement.Attributes.Append(encodingType);
            securityTokenReferenceElement.AppendChild(securityTokenReferenceReferenceElement);

            var signatureElement = SignedXmlHelper.SignAuthenticationRequest(timestampElement, certificate, "#_0", securityTokenReferenceElement);
            securityElement.AppendChild(signatureElement);

            var bodyElement = xmlDocument.CreateElement(CfdiDescargaMasivaNamespaces.S11Prefix, "Body", CfdiDescargaMasivaNamespaces.S11NamespaceUrl);
            envelopElement.AppendChild(bodyElement);

            var autenticaElement = xmlDocument.CreateElement("Autentica");
            autenticaElement.SetAttribute("xmlns", "http://DescargaMasivaTerceros.gob.mx");
            bodyElement.AppendChild(autenticaElement);

            return xmlDocument.OuterXml;
        }

        public AutenticacionResult GetSoapResponseResult(SoapRequestResult soapRequestResult)
        {
            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(soapRequestResult.WebResponse);

            var AutenticaResultElement = xmlDocument.GetElementsByTagName("AutenticaResult")[0];
            if (AutenticaResultElement != null)
            {
                var token = AutenticaResultElement.InnerXml;
                return AutenticacionResult.CreateInstance(token, null, null, soapRequestResult.WebResponse);
            }

            var errorElement = xmlDocument.GetElementsByTagName("s:Fault")[0];
            if (errorElement != null)
            {
                var faultCode = xmlDocument.GetElementsByTagName("faultcode")[0].InnerXml;
                var faultString = xmlDocument.GetElementsByTagName("faultstring")[0].InnerXml;
                return AutenticacionResult.CreateInstance(null, faultCode, faultString, soapRequestResult.WebResponse);
            }

            throw new ArgumentException("Web response is not in a valid format.", nameof(soapRequestResult.WebResponse));
        }

        public AutenticacionResult SendSoapRequest(string soapRequestContent)
        {
            var httpWebRequestSoapService = new HttpWebRequestSoapService(Url, SoapActionUrl);
            var soapRequestResult = httpWebRequestSoapService.SendSoapRequest(soapRequestContent, null);
            return GetSoapResponseResult(soapRequestResult);
        }
    }
}