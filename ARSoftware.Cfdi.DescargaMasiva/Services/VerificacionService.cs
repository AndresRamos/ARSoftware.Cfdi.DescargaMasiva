using System;
using System.Collections.Generic;
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

        public async Task<VerificacionResult> SendSoapRequestAsync(string soapRequestContent,
                                                                   string authorizationHttpRequestHeader,
                                                                   CancellationToken cancellationToken)
        {
            SoapRequestResult soapRequestResult = await _httpSoapClient.SendRequestAsync(CfdiDescargaMasivaWebServiceUrls.VerificacionUrl,
                CfdiDescargaMasivaWebServiceUrls.VerificacionSoapActionUrl,
                soapRequestContent,
                authorizationHttpRequestHeader,
                cancellationToken);

            return GetSoapResponseResult(soapRequestResult);
        }

        public VerificacionResult GetSoapResponseResult(SoapRequestResult soapRequestResult)
        {
            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(soapRequestResult.ResponseContent);

            if (xmlDocument.GetElementsByTagName("VerificaSolicitudDescargaResult").Count <= 0)
            {
                throw new ArgumentException("El resultado no estan en un formato valido.", nameof(soapRequestResult.ResponseContent));
            }

            XmlNode verificaSolicitudDescargaResultElement = xmlDocument.GetElementsByTagName("VerificaSolicitudDescargaResult")[0];
            string codigoEstadoSolicitud = verificaSolicitudDescargaResultElement.Attributes.GetNamedItem("CodigoEstadoSolicitud")?.Value;
            string estadoSolicitud = verificaSolicitudDescargaResultElement.Attributes.GetNamedItem("EstadoSolicitud")?.Value;
            string codEstatus = verificaSolicitudDescargaResultElement.Attributes.GetNamedItem("CodEstatus")?.Value;
            string numeroCfdis = verificaSolicitudDescargaResultElement.Attributes.GetNamedItem("NumeroCFDIs")?.Value;
            string mensaje = verificaSolicitudDescargaResultElement.Attributes.GetNamedItem("Mensaje")?.Value;

            var idsPaquetesList = new List<string>();

            if (estadoSolicitud == "3")
            {
                XmlNodeList idsPaquetesElements = xmlDocument.GetElementsByTagName("IdsPaquetes");

                foreach (XmlNode idPaqueteElement in idsPaquetesElements)
                {
                    idsPaquetesList.Add(idPaqueteElement.InnerText);
                }
            }

            return VerificacionResult.CreateInstance(codEstatus,
                codigoEstadoSolicitud,
                estadoSolicitud,
                numeroCfdis,
                mensaje,
                idsPaquetesList,
                soapRequestResult.ResponseContent);
        }
    }
}
