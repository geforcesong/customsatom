
namespace ProTemplate.Web
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.ServiceModel.DomainServices.Hosting;
    using System.ServiceModel.DomainServices.Server;


    // The MetadataTypeAttribute identifies DeclarationItemMetadata as the class
    // that carries additional metadata for the DeclarationItem class.
    [MetadataTypeAttribute(typeof(DeclarationItem.DeclarationItemMetadata))]
    public partial class DeclarationItem
    {

        // This class allows you to attach custom attributes to properties
        // of the DeclarationItem class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class DeclarationItemMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private DeclarationItemMetadata()
            {
            }

            public string ControlItem { get; set; }

            public string CountryCode { get; set; }

            public string CurrencyCode { get; set; }

            public int DeclarationId { get; set; }

            public Nullable<decimal> DeclaredPrice { get; set; }

            public Nullable<decimal> DeclaredQuantity { get; set; }

            public Nullable<decimal> DeclaredTotalPrice { get; set; }

            public string DeclaredUnitCode { get; set; }

            public string DutyCode { get; set; }

            public Nullable<decimal> LegalQuantity { get; set; }

            public string LegalUnitCode { get; set; }

            public string Model { get; set; }

            public string Name { get; set; }

            public string Number { get; set; }

            public string ProductNumber { get; set; }

            public string Purpose { get; set; }

            public Nullable<decimal> SecondQuantity { get; set; }

            public string SecondUnitCode { get; set; }

            public int Sequence { get; set; }

            public int SortOrder { get; set; }

            public string SubNumber { get; set; }

            public string VersionNumber { get; set; }

            public Nullable<decimal> WorkFee { get; set; }
        }
    }
}
