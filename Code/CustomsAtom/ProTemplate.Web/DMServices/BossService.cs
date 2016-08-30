
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
        // To support paging you will need to add ordering to the 'Boss' query.
        public IQueryable<Boss> GetBoss()
        {
            return this.ObjectContext.Boss;
        }

        public bool BossNameExisted(string bossName, int ID)
        {
            return ObjectContext.Boss.Any(o => o.Name == bossName && o.ID != ID);
        }

        public void InsertBoss(Boss boss)
        {
            if ((boss.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(boss, EntityState.Added);
            }
            else
            {
                this.ObjectContext.Boss.AddObject(boss);
            }
        }

        public void UpdateBoss(Boss currentBoss)
        {
            this.ObjectContext.Boss.AttachAsModified(currentBoss, this.ChangeSet.GetOriginal(currentBoss));
        }

        public void DeleteBoss(Boss boss)
        {
            if ((boss.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(boss, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.Boss.Attach(boss);
                this.ObjectContext.Boss.DeleteObject(boss);
            }
        }
    }
}


