using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Microsoft.Commerce.Utilities.Web;
using System.Xml;
using Microsoft.VisualBasic;
using ProcessorUtilities;

namespace ParseLandingDeclaration
{
    public class LandingCrawler
    {
        public static LandingNetInfo QueryLading(LandingNetInfo info)
        {
            if (string.IsNullOrEmpty(info.BillNumber))
                return null;
            return RunParseLading(info);
        }

        private static LandingNetInfo RunParseLading(LandingNetInfo info)
        {
            if (!string.IsNullOrEmpty(info.BillNumber))
            {
                string htmlContent = CrawlerMan.SaveWebPage(string.Format("http://edi.easipass.com/dataportal/query.do?ctno=&blno={0}&pagesize=100&submit=%E6%9F%A5%E8%AF%A2&qn=dp_cst_vsl", info.BillNumber));
                if (!string.IsNullOrEmpty(htmlContent))
                {
                    var ret = ParseAdmissionLadingDeclaration(info, htmlContent);
                    if (ret == null)
                    {
                        string realUrl = GetRealUrl(info, htmlContent);
                        if (!string.IsNullOrEmpty(realUrl))
                        {
                            htmlContent = CrawlerMan.SaveWebPage(realUrl);
                            if (!string.IsNullOrEmpty(htmlContent))
                            {
                                ret = ParseAdmissionLadingDeclaration(info, htmlContent);
                            }
                        }
                    }
                    return ret;
                }
            }
            return null;
        }

        private static string GetRealUrl(LandingNetInfo info, string htmlContent)
        {
            string content = HtmlParseUtils.FormatHtml(htmlContent, false, true);
            string strUrlTemp = string.Empty;
            List<string> headList = HtmlParseUtils.GetSubStrings(content, "<td align=\"left\" bgcolor=\"#[A-Za-z0-9]+\">", null, "</td>", "<td align=\"left\" bgcolor=\"#EEEEEE\">", "<td align=\"left\" bgcolor=\"#E1E1E1\">", "</td>");
            if (headList != null)
            {
                for (int i = 0; i < headList.Count / 3; i++)
                {
                    try
                    {
                        var doc = new XmlDocument();
                        doc.LoadXml(headList[3 * i]);
                        headList[3 * i] = doc.InnerText;
                        if (headList[3 * i + 1].Length > 6)
                        {
                            headList[3 * i + 1] = headList[3 * i + 1].Substring(headList[3 * i + 1].Length - 6, 6);
                        }
                        if (info.BillNumber == headList[3 * i + 2] && info.Conveyance == headList[3 * i] && info.VoyageNumber == headList[3 * i + 1])
                        {
                            strUrlTemp = string.Format("http://edi.easipass.com/dataportal/query.do?qn=dp_cst_query_billdetail&blno={0}&vslname={1}&voyage={2}", headList[3 * i + 2], headList[3 * i], headList[3 * i + 1]);
                            break;
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
            }
            return strUrlTemp;
        }

        private static LandingNetInfo ParseAdmissionLadingDeclaration(LandingNetInfo info, string htmlContent)
        {
            string content = HtmlParseUtils.FormatHtml(htmlContent, false, true);
            if (string.IsNullOrEmpty(content))
            {
                return null;
            }
            List<string> headList = HtmlParseUtils.GetSubStrings(content, "<td align=\"left\">", null, "</td>", "<td align=\"left\">", null, "</td>");
            if (headList == null)
            {
                return null;
            }
            try
            {
                if (info.BillNumber != headList[2])
                {
                    return null;
                }
                LandingNetInfo ret = new LandingNetInfo();
                ret.ConveyanceOnline = headList[0] == null ? "" : headList[0];
                ret.VoyageNumberOnline = headList[1] == null ? "" : headList[1];
                ret.GrossWeightOnline = Information.IsNumeric(headList[3]) ? Decimal.Parse(headList[3]) : 0;
                ret.PackageAmountOnline = Information.IsNumeric(headList[4]) ? int.Parse(headList[4]) : 0;
                //add detail
                List<string> detailList = HtmlParseUtils.GetSubStrings(content, "<tr height=\"21\">", null, "</tr>", null, null, null);
                foreach (string s in detailList)
                {
                    List<string> list = HtmlParseUtils.GetSubStrings(s, "<td align=\"left\" bgcolor=\"#[A-Za-z0-9]+\">", null, "</td>", "<td align=\"left\" bgcolor=\"#EEEEEE\">", "<td align=\"left\" bgcolor=\"#E1E1E1\">", "</td>");
                    if (list != null)
                    {
                        try
                        {
                            //NewAdmissionLadingDeclarationContainer admissionLadingDeclarationContainer = admissionLadingDeclaration.OnlineContainers.FirstOrDefault(container => container.NumberOnline == list[0]);
                            //if (admissionLadingDeclarationContainer == null)
                            //{
                            //    admissionLadingDeclarationContainer = new NewAdmissionLadingDeclarationContainer
                            //                                              {
                            //                                                  NumberOnline = list[0]
                            //                                              };
                            //    admissionLadingDeclaration.OnlineContainers.Add(admissionLadingDeclarationContainer);
                            //}
                            //admissionLadingDeclarationContainer.GrossWeightOnline = Information.IsNumeric(list[1]) ? new Decimal?(Decimal.Parse(list[1])) : null;
                            //admissionLadingDeclarationContainer.PackageAmountOnline = Information.IsNumeric(list[1]) ? new int?(int.Parse(list[2])) : null;
                            //admissionLadingDeclarationContainer.ReturnReceiptDescriptionOnline = list[3];
                            //string strNewDateTime = list[4].Substring(0, 4) + "-" + list[4].Substring(4, 2) + "-" + list[4].Substring(6, 2) + " " + list[4].Substring(8, 2) + ":" + list[4].Substring(10, 2) + ":00";
                            //admissionLadingDeclarationContainer.ReceivedReturnReceiptDateTimeOnline = Information.IsDate(strNewDateTime) ? new DateTime?(DateTime.Parse(strNewDateTime)) : null;
                            //admissionLadingDeclarationContainer.OwnerOnline = list[5];
                            //admissionLadingDeclarationContainer.UnladingPortCodeOnline = list[6];
                            //admissionLadingDeclarationContainer.COSTRPNumberOnline = list[7];
                            ret.OnlineContainerNumber += list[0] + ",";

                        }
                        catch { }
                        finally
                        {
                            ret.OnlineContainerCount++;
                        }
                    }
                }
                return ret;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}