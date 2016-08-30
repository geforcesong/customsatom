
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


    // The MetadataTypeAttribute identifies FeeTypeMetadata as the class
    // that carries additional metadata for the FeeType class.
    [MetadataTypeAttribute(typeof(FeeType.FeeTypeMetadata))]
    public partial class FeeType
    {

        // This class allows you to attach custom attributes to properties
        // of the FeeType class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class FeeTypeMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private FeeTypeMetadata()
            {
            }

            public decimal Amount { get; set; }

            public string Code { get; set; }

            public decimal Cost { get; set; }

            public EntityCollection<CustomerFeeSetting> CustomerFeeSetting { get; set; } 

            public EntityCollection<FinancialExportDeclaration> FinancialExportDeclaration { get; set; }

            public string Name { get; set; }
        }
    }
}
