
namespace ProTemplate.Web
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.ServiceModel.DomainServices.Hosting;
    using System.ServiceModel.DomainServices.Server;


    // The MetadataTypeAttribute identifies DoubleCheckDeclarationItemMetadata as the class
    // that carries additional metadata for the DoubleCheckDeclarationItem class.
    [MetadataTypeAttribute(typeof(DoubleCheckDeclarationItem.DoubleCheckDeclarationItemMetadata))]
    public partial class DoubleCheckDeclarationItem
    {

        // This class allows you to attach custom attributes to properties
        // of the DoubleCheckDeclarationItem class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class DoubleCheckDeclarationItemMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private DoubleCheckDeclarationItemMetadata()
            {
            }

            public string ControlNumber { get; set; }

            public string CurrencyName { get; set; }

            public string DeclaredQuantity { get; set; }

            public string DeclaredUnitName { get; set; }

            public DoubleCheckDeclaration DoubleCheckDeclaration { get; set; }

            public int DoubleCheckDeclarationId { get; set; }

            public string FirstQuantity { get; set; }

            public string FirstUnitName { get; set; }

            public string HSCode { get; set; }
            [Key]
            public int ID { get; set; }

            public string Name { get; set; }

            public string SecondQuantity { get; set; }

            public string SecondUnitName { get; set; }

            public int SortOrder { get; set; }

            public string TotalAmount { get; set; }
        }
    }
}
