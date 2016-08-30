using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Text;
using Microsoft.Commerce.Utilities.Web;
using ProcessorUtilities;

namespace ProTemplate.Web.DataCrawler
{
    public class LeaveDockDateCrawler
    {
        public static string FindExportDate(string Conveyance, string VoyageNumber, string strDock)
        {
            string strRet = string.Empty;
            Conveyance = Conveyance == null ? "" : Conveyance;
            VoyageNumber = VoyageNumber == null ? "" : VoyageNumber;
            strDock = strDock == null ? "" : strDock;
            VoyageNumber = VoyageNumber.Length > 6 ? VoyageNumber.Substring(VoyageNumber.Length - 6, 6) : VoyageNumber;
            switch (strDock)
            {
                case "洋山三期":
                    strRet = GetDataFromSite("http://query.sgict.com.cn/query/search/comm/shipname.jsp?ename=", Conveyance,
                                        VoyageNumber,
                                        new DataFormat("<tr class=\"result_tr", "</tr>", "<td>", "</td>", 0, 2, 6));
                    break;
                case "洋山一期":
                    strRet =
                        GetDataFromSite("http://www.shsict.com/as/query/search/comm/shipname.jsp?ename=", Conveyance,
                                        VoyageNumber,
                                        new DataFormat("<tr bgcolor=\"#E6EDFD\">",
                                                       "</tr>", "<td height=\"35\">", "</td>", 0, 2, 6));
                    break;
                case "外一期":
                    strRet = GetWaiYi(Conveyance, VoyageNumber);
                    //GetDataFromSite("http://www.spict.com/port/cntr/vsl.cfm", Conveyance,
                    //                VoyageNumber,
                    //                new DataFormat("<tr bgcolor=",
                    //                               "</tr>", "<td>", "</td>", 1, 3, 7));
                    break;
                case "外二期":
                    strRet =
                            GetDataFromSite("http://www.sipgzct.com/wat/controllerServlet.do?querysid=g0001&queryid=0001004&method=doquery&shipname=", Conveyance,
                                        VoyageNumber,
                                        new DataFormat("<tr class=\"result_tr", "</tr>", "<td>", "</td>", 0, 2, 6));
                    break;
                case "外四期":
                    strRet =
                            GetDataFromSite("http://www.sect.com.cn/wat/controllerServlet.do?querysid=g0001&queryid=0001004&method=doquery&shipname=", Conveyance,
                                        VoyageNumber,
                                        new DataFormat("<tr class=\"result_tr", "</tr>", "<td>", "</td>", 0, 2, 6));
                    break;
                case "外五期":
                    strRet =
                            GetDataFromSite("http://www.smct.com.cn/wat/controllerServlet.do?querysid=g0001&queryid=0001004&method=doquery&shipname=", Conveyance,
                                        VoyageNumber,
                                        new DataFormat("<tr class=\"result", "</tr>", "<td align=\"center\" class=\"x2\">", "</td>", 0, 2, 6));
                    break;
                default:
                    //strRet =
                    //        GetDataFromSite("http://218.1.104.251/zhbwat/controllerServlet.do?queryid=g0001&queryid=0001004&method=doquery&shipname=", Conveyance,
                    //                    VoyageNumber,
                    //                    new DataFormat("<tr class=\"result", "</tr>", "<td align=\"center\" class=\"x2\">", "</td>", 0, 2, 6));
                
                    break;
            }

            return strRet;
        }

        private static string GetWai4(string Conveyance, string VoyageNumber)
        {
            string strRet = string.Empty;
            string strUrl = "http://www.sect.com.cn/wat/controllerServlet.do?queryid=0001004";
            var headers = new WebHeaderCollection();
            headers.Add("Host", "www.sect.com.cn");
            headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/534.31 (KHTML, like Gecko) Chrome/12.0.745.0 Safari/534.31");
            headers.Add("Content-Length", "47");
            headers.Add("Referer", "http://www.sect.com.cn/wat/controllerServlet.do?method=getsubformsinput&querysid=g0001");
            headers.Add("Accept-Language", "zh-CN");
            headers.Add("Content-Type", "application/x-www-form-urlencode");
            headers.Add("Accept-Encoding", "gzip, deflate");
            headers.Add("Connection", "Keep-Alive");
            headers.Add("Pragma", "no-cache");

            var postDataTemplte = "querysid=g0001&method=doquery&shipname={0}";
            var postData = string.Format(postDataTemplte, Conveyance);

            string content = CrawlerMan.PostWebRequest(strUrl, postData, Encoding.UTF8);

            string tableContent = HtmlParseUtils.GetSubString(content, "<table id=\"ctable\"", "", "</table>", "", "", "");
            List<string> leaveDateList = new List<string>();
            List<string> trContentList = HtmlParseUtils.GetSubStrings(tableContent, "<tr>", "", "</tr>", "", "", "");
            if (trContentList != null)
            {
                foreach (string trContent in trContentList)
                {
                    List<string> tdContentList = HtmlParseUtils.GetSubStrings(trContent, "<td  >", "", "&nbsp;</td>", "<td  >", "", "&nbsp;</td>");
                    if (tdContentList != null && tdContentList.Count == 9)
                    {
                        if (tdContentList[2] == VoyageNumber && tdContentList[3] == "E")
                        {
                            if (!string.IsNullOrEmpty(tdContentList[7]))
                                leaveDateList.Add(tdContentList[7]);
                        }
                    }
                }
            }

            if (leaveDateList.Count > 0)
            {
                var maxData = (from a in leaveDateList
                               orderby a descending
                               select a).First();
                return maxData;
            }

            return "";
        }

        private static string GetWaiYi(string Conveyance, string VoyageNumber)
        {
            string strRet = string.Empty;
            string strUrl = "http://www.spict.com/portinfo/Normal/ShipPlanPage.aspx";
            var headers = new WebHeaderCollection();
            headers.Add("Referer", "http://www.spict.com/");
            headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/534.31 (KHTML, like Gecko) Chrome/12.0.745.0 Safari/534.31");
            headers.Add("Origin", "http://www.spict.com");
            headers.Add("Content-Type", "application/x-www-form-urlencoded");

            var postDataTemplte = "__EVENTTARGET=&__EVENTARGUMENT=&__VIEWSTATE=%2FwEPDwULLTEzNDQ0MzYzODYPZBYCZg9kFgICAw9kFgICAQ88KwAHAQAPFgIeDl8hVXNlVmlld1N0YXRlZ2QWAmYPZBYEZg9kFgJmD2QWAmYPZBYCAgEPZBYKAgEPFgIeA2FsdAUt5LiK5rW35rWm5Lic5Zu96ZmF6ZuG6KOF566x56CB5aS05pyJ6ZmQ5YWs5Y%2B4ZAIDDzwrAAQBAA8WAh8AZ2RkAgUPFCsABA8WAh8AZ2RkZDwrAAQBABYCHgRUZXh0BQzlhazkvJfnlKjmiLdkAgcPFCsABA8WBB4FVmFsdWUFPX4vVXNlckxvZ2luLmFzcHg%2FUmV0dXJuVXJsPS9wb3J0aW5mby9Ob3JtYWwvU2hpcFBsYW5QYWdlLmFzcHgfAGdkZGQ8KwAEAQAWAh8CBQbnmbvlvZVkAgkPPCsABAEADxYCHwBnZGQCAg9kFgJmD2QWAmYPZBYCZg9kFgJmD2QWBGYPZBYCZg9kFgICAQ8UKwAHDxYCHwBnZGRkZGRkD2QQFgJmAgEWAjwrAAsCABYGHwIFDOWFrOWFseafpeivoh4OUnVudGltZUNyZWF0ZWRnHghFeHBhbmRlZGcBD2QQFgpmAgECAgIDAgQCBQIGAgcCCAIJFgoUKwACFggfAgUY6Ii56Ii26K6h5YiS5L%2Bh5oGv5p%2Bl6K%2BiHgtOYXZpZ2F0ZVVybAUafi9Ob3JtYWwvU2hpcFBsYW5QYWdlLmFzcHgfBGceCFNlbGVjdGVkZ2QUKwACFgYfAgUY6L%2Bb566x6K6h5YiS5L%2Bh5oGv5p%2Bl6K%2BiHwYFG34vTm9ybWFsL0NUTkluUGxhblBhZ2UuYXNweB8EZ2QUKwACFgYfAgUS5Zy656uZ5pS25o2u5p%2Bl6K%2BiHwYFIX4vTm9ybWFsL1lhcmRSZWNlaXB0SW5mb1BhZ2UuYXNweB8EZ2QUKwACFgYfAgUS566x6LSn5L%2Bh5oGv5p%2Bl6K%2BiHwYFG34vTm9ybWFsL0NUTkdJbmZvc1BhZ2UuYXNweB8EZ2QUKwACFgYfAgUb5aSW6ZuG5Y2h5Yqo5oCB5L%2Bh5oGv5p%2Bl6K%2BiHwYFG34vTm9ybWFsL091dFlhcmRDTFBhZ2UuYXNweB8EZ2QUKwACFgYfAgUV55S15a2Q6KOF566x5Y2V5p%2Bl6K%2BiHwYFIH4vTm9ybWFsL1BhY2tpbmdMaXN0RURJUGFnZS5hc3B4HwRnZBQrAAIWBh8CBR7mtbflhbPmlL7ooYzmg4XlhrXkv6Hmga%2Fmn6Xor6IfBgUkfi9Ob3JtYWwvQ3VzdG9tc1JlbGVhc2VJbmZvUGFnZS5hc3B4HwRnZBQrAAIWBh8CBQ%2FotoXpmZDnrrHlj5fnkIYfBgUZfi9Ob3JtYWwvREdBcHBseVBhZ2UuYXNweB8EZ2QUKwACFgYfAgUV6LaF6ZmQ566x5Y%2BX55CG5p%2Bl6K%2BiHwYFH34vTm9ybWFsL0RHQXBwbHlTZWFyY2hQYWdlLmFzcHgfBGdkFCsAAhYGHwIFIOe%2BjuWbveiIque6vzQ45bCP5pe25oiq5YWz5L%2Bh5oGvHwYFGX4vTm9ybWFsL1VTQUxpbmVQYWdlLmFzcHgfBGdkZDwrAAsBABYEHwIFDOmrmOe6p%2Bafpeivoh8EZ2RkAgIPZBYCZg9kFgJmD2QWAmYPZBYCZg9kFgJmD2QWAgIBD2QWDGYPZBYGAgEPPCsABAEADxYCHwBnZGQCAw88KwAFAQAPFgQfAwYAAJQYC57OCB8AZ2RkAgUPPCsABQEADxYEHwMGAIBcagupzggfAGdkZAIBDzwrAAcBAA8WAh8AZ2QWAmYPZBYCZg9kFgICAQ9kFgJmD2QWAmYPZBYCZg9kFgRmD2QWAmYPZBYKAgEPPCsABAEADxYCHwBnZGQCAw8UKwAFDxYEHg9EYXRhU291cmNlQm91bmRnHwBnZGRkPCsACQEIFCsABBYEHhJFbmFibGVDYWxsYmFja01vZGVoHidFbmFibGVTeW5jaHJvbml6YXRpb25PblBlcmZvcm1DYWxsYmFjayBoZA8WAh4KSXNTYXZlZEFsbGcPFCsACxQrAAEWCB8CBQbpqbPoiLkfAwUDQkFSHghJbWFnZVVybGUfBGcUKwABFggfAgUJ5pWj6LSn6Ii5HwMFA0JVTB8MZR8EZxQrAAEWCB8CBQnov5DovaboiLkfAwUDQ0FSHwxlHwRnFCsAARYIHwIFCeadgui0p%2BiIuR8DBQNDQVMfDGUfBGcUKwABFggfAgUP5YWo6ZuG6KOF566x6Ii5HwMFA0ZDUx8MZR8EZxQrAAEWCB8CBSfml6DoiLnmnLrvvIzmnInmoLzmp73kvY3kuYvpm4boo4XnrrHoiLkfAwUDTlNDHwxlHwRnFCsAARYIHwIFBuayueiIuR8DBQNPSUwfDGUfBGcUKwABFggfAgUJ5rua6KOF6Ii5HwMFA1JPUh8MZR8EZxQrAAEWCB8CBQ%2FljYrpm4boo4XnrrHoiLkfAwUDU0VDHwxlHwRnFCsAARYIHwIFBua4uOiIuR8DBQNTRUUfDGUfBGcUKwABFggfAgUn5pyJ6Ii55py677yM5pyJ5qC85qe95L2N5LmL6ZuG6KOF566x6Ii5HwMFA1NTQx8MZR8EZ2RkZGQCBw88KwAEAQAPFgIfAGdkZAIJDxQrAAUPFgQfCGcfAGdkZGQ8KwAJAQgUKwAEFgQfCWgfCmhkDxYCHwtnDxQrAAQUKwABFggfAgUG5YWo6YOoHwMFATQfDGUfBGcUKwABFggfAgUG5pyq6Z2gHwMFATEfDGUfBGcUKwABFggfAgUG6Z2g5rOKHwMFATIfDGUfBGcUKwABFggfAgUG56a75rOKHwMFATMfDGUfBGdkZGRkAg0PPCsABgEADxYCHwBnZGQCAQ9kFgJmD2QWBgIBDzwrAAQBAA8WAh8AZ2RkAgMPFCsABQ8WBB8IZx8AZ2RkZDwrAAkBCBQrAAQWBB8JaB8KaGRkD2QQFgFmFgEUKwABFgIeD0NvbFZpc2libGVJbmRleGZkZGQCBw88KwAGAQAPFgIfAGdkZAICDzwrABYEAA8WBB8AZx8IZ2QGD2QQFg9mAgECAgIDAgQCBQIGAgcCCAIJAgoCCwIMAg0CDhYPPCsACwEAFgYfDWYeBVdpZHRoGwAAAAAAAF5AAQAAAB4KQ29sVmlzaWJsZWc8KwALAQAWBh8NAgEfDhsAAAAAAABeQAEAAAAfD2c8KwALAQAWBh8NAgIfDhsAAAAAAABeQAEAAAAfD2c8KwALAQAWBh8NAgMfDhsAAAAAAABeQAEAAAAfD2c8KwALAQAWBh8NAgQfDhsAAAAAAABeQAEAAAAfD2c8KwALAQAWBh8OGwAAAAAAAF5AAQAAAB8NAgYfD2c8KwALAQAWBh8OGwAAAAAAAF5AAQAAAB8NAgcfD2c8KwALAQAWBh8OGwAAAAAAAF5AAQAAAB8NAgUfD2c8KwALAQAWBh8OGwAAAAAAAF5AAQAAAB8NAggfD2c8KwALAQAWBh8PZx8NAgkfDhsAAAAAAABeQAEAAAA8KwALAQAWBh8OGwAAAAAAAF5AAQAAAB8NAgofD2c8KwALAQAWBh8OGwAAAAAAAF5AAQAAAB8NAgsfD2c8KwALAQAWBh8NAgwfDhsAAAAAAABeQAEAAAAfD2c8KwALAQAWBh8NAg4fDhsAAAAAAABeQAEAAAAfD2g8KwALAQAWBh8NAg0eB0NhcHRpb24FASMfD2gPFg8CAQIBAgECAQIBAgICAgICAgICAQICAgICAQIBAgEWAgWZAURldkV4cHJlc3MuV2ViLkFTUHhHcmlkVmlldy5HcmlkVmlld0RhdGFUZXh0Q29sdW1uLCBEZXZFeHByZXNzLldlYi5BU1B4R3JpZFZpZXcudjEwLjEsIFZlcnNpb249MTAuMS41LjAsIEN1bHR1cmU9bmV1dHJhbCwgUHVibGljS2V5VG9rZW49Yjg4ZDE3NTRkNzAwZTQ5YQWdAURldkV4cHJlc3MuV2ViLkFTUHhHcmlkVmlldy5HcmlkVmlld0RhdGFUaW1lRWRpdENvbHVtbiwgRGV2RXhwcmVzcy5XZWIuQVNQeEdyaWRWaWV3LnYxMC4xLCBWZXJzaW9uPTEwLjEuNS4wLCBDdWx0dXJlPW5ldXRyYWwsIFB1YmxpY0tleVRva2VuPWI4OGQxNzU0ZDcwMGU0OWEKPCsABwEAFgIeCFBhZ2VTaXplAhQMFCsAARYEHhBDb2x1bW5SZXNpemVNb2RlAgEeD0FsbG93Rm9jdXNlZFJvd2dkAgMPPCsACAEADxYCHwBnZGQCBA8PZA8QFgFmFgEWAh4OUGFyYW1ldGVyVmFsdWVkFgECBWRkAgcPD2QPEBYCZgIBFgIWAh8UBgAAlBgLns4IFgIfFAYAgFxqC6nOCBYCZmZkZBgBBR5fX0NvbnRyb2xzUmVxdWlyZVBvc3RCYWNrS2V5X18WDgUTY3RsMDAkQVNQeFNwbGl0dGVyMgUfY3RsMDAkQVNQeFNwbGl0dGVyMiRBU1B4TmF2QmFyMQU2Y3RsMDAkQVNQeFNwbGl0dGVyMiRDb250ZW50UGxhY2VIb2xkZXIxJGR0U3RhcnRpbmckREREBTxjdGwwMCRBU1B4U3BsaXR0ZXIyJENvbnRlbnRQbGFjZUhvbGRlcjEkZHRTdGFydGluZyREREQkQyRGTlAFNmN0bDAwJEFTUHhTcGxpdHRlcjIkQ29udGVudFBsYWNlSG9sZGVyMSRkdEV4cGlyaW5nJERERAU8Y3RsMDAkQVNQeFNwbGl0dGVyMiRDb250ZW50UGxhY2VIb2xkZXIxJGR0RXhwaXJpbmckREREJEMkRk5QBTNjdGwwMCRBU1B4U3BsaXR0ZXIyJENvbnRlbnRQbGFjZUhvbGRlcjEkU2VhcmNoUGFnZXMFQmN0bDAwJEFTUHhTcGxpdHRlcjIkQ29udGVudFBsYWNlSG9sZGVyMSRTZWFyY2hQYWdlcyRjYlNoaXBGb3JtJERERAVMY3RsMDAkQVNQeFNwbGl0dGVyMiRDb250ZW50UGxhY2VIb2xkZXIxJFNlYXJjaFBhZ2VzJGNiU2hpcEJlcnRoaW5nU3RhdHVzJERERAU9Y3RsMDAkQVNQeFNwbGl0dGVyMiRDb250ZW50UGxhY2VIb2xkZXIxJFNlYXJjaFBhZ2VzJGJ0blNlYXJjaAVAY3RsMDAkQVNQeFNwbGl0dGVyMiRDb250ZW50UGxhY2VIb2xkZXIxJFNlYXJjaFBhZ2VzJGNiVmVzc2VsJERERAU%2BY3RsMDAkQVNQeFNwbGl0dGVyMiRDb250ZW50UGxhY2VIb2xkZXIxJFNlYXJjaFBhZ2VzJGJ0blNlYXJjaDIFMGN0bDAwJEFTUHhTcGxpdHRlcjIkQ29udGVudFBsYWNlSG9sZGVyMSRncmlkVmlldwUxY3RsMDAkQVNQeFNwbGl0dGVyMiRDb250ZW50UGxhY2VIb2xkZXIxJHBvcHVwSW5mb6OM%2BVi7gXkagyJscSlWWPYNe%2B3L&__EVENTVALIDATION=%2FwEWBQLnz8uUCQKexuKCBQLs4NumAwLXq87iCQK8g52dAsCct2EJrd7Yb2I5nDP0%2BYFnQORj&ctl00_ASPxSplitter2_CS=%7B%22i%22%3A%5B%7B%22s%22%3A88%2C%22st%22%3A%22px%22%2C%22c%22%3Afalse%7D%2C%7B%22s%22%3A100%2C%22st%22%3A%22%25%22%2C%22c%22%3Afalse%2C%22i%22%3A%5B%7B%22s%22%3A218%2C%22st%22%3A%22px%22%2C%22c%22%3Afalse%7D%2C%7B%22s%22%3A100%2C%22st%22%3A%22%25%22%2C%22c%22%3Afalse%2C%22i%22%3A%5B%7B%22s%22%3A100%2C%22st%22%3A%22%25%22%2C%22c%22%3Afalse%7D%5D%7D%5D%7D%5D%7D&ctl00_ASPxSplitter2_ASPxNavBar1GS=1%3B1&ctl00_ASPxSplitter2_ContentPlaceHolder1_dtStarting_Raw=1326240000000&ctl00%24ASPxSplitter2%24ContentPlaceHolder1%24dtStarting={1}&ctl00_ASPxSplitter2_ContentPlaceHolder1_dtStarting_DDDWS=0%3A0%3A-1%3A-10000%3A-10000%3A0%3A-10000%3A-10000&ctl00_ASPxSplitter2_ContentPlaceHolder1_dtStarting_DDD_C_FNPWS=0%3A0%3A-1%3A-10000%3A-10000%3A0%3A0px%3A-10000&ctl00%24ASPxSplitter2%24ContentPlaceHolder1%24dtStarting%24DDD%24C=01%2F11%2F2012%3A01%2F11%2F2012&ctl00_ASPxSplitter2_ContentPlaceHolder1_dtExpiring_Raw=1327449600000&ctl00%24ASPxSplitter2%24ContentPlaceHolder1%24dtExpiring={2}&ctl00_ASPxSplitter2_ContentPlaceHolder1_dtExpiring_DDDWS=0%3A0%3A-1%3A-10000%3A-10000%3A0%3A-10000%3A-10000&ctl00_ASPxSplitter2_ContentPlaceHolder1_dtExpiring_DDD_C_FNPWS=0%3A0%3A-1%3A-10000%3A-10000%3A0%3A0px%3A-10000&ctl00%24ASPxSplitter2%24ContentPlaceHolder1%24dtExpiring%24DDD%24C=01%2F25%2F2012%3A01%2F25%2F2012&ctl00_ASPxSplitter2_ContentPlaceHolder1_SearchPagesATI=1&ctl00_ASPxSplitter2_ContentPlaceHolder1_SearchPages_cbShipForm_VI=&ctl00%24ASPxSplitter2%24ContentPlaceHolder1%24SearchPages%24cbShipForm=&ctl00_ASPxSplitter2_ContentPlaceHolder1_SearchPages_cbShipForm_DDDWS=0%3A0%3A-1%3A-10000%3A-10000%3A0%3A-10000%3A-10000&ctl00_ASPxSplitter2_ContentPlaceHolder1_SearchPages_cbShipForm_DDD_LDeletedItems=&ctl00_ASPxSplitter2_ContentPlaceHolder1_SearchPages_cbShipForm_DDD_LInsertedItems=&ctl00_ASPxSplitter2_ContentPlaceHolder1_SearchPages_cbShipForm_DDD_LCustomCallback=&ctl00%24ASPxSplitter2%24ContentPlaceHolder1%24SearchPages%24cbShipForm%24DDD%24L=&ctl00%24ASPxSplitter2%24ContentPlaceHolder1%24SearchPages%24cbShipForm%24CVS=&ctl00_ASPxSplitter2_ContentPlaceHolder1_SearchPages_cbShipBerthingStatus_VI=&ctl00%24ASPxSplitter2%24ContentPlaceHolder1%24SearchPages%24cbShipBerthingStatus=&ctl00_ASPxSplitter2_ContentPlaceHolder1_SearchPages_cbShipBerthingStatus_DDDWS=0%3A0%3A-1%3A-10000%3A-10000%3A0%3A-10000%3A-10000&ctl00_ASPxSplitter2_ContentPlaceHolder1_SearchPages_cbShipBerthingStatus_DDD_LDeletedItems=&ctl00_ASPxSplitter2_ContentPlaceHolder1_SearchPages_cbShipBerthingStatus_DDD_LInsertedItems=&ctl00_ASPxSplitter2_ContentPlaceHolder1_SearchPages_cbShipBerthingStatus_DDD_LCustomCallback=&ctl00%24ASPxSplitter2%24ContentPlaceHolder1%24SearchPages%24cbShipBerthingStatus%24DDD%24L=&ctl00%24ASPxSplitter2%24ContentPlaceHolder1%24SearchPages%24cbShipBerthingStatus%24CVS=&ctl00_ASPxSplitter2_ContentPlaceHolder1_SearchPages_cbVessel_VI=HM-ADE&ctl00%24ASPxSplitter2%24ContentPlaceHolder1%24SearchPages%24cbVessel={0}&ctl00_ASPxSplitter2_ContentPlaceHolder1_SearchPages_cbVessel_DDDWS=0%3A0%3A12000%3A276%3A199%3A0%3A-10000%3A-10000&ctl00_ASPxSplitter2_ContentPlaceHolder1_SearchPages_cbVessel_DDD_LDeletedItems=&ctl00_ASPxSplitter2_ContentPlaceHolder1_SearchPages_cbVessel_DDD_LInsertedItems=&ctl00_ASPxSplitter2_ContentPlaceHolder1_SearchPages_cbVessel_DDD_LCustomCallback=&ctl00%24ASPxSplitter2%24ContentPlaceHolder1%24SearchPages%24cbVessel%24DDD%24L=HM-ADE&ctl00%24ASPxSplitter2%24ContentPlaceHolder1%24SearchPages%24cbVessel%24CVS=&ctl00%24ASPxSplitter2%24ContentPlaceHolder1%24SearchPages%24btnSearch2=&ctl00%24ASPxSplitter2%24ContentPlaceHolder1%24gridView%24DXSelInput=&ctl00%24ASPxSplitter2%24ContentPlaceHolder1%24gridView%24DXFocusedRowInput=-1&ctl00%24ASPxSplitter2%24ContentPlaceHolder1%24gridView%24CallbackState=%2FwEWBB4ERGF0YQXQBEFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFFQUFBQUFaV1pYTnpaV3dHNklpNTVaQ05Cd0FBQ0ZabGMzTmxiRVZPRE9pTHNlYVdoK2lJdWVXUWpRY0FBQWRKVm05NVlXZGxET2kvbStXUG8raUlxdWFzb1FjQUFBTkJWRUlNNWE2ZTZabUY2WjJnNXJPS0NBQUFBMFZVUWd6b3JxSGxpSkxwbmFEbXM0b0lBQUFEUlZSRURPaXVvZVdJa3VlbXUrYXppZ2dBQUFOU1ZFUU01YTZlNlptRjU2YTc1ck9LQ0FBQUIwVlVRMVJPU1U0TTZMK2I1NjZ4NWJ5QTVhZUxDQUFBQ0VWRlZFTlVUa2xPRE9pL20rZXVzZWFJcXVhdG9nZ0FBQkJKYm5SbGNtZHlZWFJwYjI1UWIzSjBET2lCbE9XS3FPZWdnZVdrdEFjQUFBaEpjME4xZEU5bVpnbmx0N0xtaUtybGhiTUhBQUFKU1ZadmVXRm5aVWxFRXVpL20rV1BvK2lJcXVhc29lZThsdVdQdHdjQUFBZEZWbTk1WVdkbERPV0h1dVdQbytpSXF1YXNvUWNBQUFsRlZtOTVZV2RsU1VRUzVZZTY1WStqNklpcTVxeWg1N3lXNVkrM0J3QUFDRlpsYzNObGJFTk9ET1M0cmVhV2graUl1ZVdRalFjQUFBdERiMngxYlc1SmJtUmxlQXhEYjJ4MWJXNGdTVzVrWlhnREFBQUNBQUFBQ1VsV2IzbGhaMlZKUkFsRlZtOTVZV2RsU1VRSEFBY0EeBVN0YXRlBZADQnc4SEFBSUJCd0VDQVFjQ0FnRUhBd0lCQndRQ0FRY0dBZ0VIQndJQkJ3VUNBUWNJQWdFSENRSUJCd29DQVFjTEFnRUhEQUlCQnc0Q0FBY05BZ0FIRHdjQUJ3RUNBQUFBQUFBQVhrQUhBUWNCQWdBQUFBQUFBRjVBQndJSEFRSUFBQUFBQUFCZVFBY0RCd0VDQUFBQUFBQUFYa0FIQkFjQkFnQUFBQUFBQUY1QUJ3VUhBUUlBQUFBQUFBQmVRQWNHQndFQ0FBQUFBQUFBWGtBSEJ3Y0JBZ0FBQUFBQUFGNUFCd2dIQVFJQUFBQUFBQUJlUUFjSkJ3RUNBQUFBQUFBQVhrQUhDZ2NCQWdBQUFBQUFBRjVBQndzSEFRSUFBQUFBQUFCZVFBY01Cd0VDQUFBQUFBQUFYa0FIRFFjQkFnQUFBQUFBQUY1QUJ3NEhBUUlBQUFBQUFBQUFBQWNBQndBSEFBSUJCdi8vQ1FJTFEyOXNkVzF1U1c1a1pYZ0pBZ0FDQUFNSEFBSUJCd0FDQVFjQQ%3D%3D&ctl00%24ASPxSplitter2%24ContentPlaceHolder1%24gridView%24DXColResizedInput=&ctl00%24ASPxSplitter2%24ContentPlaceHolder1%24gridView%24DXSyncInput=&ctl00_ASPxSplitter2_ContentPlaceHolder1_popupInfoWS=0%3A0%3A-1%3A-10000%3A-10000%3A0%3A500px%3A400px&DXScript=1_23%2C1_49%2C1_36%2C2_21%2C2_27%2C2_28%2C2_20%2C1_27%2C1_44%2C1_41%2C2_15%2C1_50%2C2_23%2C2_14%2C1_31%2C1_30%2C1_42%2C3_7%2C3_6%2C2_24%2C2_26";

            var postData = string.Format(postDataTemplte, "", DateTime.Today.AddDays(-7).ToString("yyyy-MM-dd"), DateTime.Today.AddDays(7).ToString("yyyy-MM-dd"));


            string content = CrawlerMan.PostWebRequest(strUrl, postData, Encoding.UTF8);

            string postDataParam = string.Empty;
            var paramList = HtmlParseUtils.GetSubStrings(content, "<td class=\"dxeListBoxItem dxeLTM\">" + Conveyance, "", "</td>", "<td class=\"dxeListBoxItem dxeLTM\">", null, "</td>");
            if (paramList != null)
            {
                foreach (string parm in paramList)
                {
                    if (parm.Contains(Conveyance) && parm.Contains(VoyageNumber))
                    {
                        postDataParam = parm;

                        break;
                    }
                }
            }
            if (!string.IsNullOrEmpty(postDataParam))
            {
                var postDataReal = string.Format(postDataTemplte, postDataParam, DateTime.Today.AddDays(-7).ToString("yyyy-MM-dd"), DateTime.Today.AddDays(7).ToString("yyyy-MM-dd"));
                content = CrawlerMan.PostWebRequest(strUrl, postDataReal, Encoding.UTF8);
                var trList = HtmlParseUtils.GetSubStrings(content,
                                                          "<tr id=\"ctl00_ASPxSplitter2_ContentPlaceHolder1_gridView_DXDataRow",
                                                          "", "</tr>",
                                                          "",
                                                          null, "");
                if (trList != null && trList.Count > 0)
                {
                    foreach (var tr in trList)
                    {
                        if (tr.Contains(VoyageNumber))
                        {
                            var tdList = HtmlParseUtils.GetSubStrings(tr,
                                                                      "<td class=\"dxgv\" style=\"white-space:nowrap;\">",
                                                                      "", "</td>",
                                                                      "<td class=\"dxgv\" style=\"white-space:nowrap;\">",
                                                                      null, "</td>");
                            if (tdList != null && tdList.Count > 0)
                            {
                                strRet = tdList[7];
                                break;
                            }
                        }
                    }
                }
            }
            else
            {
                strRet = "";
            }
            return strRet;
        }

        private static string GetDataFromSite(string Site, string Conveyance, string VoyageNumber, DataFormat dataFormat)
        {
            string htmlContent = string.Empty;
            if (dataFormat.ConveyanceColumnNumber == 0)
                htmlContent = CrawlerMan.SaveWebPage(Site + Conveyance);
            else
            {
                try
                {
                    WebClient web = new WebClient();
                    web.Encoding = Encoding.UTF8;
                    htmlContent = web.DownloadString(Site);
                }
                catch (Exception ex)
                {
                    
                }
            }
            if (!string.IsNullOrEmpty(htmlContent))
            {
                return ParsePortData(htmlContent, Conveyance, VoyageNumber, dataFormat);
            }
            else
            {
                return "";
            }

        }

        private static string ParsePortData(string htmlContent, string Conveyance, string VoyageNumber, DataFormat dataFormat)
        {
            string content = dataFormat.ConveyanceColumnNumber == 0 ? HtmlParseUtils.FormatHtml(htmlContent, false, true) : htmlContent;
            List<string> LeaveDateList = new List<string>();
            if (!string.IsNullOrEmpty(content))
            {
                //add detail
                string form = HtmlParseUtils.GetSubString(content, "<form", null, "</form>", null, null, null);
                List<string> detailList = HtmlParseUtils.GetSubStrings(form, "<tr>", null, "</tr>", null, null, null);
                if (detailList != null)
                {
                    foreach (string s in detailList)
                    {
                        List<string> list =
                            HtmlParseUtils.GetSubStrings(s, "<td>", null, "</td>",
                                                         "<td>", "</td>", null);
                        if (list != null)
                        {
                            try
                            {
                                if (dataFormat.ConveyanceColumnNumber != 0)
                                {
                                    if (list[dataFormat.ConveyanceColumnNumber - 1] == Conveyance &&
                                        list[dataFormat.VoyageNumberColumnNumber].Contains(VoyageNumber))
                                    {
                                        LeaveDateList.Add(list[dataFormat.LeaveDateColumnNumber]);
                                    }
                                }
                                else
                                {
                                    if (list[dataFormat.VoyageNumberColumnNumber].Contains(VoyageNumber) && list[3].Contains("E"))
                                        LeaveDateList.Add(list[dataFormat.LeaveDateColumnNumber]);
                                }
                            }
                            catch (Exception ex)
                            {
                                
                            }
                        }
                    }
                }
            }
            if (LeaveDateList.Count > 0)
            {
                //Encoding encoding = Encoding.BigEndianUnicode;
                //byte[]   unicodeBytes = encoding.GetBytes(LeaveDateList[0]);

                //byte[]   unicodeNewBytes = Encoding.Convert(Encoding.UTF8, Encoding.Unicode, unicodeBytes);

                //char[] unicodeChar = new char[encoding.GetCharCount(unicodeNewBytes, 0, unicodeNewBytes.Length)];

                //string strRet = new string(unicodeChar);
                return LeaveDateList[0];
            }
            else
                return "";
        }
    }

    class DataFormat
    {
        public string TRHead = string.Empty;
        public string TRTail = string.Empty;
        public string TDHead = string.Empty;
        public string TDTail = string.Empty;
        public int VoyageNumberColumnNumber;
        public int LeaveDateColumnNumber;
        public int ConveyanceColumnNumber;

        public DataFormat(string strTRHead, string strTRTail, string strTDHead, string strTDTail, int iConveyanceColumnNumber, int iVoyageNumberColumnNumber, int iLeaveDateColumnNumber)
        {
            TRHead = strTRHead;
            TRTail = strTRTail;
            TDHead = strTDHead;
            TDTail = strTDTail;
            VoyageNumberColumnNumber = iVoyageNumberColumnNumber;
            LeaveDateColumnNumber = iLeaveDateColumnNumber;
            ConveyanceColumnNumber = iConveyanceColumnNumber;
        }
    }
}