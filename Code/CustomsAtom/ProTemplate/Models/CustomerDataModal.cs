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
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ProTemplate.Models
{
    public class CustomerDataModel : DataModel
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

        //ID
        public int ID { get; set; }
        //Name
        private string _name;
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                //Validator.ValidateProperty(value, new ValidationContext(this, null, null) { MemberName = "Name" });
                _name = value;
                NotifyPropertyChanged("Name");
            }
        }
        // Pinyin
        private string _pinYin;
        public string PinYin
        {
            get
            {
                return _pinYin;
            }
            set
            {
                _pinYin = value;
                NotifyPropertyChanged("PinYin");
            }
        }

        //Address
        private string _address;
        public string Address
        {
            get
            {
                return _address;
            }
            set
            {
                _address = value;
                NotifyPropertyChanged("Address");
            }
        }

        //Phone number
        private string _phoneNumber;
        public string PhoneNumber
        {
            get
            {
                return _phoneNumber;
            }
            set
            {
                _phoneNumber = value;
                NotifyPropertyChanged("PhoneNumber");
            }
        }

        //Boss Name
        private string _bossName;
        public string BossName
        {
            get
            {
                return _bossName;
            }
            set
            {
                _bossName = value;
                NotifyPropertyChanged("BossName");
            }
        }

        private bool _isSelected;
        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }
            set
            {
                _isSelected = value;
                NotifyPropertyChanged("IsSelected");
            }
        }

        private bool _isActive=true;
        public bool IsActive
        {
            get
            {
                return _isActive;
            }
            set
            {
                _isActive = value;
                NotifyPropertyChanged("IsActive");
            }
        }
    }
}
