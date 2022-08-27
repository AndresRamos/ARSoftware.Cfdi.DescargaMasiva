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
    public class DescargaService : IDescargaService
    {
        private readonly IHttpSoapClient _httpSoapClient;

        public DescargaService(IHttpSoapClient httpSoapClient)
        {
            _httpSoapClient = httpSoapClient;
        }

        public string GenerateSoapRequestEnvelopeXmlContent(DescargaRequest descargaRequest, X509Certificate2 certificate)
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

            XmlElement peticionDescargaMasivaTercerosEntradaElement = xmlDocument.CreateElement(CfdiDescargaMasivaNamespaces.DesPrefix,
                "PeticionDescargaMasivaTercerosEntrada",
                CfdiDescargaMasivaNamespaces.DesNamespaceUrl);
            bodyElement.AppendChild(peticionDescargaMasivaTercerosEntradaElement);

            XmlElement peticionDescargaElement = xmlDocument.CreateElement(CfdiDescargaMasivaNamespaces.DesPrefix,
                "peticionDescarga",
                CfdiDescargaMasivaNamespaces.DesNamespaceUrl);
            peticionDescargaElement.SetAttribute("IdPaquete", descargaRequest.PackageId);
            peticionDescargaElement.SetAttribute("RfcSolicitante", descargaRequest.RequestingRfc);

            XmlElement signatureElement = SignedXmlHelper.SignRequest(peticionDescargaElement, certificate);
            peticionDescargaElement.AppendChild(signatureElement);
            peticionDescargaMasivaTercerosEntradaElement.AppendChild(peticionDescargaElement);

            return xmlDocument.OuterXml;
        }

        public async Task<SoapRequestResult> SendSoapRequestAsync(string soapRequestContent,
                                                                  AccessToken accessToken,
                                                                  CancellationToken cancellationToken = default)
        {
            return await _httpSoapClient.SendRequestAsync(CfdiDescargaMasivaWebServiceUrls.DescargaUrl,
                CfdiDescargaMasivaWebServiceUrls.DescargaSoapActionUrl,
                accessToken,
                soapRequestContent,
                cancellationToken);
        }

        public async Task<DescargaResult> SendSoapRequestAsync(DescargaRequest descargaRequest,
                                                               X509Certificate2 certificate,
                                                               CancellationToken cancellationToken = default)
        {
            string soapRequestContent = GenerateSoapRequestEnvelopeXmlContent(descargaRequest, certificate);

            SoapRequestResult soapRequestResult = await _httpSoapClient.SendRequestAsync(CfdiDescargaMasivaWebServiceUrls.DescargaUrl,
                CfdiDescargaMasivaWebServiceUrls.DescargaSoapActionUrl,
                descargaRequest.AccessToken,
                soapRequestContent,
                cancellationToken);

            return GetSoapResponseResult(soapRequestResult);
        }

        public DescargaResult GetSoapResponseResult(SoapRequestResult soapRequestResult)
        {
            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(soapRequestResult.ResponseContent);

            XmlNode element = xmlDocument.GetElementsByTagName("h:respuesta")[0];
            if (element is null)
            {
                throw new ArgumentException("El resultado no estan en un formato valido.", nameof(soapRequestResult.ResponseContent));
            }

            if (element.Attributes is null)
            {
                throw new ArgumentException("El nodo no tiene atributos.", nameof(soapRequestResult.ResponseContent));
            }

            string codEstatus = element.Attributes.GetNamedItem("CodEstatus").Value;
            string mensaje = element.Attributes.GetNamedItem("Mensaje").Value;
            string paqete = xmlDocument.GetElementsByTagName("Paquete")[0].InnerXml;

            return DescargaResult.CreateInstance(codEstatus, mensaje, paqete, soapRequestResult.ResponseContent);
        }
    }
}
