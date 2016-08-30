
namespace ProTemplate.Web
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.ServiceModel.DomainServices.Hosting;
    using System.ServiceModel.DomainServices.Server;


    // The MetadataTypeAttribute identifies DeclarationContainerMetadata as the class
    // that carries additional metadata for the DeclarationContainer class.
    [MetadataTypeAttribute(typeof(DeclarationContainer.DeclarationContainerMetadata))]
    public partial class DeclarationContainer
    {

        // This class allows you to attach custom attributes to properties
        // of the DeclarationContainer class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class DeclarationContainerMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private DeclarationContainerMetadata()
            {
            }

            public string AdmissionStatus { get; set; }

            public Declaration Declaration { get; set; }

            public int DeclarationId { get; set; }

            public string LadingStatus { get; set; }

            public string Model { get; set; }

            public string Number { get; set; }

            public int Sequence { get; set; }

            public int SortOrder { get; set; }

            public string Weight { get; set; }
        }
    }
}
