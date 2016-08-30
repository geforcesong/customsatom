using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using ProcessorUtilities;

namespace ParseShip
{
    class Program
    {
        private const string _strRootSite = "http://edi.easipass.com/dataportal/q.do?qn=dp_export_shipinfo";
        private static readonly ShipContextDataContext context = new ShipContextDataContext();
        private static List<Ship> lstShip = new List<Ship>();

        private static void Main()
        {
            ClearOldData();
            ParseRootPage();
        }

        private static void ClearOldData()
        {
            //StorageContext.Delete<WebPage>(WebPage.Fields.ID.IS_NOT_NULL());
            context.Ships.DeleteAllOnSubmit<Ship>(context.Ships.Where(o=>o.Conveyance != null));
            context.SubmitChanges();
        }

        private static void ParseRootPage()
        {
            string rootHtmlContent = SaveWebPage(_strRootSite);
            if (!string.IsNullOrEmpty(rootHtmlContent))
            {
                string qid = HtmlParseUtils.GetSubString(rootHtmlContent, "<input name=\"qid\" value=\"", null, "\" type=\"hidden\" />", null, null, null);
                if (!string.IsNullOrEmpty(qid))
                {
                    for (int page = 0; page <= 30; page++)
                    {
                        string htmlContent =
                            SaveWebPage(
                                string.Format(
                                    "http://edi.easipass.com/dataportal/query.do?vessel1=&vess_call1=&voyage1=&carr_code1=&ship_agen1=&berth1=&esti_sail1=&esti_sail2=&qid={0}&pagesize=300&page={1}",
                                    qid, page));
                        if (!string.IsNullOrEmpty(htmlContent))
                        {
                            ParseShip(htmlContent);
                        }
                        
                    }
                    context.Ships.InsertAllOnSubmit(lstShip);
                    context.SubmitChanges();
                }
            }
        }

        private static void ParseShip(string htmlContent)
        {
            string content = HtmlParseUtils.FormatHtml(htmlContent, false, true);
            if (!string.IsNullOrEmpty(content))
            {
                List<string> trList = HtmlParseUtils.GetSubStrings(content, "<tr height=\"21\">", null, "</tr>", null, null, null);
                if (trList != null)
                {
                    foreach (string s in trList)
                    {
                        List<string> list = HtmlParseUtils.GetSubStrings(s, "<td align=\"left\" bgcolor=\"#[A-Za-z0-9]+\">", null, "</td>", "<td align=\"left\" bgcolor=\"#EEEEEE\">", "<td align=\"left\" bgcolor=\"#E1E1E1\">", "</td>");
                        if (list != null && list.Count == 8)
                        {
                            try
                            {
                                //Ship ship = StorageContext.FindOne<Ship>(Ship.Fields.Conveyance.EQUALS(list[0]) && Ship.Fields.VoyageNumber.EQUALS(list[2]));
                                //if (ship == null)
                                //{
                                var ship = new Ship();
                                ship.Conveyance = list[0];
                                ship.VoyageNumber = list[2];
                                //}
                                ship.CallNumber = list[1];
                                ship.Conveyancer = list[3];
                                ship.Agent = list[4];
                                ship.Dock = list[5];
                                if (list[6].Length >= 8)
                                {
                                    string strNewDateTime = list[6].Substring(0, 4) + "-" + list[6].Substring(4, 2) + "-" + list[6].Substring(6, 2);
                                    ship.LeaveDate = Information.IsDate(strNewDateTime) ? new DateTime?(DateTime.Parse(strNewDateTime)) : null;
                                }
                                else
                                {
                                    ship.LeaveDate = null;
                                }
                                ship.IMONumber = list[7];
                                ship.Html = s;

                                if (!lstShip.Any(o => o.Conveyance == ship.Conveyance && o.VoyageNumber == ship.VoyageNumber))
                                {
                                    lstShip.Add(ship);

                                }
                                //ship.Save();
                            }
                            catch (Exception ex)
                            {
                                ex.HelpLink = string.Format("{0} Conveyance:{1}; VoyageNumber:{2}", list, list[0], list[1]);
                                
                            }
                        }
                    }
                    
                }
            }
        }

        private static string SaveWebPage(string url)
        {
            try
            {
                var web = new WebClient();
                web.Encoding = Encoding.UTF8;
                //WebProxy proxy = new WebProxy("http://jpnproxy.fareast.corp.microsoft.com:80/", true);
                //web.Proxy = proxy;

                string content = HtmlParseUtils.FormatHtml(web.DownloadString(url), false, true);
                return content;
            }
            catch (Exception ex)
            {
                ex.HelpLink = url;
                return null;
            }
        }
    }
}
