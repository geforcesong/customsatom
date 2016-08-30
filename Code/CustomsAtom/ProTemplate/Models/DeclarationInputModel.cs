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
    public class DeclarationInputModel : DataModel
    {
        public int ID { get; set; }
        public int DeclarationId { get; set; }
        public DateTime ReceivedDate { get; set; }

        private string _customerName;
        public string CustomerName
        {
            get { return _customerName; }
            set
            {
                _customerName = value;
                NotifyPropertyChanged("CustomerName");
            }
        }

        private string _declarationNumber;
        public string DeclarationNumber
        {
            get
            {
                return _declarationNumber;
            }
            set
            {
                _declarationNumber = value;
                NotifyPropertyChanged("DeclarationNumber");
            }
        }

        private string _approvalNumber;
        public string ApprovalNumber
        {
            get
            {
                return _approvalNumber;
            }
            set
            {
                _approvalNumber = value;
                NotifyPropertyChanged("ApprovalNumber");
            }
        }

        private string _transactionName;
        public string TransactionName
        {
            get
            {
                return _transactionName;
            }
            set
            {
                _transactionName = value;
                NotifyPropertyChanged("TransactionName");
            }
        }

        private string _districtName;
        public string DistrictName
        {
            get { return _districtName; }
            set
            {
                _districtName = value;
                NotifyPropertyChanged("DistrictName");
            }
        }

        private string _tradeName;
        public string TradeName
        {
            get { return _tradeName; }
            set
            {
                _tradeName = value;
                NotifyPropertyChanged("TradeName");
            }
        }

        private string _feightFeeRate;
        public string FeightFeeRate
        {
            get { return _feightFeeRate; }
            set
            {
                _feightFeeRate = value;
                NotifyPropertyChanged("FeightFeeRate");
            }
        }

        private string _insuranceFeeRate;
        public string InsuranceFeeRate
        {
            get { return _insuranceFeeRate; }
            set
            {
                _insuranceFeeRate = value;
                NotifyPropertyChanged("InsuranceFeeRate");
            }
        }

        private string _customhouseName;
        public string CustomhouseName
        {
            get { return _customhouseName; }
            set
            {
                _customhouseName = value;
                NotifyPropertyChanged("CustomhouseName");
            }
        }

        private string _countryName;
        public string CountryName
        {
            get { return _countryName; }
            set
            {
                _countryName = value;
                NotifyPropertyChanged("CountryName");
            }
        }

        public string ManualNumber { get; set; }
        public string LicenseNumber { get; set; }
        public string PackageAmount { get; set; }
        public string ContractNumber { get; set; }
        public string GrossWeight { get; set; }
        public string NetWeight { get; set; }
        public string ContainerNumbers { get; set; }
        public string DocumentCodes { get; set; }
        public string ExaminationNumber { get; set; }
        public string PayName { get; set; }
        public string RelatedSystemNumber { get; set; }
        public string InsureFeeCurrencyName { get; set; }
        public string FreightFeeCurrencyName { get; set; }
    }
}
