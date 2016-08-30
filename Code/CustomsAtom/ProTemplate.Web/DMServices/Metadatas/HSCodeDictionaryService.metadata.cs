
namespace ProTemplate.Web
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.ServiceModel.DomainServices.Hosting;
    using System.ServiceModel.DomainServices.Server;


    // The MetadataTypeAttribute identifies HSCodeDictionaryMetadata as the class
    // that carries additional metadata for the HSCodeDictionary class.
    [MetadataTypeAttribute(typeof(HSCodeDictionary.HSCodeDictionaryMetadata))]
    public partial class HSCodeDictionary
    {

        // This class allows you to attach custom attributes to properties
        // of the HSCodeDictionary class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class HSCodeDictionaryMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private HSCodeDictionaryMetadata()
            {
            }

            public string Code { get; set; }

            public string DeclarationFactor { get; set; }

            public string FirstUnitName { get; set; }

            public int ID { get; set; }

            public string ManagementName { get; set; }

            public string Name { get; set; }

            public string SecondUnitName { get; set; }
        }
    }
}
