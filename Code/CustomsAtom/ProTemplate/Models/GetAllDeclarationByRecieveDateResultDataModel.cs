using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace ProTemplate.Models
{
    public class GetAllDeclarationByReceiveDateResultDataModel : DataModel
    {
        public GetAllDeclarationByReceiveDateResultDataModel()
        {
            IsLocked = false;
        }
        public int Index { get; set; }
        public int ID { get; set; }

        private string _customerName;
        public string CustomerName { get { return _customerName; }
            set
            {
                if (!IsLocked)
                    _customerName = value;
                NotifyPropertyChanged("CustomerName");
            }
        }

        private DateTime? _receivedDate;
        public DateTime? ReceivedDate { get { return _receivedDate; }
            set {
                if (!IsLocked)
                    _receivedDate = value;
                NotifyPropertyChanged("ReceivedDate");
            }
        }

        private string _declarationNumber;
        public string DeclarationNumber { get { return _declarationNumber; } 
            set {
                if (!IsLocked)
                    _declarationNumber = value;
                NotifyPropertyChanged("DeclarationNumber");
            } }

        private string _manualNumber;
        public string ManualNumber
        {
            get { return _manualNumber; }
            set {
                if (!IsLocked)
                    _manualNumber = value;
                NotifyPropertyChanged("ManualNumber");
            }
        }

        private string _traderName;
        public string TraderName
        {
            get
            {
                return _traderName;
            }
            set
            {
                if (!IsLocked)
                    _traderName = value;
                NotifyPropertyChanged("TraderName");
            }
        }

        private string _conveyance;
        public string Conveyance
        {
            get { return _conveyance; }
            set
            {
                if (!IsLocked)
                    _conveyance = value;
                NotifyPropertyChanged("Conveyance");
            }
        }

        private string _voyageNumber;
        public string VoyageNumber
        {
            get { return _voyageNumber; }
            set
            {
                if (!IsLocked)
                    _voyageNumber = value;
                NotifyPropertyChanged("VoyageNumber");
            }
        }

        private string _billNumber;
        public string BillNumber
        {
            get { return _billNumber; }
            set
            {
                if (!IsLocked)
                    _billNumber = value;
                NotifyPropertyChanged("BillNumber");
            }
        }

        private string _tradeName;
        public string TradeName
        {
            get { return _tradeName; }
            set
            {
                if (!IsLocked)
                    _tradeName = value;
                NotifyPropertyChanged("TradeName");
            }
        }

        private string _approvalNumber;
        public string ApprovalNumber
        {
            get { return _approvalNumber; }
            set
            {
                if (!IsLocked)
                    _approvalNumber = value;
                NotifyPropertyChanged("ApprovalNumber");
            }
        }

        public int? PackageAmount { get; set; }
        public decimal? GrossWeight { get; set; }
        public string PrerecordWarehouseWarrant { get; set; }
        public DateTime? CreatedDate { get; set; }
        private string _declarationStatus;
        public string DeclarationStatus
        {
            get
            {
                return _declarationStatus;
            }
            set
            {
                _declarationStatus = value;
                NotifyPropertyChanged("DeclarationStatus");
            }
        }
        public string DeclarationStatusRemark { get; set; }
        public DateTime? DeclarationDate { get; set; }
        public string _drawbackStatus;
        public string DrawbackStatus
        {
            get
            {
                return _drawbackStatus;
            }
            set
            {
                _drawbackStatus = value;
                NotifyPropertyChanged("DrawbackStatus");
            }
        }
        public string DrawbackStatusRemark { get; set; }
        public DateTime? DrawbackDate { get; set; }
        public string VerificationStatus { get; set; }
        public string VerificationStatusDetail { get; set; }
        public int TotalItems { get; set; }
        public int TotalContainers { get; set; }
        public string Dock { get; set; }
        public DateTime? ShipLeaveDate { get; set; }
        public string RelatedSystemNumber { get; set; }
        public string Remark { get; set; }
        public string LadingStatus { get; set; }
        public string AdmissionStatus { get; set; }
        public string HasExamination { get; set; }
        public string ExaminationNumber { get; set; }
        public int BillCount { get; set; }
        public bool IsLocked { get; set; }
        public string ContainerNumbers { get; set; }
        /*
        public string PreEntryNumber { get; set; }
        public string ControlNumber { get; set; }
        public DateTime? IEDate { get; set; }
        public string TraderCode { get; set; }
        public string OwnerCode { get; set; }
        public string OwnerName { get; set; }
        public string AgentCode { get; set; }
        public string AgentName { get; set; }
        public string LicenseNumber { get; set; }
        public decimal? FreightFeeRate { get; set; }
        public decimal? InsuranceFeeRate { get; set; }
        public decimal? OtherFeeRate { get; set; }
        public decimal? NetWeight { get; set; }
        public string ContainerQuantity { get; set; }
        public string RelatedDeclarationNumber { get; set; }
        public string RelatedManualNumber { get; set; }
        public string ProductNumber { get; set; }
        public string Note { get; set; }
        public string TrasnsportName { get; set; }
        public string LevyName { get; set; }
        public string PayName { get; set; }
        public string CountryName { get; set; }
        public string PortName { get; set; }
        public string DistrictName { get; set; }
        public string CustomhouseName { get; set; }
        public string TransactionName { get; set; }
        public string FreightFeeMarkName { get; set; }
        public string FreightFeeCurrencyName { get; set; }
        public string InsuranceFeeMarkName { get; set; }
        public string InsuranceFeeCurrencyName { get; set; }
        public string OtherFeeMarkName { get; set; }
        public string OtherFeeCurrencyName { get; set; }
        public string WrapName { get; set; }
        */
    }
}
