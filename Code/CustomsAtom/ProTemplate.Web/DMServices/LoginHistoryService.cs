
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
        // To support paging you will need to add ordering to the 'LoginHistory' query.
        public IQueryable<LoginHistory> GetLoginHistory()
        {
            return this.ObjectContext.LoginHistory;
        }

        public IQueryable<LoginHistory> GetLoginHistoryByDate(DateTime from, DateTime to)
        {
            return this.ObjectContext.LoginHistory.Where(o => o.LoginDate >= from && o.LoginDate <= to);
        }

        public void InsertLoginHistory(LoginHistory loginHistory)
        {
            if ((loginHistory.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(loginHistory, EntityState.Added);
            }
            else
            {
                this.ObjectContext.LoginHistory.AddObject(loginHistory);
            }
        }

        public void UpdateLoginHistory(LoginHistory currentLoginHistory)
        {
            this.ObjectContext.LoginHistory.AttachAsModified(currentLoginHistory, this.ChangeSet.GetOriginal(currentLoginHistory));
        }

        public void DeleteLoginHistory(LoginHistory loginHistory)
        {
            if ((loginHistory.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(loginHistory, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.LoginHistory.Attach(loginHistory);
                this.ObjectContext.LoginHistory.DeleteObject(loginHistory);
            }
        }
    }
}


