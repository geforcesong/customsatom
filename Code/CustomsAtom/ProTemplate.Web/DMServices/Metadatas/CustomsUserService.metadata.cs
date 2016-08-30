using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ProTemplate.Web.DMServices.Metadatas
{
    [MetadataTypeAttribute(typeof(CustomsUserService.CustomsUserMetadata))]
    public partial class CustomsUserService
    {
        internal sealed class CustomsUserMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private CustomsUserMetadata()
            {
            }

            public int ID { get; set; }

            public string Name { get; set; }

            public string CustomsNo { get; set; }

            public string IdentityNo { get; set; }
        }
    }
}