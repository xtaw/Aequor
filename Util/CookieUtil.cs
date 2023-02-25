using CefSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aequor.Util
{

    internal class CookieUtil
    {

        private CookieUtil() { }

        public static List<Cookie> GetCookiesByDomain(string domain)
        {
            List<Cookie> result = new List<Cookie>();
            foreach (Cookie cookie in Cef.GetGlobalCookieManager().VisitAllCookiesAsync().Result)
            {
                if (cookie.Domain.Equals(domain))
                {
                    result.Add(cookie);
                }
            }
            return result;
        }

    }

}
