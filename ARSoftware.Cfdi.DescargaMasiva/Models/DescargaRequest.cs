namespace ARSoftware.Cfdi.DescargaMasiva.Models
{
    public class DescargaRequest
    {
        private DescargaRequest()
        {
        }

        public string PackageId { get; private set; }
        public string RequestingRfc { get; private set; }
        public AccessToken AccessToken { get; private set; }

        public static DescargaRequest CreateInstace(string packageId, string requestingRfc, AccessToken accessToken)
        {
            return new DescargaRequest { PackageId = packageId, RequestingRfc = requestingRfc, AccessToken = accessToken };
        }
    }
}
