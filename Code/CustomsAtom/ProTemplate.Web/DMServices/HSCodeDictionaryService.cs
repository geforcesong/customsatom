
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
        public IQueryable<HSCodeDictionary> GetHSCodeDictionary()
        {
            return this.ObjectContext.HSCodeDictionary;
        }
    }
}