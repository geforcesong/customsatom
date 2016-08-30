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
    public class FinancialExportDeclarationDataModel : DataModel
    {
        public int ID { get; set; }
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

        private string _feeTypeName;
        public string FeeTypeName
        {
            get
            {
                return _feeTypeName;
            }
            set
            {
                _feeTypeName = value;
                NotifyPropertyChanged("FeeTypeName");
            }
        }

        public string FeeTypeCode { get; set; }

        private decimal? _amount;
        public decimal? Amount
        {
            get
            {
                return _amount;
            }
            set
            {
                _amount = value;
                NotifyPropertyChanged("Amount");
            }
        }
        private decimal? _cost;
        public decimal? Cost
        {
            get
            {
                return _cost;
            }
            set
            {
                _cost = value;
                NotifyPropertyChanged("Cost");
            }
        }

        private string _remark;
        public string Remark
        {
            get
            {
                return _remark;
            }
            set
            {
                _remark = value;
                NotifyPropertyChanged("Remark");
            }
        }


    }
}
