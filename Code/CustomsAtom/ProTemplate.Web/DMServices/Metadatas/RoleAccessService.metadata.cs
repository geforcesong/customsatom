
namespace ProTemplate.Web
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.ServiceModel.DomainServices.Hosting;
    using System.ServiceModel.DomainServices.Server;


    // The MetadataTypeAttribute identifies RoleAccessMetadata as the class
    // that carries additional metadata for the RoleAccess class.
    [MetadataTypeAttribute(typeof(RoleAccess.RoleAccessMetadata))]
    public partial class RoleAccess
    {

        // This class allows you to attach custom attributes to properties
        // of the RoleAccess class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class RoleAccessMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private RoleAccessMetadata()
            {
            }

            public bool CanCreate { get; set; }

            public bool CanDelete { get; set; }

            public bool CanRead { get; set; }

            public bool CanUpdate { get; set; }

            public Role Role { get; set; }

            public int RoleId { get; set; }

            public UIPage UIPage { get; set; }

            public int UIPageId { get; set; }
        }
    }
}
