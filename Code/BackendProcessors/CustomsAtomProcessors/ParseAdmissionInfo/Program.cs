using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Net;
using System.Net.Cache;
using System.IO;
using ProcessorUtilities;

namespace ParseAdmissionInfo
{
    class Program
    {
        private static readonly SqlConnection conn = new SqlConnection("Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=CustomsAtom;Data Source=localhost");
        private static List<ExportDelcaration> lstExportDeclaration = new List<ExportDelcaration>();

        static void Main(string[] args)
        {
            conn.Open();

            ParseRootPage();
        }

        private static void ParseRootPage()
        {
            try
            {
                string sql = string.Format("select DeclarationNumber from Declaration where (AdmissionStatus not like '%已结关%' OR AdmissionStatus IS NULL) AND BillNumber != '' AND (ReceivedDate > '{0}')", DateTime.Now.AddDays(-1000).ToString("yyyy-MM-dd HH:mm:ss"));

                DataSet ds = new DataSet();
                using (SqlDataAdapter adapter = new SqlDataAdapter(sql, conn))
                {
                    adapter.Fill(ds);
                }
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    string sqlUpdate = "Update StageDeclaration set admission_status = '{1}' where pre_entry_id = '{0}'";

                    SqlCommand cmm = new SqlCommand(string.Format(sqlUpdate, row["DeclarationNumber"].ToString(), GetAdmissionStatus(row["DeclarationNumber"].ToString())), conn);
                    cmm.ExecuteNonQuery();
                }

            }
            catch (Exception ex)
            {
            }
        }

        private static string GetAdmissionStatus(string declarationNumber)
        {
            string status = string.Empty;
            string url = "http://query.customs.gov.cn/HYW2007DataQuery/FormStatusQuery.aspx";
            var postDataTemplte = "__EVENTTARGET=&__EVENTARGUMENT=&__VIEWSTATE=%2FwEPDwUKLTMzOTU1MzU5Ng9kFgICAw9kFgQCDQ8PFgIeEU51bWJlckluZm9ybWF0aW9uBQRacldOFgIeBXN0eWxlBQ9ESVNQTEFZOklOTElORTtkAhUPDxYCHgdWaXNpYmxlaGRkZNXEcAEBlo%2FfDMG7jmbkzTUpPzn4&txtDeclareFormNo={0}&txtVerifyNumber=ZrWN&submitBtn=%E6%9F%A5%E8%AF%A2&__EVENTVALIDATION=%2FwEWBAK6w9%2FLDQK54aSfBwKduNC3CQLc7%2F3ICwQDK%2FUHc1COiN5XKtcMpVIoUe8R";

            string content = PostWebRequest(url, string.Format(postDataTemplte, declarationNumber), Encoding.UTF8);

            content = HtmlParseUtils.FormatHtml(content, false, true);

            if (!string.IsNullOrEmpty(content))
            {
                status = HtmlParseUtils.GetSubString(content, "<span id=\"lblResult\" class=\"color04\">", null, "</span>", "<span id=\"lblResult\" class=\"color04\">", null, "</span>");
            }

            //<span id="lblResult" class="color04">接单环节：已接单</span>

            return status;
        }

        private static string PostWebRequest(string postUrl, string paramData, Encoding dataEncode)
        {
            string ret = string.Empty;
            try
            {
                byte[] byteArray = dataEncode.GetBytes(paramData); //转化
                HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(new Uri(postUrl));
                webReq.Method = "POST";
                webReq.ContentType = "application/x-www-form-urlencoded";
                webReq.Accept = "image/jpeg, image/gif, image/pjpeg, application/x-ms-application, application/xaml+xml, application/x-ms-xbap, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, */*";
                webReq.UserAgent = "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; WOW64; Trident/5.0)";
                //webReq.
                RequestCachePolicy p = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore);
                webReq.CachePolicy = p;

                webReq.Host = "query.customs.gov.cn";
                webReq.Referer = "http://query.customs.gov.cn/HYW2007DataQuery/FormStatusQuery.aspx";
                webReq.ContentLength = byteArray.Length;
                webReq.KeepAlive = true;
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
    internal class ExportDelcaration
    {
        public string DeclarationNumber;
        public string AdmissionStatus;
    }
}
