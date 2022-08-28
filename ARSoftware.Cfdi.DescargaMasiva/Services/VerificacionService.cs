using System.Collections.Generic;
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
    public class VerificacionService : IVerificacionService
    {
        private readonly IHttpSoapClient _httpSoapClient;

        public VerificacionService(IHttpSoapClient httpSoapClient)
        {
            _httpSoapClient = httpSoapClient;
        }

        public string GenerateSoapRequestEnvelopeXmlContent(VerificacionRequest verificacionRequest, X509Certificate2 certificate)
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

            XmlElement verificaSolicitudDescargaElement = xmlDocument.CreateElement(CfdiDescargaMasivaNamespaces.DesPrefix,
                "VerificaSolicitudDescarga",
                CfdiDescargaMasivaNamespaces.DesNamespaceUrl);
            bodyElement.AppendChild(verificaSolicitudDescargaElement);

            XmlElement solicitudElement = xmlDocument.CreateElement(CfdiDescargaMasivaNamespaces.DesPrefix,
                "solicitud",
                CfdiDescargaMasivaNamespaces.DesNamespaceUrl);
            solicitudElement.SetAttribute("IdSolicitud", verificacionRequest.RequestId);
            solicitudElement.SetAttribute("RfcSolicitante", verificacionRequest.RequestingRfc);

            XmlElement signatureElement = SignedXmlHelper.SignRequest(solicitudElement, certificate);
            solicitudElement.AppendChild(signatureElement);
            verificaSolicitudDescargaElement.AppendChild(solicitudElement);

            return xmlDocument.OuterXml;
        }

        public async Task<SoapRequestResult> SendSoapRequestAsync(string soapRequestContent,
                                                                  AccessToken accessToken,
                                                                  CancellationToken cancellationToken = default)
        {
            return await _httpSoapClient.SendRequestAsync(CfdiDescargaMasivaWebServiceUrls.VerificacionUrl,
                CfdiDescargaMasivaWebServiceUrls.VerificacionSoapActionUrl,
                accessToken,
                soapRequestContent,
                cancellationToken);
        }

        public async Task<VerificacionResult> SendSoapRequestAsync(VerificacionRequest verificacionRequest,
                                                                   X509Certificate2 certificate,
                                                                   CancellationToken cancellationToken = default)
        {
            string soapRequestContent = GenerateSoapRequestEnvelopeXmlContent(verificacionRequest, certificate);

            SoapRequestResult soapRequestResult = await _httpSoapClient.SendRequestAsync(CfdiDescargaMasivaWebServiceUrls.VerificacionUrl,
                CfdiDescargaMasivaWebServiceUrls.VerificacionSoapActionUrl,
                verificacionRequest.AccessToken,
                soapRequestContent,
                cancellationToken);

            return GetSoapResponseResult(soapRequestResult);
        }

        public VerificacionResult GetSoapResponseResult(SoapRequestResult soapRequestResult)
        {
            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(soapRequestResult.ResponseContent);

            XmlNode verificaSolicitudDescargaResultElement = xmlDocument.GetElementsByTagName("VerificaSolicitudDescargaResult")[0];
            if (verificaSolicitudDescargaResultElement is null)
            {
                throw new InvalidResponseContentException("Element VerificaSolicitudDescargaResult is missing in response.",
                    soapRequestResult.ResponseContent);
            }

            if (verificaSolicitudDescargaResultElement.Attributes is null)
            {
                throw new InvalidResponseContentException("Attributes property of Element VerificaSolicitudDescargaResult is null.",
                    soapRequestResult.ResponseContent);
            }

            string downloadRequestStatusNumber = verificaSolicitudDescargaResultElement.Attributes.GetNamedItem("EstadoSolicitud")?.Value ??
                                                 string.Empty;
            string downloadRequestStatusCode =
                verificaSolicitudDescargaResultElement.Attributes.GetNamedItem("CodigoEstadoSolicitud")?.Value ?? string.Empty;
            string numberOfCfdis = verificaSolicitudDescargaResultElement.Attributes.GetNamedItem("NumeroCFDIs")?.Value ?? string.Empty;
            string requestStatusCode = verificaSolicitudDescargaResultElement.Attributes.GetNamedItem("CodEstatus")?.Value ?? string.Empty;
            string requestStatusMessage = verificaSolicitudDescargaResultElement.Attributes.GetNamedItem("Mensaje")?.Value ?? string.Empty;

            var packageIdsList = new List<string>();

            if (downloadRequestStatusNumber == "3")
            {
                XmlNodeList idsPaquetesElements = xmlDocument.GetElementsByTagName("IdsPaquetes");

                foreach (XmlNode idPaqueteElement in idsPaquetesElements)
                {
                    packageIdsList.Add(idPaqueteElement.InnerText);
                }
            }

            return VerificacionResult.CreateInstance(packageIdsList,
                downloadRequestStatusNumber,
                downloadRequestStatusCode,
                numberOfCfdis,
                requestStatusCode,
                requestStatusMessage,
                soapRequestResult.HttpStatusCode,
                soapRequestResult.ResponseContent);
        }
    }
}
