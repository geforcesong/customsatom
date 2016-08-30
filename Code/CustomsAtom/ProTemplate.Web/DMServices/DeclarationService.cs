using System.IO;
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
    using System.Data.Objects;
    using System.Data.Common;


    // Implements application logic using the CustomsAtomEntities context.
    // TODO: Add your application logic to these methods or in additional methods.
    // TODO: Wire up authentication (Windows/ASP.NET Forms) and uncomment the following to disable anonymous access
    // Also consider adding roles to restrict access as appropriate.
    // [RequiresAuthentication]
    public partial class CustomsAtomService : LinqToEntitiesDomainService<CustomsAtomEntities>
    {

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'Declaration' query.
        public IQueryable<Declaration> GetDeclaration()
        {
            return this.ObjectContext.Declaration.Include("Customer").Include("DeclarationItem").Include("DeclarationDocument");
        }

        public void RefreshDeclaration(int declarationID)
        {
            var declaration = (from d in this.ObjectContext.Declaration
                               where d.ID == declarationID
                               select d).SingleOrDefault();
            this.ObjectContext.Refresh(RefreshMode.StoreWins, declaration);
        }

        public Declaration GetDeclarationByID(int declarationID)
        {
            var declaration = (from d in this.ObjectContext.Declaration
                               where d.ID == declarationID
                               select d).SingleOrDefault();
            //this.ObjectContext.Refresh(RefreshMode.StoreWins, declaration);
            if (declaration != null)
            {
                ObjectContext.LoadProperty<Declaration>(declaration, d => d.DeclarationContainer);
                ObjectContext.LoadProperty<Declaration>(declaration, d => d.DeclarationDocument);
                ObjectContext.LoadProperty<Declaration>(declaration, d => d.DeclarationImage);
                ObjectContext.LoadProperty<Declaration>(declaration, d => d.DeclarationItem);
                ObjectContext.LoadProperty<Declaration>(declaration, d => d.FinancialExportDeclaration);
                ObjectContext.LoadProperty<Declaration>(declaration, d => d.Customer);
                //foreach (var f in declaration.FinancialExportDeclaration)
                //{
                //    ObjectContext.LoadProperty<FinancialExportDeclaration>(f, d => d.FeeType);
                //}
                try
                {
                    // 获取报关单的时候，检查保存图片文件夹是否存在，如果不存在，创建一个
                    string folderPath = System.Web.HttpContext.Current.Request.PhysicalApplicationPath + "UserUploads\\" + declaration.ID.ToString() + "\\";
                    if (!Directory.Exists(folderPath))
                        Directory.CreateDirectory(folderPath);
                }
                catch { }
            }
            return declaration;
        }

        public List<Declaration> GetDeclarationByIDs(List<int> ids)
        {
            var declarations = (from d in this.ObjectContext.Declaration
                               where ids.Contains(d.ID)
                               select d).ToList();
            foreach (var declaration in declarations)
            {
                ObjectContext.LoadProperty<Declaration>(declaration, d => d.Customer);
                ObjectContext.LoadProperty<Declaration>(declaration, d => d.FinancialExportDeclaration);
            }
            return declarations;
        }

        public bool CheckExsitingDeclaration(string declarationNumber, string approveNumber)
        {
            if(!string.IsNullOrEmpty(declarationNumber))
            {
                var query = from d in this.ObjectContext.Declaration
                            where d.DeclarationNumber == declarationNumber
                            select d;
                if (query.Count() > 0)
                    return true;
            }

            if (!string.IsNullOrEmpty(approveNumber))
            {
                var query1 = from d in this.ObjectContext.Declaration
                             where d.ApprovalNumber == approveNumber
                            select d;
                if (query1.Count() > 0)
                    return true;
            }
            return false;
        }

        public void InsertDeclaration(Declaration declaration)
        {
            if ((declaration.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(declaration, EntityState.Added);
            }
            else
            {
                this.ObjectContext.Declaration.AddObject(declaration);
            }            
        }

        public void UpdateDeclaration(Declaration currentDeclaration)
        {
            this.ObjectContext.Declaration.AttachAsModified(currentDeclaration, this.ChangeSet.GetOriginal(currentDeclaration));

            /* 更新doublecheck
            var doubleCheck = (from a in this.ObjectContext.DoubleCheckDeclaration
                              where a.DeclarationId == currentDeclaration.ID
                              select a).FirstOrDefault();
            if (doubleCheck != null)
            {
                var customerName = (from c in this.ObjectContext.Customer
                                    where c.ID == currentDeclaration.CustomerID
                                    select c.Name).FirstOrDefault();
                doubleCheck.CustomerName = customerName;
                doubleCheck.ApprovalNumber = currentDeclaration.ApprovalNumber;
                doubleCheck.DeclarationNumber = currentDeclaration.DeclarationNumber;
                doubleCheck.RelatedSystemNumber = currentDeclaration.RelatedSystemNumber;
                var transactionName = (from t in this.ObjectContext.Transaction
                                    where t.Code == currentDeclaration.TransactionCode
                                    select t.Name).FirstOrDefault();
                doubleCheck.TransactionName = transactionName;
                //district
                var districtName = (from t in this.ObjectContext.District
                                    where t.Code == currentDeclaration.DistrictCode
                                    select t.Name).FirstOrDefault();
                doubleCheck.DistrictName = districtName;
                //trade
                var tradeName = (from t in this.ObjectContext.Trade
                                    where t.Code == currentDeclaration.TradeCode
                                    select t.Name).FirstOrDefault();
                doubleCheck.TradeName = tradeName;
                doubleCheck.ContainerCount = currentDeclaration.ContainerQuantity;
                //doubleCheck.TotalAmount = currentDeclaration.PackageAmount;
                //doubleCheck.CurrencyName
                doubleCheck.FeightFeeRate = string.Format("{0}", currentDeclaration.FreightFeeRate ?? 0);
                doubleCheck.InsuranceFeeRate = string.Format("{0}", currentDeclaration.InsuranceFeeRate ?? 0);
                //CustomerHouse
                var customerHouseName = (from t in this.ObjectContext.Customhouse
                                 where t.Code == currentDeclaration.CustomhouseCode
                                 select t.Name).FirstOrDefault();
                doubleCheck.CustomhouseName = customerHouseName;
                //CountryName
                var countryName = (from t in this.ObjectContext.Country
                                         where t.Code == currentDeclaration.CountryCode
                                         select t.Name).FirstOrDefault();
                doubleCheck.CountryName = countryName;
            }*/
        }

        public void DeleteDeclaration(Declaration declaration)
        {
            if ((declaration.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(declaration, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.Declaration.Attach(declaration);
                this.ObjectContext.Declaration.DeleteObject(declaration);
            }
        }
    }
}


