
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
    // TODO: add the EnableClientAccessAttribute to this class to expose this DomainService to clients.
    public partial class CustomsAtomService : LinqToEntitiesDomainService<CustomsAtomEntities>
    {

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'DeclarationItem' query.
        public IQueryable<DeclarationItem> GetDeclarationItem()
        {
            return this.ObjectContext.DeclarationItem;
        }

        public void InsertDeclarationItem(DeclarationItem declarationItem)
        {
            if ((declarationItem.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(declarationItem, EntityState.Added);
            }
            else
            {
                this.ObjectContext.DeclarationItem.AddObject(declarationItem);
            }
        }

        public void UpdateDeclarationItem(DeclarationItem currentDeclarationItem)
        {
            this.ObjectContext.DeclarationItem.AttachAsModified(currentDeclarationItem, this.ChangeSet.GetOriginal(currentDeclarationItem));
        }

        public void DeleteDeclarationItem(DeclarationItem declarationItem)
        {
            if ((declarationItem.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(declarationItem, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.DeclarationItem.Attach(declarationItem);
                this.ObjectContext.DeclarationItem.DeleteObject(declarationItem);
            }
        }
    }
}


