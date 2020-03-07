using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Xml;
using ARSoftware.Cfdi.DescargaMasiva.Constants;
using ARSoftware.Cfdi.DescargaMasiva.Helpers;
using ARSoftware.Cfdi.DescargaMasiva.Models;

namespace ARSoftware.Cfdi.DescargaMasiva.Services
{
    public class VerificacionService
    {
        public readonly string SoapActionUrl;
        public readonly string Url;

        public VerificacionService(string url, string action)
        {
            Url = url;
            SoapActionUrl = action;
        }

        public string GenerateSoapRequestEnvelopeXmlContent(VerificacionRequest verificacionRequest, X509Certificate2 certificate)
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

            var verificaSolicitudDescargaElement = xmlDocument.CreateElement(CfdiDescargaMasivaNamespaces.DesPrefix, "VerificaSolicitudDescarga", CfdiDescargaMasivaNamespaces.DesNamespaceUrl);
            bodyElement.AppendChild(verificaSolicitudDescargaElement);

            var solicitudElement = xmlDocument.CreateElement(CfdiDescargaMasivaNamespaces.DesPrefix, "solicitud", CfdiDescargaMasivaNamespaces.DesNamespaceUrl);
            solicitudElement.SetAttribute("IdSolicitud", verificacionRequest.RequestId);
            solicitudElement.SetAttribute("RfcSolicitante", verificacionRequest.RequestingRfc);

            var signatureElement = SignedXmlHelper.SignRequest(solicitudElement, certificate);
            solicitudElement.AppendChild(signatureElement);
            verificaSolicitudDescargaElement.AppendChild(solicitudElement);

            return xmlDocument.OuterXml;
        }

        public VerificacionResult GetSoapResponseResult(SoapRequestResult soapRequestResult)
        {
            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(soapRequestResult.WebResponse);

            if (xmlDocument.GetElementsByTagName("VerificaSolicitudDescargaResult").Count > 0)
            {
                var verificaSolicitudDescargaResultElement = xmlDocument.GetElementsByTagName("VerificaSolicitudDescargaResult")[0];
                var codigoEstadoSolicitud = verificaSolicitudDescargaResultElement.Attributes.GetNamedItem("CodigoEstadoSolicitud")?.Value;
                var estadoSolicitud = verificaSolicitudDescargaResultElement.Attributes.GetNamedItem("EstadoSolicitud")?.Value;
                var codEstatus = verificaSolicitudDescargaResultElement.Attributes.GetNamedItem("CodEstatus")?.Value;
                var numeroCfdis = verificaSolicitudDescargaResultElement.Attributes.GetNamedItem("NumeroCFDIs")?.Value;
                var mensaje = verificaSolicitudDescargaResultElement.Attributes.GetNamedItem("Mensaje")?.Value;

                var idsPaquetesList = new List<string>();

                if (estadoSolicitud == "3")
                {
                    var idsPaquetesElements = xmlDocument.GetElementsByTagName("IdsPaquetes");

                    foreach (XmlNode idPaqueteElement in idsPaquetesElements)
                    {
                        idsPaquetesList.Add(idPaqueteElement.InnerText);
                    }
                }

                return VerificacionResult.CreateInstance(codEstatus, codigoEstadoSolicitud, estadoSolicitud, numeroCfdis, mensaje, idsPaquetesList, soapRequestResult.WebResponse);
            }

            throw new ArgumentNullException("El resultado no contiene el nodo VerificaSolicitudDescargaResult");
        }

        public VerificacionResult SendSoapRequest(string soapRequestContent, string authorizationHttpRequestHeader)
        {
            var httpWebRequestSoapService = new HttpWebRequestSoapService(Url, SoapActionUrl);
            var soapRequestResult = httpWebRequestSoapService.SendSoapRequest(soapRequestContent, authorizationHttpRequestHeader);
            return GetSoapResponseResult(soapRequestResult);
        }
    }
}