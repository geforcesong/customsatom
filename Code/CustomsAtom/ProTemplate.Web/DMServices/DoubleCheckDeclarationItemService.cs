
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
        // To support paging you will need to add ordering to the 'DoubleCheckDeclarationItem' query.
        public IQueryable<DoubleCheckDeclarationItem> GetDoubleCheckDeclarationItem()
        {
            return this.ObjectContext.DoubleCheckDeclarationItem;
        }
        
        public void InsertDoubleCheckDeclarationItem(DoubleCheckDeclarationItem doubleCheckDeclarationItem)
        {
            if ((doubleCheckDeclarationItem.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(doubleCheckDeclarationItem, EntityState.Added);
            }
            else
            {
                this.ObjectContext.DoubleCheckDeclarationItem.AddObject(doubleCheckDeclarationItem);
            }
        }
        
        public void UpdateDoubleCheckDeclarationItem(DoubleCheckDeclarationItem currentDoubleCheckDeclarationItem)
        {
            this.ObjectContext.DoubleCheckDeclarationItem.AttachAsModified(currentDoubleCheckDeclarationItem, this.ChangeSet.GetOriginal(currentDoubleCheckDeclarationItem));
        }
        
        public void DeleteDoubleCheckDeclarationItem(DoubleCheckDeclarationItem doubleCheckDeclarationItem)
        {
            if ((doubleCheckDeclarationItem.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(doubleCheckDeclarationItem, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.DoubleCheckDeclarationItem.Attach(doubleCheckDeclarationItem);
                this.ObjectContext.DoubleCheckDeclarationItem.DeleteObject(doubleCheckDeclarationItem);
            }
        }
    }
}


