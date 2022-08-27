using System.Web;
using ARSoftware.Cfdi.DescargaMasiva.Helpers;

namespace ARSoftware.Cfdi.DescargaMasiva.Models
{
    public class AccessToken
    {
        private AccessToken()
        {
        }

        public string Value { get; private set; }

        public bool IsValid => !string.IsNullOrWhiteSpace(Value);

        public string DecodedValue => HttpUtility.UrlDecode(Value);

        public string HttpAuthorizationHeader => SoapRequestHelper.CreateHttpAuthorizationHeaderFromToken(Value);

        public static AccessToken CreateInstance(string token)
        {
            return new AccessToken { Value = token };
        }

        public static AccessToken CreateEmpty()
        {
            return new AccessToken { Value = string.Empty };
        }
    }
}
