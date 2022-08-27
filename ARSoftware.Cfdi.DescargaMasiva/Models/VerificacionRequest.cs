namespace ARSoftware.Cfdi.DescargaMasiva.Models
{
    public class VerificacionRequest
    {
        private VerificacionRequest()
        {
        }

        public string RequestId { get; private set; }
        public string RequestingRfc { get; private set; }
        public AccessToken AccessToken { get; private set; }

        public static VerificacionRequest CreateInstance(string requestId, string requestingRfc, AccessToken accessToken)
        {
            return new VerificacionRequest { RequestId = requestId, RequestingRfc = requestingRfc, AccessToken = accessToken };
        }
    }
}
