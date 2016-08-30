
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


    // The MetadataTypeAttribute identifies BossMetadata as the class
    // that carries additional metadata for the Boss class.
    [MetadataTypeAttribute(typeof(Boss.BossMetadata))]
    public partial class Boss
    {

        // This class allows you to attach custom attributes to properties
        // of the Boss class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class BossMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private BossMetadata()
            {
            }

            [Include]
            public EntityCollection<Customer> Customer { get; set; }

            public int ID { get; set; }

            public string Name { get; set; }

            public string PinYin { get; set; }
        }
    }
}
