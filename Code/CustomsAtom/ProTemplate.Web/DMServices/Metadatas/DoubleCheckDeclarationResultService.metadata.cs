
namespace ProTemplate.Web
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.ServiceModel.DomainServices.Hosting;
    using System.ServiceModel.DomainServices.Server;


    // The MetadataTypeAttribute identifies DoubleCheckDeclarationItemMetadata as the class
    // that carries additional metadata for the DoubleCheckDeclarationItem class.
    [MetadataTypeAttribute(typeof(DoubleCheckDeclarationVarifyResult.DoubleCheckDeclarationVarifyResultMetadata))]
    public partial class DoubleCheckDeclarationVarifyResult
    {
        internal sealed class DoubleCheckDeclarationVarifyResultMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private DoubleCheckDeclarationVarifyResultMetadata()
            {
            }
            public string DeclarationNumber { get; set; }
            [Key]
            public int ID { get; set; }
        }
    }
}
