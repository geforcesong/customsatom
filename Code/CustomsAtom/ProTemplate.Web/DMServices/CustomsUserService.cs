
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
    using Microsoft.Commerce.Utilities.Web;
    using System.Threading;


    // Implements application logic using the CustomsAtomEntities context.
    // TODO: Add your application logic to these methods or in additional methods.
    // TODO: Wire up authentication (Windows/ASP.NET Forms) and uncomment the following to disable anonymous access
    // Also consider adding roles to restrict access as appropriate.
    // [RequiresAuthentication]
    partial class CustomsAtomService : LinqToEntitiesDomainService<CustomsAtomEntities>
    {

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'CustomsUser' query.
        public IQueryable<CustomsUser> GetCustomsUser()
        {
            return this.ObjectContext.CustomsUser;
        }

        public IQueryable<CustomsUser> GetCustomsUserWithScroe()
        {
            int i = 1;
            foreach (var u in this.ObjectContext.CustomsUser)
            {
                if (i % 8 == 0)
                {
                    
                    Thread.Sleep(60000);
                }
                i++;
                var score = GetScore(u.CustomsNo, u.IdentityNo);
                u.IdentityNo += "@" + score;
            }
            return this.ObjectContext.CustomsUser;
        }

        private string GetScore(string code, string identityCode)
        {
            string url=@"http://query.customs.gov.cn/HYW2007DataQuery/DeclarerQuery.aspx";
            System.Net.WebClient wb = new System.Net.WebClient();
            System.Collections.Specialized.NameValueCollection header = new System.Collections.Specialized.NameValueCollection();
            header.Add("Accept-Encoding", "gzip, deflate");
            header.Add("Accept-Language", "zh-CN");
            header.Add("Accept", "text/html, application/xhtml+xml, */*");
            header.Add("Referer", url);
            header.Add("Content-Type", "application/x-www-form-urlencoded");
            wb.Headers.Add(header);
            System.Collections.Specialized.NameValueCollection data = new System.Collections.Specialized.NameValueCollection();
            data.Add("__EVENTVALIDATION", "/wEWBgK53r3RBwLDs/j9BgLeia3JAgLCi9reAwLR2ZO3CALF6PvoBV0O2QqC3ddM8JD5cOt1cYROt0Tf");
            data.Add("__VIEWSTATE", "/wEPDwUKMjEzNTIyODIzNw9kFgICAw9kFgQCEw8PFgIeB1Zpc2libGVnZBYKAgMPDxYCHgRUZXh0BQgyMjAxMTQzMWRkAgcPDxYCHwEFCeWImOWjq+adsGRkAgsPDxYCHwEFCjMxMjA5ODAwMTVkZAIPDw8WAh8BBREyMDEyLTItMTMgOToyMjoyOGRkAhMPDxYCHwEFATBkZAIVDw8WAh8AaGQWAgIBDw8WAh8BBTDouqvku73pqozor4HmnKrpgJrov4fmiJbmnKrmn6Xmib7liLDljLnphY3nu5PmnpxkZGSnXunFvwMxYdEXMS4KusLqdc9K/A==");
            data.Add("btnSubmit", "查询");
            data.Add("txtDeclarerCode", code);
            data.Add("txtDeclarerId", identityCode);

            byte[] b = wb.UploadValues(url, "Post", data);
            string strData = System.Text.Encoding.UTF8.GetString(b);
            string content = HtmlParseUtils.FormatHtml(strData, false, true);
            if (!string.IsNullOrEmpty(content))
            {
                List<string> trList = HtmlParseUtils.GetSubStrings(content, "<span id=\"lblScore\">", null, "</span>", "<span id=\"lblScore\">", null, "</span>");
                if (trList != null && trList.Count == 1)
                    return trList[0];
                else
                    return "没有积分";
            }
            else
                return "无法取到积分";
        }

        public void InsertCustomsUser(CustomsUser customsUser)
        {
            if ((customsUser.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(customsUser, EntityState.Added);
            }
            else
            {
                this.ObjectContext.CustomsUser.AddObject(customsUser);
            }
        }

        public void UpdateCustomsUser(CustomsUser currentCustomsUser)
        {
            this.ObjectContext.CustomsUser.AttachAsModified(currentCustomsUser, this.ChangeSet.GetOriginal(currentCustomsUser));
        }

        public void DeleteCustomsUser(CustomsUser customsUser)
        {
            if ((customsUser.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(customsUser, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.CustomsUser.Attach(customsUser);
                this.ObjectContext.CustomsUser.DeleteObject(customsUser);
            }
        }
    }
}


