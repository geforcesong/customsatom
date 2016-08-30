
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
        // To support paging you will need to add ordering to the 'UserGroup' query.
        public IQueryable<UserGroup> GetUserGroup()
        {
            foreach (var u in ObjectContext.UserGroup)
            {
                ObjectContext.LoadProperty<UserGroup>(u, a => a.UserGroupCustomer);
                foreach (var b in u.UserGroupCustomer)
                {
                    ObjectContext.LoadProperty<UserGroupCustomer>(b, a => a.Customer);
                }
            }
            return this.ObjectContext.UserGroup;
        }

        public void InsertUserGroup(UserGroup userGroup)
        {
            if ((userGroup.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(userGroup, EntityState.Added);
            }
            else
            {
                this.ObjectContext.UserGroup.AddObject(userGroup);
            }
        }

        public void UpdateUserGroup(UserGroup currentUserGroup)
        {
            this.ObjectContext.UserGroup.AttachAsModified(currentUserGroup, this.ChangeSet.GetOriginal(currentUserGroup));
        }

        public void DeleteUserGroup(UserGroup userGroup)
        {
            if ((userGroup.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(userGroup, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.UserGroup.Attach(userGroup);
                this.ObjectContext.UserGroup.DeleteObject(userGroup);
            }
        }
    }
}


