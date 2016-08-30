
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
        // To support paging you will need to add ordering to the 'CustomerFeeSetting' query.
        public IQueryable<CustomerFeeSetting> GetCustomerFeeSettingByCustomerID(int CustomerID)
        {

            return this.ObjectContext.CustomerFeeSetting.Include("FeeType").Where(o => o.CustomerID == CustomerID);
        }

        public IQueryable<CustomerFeeSetting> GetCustomerFeeSetting()
        {

            return this.ObjectContext.CustomerFeeSetting.Include("FeeType");
        }

        public void InsertCustomerFeeSetting(CustomerFeeSetting customerFeeSetting)
        {
            if ((customerFeeSetting.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(customerFeeSetting, EntityState.Added);
            }
            else
            {
                this.ObjectContext.CustomerFeeSetting.AddObject(customerFeeSetting);
            }
        }

        public void UpdateCustomerFeeSetting(CustomerFeeSetting currentCustomerFeeSetting)
        {
            this.ObjectContext.CustomerFeeSetting.AttachAsModified(currentCustomerFeeSetting, this.ChangeSet.GetOriginal(currentCustomerFeeSetting));
        }

        public void DeleteCustomerFeeSetting(CustomerFeeSetting customerFeeSetting)
        {
            if ((customerFeeSetting.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(customerFeeSetting, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.CustomerFeeSetting.Attach(customerFeeSetting);
                this.ObjectContext.CustomerFeeSetting.DeleteObject(customerFeeSetting);
            }
        }
    }
}


