namespace ARSoftware.Cfdi.DescargaMasiva.Models
{
    public class AutenticacionResult
    {
        private AutenticacionResult()
        {
        }

        public string Token { get; private set; }
        public string FaultCode { get; private set; }
        public string FaultString { get; private set; }
        public string ResponseContent { get; private set; }

        public static AutenticacionResult CreateInstance(string token, string faultCode, string faultString, string responseContent)
        {
            return new AutenticacionResult
            {
                Token = token, FaultCode = faultCode, FaultString = faultString, ResponseContent = responseContent
            };
        }
    }
}
