
namespace ProTemplate.Web
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Data.Objects.DataClasses;
    using System.Linq;
    using System.ServiceModel.DomainServices.Hosting;
    using System.ServiceModel.DomainServices.Server;


    // The MetadataTypeAttribute identifies RoleMetadata as the class
    // that carries additional metadata for the Role class.
    [MetadataTypeAttribute(typeof(Role.RoleMetadata))]
    public partial class Role
    {

        // This class allows you to attach custom attributes to properties
        // of the Role class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class RoleMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private RoleMetadata()
            {
            }

            public DateTime CreatedDate { get; set; }

            public int Id { get; set; }

            public int ModifiedByUserId { get; set; }

            public DateTime ModifiedDate { get; set; }

            public string Name { get; set; }

            [Include]
            public EntityCollection<RoleAccess> RoleAccess { get; set; }

            [Include]
            public EntityCollection<UserRole> UserRole { get; set; }
        }
    }
}
