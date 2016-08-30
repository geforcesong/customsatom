
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
        // To support paging you will need to add ordering to the 'RoleAccess' query.
        public IQueryable<RoleAccess> GetRoleAccess()
        {
            return this.ObjectContext.RoleAccess;
        }

        public void InsertRoleAccess(RoleAccess roleAccess)
        {
            if ((roleAccess.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(roleAccess, EntityState.Added);
            }
            else
            {
                this.ObjectContext.RoleAccess.AddObject(roleAccess);
            }
        }

        public void UpdateRoleAccess(RoleAccess currentRoleAccess)
        {
            this.ObjectContext.RoleAccess.AttachAsModified(currentRoleAccess, this.ChangeSet.GetOriginal(currentRoleAccess));
        }

        public void DeleteRoleAccess(RoleAccess roleAccess)
        {
            if ((roleAccess.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(roleAccess, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.RoleAccess.Attach(roleAccess);
                this.ObjectContext.RoleAccess.DeleteObject(roleAccess);
            }
        }
    }
}


