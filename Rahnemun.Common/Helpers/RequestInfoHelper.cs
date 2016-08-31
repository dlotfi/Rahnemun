using System;
using System.Collections.Specialized;
using System.Linq;

namespace Rahnemun.Common
{
    public static class RequestInfoHelper
    {
        public static string GetUserAgent(NameValueCollection requestData)
        {
            return requestData["HTTP_USER_AGENT"];
        }

        public static string GetUserIP(NameValueCollection requestData)
        {
            var ip = !String.IsNullOrEmpty(requestData["HTTP_X_FORWARDED_FOR"])
                ? requestData["HTTP_X_FORWARDED_FOR"]
                : requestData["REMOTE_ADDR"];
            if (ip.Contains(","))
                ip = ip.Split(',').First().Trim();
            return ip;
        }
    }
}