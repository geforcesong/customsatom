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
    public class DeclarationItemDataModel : DataModel
    {
        public int Sequence { get; set; }
        public int DeclarationId { get; set; }

        private int _index;
        public int Index
        {
            get
            {
                return _index;
            }
            set
            {
                _index = value;
                NotifyPropertyChanged("Index");
            }
        }

        public int SortOrder { get; set; }

        private string _number;
        public string Number
        {
            get
            {
                return _number;
            }
            set
            {
                _number = value;
                NotifyPropertyChanged("Number");
            }
        }

        private string _subNumber;
        public string SubNumber
        {
            get
            {
                return _subNumber;
            }
            set
            {
                _subNumber = value;
                NotifyPropertyChanged("SubNumber");
            }
        }

        private string _name;
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                NotifyPropertyChanged("Name");
            }
        }

        private string _model;
        public string Model
        {
            get
            {
                return _model;
            }
            set
            {
                _model = value;
                NotifyPropertyChanged("Model");
            }
        }

        private string _currencyCode;
        public string CurrencyCode
        {
            get
            {
                return _currencyCode;
            }
            set
            {
                _currencyCode = value;
                NotifyPropertyChanged("CurrencyCode");
            }
        }

        private string _currencyName;
        public string CurrencyName
        {
            get
            {
                return _currencyName;
            }
            set
            {
                _currencyName = value;
                NotifyPropertyChanged("CurrencyName");
            }
        }

        private decimal _declaredQuantity;
        public decimal DeclaredQuantity
        {
            get
            {
                return _declaredQuantity;
            }
            set
            {
                _declaredQuantity = value;
                NotifyPropertyChanged("DeclaredQuantity");
            }
        }

        private string _declaredUnitCode;
        public string DeclaredUnitCode
        {
            get
            {
                return _declaredUnitCode;
            }
            set
            {
                _declaredUnitCode = value;
                NotifyPropertyChanged("DeclaredUnitCode");
            }
        }

        private string _declaredUnitName;
        public string DeclaredUnitName
        {
            get
            {
                return _declaredUnitName;
            }
            set
            {
                _declaredUnitName = value;
                NotifyPropertyChanged("DeclaredUnitName");
            }
        }


        private decimal _declaredPrice;
        public decimal DeclaredPrice
        {
            get
            {
                return _declaredPrice;
            }
            set
            {
                _declaredPrice = value;
                NotifyPropertyChanged("DeclaredPrice");
            }
        }

        private decimal _declaredTotalPrice;
        public decimal DeclaredTotalPrice
        {
            get
            {
                return _declaredTotalPrice;
            }
            set
            {
                _declaredTotalPrice = value;
                NotifyPropertyChanged("DeclaredTotalPrice");
            }
        }

        private decimal _legalQuantity;
        public decimal LegalQuantity
        {
            get
            {
                return _legalQuantity;
            }
            set
            {
                _legalQuantity = value;
                NotifyPropertyChanged("LegalQuantity");
            }
        }

        private string _legalUnitCode;
        public string LegalUnitCode
        {
            get
            {
                return _legalUnitCode;
            }
            set
            {
                _legalUnitCode = value;
                NotifyPropertyChanged("LegalUnitCode");
            }
        }

        private string _legalUnitName;
        public string LegalUnitName
        {
            get
            {
                return _legalUnitName;
            }
            set
            {
                _legalUnitName = value;
                NotifyPropertyChanged("LegalUnitName");
            }
        }
        
        private decimal _secondQuantity;
        public decimal SecondQuantity
        {
            get
            {
                return _secondQuantity;
            }
            set
            {
                _secondQuantity = value;
                NotifyPropertyChanged("SecondQuantity");
            }
        }

        
        private string _secondUnitCode;
        public string SecondUnitCode
        {
            get
            {
                return _secondUnitCode;
            }
            set
            {
                _secondUnitCode = value;
                NotifyPropertyChanged("SecondUnitCode");
            }
        }


        private string _secondUnitName;
        public string SecondUnitName
        {
            get
            {
                return _secondUnitName;
            }
            set
            {
                _secondUnitName = value;
                NotifyPropertyChanged("SecondUnitName");
            }
        }

        private string _versionNumber;
        public string VersionNumber
        {
            get
            {
                return _versionNumber;
            }
            set
            {
                _versionNumber = value;
                NotifyPropertyChanged("VersionNumber");
            }
        }

        private string _productNumber;
        public string ProductNumber
        {
            get
            {
                return _productNumber;
            }
            set
            {
                _productNumber = value;
                NotifyPropertyChanged("ProductNumber");
            }
        }

        private string _purpose;
        public string Purpose
        {
            get
            {
                return _purpose;
            }
            set
            {
                _purpose = value;
                NotifyPropertyChanged("Purpose");
            }
        }

        private string _countryCode;
        public string CountryCode
        {
            get
            {
                return _countryCode;
            }
            set
            {
                _countryCode = value;
                NotifyPropertyChanged("CountryCode");
            }
        }

        private string _controlItem;
        public string ControlItem
        {
            get
            {
                return _controlItem;
            }
            set
            {
                _controlItem = value;
                NotifyPropertyChanged("ControlItem");
            }
        }

        private string _dutyCode;
        public string DutyCode
        {
            get
            {
                return _dutyCode;
            }
            set
            {
                _dutyCode = value;
                NotifyPropertyChanged("DutyCode");
            }
        }

        private string _dutyName;
        public string DutyName
        {
            get
            {
                return _dutyName;
            }
            set
            {
                _dutyName = value;
                NotifyPropertyChanged("DutyName");
            }
        }

        private string _workFee;
        public string WorkFee
        {
            get
            {
                return _workFee;
            }
            set
            {
                _workFee = value;
                NotifyPropertyChanged("WorkFee");
            }
        }
    }
}
