
namespace ProTemplate.Web
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.ServiceModel.DomainServices.Hosting;
    using System.ServiceModel.DomainServices.Server;


    // The MetadataTypeAttribute identifies DeclarationImageMetadata as the class
    // that carries additional metadata for the DeclarationImage class.
    [MetadataTypeAttribute(typeof(DeclarationImage.DeclarationImageMetadata))]
    public partial class DeclarationImage
    {

        // This class allows you to attach custom attributes to properties
        // of the DeclarationImage class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class DeclarationImageMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private DeclarationImageMetadata()
            {
            }

            public Declaration Declaration { get; set; }

            public int DeclarationId { get; set; }

            public string ScanImageName { get; set; }

            public int Sequence { get; set; }
        }
    }
}
