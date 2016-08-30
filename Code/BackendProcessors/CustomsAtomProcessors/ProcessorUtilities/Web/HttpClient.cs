#region Using Directives

using System;
using System.Net;
using System.Text;

#endregion

namespace Microsoft.Commerce.Utilities.Web
{
    public class HttpClient : WebClient
    {
        // Cookie 容器
        private CookieContainer _cookieContainer;

        /// <summary>
        /// 创建一个新的 WebClient 实例。
        /// </summary>
        public HttpClient()
        {
            _cookieContainer = new CookieContainer();
        }

        /// <summary>
        /// 创建一个新的 WebClient 实例。
        /// </summary>
        /// <param name="cookies">Cookie 容器</param>
        public HttpClient(CookieContainer cookies)
        {
            _cookieContainer = cookies;
        }

        /// <summary>
        /// Cookie 容器
        /// </summary>
        public CookieContainer Cookies
        {
            get
            {
                return _cookieContainer;
            }
            set
            {
                _cookieContainer = value;
            }
        }

        /// <summary>
        /// 返回带有 Cookie 的 HttpWebRequest。
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        protected override WebRequest GetWebRequest(Uri address)
        {
            WebRequest request = base.GetWebRequest(address);
            if (request is HttpWebRequest)
            {
                var httpRequest = request as HttpWebRequest;
                httpRequest.CookieContainer = _cookieContainer;
            }
            return request;
        }

        /// <summary>
        /// 向指定的 URL POST 数据，并返回页面
        /// </summary>
        /// <param name="uriString">POST URL</param>
        /// <param name="postString">POST 的 数据</param>
        /// <param name="msg"></param>
        /// <returns>页面的源文件</returns>
        public string PostData(string uriString, string postString, out string msg)
        {
            try
            {
                byte[] postData = Encoding.GetBytes(postString);
                Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                byte[] responseData = UploadData(uriString, "POST", postData);
                string srcString = Encoding.GetString(responseData);
                srcString = srcString.Replace("\t", "");
                srcString = srcString.Replace("\r", "");
                srcString = srcString.Replace("\n", "");
                msg = string.Empty;
                return srcString;
            }
            catch (WebException we)
            {
                msg = we.Message;
                return string.Empty;
            }
        }

        /// <summary>
        /// 获得指定 URL 的源文件
        /// </summary>
        /// <param name="uriString">页面 URL</param>
        /// <param name="msg"></param>
        /// <returns>页面的源文件</returns>
        public string GetSrc(string uriString, out string msg)
        {
            try
            {
                byte[] responseData = DownloadData(uriString);
                string srcString = Encoding.GetString(responseData);
                srcString = srcString.Replace("\t", "");
                srcString = srcString.Replace("\r", "");
                srcString = srcString.Replace("\n", "");
                msg = string.Empty;
                return srcString;
            }
            catch (WebException we)
            {
                msg = we.Message;
                return string.Empty;
            }
        }

        /// <summary>
        /// 从指定的 URL 下载文件到本地
        /// </summary>
        /// <param name="urlString">文件 URL</param>
        /// <param name="fileName">本地文件的完成路径</param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool GetFile(string urlString, string fileName, out string msg)
        {
            try
            {
                DownloadFile(urlString, fileName);
                msg = string.Empty;
                return true;
            }
            catch (WebException we)
            {
                msg = we.Message;
                return false;
            }
        }
    }
}