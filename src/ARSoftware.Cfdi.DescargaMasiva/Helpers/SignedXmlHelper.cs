using System;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Xml;
using ARSoftware.Cfdi.DescargaMasiva.Constants;

namespace ARSoftware.Cfdi.DescargaMasiva.Helpers
{
    public static class SignedXmlHelper
    {
        /// <summary>
        ///     This method is used to sign all requests like solicitud, verificacion and descarga.
        ///     For autenticacion use the other method
        /// </summary>
        public static XmlElement SignRequest(XmlElement xmlElement, X509Certificate2 x509Certificate2)
        {
            SignedXml signedXml = new(xmlElement) { SigningKey = x509Certificate2.GetRSAPrivateKey() };

            if (signedXml.SignedInfo is null)
                throw new InvalidOperationException("SignedInfo cannot be null.");

            signedXml.SignedInfo.SignatureMethod = SignedXml.XmlDsigRSASHA1Url;

            Reference reference = new() { Uri = "", DigestMethod = SignedXml.XmlDsigSHA1Url };
            reference.AddTransform(new XmlDsigEnvelopedSignatureTransform());
            signedXml.AddReference(reference);

            KeyInfoX509Data keyInfoX509Data = new(x509Certificate2);
            keyInfoX509Data.AddIssuerSerial(x509Certificate2.Issuer, x509Certificate2.SerialNumber);

            KeyInfo keyInfo = new();
            keyInfo.AddClause(keyInfoX509Data);
            signedXml.KeyInfo = keyInfo;

            signedXml.ComputeSignature();

            return signedXml.GetXml();
        }

        /// <summary>
        ///     This method is only used to sign the autenticacion service
        /// </summary>
        public static XmlElement SignAuthenticationRequest(XmlElement xmlElement,
            X509Certificate2 x509Certificate2,
            string referenceUri,
            XmlElement securityTokenReferenceElement)
        {
            SignedXmlWithId signedXml = new(xmlElement) { SigningKey = x509Certificate2.GetRSAPrivateKey() };

            if (signedXml.SignedInfo is null)
                throw new InvalidOperationException("SignedInfo cannot be null.");

            signedXml.SignedInfo.SignatureMethod = SignedXml.XmlDsigRSASHA1Url;
            signedXml.SignedInfo.CanonicalizationMethod = SignedXml.XmlDsigExcC14NTransformUrl;

            Reference reference = new() { Uri = referenceUri, DigestMethod = SignedXml.XmlDsigSHA1Url };
            reference.AddTransform(new XmlDsigExcC14NTransform());
            signedXml.AddReference(reference);

            KeyInfo keyInfo = new();
            KeyInfoNode keyInfoNode = new() { Value = securityTokenReferenceElement };
            keyInfo.AddClause(keyInfoNode);
            signedXml.KeyInfo = keyInfo;

            signedXml.ComputeSignature();

            return signedXml.GetXml();
        }

        /// <summary>
        ///     Custom SignedXml class to be able to work with soap security Ids because the original implementation will not find
        ///     them.
        ///     This class is only used in the authenticacion service
        ///     Solution based on
        ///     https://stackoverflow.com/questions/35735341/malformed-reference-element and
        ///     https://stackoverflow.com/questions/5099156/malformed-reference-element-when-adding-a-reference-based-on-an-id-attribute-w
        /// </summary>
        internal sealed class SignedXmlWithId : SignedXml
        {
            public SignedXmlWithId(XmlDocument xml) : base(xml)
            {
            }

            public SignedXmlWithId(XmlElement xmlElement) : base(xmlElement)
            {
            }

            public override XmlElement GetIdElement(XmlDocument doc, string id)
            {
                if (doc is null)
                    throw new ArgumentNullException(nameof(doc), "The XmlDocument cannot be null.");

                // check to see if it's a standard ID reference
                XmlElement idElem = base.GetIdElement(doc, id);

                if (idElem is null)
                {
                    XmlNamespaceManager nsManager = new(doc.NameTable);
                    nsManager.AddNamespace(CfdiDescargaMasivaNamespaces.WsuPrefix, CfdiDescargaMasivaNamespaces.WsuNamespaceUrl);

                    idElem = doc.SelectSingleNode("//*[@wsu:Id=\"" + id + "\"]", nsManager) as XmlElement;
                }

                return idElem;
            }
        }
    }
}
