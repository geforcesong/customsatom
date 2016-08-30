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
    public class FeeTypeDataModel : DataModel
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

        public string _code;
        public string Code
        {
            get
            {
                return _code;
            }
            set
            {
                _code = value;
                NotifyPropertyChanged("Code");
            }
        }

        public string _name;
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
        public decimal? _amount;
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
        public decimal? _cost;
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
        public string _remark;
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
