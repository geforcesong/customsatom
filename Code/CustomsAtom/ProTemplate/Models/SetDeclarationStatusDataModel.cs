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
    public class SetDeclarationStatusDataModel : DataModel
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
        private string _declarationNumber;
        public string DeclarationNumber
        {
            get
            {
                return _declarationNumber;
            }
            set
            {
                _declarationNumber = value;
                NotifyPropertyChanged("DeclarationNumber");
            }
        }
                private string _approvalNumber;
        public string ApprovalNumber
        {
            get
            {
                return _approvalNumber;
            }
            set
            {
                _approvalNumber = value;
                NotifyPropertyChanged("ApprovalNumber");
            }
        }
        private string _billNumber;
        public string BillNumber
        {
            get
            {
                return _billNumber;
            }
            set
            {
                _billNumber = value;
                NotifyPropertyChanged("BillNumber");
            }
        }
        private string _traderName;
        public string TraderName
        {
            get
            {
                return _traderName;
            }
            set
            {
                _traderName = value;
                NotifyPropertyChanged("TraderName");
            }
        }
        private string _declarationStatus;
        public string DeclarationStatus
        {
            get
            {
                return _declarationStatus;
            }
            set
            {
                _declarationStatus = value;
                NotifyPropertyChanged("DeclarationStatus");
            }
        }
        private string _conveyance;
        public string Conveyance
        {
            get
            {
                return _conveyance;
            }
            set
            {
                _conveyance = value;
                NotifyPropertyChanged("Conveyance");
            }
        }
                private string _voyageNumber;
        public string VoyageNumber
        {
            get
            {
                return _voyageNumber;
            }
            set
            {
                _voyageNumber = value;
                NotifyPropertyChanged("VoyageNumber");
            }
        }
        private string _drawbackStatus;
        public string DrawbackStatus
        {
            get
            {
                return _drawbackStatus;
            }
            set
            {
                _drawbackStatus = value;
                NotifyPropertyChanged("DrawbackStatus");
            }
        }
    }
}
