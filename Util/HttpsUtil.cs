using CefSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Aequor.Util
{

    internal class HttpsUtil
    {

        private HttpsUtil() { }

        public static byte[] SendGet(string url, List<CefSharp.Cookie>? cookies = null, params string[] headers)
        {
            try
            {
                HttpClientHandler handler = new()
                {
                    AutomaticDecompression = DecompressionMethods.All
                };
                if (cookies != null)
                {
                    var cookieContainer = new CookieContainer();
                    foreach (CefSharp.Cookie cookie in cookies)
                    {
                        cookieContainer.Add(new System.Net.Cookie(cookie.Name, cookie.Value, cookie.Path, cookie.Domain));
                    }
                    handler.CookieContainer = cookieContainer;
                }
                using HttpClient client = new(handler);
                client.DefaultRequestHeaders.Accept.ParseAdd("application/json");
                client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/110.0.0.0 Safari/537.36 Edg/110.0.1587.56");
                for (int i = 0; i < headers.Length; i += 2)
                {
                    client.DefaultRequestHeaders.Add(headers[i], headers[i + 1]);
                }
                return client.GetAsync(url).Result.Content.ReadAsByteArrayAsync().Result;
            }
            catch
            {
                return Array.Empty<byte>();
            }
        }

        public static string? SendGetString(string url, List<CefSharp.Cookie>? cookies = null, params string[] headers)
        {
            return Encoding.UTF8.GetString(SendGet(url, cookies, headers));
        }

    }

}
