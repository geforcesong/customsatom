using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProTemplate.Web.Utility
{
    public class IPMan
    {
        public static string GetClientIP(HttpRequest request)
        {
            string result = request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (null == result || result == String.Empty)
            {
                result = request.ServerVariables["REMOTE_ADDR"];
            }

            if (null == result || result == String.Empty)
            {
                result = request.UserHostAddress;
            }
            return result;
        }
    }
}