namespace ARSoftware.Cfdi.DescargaMasiva.Models
{
    public class AutenticacionResult
    {
        private AutenticacionResult()
        {
        }

        public AccessToken AccessToken { get; private set; }
        public string FaultCode { get; private set; }
        public string FaultString { get; private set; }
        public string ResponseContent { get; private set; }

        public static AutenticacionResult CreateInstance(AccessToken accessToken,
                                                         string faultCode,
                                                         string faultString,
                                                         string responseContent)
        {
            return new AutenticacionResult
            {
                AccessToken = accessToken, FaultCode = faultCode, FaultString = faultString, ResponseContent = responseContent
            };
        }

        public static AutenticacionResult CreateSuccess(AccessToken accessToken, string responseContent)
        {
            return CreateInstance(accessToken, string.Empty, string.Empty, responseContent);
        }

        public static AutenticacionResult CreateFailure(string faultCode, string faultString, string responseContent)
        {
            return CreateInstance(AccessToken.CreateEmpty(), faultCode, faultString, responseContent);
        }
    }
}
