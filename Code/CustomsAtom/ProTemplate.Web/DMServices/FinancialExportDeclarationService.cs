
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
    using ProTemplate.Web.Utility;


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
        // To support paging you will need to add ordering to the 'FinancialExportDeclaration' query.

        public List<GetAllFinancialDeclaration> GetAllFinancialExportDeclarationByReceiveDate(int userID, string condition)
        {
            return this.ObjectContext.GetAllFinancialDeclarationByReceiveDate(userID, EncryptionUtil.Decrypt( condition)).ToList();
        }

        public List<GetAllFinancialDeclaration> GetAllFinancialExportDeclarationByDeclarationCodes(int userID, string delcarationNums)
        {
            return this.ObjectContext.GetAllFinancialDeclarationByDeclarationCodes(userID, delcarationNums).ToList();
        }

        public IQueryable<FinancialExportDeclaration> GetFinancialExportDeclaration()
        {
            return this.ObjectContext.FinancialExportDeclaration;
        }

        public void InsertFinancialExportDeclaration(FinancialExportDeclaration financialExportDeclaration)
        {
            if ((financialExportDeclaration.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(financialExportDeclaration, EntityState.Added);
            }
            else
            {
                this.ObjectContext.FinancialExportDeclaration.AddObject(financialExportDeclaration);
            }
        }

        public void UpdateFinancialExportDeclaration(FinancialExportDeclaration currentFinancialExportDeclaration)
        {
            this.ObjectContext.FinancialExportDeclaration.AttachAsModified(currentFinancialExportDeclaration, this.ChangeSet.GetOriginal(currentFinancialExportDeclaration));
        }

        public void DeleteFinancialExportDeclaration(FinancialExportDeclaration financialExportDeclaration)
        {
            if ((financialExportDeclaration.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(financialExportDeclaration, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.FinancialExportDeclaration.Attach(financialExportDeclaration);
                this.ObjectContext.FinancialExportDeclaration.DeleteObject(financialExportDeclaration);
            }
        }
    }
}


