
namespace ProTemplate.Web
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.ServiceModel.DomainServices.Hosting;
    using System.ServiceModel.DomainServices.Server;


    // The MetadataTypeAttribute identifies DeclarationDocumentMetadata as the class
    // that carries additional metadata for the DeclarationDocument class.
    [MetadataTypeAttribute(typeof(DeclarationDocument.DeclarationDocumentMetadata))]
    public partial class DeclarationDocument
    {

        // This class allows you to attach custom attributes to properties
        // of the DeclarationDocument class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class DeclarationDocumentMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private DeclarationDocumentMetadata()
            {
            }

            public string CertificateNumber { get; set; }

            public Declaration Declaration { get; set; }

            public int DeclarationId { get; set; }

            public string Document { get; set; }

            public Document Document1 { get; set; }

            public int Sequence { get; set; }

            public int SortOrder { get; set; }
        }
    }
}
