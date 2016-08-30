
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


    // The MetadataTypeAttribute identifies CustomerMetadata as the class
    // that carries additional metadata for the Customer class.
    [MetadataTypeAttribute(typeof(Customer.CustomerMetadata))]
    public partial class Customer
    {

        // This class allows you to attach custom attributes to properties
        // of the Customer class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class CustomerMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private CustomerMetadata()
            {
            }

            public string Address { get; set; }

            public Boss Boss { get; set; }

            public int BossId { get; set; }

            public string City { get; set; }

            public int ID { get; set; }

            public string Name { get; set; }

            public string PhoneNumber { get; set; }

            public string PinYin { get; set; }

            public string PostalCode { get; set; }

            [Include]
            public EntityCollection<UserGroupCustomer> UserGroupCustomer { get; set; }
        }
    }
}
