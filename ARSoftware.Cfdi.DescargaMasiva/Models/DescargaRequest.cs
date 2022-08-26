namespace ARSoftware.Cfdi.DescargaMasiva.Models
{
    public class DescargaRequest
    {
        private DescargaRequest()
        {
        }

        public string PackageId { get; private set; }
        public string RequestingRfc { get; private set; }
        public string Token { get; private set; }

        public static DescargaRequest CreateInstace(string packageId, string requestingRfc, string token)
        {
            return new DescargaRequest { PackageId = packageId, RequestingRfc = requestingRfc, Token = token };
        }
    }
}
