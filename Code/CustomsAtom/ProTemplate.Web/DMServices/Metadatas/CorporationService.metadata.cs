
namespace ProTemplate.Web
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.ServiceModel.DomainServices.Hosting;
    using System.ServiceModel.DomainServices.Server;


    // The MetadataTypeAttribute identifies CorporationMetadata as the class
    // that carries additional metadata for the Corporation class.
    [MetadataTypeAttribute(typeof(Corporation.CorporationMetadata))]
    public partial class Corporation
    {

        // This class allows you to attach custom attributes to properties
        // of the Corporation class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class CorporationMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private CorporationMetadata()
            {
            }

            public string CorporationCode { get; set; }

            public string CorporationName { get; set; }

            public string Level { get; set; }
        }
    }
}
