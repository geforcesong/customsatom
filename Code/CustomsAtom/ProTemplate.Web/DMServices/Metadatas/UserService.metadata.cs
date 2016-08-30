
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


    // The MetadataTypeAttribute identifies UserMetadata as the class
    // that carries additional metadata for the User class.
    [MetadataTypeAttribute(typeof(User.UserMetadata))]
    public partial class User
    {

        // This class allows you to attach custom attributes to properties
        // of the User class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class UserMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private UserMetadata()
            {
            }

            public string Alias { get; set; }

            public string Comment { get; set; }

            public DateTime CreatedDate { get; set; }

            public string Email { get; set; }

            public int Id { get; set; }

            public bool IsActived { get; set; }

            public Nullable<DateTime> LastLoginDate { get; set; }

            public Nullable<DateTime> LastPasswordChangedDate { get; set; }

            public string Mobile { get; set; }

            public int ModifiedByUserId { get; set; }

            public DateTime ModifiedDate { get; set; }

            public string Name { get; set; }

            public string Password { get; set; }

            public UserGroup UserGroup { get; set; }

            public int UserGroupId { get; set; }

            [Include]
            public EntityCollection<UserRole> UserRole { get; set; }
        }
    }
}
