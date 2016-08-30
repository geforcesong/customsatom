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
    public class LoginHistoryDataModel : DataModel
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
        private string _userName;
        public string UserName
        {
            get
            {
                return _userName;
            }
            set
            {
                _userName = value;
                NotifyPropertyChanged("UserName");
            }
        }
        //Name
        private DateTime _loginDate;
        public DateTime LoginDate
        {
            get
            {
                return _loginDate;
            }
            set
            {
                _loginDate = value;
                NotifyPropertyChanged("LoginDate");
            }
        }
        //Name
        private string _loginIP;
        public string LoginIP
        {
            get
            {
                return _loginIP;
            }
            set
            {
                _loginIP = value;
                NotifyPropertyChanged("LoginIP");
            }
        }
    }
}
