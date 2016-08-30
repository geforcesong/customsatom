using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Net;
using System.Text;
using System.Net.Cache;
using HtmlAgilityPack;


namespace ProTemplate.Web.Utility
{
    public class YSWebExtensions
    {
        private static readonly HtmlDocument Doc = new HtmlDocument();
        private static Uri LoginUrl = new Uri("http://www.eport.sh.cn/cas/login");

        public static string GetInputNodeValue(string html, string idOrName)
        {
            string strRet = string.Empty;
            Doc.LoadHtml(html);
            HtmlNode inputNode = Doc.DocumentNode.SelectNodes(string.Format("//input[@name='{0}' or @id='{0}']", idOrName)).FirstOrDefault();
            if (inputNode != null && inputNode.Attributes["value"] != null)
            {
                strRet = inputNode.Attributes["value"].Value;
            }

            return strRet;
        }

        public static HttpWebRequest CreateGetRequest(string requestUrl, string requestContent, RequestCachePolicy cachePolicy, CookieContainer cookieContainer, string refer, string host)
        {
            //Contract.Requires(!string.IsNullOrEmpty(requestUrl));

            //var getRequestObservable = new BehaviorSubject<HttpWebRequest>(null);
            var request = WebRequest.Create(!string.IsNullOrEmpty(requestContent) ? string.Format("{0}?{1}", requestUrl, requestContent) : requestUrl) as HttpWebRequest;
            if (request != null)
            {
                request.Method = "GET";
                //Cache
                if (cachePolicy != null)
                {
                    request.CachePolicy = cachePolicy;
                }
                //Client
                request.Accept = "*/*";
                request.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; Trident/4.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; .NET CLR 1.1.4322; InfoPath.1; .NET CLR 3.0.4506.2152; .NET4.0C; .NET4.0E)";
                //Cookie
                if (cookieContainer != null)
                {
                    request.CookieContainer = cookieContainer;
                }
                //Miscellaneous
                request.Referer = refer ?? string.Empty;
                // Transport
                request.KeepAlive = true;
                //request.Host = host ?? string.Empty;

                //getRequestObservable.OnNext(request);
            }
            return request;
        }

        public static HttpWebRequest CreatePostRequest(string requestUrl, string requestContent, Encoding encoding, RequestCachePolicy cachePolicy, CookieContainer cookieContainer, string refer, string host)
        {
            var request = WebRequest.Create(requestUrl) as HttpWebRequest;
            if (request != null)
            {
                request.Method = "POST";
                //Cache
                if (cachePolicy != null)
                {
                    request.CachePolicy = cachePolicy;
                }
                //request.Headers.Set("Pragma", "no-cache");
                //Client
                request.Accept = "*/*";
                request.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; Trident/4.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; .NET CLR 1.1.4322; InfoPath.1; .NET CLR 3.0.4506.2152; .NET4.0C; .NET4.0E)";
                //Cookie
                if (cookieContainer != null)
                {
                    request.CookieContainer = cookieContainer;
                }
                //Entity
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = requestContent.Length;
                //Miscellaneous
                request.Referer = refer ?? string.Empty;
                // Transport
                request.KeepAlive = true;
                //request.Host = host ?? string.Empty;
            }
            Stream stream = request.GetRequestStream();
            using (stream)
            {
                using (var streamWriter = new StreamWriter(stream, encoding))
                {
                    streamWriter.Write(requestContent);
                }
            }
            return request;
        }

        public static string CreateResponseHtml(HttpWebRequest request, Encoding encoding, CookieContainer cookieContainer)
        {
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;

            response.Cookies = cookieContainer.GetCookies(LoginUrl);
            using (Stream responseStream = response.GetResponseStream())
            {
                if (responseStream == null)
                {
                    return string.Empty;
                }
                using (var streamReader = new StreamReader(responseStream, encoding))
                {
                    return streamReader.ReadToEnd();
                }
            }
        }

        public static System.Drawing.Image CreateResponseImage(HttpWebRequest request, CookieContainer cookieContainer)
        {
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            response.Cookies = cookieContainer.GetCookies(request.RequestUri);
            using (Stream responseStream = response.GetResponseStream())
            {
                if (responseStream == null)
                {
                    return null;
                }
                return System.Drawing.Image.FromStream(responseStream);
            }
        }
        public static string GetResponseHtml(string strUrl, string requestContent, Encoding encoding, CookieContainer cookieContainer)
        {
            HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(strUrl);
            myHttpWebRequest.Accept = "*/*";
            myHttpWebRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; Trident/4.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; .NET CLR 1.1.4322; InfoPath.1; .NET CLR 3.0.4506.2152; .NET4.0C; .NET4.0E)";
            myHttpWebRequest.Method = "POST";
            myHttpWebRequest.ContentType = "application/x-www-form-urlencoded";
            myHttpWebRequest.ContentLength = encoding.GetBytes(requestContent).Length;
            //Miscellaneous
            //myHttpWebRequest.Referer = refer ?? string.Empty;
            // Transport
            myHttpWebRequest.KeepAlive = true;
            //myHttpWebRequest.Host = host ?? string.Empty;
            Stream stream = myHttpWebRequest.GetRequestStream();
            StreamWriter writer = new StreamWriter(stream, encoding);
            writer.Write(requestContent);
            writer.Close();
            stream.Close();
            myHttpWebRequest.CookieContainer = cookieContainer;//*
            //刚才那个CookieContainer已经存有了Cookie,把它附加到HttpWebRequest中则能直接通过验证
            HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();

            myHttpWebResponse.Cookies = cookieContainer.GetCookies(LoginUrl);
            var myResponseStream = myHttpWebResponse.GetResponseStream();
            var myStreamReader = new StreamReader(myResponseStream, Encoding.UTF8);
            string outdata = myStreamReader.ReadToEnd();

            myStreamReader.Close();
            myResponseStream.Close();
            return outdata;
            //再次显示"登录"
            //如果把*行注释调，就显示"没有登录"
        }
    }
}