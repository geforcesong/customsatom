using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Commerce.Utilities.Web;

namespace ProTemplate.Web.DataCrawler
{
    public class ContainerAdmissionStatusCrawler
    {
        public static string RunParseAdmission(string billNumber)
        {
            if (string.IsNullOrEmpty(billNumber))
                return null;
            try
            {
                string _strRootSite = "http://edi.easipass.com/dataportal/q.do?qn=dp_query_letpas";
                string rootHtmlContent = CrawlerMan.SaveWebPage(_strRootSite);
                if (!string.IsNullOrEmpty(rootHtmlContent))
                {
                    string qid = HtmlParseUtils.GetSubString(rootHtmlContent, "<input name=\"qid\" value=\"", null, "\" type=\"hidden\" />", null, null, null);
                    if (!string.IsNullOrEmpty(qid))
                    {
                        if (!string.IsNullOrEmpty(billNumber))
                        {
                            string htmlContent = CrawlerMan.SaveWebPage(string.Format("http://edi.easipass.com/dataportal/query.do?entryid=&blno={0}&ctnno=&pagesize=100&submit=%E6%89%A7%E8%A1%8C&qid={1}", billNumber, qid));
                            if (!string.IsNullOrEmpty(htmlContent))
                            {
                                return ParseAdmissionDeclarationContainer(billNumber, htmlContent);
                            }
                        }
                    }
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        private static string ParseAdmissionDeclarationContainer(string billNumber, string htmlContent)
        {
            string content = HtmlParseUtils.FormatHtml(htmlContent, false, true);
            if (!string.IsNullOrEmpty(content))
            {
                List<string> trList = HtmlParseUtils.GetSubStrings(content, "<tr height=\"21\">", null, "</tr>", null, null, null);
                if (trList != null)
                {
                    string strStatus = string.Empty;
                    foreach (string s in trList)
                    {
                        List<string> list = HtmlParseUtils.GetSubStrings(s, "<td align=\"left\" bgcolor=\"#[A-Za-z0-9]+\">", null, "</td>", "<td align=\"left\" bgcolor=\"#EEEEEE\">", "<td align=\"left\" bgcolor=\"#E1E1E1\">", "</td>");
                        if (list != null && list.Count == 10)
                        {
                            try
                            {
                                strStatus += list[6] + ",";
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                    }
                    if (strStatus.Contains("未放行") && strStatus.Contains("已放行"))
                    {
                        strStatus = "部分放行";
                    }
                    else
                    {
                        if (strStatus.Contains("未放行"))
                        {
                            strStatus = "未放行";
                        }
                        else
                        {
                            strStatus = "已放行";
                        }
                    }
                    return strStatus.TrimEnd(',');
                }
            }
            return null;
        }
    }
}