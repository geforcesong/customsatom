
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


    // The MetadataTypeAttribute identifies DeclarationMetadata as the class
    // that carries additional metadata for the Declaration class.
    [MetadataTypeAttribute(typeof(Declaration.DeclarationMetadata))]
    public partial class Declaration
    {

        // This class allows you to attach custom attributes to properties
        // of the Declaration class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class DeclarationMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private DeclarationMetadata()
            {
            }

            public string AgentCode { get; set; }

            public string AgentName { get; set; }

            public string ApprovalNumber { get; set; }

            public string BillNumber { get; set; }

            public string ContainerQuantity { get; set; }

            public string ControlNumber { get; set; }

            public string Conveyance { get; set; }

            public string CountryCode { get; set; }

            public Nullable<DateTime> CreatedDate { get; set; }
            [Include]
            public Customer Customer { get; set; }

            public int CustomerID { get; set; }

            public string CustomhouseCode { get; set; }

            [Include]
            public EntityCollection<DeclarationContainer> DeclarationContainer { get; set; }

            public Nullable<DateTime> DeclarationDate { get; set; }

            [Include]
            public EntityCollection<DeclarationDocument> DeclarationDocument { get; set; }

            [Include]
            public EntityCollection<DeclarationImage> DeclarationImage { get; set; }

            [Include]
            public EntityCollection<DeclarationItem> DeclarationItem { get; set; }

            [Include]
            public EntityCollection<FinancialExportDeclaration> FinancialExportDeclaration { get; set; }

            public string DeclarationNumber { get; set; }

            public string DeclarationStatus { get; set; }

            public string DeclarationStatusRemark { get; set; }

            public string DistrictCode { get; set; }

            public string Dock { get; set; }

            public EntityCollection<DoubleCheckDeclaration> DoubleCheckDeclaration { get; set; }

            public Nullable<DateTime> DrawbackDate { get; set; }

            public string DrawbackStatus { get; set; }

            public string DrawbackStatusRemark { get; set; }

            public string FreightFeeCurrencyCode { get; set; }

            public string FreightFeeMarkCode { get; set; }

            public Nullable<decimal> FreightFeeRate { get; set; }

            public Nullable<decimal> GrossWeight { get; set; }

            [Key]
            public int ID { get; set; }

            public Nullable<DateTime> IEDate { get; set; }

            public string InsuranceFeeCurrencyCode { get; set; }

            public string InsuranceFeeMarkCode { get; set; }

            public Nullable<decimal> InsuranceFeeRate { get; set; }

            public string LevyCode { get; set; }

            public string LicenseNumber { get; set; }

            public string ManualNumber { get; set; }

            public Nullable<decimal> NetWeight { get; set; }

            public string Note { get; set; }

            public string OtherFeeCurrencyCode { get; set; }

            public string OtherFeeMarkCode { get; set; }

            public Nullable<decimal> OtherFeeRate { get; set; }

            public string OwnerCode { get; set; }

            public string OwnerName { get; set; }

            public Nullable<int> PackageAmount { get; set; }

            public string PayCode { get; set; }

            public string PortCode { get; set; }

            public string PreEntryNumber { get; set; }

            public string PrerecordWarehouseWarrant { get; set; }

            public string ProductNumber { get; set; }

            public DateTime ReceivedDate { get; set; }

            public string RelatedDeclarationNumber { get; set; }

            public string RelatedManualNumber { get; set; }

            public string RelatedSystemNumber { get; set; }

            public string Remark { get; set; }

            public Nullable<DateTime> ShipLeaveDate { get; set; }

            public string TradeCode { get; set; }

            public string TraderCode { get; set; }

            public string TraderName { get; set; }

            public string TransactionCode { get; set; }

            public string TransportCode { get; set; }

            public string VerificationStatus { get; set; }

            public string VerificationStatusDetail { get; set; }

            public string VoyageNumber { get; set; }

            public string WrapCode { get; set; }

            public string FinancialRemark { get; set; }
        }
    }
}
