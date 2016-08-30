
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

    //public class TempDeclaration
    //{
    //    public int ID { get; set; }
    //    public int Count { get; set; }
    //    //internal sealed class TempDeclarationMetadata
    //    //{

    //    //    // Metadata classes are not meant to be instantiated.
    //    //    private TempDeclarationMetadata()
    //    //    {
    //    //    }

    //    //    public int ID {get;set;}
    //    //    public int Count{get;set;}
    //    //}
    //}

    [MetadataTypeAttribute(typeof(TempDeclaration.TempDeclarationMetadata))]
    public partial class TempDeclaration
    {
        internal sealed class TempDeclarationMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private TempDeclarationMetadata()
            {
            }

            [Key]
            public int ID { get; set; }
        }
    }
}