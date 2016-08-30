
namespace ProTemplate.Web
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.ServiceModel.DomainServices.Hosting;
    using System.ServiceModel.DomainServices.Server;


    // The MetadataTypeAttribute identifies CustomerFeeSettingMetadata as the class
    // that carries additional metadata for the CustomerFeeSetting class.
    [MetadataTypeAttribute(typeof(CustomerFeeSetting.CustomerFeeSettingMetadata))]
    public partial class CustomerFeeSetting
    {

        // This class allows you to attach custom attributes to properties
        // of the CustomerFeeSetting class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class CustomerFeeSettingMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private CustomerFeeSettingMetadata()
            {
            }

            public decimal Amount { get; set; }

            public decimal Cost { get; set; }

            public Customer Customer { get; set; }

            public int CustomerID { get; set; }

            public string FeeTypeCode { get; set; }
            [Include]
            public FeeType FeeType { get; set; }
        }
    }
}
