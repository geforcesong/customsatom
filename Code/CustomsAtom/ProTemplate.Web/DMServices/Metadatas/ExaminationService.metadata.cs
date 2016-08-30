
namespace ProTemplate.Web
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.ServiceModel.DomainServices.Hosting;
    using System.ServiceModel.DomainServices.Server;


    // The MetadataTypeAttribute identifies ExaminationMetadata as the class
    // that carries additional metadata for the Examination class.
    [MetadataTypeAttribute(typeof(Examination.ExaminationMetadata))]
    public partial class Examination
    {

        // This class allows you to attach custom attributes to properties
        // of the Examination class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class ExaminationMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private ExaminationMetadata()
            {
            }

            public Nullable<decimal> Amount { get; set; }

            public string ApprovedNumber { get; set; }
            [Include]
            public Customer Customer { get; set; }

            public int CustomerID { get; set; }

            public string DeclarationNumber { get; set; }

            public string ExaminationNumber { get; set; }

            public string ExaminationStatus { get; set; }

            public string GoodsName { get; set; }

            public int ID { get; set; }

            public string Password { get; set; }

            public Nullable<decimal> Quantity { get; set; }

            public DateTime ReceiveDate { get; set; }

            public string RelatedSystemNumber { get; set; }

            public string Remark { get; set; }

            public string TransferNumber { get; set; }

            public decimal ExaminationFee { get; set; }

            public string IsRelated { get; set; }
        }
    }
}
