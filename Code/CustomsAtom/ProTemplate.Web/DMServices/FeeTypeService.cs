
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
        // To support paging you will need to add ordering to the 'FeeType' query.
        public IQueryable<FeeType> GetFeeType()
        {
            return this.ObjectContext.FeeType;
        }

        public void InsertFeeType(FeeType feeType)
        {
            if ((feeType.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(feeType, EntityState.Added);
            }
            else
            {
                this.ObjectContext.FeeType.AddObject(feeType);
            }
        }

        public void UpdateFeeType(FeeType currentFeeType)
        {
            this.ObjectContext.FeeType.AttachAsModified(currentFeeType, this.ChangeSet.GetOriginal(currentFeeType));
        }

        public void DeleteFeeType(FeeType feeType)
        {
            if ((feeType.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(feeType, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.FeeType.Attach(feeType);
                this.ObjectContext.FeeType.DeleteObject(feeType);
            }
        }
    }
}


