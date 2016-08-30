
namespace ProTemplate.Web
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.ServiceModel.DomainServices.Hosting;
    using System.ServiceModel.DomainServices.Server;


    // The MetadataTypeAttribute identifies FinancialExportDeclarationMetadata as the class
    // that carries additional metadata for the FinancialExportDeclaration class.
    [MetadataTypeAttribute(typeof(FinancialExportDeclaration.FinancialExportDeclarationMetadata))]
    public partial class FinancialExportDeclaration
    {

        // This class allows you to attach custom attributes to properties
        // of the FinancialExportDeclaration class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class FinancialExportDeclarationMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private FinancialExportDeclarationMetadata()
            {
            }

            public decimal Amount { get; set; }

            public decimal Cost { get; set; }

            public DateTime CreatedDate { get; set; }

            public Declaration Declaration { get; set; }

            public int DeclarationId { get; set; }

            public string FeeTypeCode { get; set; }
            [Include]
            public FeeType FeeType { get; set; }

            public string FinancialRemark { get; set; }

            public int ID { get; set; }
        }
    }
}
