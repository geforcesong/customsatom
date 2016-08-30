
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
        // To support paging you will need to add ordering to the 'UserGroupCustomer' query.
        public IQueryable<UserGroupCustomer> GetUserGroupCustomer()
        {
            return this.ObjectContext.UserGroupCustomer;
        }

        public void InsertUserGroupCustomer(UserGroupCustomer userGroupCustomer)
        {
            if ((userGroupCustomer.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(userGroupCustomer, EntityState.Added);
            }
            else
            {
                this.ObjectContext.UserGroupCustomer.AddObject(userGroupCustomer);
            }
        }

        public void UpdateUserGroupCustomer(UserGroupCustomer currentUserGroupCustomer)
        {
            this.ObjectContext.UserGroupCustomer.AttachAsModified(currentUserGroupCustomer, this.ChangeSet.GetOriginal(currentUserGroupCustomer));
        }

        public void DeleteUserGroupCustomer(UserGroupCustomer userGroupCustomer)
        {
            if ((userGroupCustomer.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(userGroupCustomer, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.UserGroupCustomer.Attach(userGroupCustomer);
                this.ObjectContext.UserGroupCustomer.DeleteObject(userGroupCustomer);
            }
        }
    }
}


