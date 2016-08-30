
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

    public partial class CustomsAtomService : LinqToEntitiesDomainService<CustomsAtomEntities>
    {

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'UserRole' query.
        public IQueryable<UserRole> GetUserRole()
        {
            return this.ObjectContext.UserRole;
        }

        public void InsertUserRole(UserRole userRole)
        {
            if ((userRole.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(userRole, EntityState.Added);
            }
            else
            {
                this.ObjectContext.UserRole.AddObject(userRole);
            }
        }

        public void UpdateUserRole(UserRole currentUserRole)
        {
            this.ObjectContext.UserRole.AttachAsModified(currentUserRole, this.ChangeSet.GetOriginal(currentUserRole));
        }

        public void DeleteUserRole(UserRole userRole)
        {
            if ((userRole.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(userRole, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.UserRole.Attach(userRole);
                this.ObjectContext.UserRole.DeleteObject(userRole);
            }
        }
    }
}


