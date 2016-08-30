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
    public class BossDataModel : DataModel
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
        //[Required(ErrorMessage = "老板名称不能为空")]
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
                //if (string.IsNullOrEmpty(value))
                //{
                //    AddErrorToPropertyAndNotifyErrorChanges("Name", new ValidationErrorInfo() { ErrorCode = 1, ErrorMessage = "老板名称不能为空" });
                //}
                //else
                //{
                //    RemoveErrorFromPropertyAndNotifyErrorChanges("Name", 1);
                //}
            }
        }
    }
}
