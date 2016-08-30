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
    public class MachineDataModel : DataModel
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
        private string _machineName;
        public string MachineName
        {
            get
            {
                return _machineName;
            }
            set
            {
                _machineName = value;
                NotifyPropertyChanged("MachineName");
            }
        }
        //Name
        private string _machineIP;
        public string MachineIP
        {
            get
            {
                return _machineIP;
            }
            set
            {
                _machineIP = value;
                NotifyPropertyChanged("MachineIP");
            }
        }
    }
}
