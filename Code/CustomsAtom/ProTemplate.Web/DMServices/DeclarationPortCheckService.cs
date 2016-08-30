
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
    using ProTemplate.Web.DataCrawler;


    // Implements application logic using the CustomsAtomEntities context.
    // TODO: Add your application logic to these methods or in additional methods.
    // TODO: Wire up authentication (Windows/ASP.NET Forms) and uncomment the following to disable anonymous access
    // Also consider adding roles to restrict access as appropriate.
    // [RequiresAuthentication]
    // TODO: add the EnableClientAccessAttribute to this class to expose this DomainService to clients.
    public partial class CustomsAtomService : LinqToEntitiesDomainService<CustomsAtomEntities>
    {

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'Declaration' query.
        public Declaration GetDeclarationPortCheckByDeclarationNumber(string declarationNumber)
        {
            CustomsAtomEntities context = new CustomsAtomEntities();

            var query = (from d in context.Declaration
                        where d.DeclarationNumber == declarationNumber
                        orderby d.ReceivedDate descending
                             select d).FirstOrDefault();
            if (query != null)
            {
                context.LoadProperty<Declaration>(query, d => d.Customer);
                context.LoadProperty<Declaration>(query, d => d.DeclarationContainer);
            }
            return query;
        }

        public Declaration GetDeclarationPortCheckByBillNumber(string billNumber)
        {
            CustomsAtomEntities context = new CustomsAtomEntities();
            var query = (from d in context.Declaration
                         where d.BillNumber == billNumber
                         orderby d.ReceivedDate descending
                         select d).FirstOrDefault();
            if (query != null)
            {
                context.LoadProperty<Declaration>(query, d => d.Customer);
                context.LoadProperty<Declaration>(query, d => d.DeclarationContainer);
            }
            return query;
        }

        public Declaration GetDeclarationPortCheckByApproveNumber(string approveNumber)
        {
            CustomsAtomEntities context = new CustomsAtomEntities();
            var query = (from d in context.Declaration
                         where d.ApprovalNumber == approveNumber
                         orderby d.ReceivedDate descending
                         select d).FirstOrDefault();
            if (query != null)
            {
                context.LoadProperty<Declaration>(query, d => d.Customer);
                context.LoadProperty<Declaration>(query, d => d.DeclarationContainer);
            }
            return query;
        }

        public List<Declaration> RefreshDeclarationPortCheck(string declarationNumbers)
        {
            if (string.IsNullOrEmpty(declarationNumbers))
                return null;
            else
            {
                string[] dNums = declarationNumbers.Split(',');
                List<Declaration> lst = new List<Declaration>();
                foreach (var d in dNums)
                {
                    var declaration = GetDeclarationPortCheckByDeclarationNumber(d);
                    if (declaration != null)
                        lst.Add(declaration);
                }
                return lst;
            }
        }

        public IList<DeclarationPortCheckResult> CheckLandingResults(string declarationNumbers)
        {
            CustomsAtomEntities context = new CustomsAtomEntities();

            if (string.IsNullOrEmpty(declarationNumbers))
                return null;
            else
            {
                string[] dNums = declarationNumbers.Split(',');
                List<DeclarationPortCheckResult> lst = new List<DeclarationPortCheckResult>();
                for(int i=0;i<dNums.Length;i++)
                {
                    string theNumber = dNums[i];
                    var declaration = (from d in context.Declaration
                                       where d.DeclarationNumber == theNumber
                                 orderby d.ReceivedDate descending
                                 select d).FirstOrDefault();
                    if (declaration != null)
                    {
                        ProTemplate.Web.DataCrawler.Data.LandingNetInfo info = new DataCrawler.Data.LandingNetInfo();
                        info.BillNumber = declaration.BillNumber;
                        info.Conveyance = declaration.Conveyance;
                        info.VoyageNumber = declaration.VoyageNumber;
                        var ret = LandingCrawler.QueryLading(info);
                        if (ret != null)
                        {
                            DeclarationPortCheckResult result = new DeclarationPortCheckResult();
                            result.ID = i;
                            result.DeclarationNumber = dNums[i];
                            result.GrossWeightOnline = ret.GrossWeightOnline.ToString();
                            result.ConveyanceOnline = ret.ConveyanceOnline;
                            result.VoyageNumberOnline = ret.VoyageNumberOnline;
                            result.PackageAmountOnline = ret.PackageAmountOnline.ToString();
                            result.OnlineContainerCount = ret.OnlineContainerCount;
                            result.OnlineContainerNumber = ret.OnlineContainerNumber.TrimEnd(',');
                            lst.Add(result);
                        }
                    }
                }
                return lst;
            }
        }

        public IList<DeclarationPortCheckResult> CheckLeaveDockDateResults(string declarationNumbers)
        {
            CustomsAtomEntities context = new CustomsAtomEntities();

            if (string.IsNullOrEmpty(declarationNumbers))
                return null;
            else
            {
                string[] dNums = declarationNumbers.Split(',');
                List<DeclarationPortCheckResult> lst = new List<DeclarationPortCheckResult>();
                for (int i = 0; i < dNums.Length; i++)
                {
                    string theNumber = dNums[i];
                    var declaration = (from d in context.Declaration
                                       where d.DeclarationNumber == theNumber
                                       orderby d.ReceivedDate descending
                                       select d).FirstOrDefault();
                    if (declaration != null)
                    {
                        string ret=null;
                        try
                        {
                            ret = LeaveDockDateCrawler.FindExportDate(declaration.Conveyance, declaration.VoyageNumber, declaration.Dock);
                        }
                        catch { }
                        DeclarationPortCheckResult result = new DeclarationPortCheckResult();
                        result.ID = i;
                        result.DeclarationNumber = dNums[i];
                        result.LeaveDockDate = String.IsNullOrEmpty(ret) ? "无法查询" : ret;
                        lst.Add(result);
                    }
                }
                return lst;
            }
        }

        public IList<DeclarationPortCheckResult> CheckContainerAdmissionStatusResults(string declarationNumbers)
        {
            CustomsAtomEntities context = new CustomsAtomEntities();

            if (string.IsNullOrEmpty(declarationNumbers))
                return null;
            else
            {
                string[] dNums = declarationNumbers.Split(',');
                List<DeclarationPortCheckResult> lst = new List<DeclarationPortCheckResult>();
                for (int i = 0; i < dNums.Length; i++)
                {
                    string theNumber = dNums[i];
                    var declaration = (from d in context.Declaration
                                       where d.DeclarationNumber == theNumber
                                       orderby d.ReceivedDate descending
                                       select d).FirstOrDefault();
                    if (declaration != null)
                    {
                        var ret = ContainerAdmissionStatusCrawler.RunParseAdmission(declaration.BillNumber);
                        DeclarationPortCheckResult result = new DeclarationPortCheckResult();
                        result.ID = i;
                        result.DeclarationNumber = dNums[i];
                        result.ContainerAdmissionStatus = ret == null ? "" : ret;
                        lst.Add(result);
                    }
                }
                return lst;
            }
        }
    }
}


