using System;
using System.Collections.Generic;
using System.Linq;
using ARSoftware.Cfdi.DescargaMasiva.Enumerations;

namespace ARSoftware.Cfdi.DescargaMasiva.Models
{
    public class SolicitudRequest
    {
        private SolicitudRequest()
        {
        }

        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }
        public TipoSolicitud RequestType { get; private set; }
        public string SenderRfc { get; private set; }
        public IEnumerable<string> RecipientsRfcs { get; private set; }
        public string RequestingRfc { get; private set; }
        public TipoComprobante DocumentType { get; private set; }
        public EstadoComprobante DocumentStatus { get; private set; }
        public string ThirdPartyRfc { get; private set; }
        public string Complement { get; private set; }
        public string Uuid { get; private set; }
        public AccessToken AccessToken { get; private set; }

        public bool HasDocumentType => DocumentType != null;
        public bool HasDocumentStatus => DocumentStatus != null;
        public bool HasComplement => !string.IsNullOrWhiteSpace(Complement);
        public bool HasUuid => !string.IsNullOrWhiteSpace(Uuid);

        public static SolicitudRequest CreateInstance(DateTime startDate,
                                                      DateTime endDete,
                                                      TipoSolicitud requestType,
                                                      string senderRfc,
                                                      IEnumerable<string> recipientsRfcs,
                                                      string requestingRfc,
                                                      AccessToken accessToken,
                                                      TipoComprobante documentType = null,
                                                      EstadoComprobante documentStatus = null,
                                                      string thirdPartyRfc = "",
                                                      string complement = "",
                                                      string uuid = "")
        {
            List<string> recipients = recipientsRfcs.ToList();
            if (recipients.Any() && recipients.Count > 5)
            {
                throw new ArgumentOutOfRangeException(nameof(RecipientsRfcs), "There can only be a maximum of 5 recipient RFC.");
            }

            return new SolicitudRequest
            {
                StartDate = startDate,
                EndDate = endDete,
                RequestType = requestType,
                SenderRfc = senderRfc,
                RecipientsRfcs = recipients,
                RequestingRfc = requestingRfc,
                AccessToken = accessToken,
                DocumentType = documentType,
                DocumentStatus = documentStatus,
                ThirdPartyRfc = thirdPartyRfc,
                Complement = complement,
                Uuid = uuid
            };
        }
    }
}
