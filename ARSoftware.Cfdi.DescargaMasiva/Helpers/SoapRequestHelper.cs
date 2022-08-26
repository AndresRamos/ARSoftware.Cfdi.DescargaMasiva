using System;
using System.Web;

namespace ARSoftware.Cfdi.DescargaMasiva.Helpers
{
    public static class SoapRequestHelper
    {
        public const string SoapSecurityTimestampFormat = "yyyy-MM-ddTHH:mm:ss.fffZ";

        public static string ToSoapSecurityTimestampString(this DateTime date)
        {
            return date.ToString(SoapSecurityTimestampFormat);
        }

        public static string ToBinarySecurityTokenId(this Guid uuid)
        {
            return $"uuid-{uuid}-1";
        }

        public static string ToSoapStartDateString(this DateTime date)
        {
            return date.ToString("yyyy-MM-dd") + "T00:00:00";
        }

        public static string ToSoapEndDateString(this DateTime date)
        {
            return date.ToString("yyyy-MM-dd") + "T23:59:59";
        }

        public static string CreateHttpAuthorizationHeaderFromToken(string token)
        {
            return $@"WRAP access_token=""{HttpUtility.UrlDecode(token)}""";
        }
    }
}
