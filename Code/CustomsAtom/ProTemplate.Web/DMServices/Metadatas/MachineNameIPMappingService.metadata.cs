
namespace ProTemplate.Web
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.ServiceModel.DomainServices.Hosting;
    using System.ServiceModel.DomainServices.Server;


    // The MetadataTypeAttribute identifies MachineNameIPMappingMetadata as the class
    // that carries additional metadata for the MachineNameIPMapping class.
    [MetadataTypeAttribute(typeof(MachineNameIPMapping.MachineNameIPMappingMetadata))]
    public partial class MachineNameIPMapping
    {

        // This class allows you to attach custom attributes to properties
        // of the MachineNameIPMapping class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class MachineNameIPMappingMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private MachineNameIPMappingMetadata()
            {
            }

            public int ID { get; set; }

            public string MachineIP { get; set; }

            public string MachineName { get; set; }
        }
    }
}
