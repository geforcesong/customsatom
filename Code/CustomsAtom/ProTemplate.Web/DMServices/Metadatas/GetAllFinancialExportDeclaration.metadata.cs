using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ProTemplate.Web
{
    [MetadataTypeAttribute(typeof(GetAllFinancialDeclaration.GetAllFinancialDeclarationMetadata))]
    public partial class GetAllFinancialDeclaration
    {
        internal sealed class GetAllFinancialDeclarationMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private GetAllFinancialDeclarationMetadata()
            {
            }

            [Key]
            public int ID { get; set; }
        }
    }
}