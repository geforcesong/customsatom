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
using System.Collections;
using System.Collections.Generic;
using ProTemplate.Utility;

namespace ProTemplate.Models
{
    public class CustomsUserDataModel : DataModel
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
                _name = value;
                NotifyPropertyChanged("Name");
            }
        }
        //Name
        private string _customsNo;
        public string CustomsNo
        {
            get
            {
                return _customsNo;
            }
            set
            {
                _customsNo = value;
                NotifyPropertyChanged("CustomsNo");
            }
        }
        //Name
        private string _identityNo;
        public string IdentityNo
        {
            get
            {
                return _identityNo;
            }
            set
            {
                _identityNo = value;
                NotifyPropertyChanged("IdentityNo");
            }
        }
    }
}
