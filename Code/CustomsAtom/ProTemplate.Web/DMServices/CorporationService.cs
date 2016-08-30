
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
    partial class CustomsAtomService : LinqToEntitiesDomainService<CustomsAtomEntities>
    {

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'Corporation' query.

        public bool DeleteCorproationByCode(string code)
        {
            var a = (from c in this.ObjectContext.Corporation where c.CorporationCode == code select c).FirstOrDefault();
            if (a != null)
            {
                this.ObjectContext.Corporation.DeleteObject(a);
                this.ObjectContext.SaveChanges();
                return true;
            }
            return false;
        }

        public IQueryable<Corporation> GetCorporation()
        {
            return this.ObjectContext.Corporation;
        }

        public void InsertCorporation(Corporation corporation)
        {
            if ((corporation.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(corporation, EntityState.Added);
            }
            else
            {
                this.ObjectContext.Corporation.AddObject(corporation);
            }
        }

        public void UpdateCorporation(Corporation currentCorporation)
        {
            this.ObjectContext.Corporation.AttachAsModified(currentCorporation, this.ChangeSet.GetOriginal(currentCorporation));
        }

        public void DeleteCorporation(Corporation corporation)
        {
            if ((corporation.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(corporation, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.Corporation.Attach(corporation);
                this.ObjectContext.Corporation.DeleteObject(corporation);
            }
        }
    }
}


