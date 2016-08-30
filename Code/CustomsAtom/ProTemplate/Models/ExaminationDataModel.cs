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
    public class ExaminationDataModel:DataModel
    {
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

        public int ID { get; set; }

        public int CustomerID { get; set; }

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

        private string _goodsName;
        public string GoodsName
        {
            get { return _goodsName; }
            set
            {
                _goodsName = value;
                NotifyPropertyChanged("GoodsName");
            }
        }

        public decimal _quantity;
        public decimal Quantity
        {
            get { return _quantity; }
            set
            {
                _quantity = value;
                NotifyPropertyChanged("Quantity");
            }
        }

        public decimal _amount;
        public decimal Amount
        {
            get { return _amount; }
            set
            {
                _amount = value;
                NotifyPropertyChanged("Amount");
            }
        }


        public string _password;
        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                NotifyPropertyChanged("Password");
            }
        }

        public string _transferNumber;
        public string TransferNumber
        {
            get { return _transferNumber; }
            set
            {
                _transferNumber = value;
                NotifyPropertyChanged("TransferNumber");
            }
        }

        public DateTime _receivedDate;
        public DateTime ReceivedDate
        {
            get { return _receivedDate; }
            set
            {
                _receivedDate = value;
                NotifyPropertyChanged("ReceivedDate");
            }
        }


        public string _examinationNumber;
        public string ExaminationNumber
        {
            get { return _examinationNumber; }
            set
            {
                _examinationNumber = value;
                NotifyPropertyChanged("ExaminationNumber");
            }
        }

        private string _examinationStatus;
        public string ExaminationStatus
        {
            get { return _examinationStatus; }
            set
            {
                _examinationStatus = value;
                NotifyPropertyChanged("ExaminationStatus");
            }
        }


        private string _remark;
        public string Remark
        {
            get { return _remark; }
            set
            {
                _remark = value;
                NotifyPropertyChanged("Remark");
            }
        }


        private string _relatedSystemNumber;
        public string RelatedSystemNumber
        {
            get { return _relatedSystemNumber; }
            set
            {
                _relatedSystemNumber = value;
                NotifyPropertyChanged("RelatedSystemNumber");
            }
        }
        private string _isRelated;
        public string IsRelated
        {
            get { return _isRelated; }
            set
            {
                _isRelated = value;
                NotifyPropertyChanged("IsRelated");
            }
        }

        private decimal _examinationFee;
        public decimal ExaminationFee
        {
            get { return _examinationFee; }
            set
            {
                _examinationFee = value;
                NotifyPropertyChanged("ExaminationFee");
            }
        }

        private decimal _examinationCost;
        public decimal ExaminationCost
        {
            get { return _examinationCost; }
            set
            {
                _examinationCost = value;
                NotifyPropertyChanged("ExaminationCost");
            }
        }
    }
}
