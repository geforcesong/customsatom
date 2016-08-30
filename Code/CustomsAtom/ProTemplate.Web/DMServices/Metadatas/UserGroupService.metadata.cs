
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


    // The MetadataTypeAttribute identifies UserGroupMetadata as the class
    // that carries additional metadata for the UserGroup class.
    [MetadataTypeAttribute(typeof(UserGroup.UserGroupMetadata))]
    public partial class UserGroup
    {

        // This class allows you to attach custom attributes to properties
        // of the UserGroup class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class UserGroupMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private UserGroupMetadata()
            {
            }

            public DateTime CreatedDate { get; set; }

            public string GroupName { get; set; }

            public int Id { get; set; }

            public int ModifiedBy { get; set; }

            public DateTime ModifiedDate { get; set; }

            [Include]
            public EntityCollection<User> User { get; set; }

            [Include]
            public EntityCollection<UserGroupCustomer> UserGroupCustomer { get; set; }
        }
    }
}
