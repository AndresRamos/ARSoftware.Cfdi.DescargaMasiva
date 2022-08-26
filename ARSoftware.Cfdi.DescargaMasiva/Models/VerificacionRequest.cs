namespace ARSoftware.Cfdi.DescargaMasiva.Models
{
    public class VerificacionRequest
    {
        private VerificacionRequest()
        {
        }

        public string RequestId { get; private set; }
        public string RequestingRfc { get; private set; }
        public string Token { get; private set; }

        public static VerificacionRequest CreateInstance(string requestId, string requestingRfc, string token)
        {
            return new VerificacionRequest { RequestId = requestId, RequestingRfc = requestingRfc, Token = token };
        }
    }
}
