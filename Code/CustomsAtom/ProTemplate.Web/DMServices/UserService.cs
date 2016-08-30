
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
        // To support paging you will need to add ordering to the 'User' query.
        public IQueryable<User> GetUser()
        {
            foreach (var u in ObjectContext.User)
            {
                ObjectContext.LoadProperty<User>(u, a => a.UserRole);
                foreach (var b in u.UserRole)
                    ObjectContext.LoadProperty<UserRole>(b, a => a.Role);
            }
            return this.ObjectContext.User;
        }

        public User Login(string userAlias, string pwd)
        {
            var query = (from u in ObjectContext.User
                         where u.Alias == userAlias && u.Password == pwd && u.IsActived
                         select u).SingleOrDefault();
            if (query != null)
            {
                ObjectContext.LoadProperty<User>(query, a => a.UserRole);
                foreach (var b in query.UserRole)
                    ObjectContext.LoadProperty<UserRole>(b, a => a.Role);

                return query;
            }
            return null;
        }

        public void InsertUser(User user)
        {
            if ((user.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(user, EntityState.Added);
            }
            else
            {
                this.ObjectContext.User.AddObject(user);
            }
        }

        public void UpdateUser(User currentUser)
        {
            this.ObjectContext.User.AttachAsModified(currentUser, this.ChangeSet.GetOriginal(currentUser));
        }

        public void DeleteUser(User user)
        {
            if ((user.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(user, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.User.Attach(user);
                this.ObjectContext.User.DeleteObject(user);
            }
        }
    }
}


