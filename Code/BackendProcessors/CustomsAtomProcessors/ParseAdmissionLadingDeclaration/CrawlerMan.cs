using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using Microsoft.Commerce.Utilities.Web;
using System.Text;
using System.IO;
using ProcessorUtilities;

namespace ProTemplate.Web.DataCrawler
{
    public class CrawlerMan
    {
        private CrawlerMan() { }

        public  static string SaveWebPage(string url)
        {
            try
            {
                var web = new WebClient
                {
                    Encoding = Encoding.UTF8
                };
                return HtmlParseUtils.FormatHtml(web.DownloadString(url), false, true);
            }
            catch (Exception)
            {
                return "";
            }
        }

        public static string PostWebRequest(string postUrl, string paramData, Encoding dataEncode)
        {
            string ret = string.Empty;
            try
            {
                byte[] byteArray = dataEncode.GetBytes(paramData); //转化
                HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(new Uri(postUrl));
                webReq.Method = "POST";
                webReq.ContentType = "application/x-www-form-urlencoded";

                webReq.ContentLength = byteArray.Length;
                webReq.KeepAlive = false;
                Stream newStream = webReq.GetRequestStream();
                newStream.Write(byteArray, 0, byteArray.Length);//写入参数
                newStream.Close();
                HttpWebResponse response = (HttpWebResponse)webReq.GetResponse();
                StreamReader sr = new StreamReader(response.GetResponseStream(), dataEncode);
                ret = sr.ReadToEnd();
                sr.Close();
                response.Close();
                newStream.Close();
            }
            catch (Exception ex)
            {
                ex.HelpLink = postUrl;
                return string.Empty;
            }
            return ret;
        }
    }
}