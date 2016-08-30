
namespace ProTemplate.Web
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.ServiceModel.DomainServices.Hosting;
    using System.ServiceModel.DomainServices.Server;


    // The MetadataTypeAttribute identifies UserGroupCustomerMetadata as the class
    // that carries additional metadata for the UserGroupCustomer class.
    [MetadataTypeAttribute(typeof(UserGroupCustomer.UserGroupCustomerMetadata))]
    public partial class UserGroupCustomer
    {

        // This class allows you to attach custom attributes to properties
        // of the UserGroupCustomer class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class UserGroupCustomerMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private UserGroupCustomerMetadata()
            {
            }

            [Include]
            public Customer Customer { get; set; }

            public int CustomerId { get; set; }

            public int ID { get; set; }

            public UserGroup UserGroup { get; set; }

            public int UserGroupId { get; set; }
        }
    }
}
