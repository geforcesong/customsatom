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
    public class CorporationDataModel : DataModel
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
        //Code
        private string _code;
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
        //Code
        private string _level;
        public string Level
        {
            get
            {
                return _level;
            }
            set
            {
                _level = value;
                NotifyPropertyChanged("Level");
            }
        }
    }
}
