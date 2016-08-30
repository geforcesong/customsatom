
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
        // To support paging you will need to add ordering to the 'MachineNameIPMapping' query.
        public IQueryable<MachineNameIPMapping> GetMachineNameIPMapping()
        {
            return this.ObjectContext.MachineNameIPMapping;
        }

        public void InsertMachineNameIPMapping(MachineNameIPMapping machineNameIPMapping)
        {
            if ((machineNameIPMapping.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(machineNameIPMapping, EntityState.Added);
            }
            else
            {
                this.ObjectContext.MachineNameIPMapping.AddObject(machineNameIPMapping);
            }
        }

        public void UpdateMachineNameIPMapping(MachineNameIPMapping currentMachineNameIPMapping)
        {
            this.ObjectContext.MachineNameIPMapping.AttachAsModified(currentMachineNameIPMapping, this.ChangeSet.GetOriginal(currentMachineNameIPMapping));
        }

        public void DeleteMachineNameIPMapping(MachineNameIPMapping machineNameIPMapping)
        {
            if ((machineNameIPMapping.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(machineNameIPMapping, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.MachineNameIPMapping.Attach(machineNameIPMapping);
                this.ObjectContext.MachineNameIPMapping.DeleteObject(machineNameIPMapping);
            }
        }
    }
}


