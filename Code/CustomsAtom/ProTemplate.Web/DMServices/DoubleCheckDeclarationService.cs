
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
        // To support paging you will need to add ordering to the 'DoubleCheckDeclaration' query.
        public void SetDoubleCheckMachineByIds(List<int> ids, string machineName)
        {
            List<DoubleCheckDeclaration> doubleCheckDeclarationList = (from c in ObjectContext.DoubleCheckDeclaration where ids.Contains(c.DeclarationId) select c).ToList();
            foreach (DoubleCheckDeclaration doubleCheck in doubleCheckDeclarationList)
            {
                doubleCheck.MachineName = machineName;
            }

            ObjectContext.SaveChanges();
        }
        public IQueryable<DoubleCheckDeclaration> GetDoubleCheckDeclaration()
        {
            return this.ObjectContext.DoubleCheckDeclaration;
        }

        public DoubleCheckDeclaration GetDoubleCheckDeclarationByDelarationID(int id)
        {
            var query = from a in this.ObjectContext.DoubleCheckDeclaration
                        join b in this.ObjectContext.Declaration on a.DeclarationId equals b.ID
                        where a.DeclarationId == id
                        orderby b.ReceivedDate descending
                        select a;
            if (query != null && query.Count() > 0)
            {
                DoubleCheckDeclaration dcd = query.First();
                ObjectContext.LoadProperty<DoubleCheckDeclaration>(dcd, d => d.DoubleCheckDeclarationItem);
                return dcd;
            }
            else
                return null;
        }

        public DoubleCheckDeclaration GetDoubleCheckDeclarationByDelarationNumber(string declarationNumber)
        {
            var query = from a in this.ObjectContext.DoubleCheckDeclaration
                        join b in this.ObjectContext.Declaration on a.DeclarationId equals b.ID
                        where a.DeclarationNumber == declarationNumber
                        orderby b.ReceivedDate descending
                        select a;
            if (query != null && query.Count() > 0)
                return query.First();
            else
                return null;
        }

        public DoubleCheckDeclaration GetDoubleCheckDeclarationByApproveNumber(string approveNumber)
        {
            var query = from a in this.ObjectContext.DoubleCheckDeclaration
                        join b in this.ObjectContext.Declaration on a.DeclarationId equals b.ID
                        where a.ApprovalNumber == approveNumber
                        orderby b.ReceivedDate descending
                        select a;
            if (query != null && query.Count() > 0)
                return query.First();
            else
                return null;
        }
        
        public void InsertDoubleCheckDeclaration(DoubleCheckDeclaration doubleCheckDeclaration)
        {
            if ((doubleCheckDeclaration.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(doubleCheckDeclaration, EntityState.Added);
            }
            else
            {
                this.ObjectContext.DoubleCheckDeclaration.AddObject(doubleCheckDeclaration);
            }
        }
        
        public void UpdateDoubleCheckDeclaration(DoubleCheckDeclaration currentDoubleCheckDeclaration)
        {
            this.ObjectContext.DoubleCheckDeclaration.AttachAsModified(currentDoubleCheckDeclaration, this.ChangeSet.GetOriginal(currentDoubleCheckDeclaration));
        }
        
        public void DeleteDoubleCheckDeclaration(DoubleCheckDeclaration doubleCheckDeclaration)
        {
            if ((doubleCheckDeclaration.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(doubleCheckDeclaration, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.DoubleCheckDeclaration.Attach(doubleCheckDeclaration);
                this.ObjectContext.DoubleCheckDeclaration.DeleteObject(doubleCheckDeclaration);
            }
        }
    }
}


