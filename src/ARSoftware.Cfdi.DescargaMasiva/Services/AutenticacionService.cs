using System;
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
    public sealed class AutenticacionService : IAutenticacionService
    {
        private readonly IHttpSoapClient _httpSoapClient;

        public AutenticacionService(IHttpSoapClient httpSoapClient)
        {
            _httpSoapClient = httpSoapClient;
        }

        public string GenerateSoapRequestEnvelopeXmlContent(AutenticacionRequest autenticacionRequest, X509Certificate2 certificate)
        {
            var xmlDocument = new XmlDocument();

            XmlElement envelopElement = xmlDocument.CreateElement(CfdiDescargaMasivaNamespaces.S11Prefix,
                "Envelope",
                CfdiDescargaMasivaNamespaces.S11NamespaceUrl);
            envelopElement.SetAttribute($"xmlns:{CfdiDescargaMasivaNamespaces.S11Prefix}", CfdiDescargaMasivaNamespaces.S11NamespaceUrl);
            envelopElement.SetAttribute($"xmlns:{CfdiDescargaMasivaNamespaces.WsuPrefix}", CfdiDescargaMasivaNamespaces.WsuNamespaceUrl);
            xmlDocument.AppendChild(envelopElement);

            XmlElement headerElement = xmlDocument.CreateElement(CfdiDescargaMasivaNamespaces.S11Prefix,
                "Header",
                CfdiDescargaMasivaNamespaces.S11NamespaceUrl);
            envelopElement.AppendChild(headerElement);

            XmlElement securityElement = xmlDocument.CreateElement(CfdiDescargaMasivaNamespaces.WssePrefix,
                "Security",
                CfdiDescargaMasivaNamespaces.WsseNamespaceUrl);
            securityElement.SetAttribute("mustUnderstand", CfdiDescargaMasivaNamespaces.S11NamespaceUrl, "1");
            headerElement.AppendChild(securityElement);

            XmlElement timestampElement = xmlDocument.CreateElement(CfdiDescargaMasivaNamespaces.WsuPrefix,
                "Timestamp",
                CfdiDescargaMasivaNamespaces.WsuNamespaceUrl);
            timestampElement.SetAttribute("Id", CfdiDescargaMasivaNamespaces.WsuNamespaceUrl, "_0");
            securityElement.AppendChild(timestampElement);

            XmlElement createdElement = xmlDocument.CreateElement(CfdiDescargaMasivaNamespaces.WsuPrefix,
                "Created",
                CfdiDescargaMasivaNamespaces.WsuNamespaceUrl);
            createdElement.InnerText = autenticacionRequest.TokenCreatedDateUtc.ToSoapSecurityTimestampString();
            timestampElement.AppendChild(createdElement);

            XmlElement expiresElement = xmlDocument.CreateElement(CfdiDescargaMasivaNamespaces.WsuPrefix,
                "Expires",
                CfdiDescargaMasivaNamespaces.WsuNamespaceUrl);
            expiresElement.InnerText = autenticacionRequest.TokenExpiresDateUtc.ToSoapSecurityTimestampString();
            timestampElement.AppendChild(expiresElement);

            XmlElement binarySecurityTokenElement = xmlDocument.CreateElement(CfdiDescargaMasivaNamespaces.WssePrefix,
                "BinarySecurityToken",
                CfdiDescargaMasivaNamespaces.WsseNamespaceUrl);
            binarySecurityTokenElement.SetAttribute("Id",
                CfdiDescargaMasivaNamespaces.WsuNamespaceUrl,
                autenticacionRequest.Uuid.ToBinarySecurityTokenId());
            binarySecurityTokenElement.SetAttribute("ValueType",
                "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-x509-token-profile-1.0#X509v3");
            binarySecurityTokenElement.SetAttribute("EncodingType",
                "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#Base64Binary");
            binarySecurityTokenElement.InnerText = Convert.ToBase64String(certificate.RawData);
            securityElement.AppendChild(binarySecurityTokenElement);

            XmlElement securityTokenReferenceElement = xmlDocument.CreateElement(CfdiDescargaMasivaNamespaces.WssePrefix,
                "SecurityTokenReference",
                CfdiDescargaMasivaNamespaces.WsseNamespaceUrl);
            XmlElement securityTokenReferenceReferenceElement = xmlDocument.CreateElement(CfdiDescargaMasivaNamespaces.WssePrefix,
                "Reference",
                CfdiDescargaMasivaNamespaces.WsseNamespaceUrl);
            XmlAttribute valueType = xmlDocument.CreateAttribute("ValueType");
            valueType.Value = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-x509-token-profile-1.0#X509v3";
            securityTokenReferenceReferenceElement.Attributes.Append(valueType);
            XmlAttribute encodingType = xmlDocument.CreateAttribute("URI");
            encodingType.Value = $"#{autenticacionRequest.Uuid.ToBinarySecurityTokenId()}";
            securityTokenReferenceReferenceElement.Attributes.Append(encodingType);
            securityTokenReferenceElement.AppendChild(securityTokenReferenceReferenceElement);

            XmlElement signatureElement =
                SignedXmlHelper.SignAuthenticationRequest(timestampElement, certificate, "#_0", securityTokenReferenceElement);
            securityElement.AppendChild(signatureElement);

            XmlElement bodyElement = xmlDocument.CreateElement(CfdiDescargaMasivaNamespaces.S11Prefix,
                "Body",
                CfdiDescargaMasivaNamespaces.S11NamespaceUrl);
            envelopElement.AppendChild(bodyElement);

            XmlElement autenticaElement = xmlDocument.CreateElement("Autentica");
            autenticaElement.SetAttribute("xmlns", "http://DescargaMasivaTerceros.gob.mx");
            bodyElement.AppendChild(autenticaElement);

            return xmlDocument.OuterXml;
        }

        public async Task<SoapRequestResult> SendSoapRequestAsync(string soapRequestContent, CancellationToken cancellationToken = default)
        {
            return await _httpSoapClient.SendRequestAsync(CfdiDescargaMasivaWebServiceUrls.AutenticacionUrl,
                CfdiDescargaMasivaWebServiceUrls.AutenticacionSoapActionUrl,
                AccessToken.CreateEmpty(),
                soapRequestContent,
                cancellationToken);
        }

        public async Task<AutenticacionResult> SendSoapRequestAsync(AutenticacionRequest autenticacionRequest,
                                                                    X509Certificate2 certificate,
                                                                    CancellationToken cancellationToken = default)
        {
            string soapRequestContent = GenerateSoapRequestEnvelopeXmlContent(autenticacionRequest, certificate);

            SoapRequestResult soapRequestResult = await _httpSoapClient.SendRequestAsync(CfdiDescargaMasivaWebServiceUrls.AutenticacionUrl,
                CfdiDescargaMasivaWebServiceUrls.AutenticacionSoapActionUrl,
                AccessToken.CreateEmpty(),
                soapRequestContent,
                cancellationToken);

            return GetSoapResponseResult(soapRequestResult);
        }

        public AutenticacionResult GetSoapResponseResult(SoapRequestResult soapRequestResult)
        {
            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(soapRequestResult.ResponseContent);

            XmlNode autenticaResultElement = xmlDocument.GetElementsByTagName("AutenticaResult")[0];
            if (autenticaResultElement != null)
            {
                var accessToken = AccessToken.CreateInstance(autenticaResultElement.InnerXml);
                return AutenticacionResult.CreateSuccess(accessToken, soapRequestResult.HttpStatusCode, soapRequestResult.ResponseContent);
            }

            XmlNode errorElement = xmlDocument.GetElementsByTagName("s:Fault")[0];
            if (errorElement is null)
                throw new InvalidResponseContentException("Elements AutenticaResult and s:Fault are missing in response.",
                    soapRequestResult.ResponseContent);

            string faultCode = xmlDocument.GetElementsByTagName("faultcode")[0]?.InnerXml ??
                               throw new InvalidOperationException("Element faultcode not found.");
            string faultString = xmlDocument.GetElementsByTagName("faultstring")[0]?.InnerXml ??
                                 throw new InvalidOperationException("Element faultstring not found.");
            return AutenticacionResult.CreateFailure(faultCode,
                faultString,
                soapRequestResult.HttpStatusCode,
                soapRequestResult.ResponseContent);
        }
    }
}
