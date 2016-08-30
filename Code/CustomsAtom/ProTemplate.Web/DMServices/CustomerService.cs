
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
        // To support paging you will need to add ordering to the 'Customer' query.
        public List<int> GetCustomerID(int UserID)
        {
            var query = from c in this.ObjectContext.User
                        join d in this.ObjectContext.UserGroup on c.UserGroupId equals d.Id
                        join e in this.ObjectContext.UserGroupCustomer on d.Id equals e.UserGroupId
                        where c.Id == UserID
                        select e.CustomerId;

            return query.ToList();
        }

        public IQueryable<Customer> GetCustomer()
        {
            return this.ObjectContext.Customer;
        }

        public void InsertCustomer(Customer customer)
        {
            if ((customer.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(customer, EntityState.Added);
            }
            else
            {
                this.ObjectContext.Customer.AddObject(customer);
            }
        }

        public void UpdateCustomer(Customer currentCustomer)
        {
            this.ObjectContext.Customer.AttachAsModified(currentCustomer, this.ChangeSet.GetOriginal(currentCustomer));
        }

        public void DeleteCustomer(Customer customer)
        {
            if ((customer.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(customer, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.Customer.Attach(customer);
                this.ObjectContext.Customer.DeleteObject(customer);
            }
        }
    }
}


