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
        public string WebResponse { get; private set; }

        public static AutenticacionResult CreateInstance(string token, string faultCode, string faultString, string webResponse)
        {
            return new AutenticacionResult
            {
                Token = token,
                FaultCode = faultCode,
                FaultString = faultString,
                WebResponse = webResponse
            };
        }
    }
}