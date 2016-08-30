
namespace ProTemplate.Web
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.ServiceModel.DomainServices.Hosting;
    using System.ServiceModel.DomainServices.Server;


    // The MetadataTypeAttribute identifies UserRoleMetadata as the class
    // that carries additional metadata for the UserRole class.
    [MetadataTypeAttribute(typeof(UserRole.UserRoleMetadata))]
    public partial class UserRole
    {

        // This class allows you to attach custom attributes to properties
        // of the UserRole class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class UserRoleMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private UserRoleMetadata()
            {
            }

            public int ID { get; set; }

            public Role Role { get; set; }

            public int RoleId { get; set; }

            public User User { get; set; }

            public int UserId { get; set; }
        }
    }
}
