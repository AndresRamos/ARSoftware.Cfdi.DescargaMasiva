using System;

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
        public string RecipientRfc { get; private set; }
        public string RequestingRfc { get; private set; }

        public static SolicitudRequest CreateInstance(DateTime startDate, DateTime endDete, TipoSolicitud requestType, string senderRfc, string recipientRfc, string requestingRfc)
        {
            return new SolicitudRequest
            {
                StartDate = startDate,
                EndDate = endDete,
                RequestType = requestType,
                SenderRfc = senderRfc,
                RecipientRfc = recipientRfc,
                RequestingRfc = requestingRfc
            };
        }
    }
}