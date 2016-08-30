
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
        // To support paging you will need to add ordering to the 'DeclarationDocument' query.
        public IQueryable<DeclarationDocument> GetDeclarationDocument()
        {
            return this.ObjectContext.DeclarationDocument;
        }

        public void InsertDeclarationDocument(DeclarationDocument declarationDocument)
        {
            if ((declarationDocument.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(declarationDocument, EntityState.Added);
            }
            else
            {
                this.ObjectContext.DeclarationDocument.AddObject(declarationDocument);
            }
        }

        public void UpdateDeclarationDocument(DeclarationDocument currentDeclarationDocument)
        {
            this.ObjectContext.DeclarationDocument.AttachAsModified(currentDeclarationDocument, this.ChangeSet.GetOriginal(currentDeclarationDocument));
        }

        public void DeleteDeclarationDocument(DeclarationDocument declarationDocument)
        {
            if ((declarationDocument.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(declarationDocument, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.DeclarationDocument.Attach(declarationDocument);
                this.ObjectContext.DeclarationDocument.DeleteObject(declarationDocument);
            }
        }
    }
}


