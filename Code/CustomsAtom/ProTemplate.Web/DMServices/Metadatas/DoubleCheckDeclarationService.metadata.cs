
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


    // The MetadataTypeAttribute identifies DoubleCheckDeclarationMetadata as the class
    // that carries additional metadata for the DoubleCheckDeclaration class.
    [MetadataTypeAttribute(typeof(DoubleCheckDeclaration.DoubleCheckDeclarationMetadata))]
    public partial class DoubleCheckDeclaration
    {

        // This class allows you to attach custom attributes to properties
        // of the DoubleCheckDeclaration class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class DoubleCheckDeclarationMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private DoubleCheckDeclarationMetadata()
            {
            }

            public string ApprovalNumber { get; set; }

            public string ContainerNumbers { get; set; }

            public string CountryName { get; set; }

            public string CustomerName { get; set; }

            public string CustomhouseName { get; set; }

            public Declaration Declaration { get; set; }

            public int DeclarationId { get; set; }

            public string DeclarationNumber { get; set; }

            public string DistrictName { get; set; }

            public string DocumentCodes { get; set; }
            [Include]
            public EntityCollection<DoubleCheckDeclarationItem> DoubleCheckDeclarationItem { get; set; }

            public string FeightFeeRate { get; set; }

            public string GrossWeight { get; set; }
            [Key]
            public int ID { get; set; }

            public string InsuranceFeeRate { get; set; }

            public string LicenseNumber { get; set; }

            public string MachineName { get; set; }

            public string ManualNumber { get; set; }

            public string NetWeight { get; set; }

            public string PackageAmount { get; set; }

            public string TradeName { get; set; }

            public string TransactionName { get; set; }

            public string WrapName { get; set; }
        }
    }
}
