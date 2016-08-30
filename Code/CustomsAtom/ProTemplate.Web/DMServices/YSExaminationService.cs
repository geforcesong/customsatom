
namespace ProTemplate.Web.DMServices
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Data;
    using System.Linq;
    using System.ServiceModel.DomainServices.EntityFramework;
    using System.ServiceModel.DomainServices.Hosting;
    using System.ServiceModel.DomainServices.Server;
    using ProTemplate.Web;
    using System.Net;
    using System.Net.Cache;
    using ProTemplate.Web.Utility;
    using System.Text;
    using System.Drawing;
    using System.IO;
    using HtmlAgilityPack;
    using System.Web;


    // Implements application logic using the CustomsAtomEntities context.
    // TODO: Add your application logic to these methods or in additional methods.
    // TODO: Wire up authentication (Windows/ASP.NET Forms) and uncomment the following to disable anonymous access
    // Also consider adding roles to restrict access as appropriate.
    // [RequiresAuthentication]
    // TODO: add the EnableClientAccessAttribute to this class to expose this DomainService to clients.
    public partial class CustomsAtomService : LinqToEntitiesDomainService<CustomsAtomEntities>
    {
        private readonly RequestCachePolicy _cachePolicy = new RequestCachePolicy(RequestCacheLevel.Reload);
        static string strCode;
        static HttpWebRequest httpWebRequest;
        static HttpWebRequest imageWebRequest;
        static CookieContainer _cookies;

        public byte[] GetYSValidImage()
        {
            _cookies = new CookieContainer();
            httpWebRequest = YSWebExtensions.CreateGetRequest("http://www.eport.sh.cn/cas/login", "", _cachePolicy, _cookies, "", "www.eport.sh.cn");
            string strHtml = YSWebExtensions.CreateResponseHtml(httpWebRequest, Encoding.UTF8, _cookies);
            strCode = YSWebExtensions.GetInputNodeValue(strHtml, "lt");
            imageWebRequest = YSWebExtensions.CreateGetRequest("http://www.eport.sh.cn/cas/simpleImg", "", _cachePolicy, _cookies, "http://www.eport.sh.cn/cas/login", "www.eport.sh.cn");
            Image image = YSWebExtensions.CreateResponseImage(imageWebRequest, _cookies);
            if (image == null)
                return null;
            else
            {
                MemoryStream ms = new MemoryStream();
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                return ms.ToArray();
            }
        }

        public bool YSLogin(string code)
        {
            httpWebRequest = YSWebExtensions.CreatePostRequest("http://www.eport.sh.cn/cas/login",
                                                string.Format(
                                                    "username={0}&password={1}&answer={2}&lt={3}&_eventId=submit&submit=+",
                                                    "flake", "781218", code,
                                                    strCode), Encoding.Default, _cachePolicy, _cookies,
                                                "http://www.eport.sh.cn/cas/login", "www.eport.sh.cn");
            string successInfo = YSWebExtensions.CreateResponseHtml(httpWebRequest, Encoding.UTF8, _cookies);
            if (successInfo.Contains("Log In Successful"))
                return true;
            else
                return false;
        }

        public YSExaminationData YSQuery(string declarationNumber)
        {
            if (declarationNumber == null || declarationNumber.Length != 18)
                return null;
            var declaration = (from d in this.ObjectContext.Declaration
                               join e in ObjectContext.YSExaminationResult on d.DeclarationNumber.Substring(9, 9) equals e.DeclarationNumber into temp
                               from tt in temp.DefaultIfEmpty()
                               from c in ObjectContext.Customer
                               where d.DeclarationNumber == declarationNumber && c.ID == d.CustomerID
                               orderby d.ReceivedDate descending
                               select new { d.ID, d.ApprovalNumber, d.BillNumber, d.Conveyance, d.DeclarationNumber, d.DeclarationStatus, d.VoyageNumber, ExaminationStatus = tt == null ? "" : tt.ExaminationStatus, tt.LastUpdateDate, c.Name }).FirstOrDefault();
            if (declaration != null)
            {
                //ObjectContext.LoadProperty<Declaration>(declaration, d => d.Customer);
                YSExaminationData yd = new YSExaminationData();
                yd.ID = declaration.ID;
                yd.ApprovalNumber = declaration.ApprovalNumber == null ? "" : declaration.ApprovalNumber;
                yd.BillNumber = declaration.BillNumber == null ? "" : declaration.BillNumber;
                yd.Conveyance = declaration.Conveyance == null ? "" : declaration.Conveyance;
                yd.CustomerName = declaration.Name;
                yd.DeclarationNumber = declaration.DeclarationNumber;
                yd.DeclarationStatus = declaration.DeclarationStatus==null?"":declaration.DeclarationStatus;
                yd.VoyageNumber = declaration.VoyageNumber == null ? "" : declaration.VoyageNumber;
                yd.YSDate = declaration.LastUpdateDate.HasValue ? declaration.LastUpdateDate.Value.ToString("yyyy年MM月dd日"): "";
                yd.YSStatus = declaration.ExaminationStatus;
                //RunParse(yd);
                return yd;
            }
            else
                return null;
        }

        public List<YSExaminationData> YSQueryByDeclarationNumbers(string declarationNumbers)
        {
            RunParse();
            string[] dNums = EncryptionUtil.Decrypt(declarationNumbers).Split(',');
            List<YSExaminationData> lst = new List<YSExaminationData>();
            foreach (var n in dNums)
            {
                YSExaminationData ys = YSQuery(n);
                if (ys != null)
                    lst.Add(ys);
            }
            return lst;
        }

        public List<YSExaminationData> YSQueryByDate(DateTime start, DateTime end)
        {
            RunParse();
            var numbers = (from d in this.ObjectContext.Declaration
                               where d.DeclarationDate >= start && d.DeclarationDate <= end
                                orderby d.DeclarationDate descending
                               select d.DeclarationNumber).Distinct();
            if (numbers.Count() == 0)
                return null;
            else
            {
                List<YSExaminationData> lst = new List<YSExaminationData>();
                foreach (var n in numbers)
                {
                    if (n.Length == 18 && n.Substring(9, 2) == "81")
                    {
                        YSExaminationData ys = YSQuery(n);
                        if (ys != null)
                            lst.Add(ys);
                    }
                }
                return lst;
            }
        }

        private void RunParse()
        {
            string postedHtmlContent =
                YSWebExtensions.GetResponseHtml(
                    "http://www.eport.sh.cn/sheport/secure/userCenter/portalAssistant/bgdYsbsgcy.action",
                //string.Format("searchTagIndex=1&today=false&pagination.pageSize=15&pagination.startIndex=0&entryId={0}&searchTag1=%E6%9F%A5%E8%AF%A2&fromDeclDate=&toDeclDate=&expImp=&fromDeclDate3=&toDeclDate3=&rskCode=&fromDeclDateTag4=&toDeclDateTag4=&buildseletpagesize=15", ysExamination.DeclarationNumber.Substring(9)), Encoding.Default,
                    string.Format("searchTagIndex=4&today=&pagination.pageSize=2000&pagination.startIndex=0&entryId=&fromDeclDate=&toDeclDate=&expImp=&fromDeclDate3=&toDeclDate3=&rskCode=&fromDeclDateTag4={0}&toDeclDateTag4={1}&goText=&buildseletpagesize=100", DateTime.Today.AddDays(-14).ToString("yyyy-MM-dd"), DateTime.Today.AddDays(1).ToString("yyyy-MM-dd")),
                    Encoding.Default,
                    httpWebRequest.CookieContainer);
            ParseKSDManifest(postedHtmlContent);
        }

        private bool ParseKSDManifest(string htmlContent)
        {
            bool bRet = false;
            HtmlAgilityPack.HtmlDocument Doc = new HtmlAgilityPack.HtmlDocument();
            Doc.LoadHtml(htmlContent);

            DateTime sDate = DateTime.Today.AddDays(-14);
            DateTime eDate = DateTime.Today.AddDays(1);

            var ysExaminationList = (from c in ObjectContext.YSExaminationResult where c.DeclarationDate >= sDate && c.DeclarationDate < eDate select c).ToList();

            HtmlNodeCollection trNodes = Doc.DocumentNode.SelectNodes("//table[@class='dataTable']/tbody/tr");
            if (trNodes != null)
            {
                foreach (HtmlNode trNode in trNodes)
                {
                    HtmlNodeCollection tdNodes = trNode.SelectNodes("td");

                    if (tdNodes != null && tdNodes.Count >= 8)
                    {
                        string declarationNumber = HttpUtility.HtmlDecode(tdNodes[1].InnerText);
                        
                        DateTime declarationDate = DateTime.Parse(HttpUtility.HtmlDecode(tdNodes[4].InnerText));
                        string ysStatus = HttpUtility.HtmlDecode(tdNodes[6].InnerText);
                        DateTime ysDate = DateTime.Parse(HttpUtility.HtmlDecode(tdNodes[7].InnerText));

                        var ysExamination = (from c in ysExaminationList where c.DeclarationNumber == declarationNumber select c).FirstOrDefault();
                        if (ysExamination != null)
                        {
                            if (ysExamination.LastUpdateDate < ysDate)
                            {
                                ysExamination.DeclarationNumber = declarationNumber;
                                ysExamination.DeclarationDate = declarationDate;
                                ysExamination.ExaminationStatus = ysStatus;
                                ysExamination.LastUpdateDate = ysDate;
                            }
                        }
                        else
                        {
                            YSExaminationResult r = new YSExaminationResult() { DeclarationNumber = declarationNumber, LastUpdateDate = ysDate, DeclarationDate = declarationDate, ExaminationStatus = ysStatus };
                            ObjectContext.AddToYSExaminationResult(r);
                            ysExaminationList.Add(r);
                        }
                        
                    }
                }
                ObjectContext.SaveChanges();
            }
            return bRet;
        }

    }
    public class YSModel
    {
        public string DelcarationNumber;
        public string DeclarationDate;
        public string YSStatus;
        public string YSDate;

    }

}


