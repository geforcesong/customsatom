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
    public class YSExaminationDataDataModel:DataModel
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
        public string ApprovalNumber { get; set; }
        public string BillNumber { get; set; }
        public string Conveyance { get; set; }
        public string CustomerName { get; set; }
        public string DeclarationNumber { get; set; }
        public string DeclarationStatus { get; set; }
        public string VoyageNumber { get; set; }
        public string YSDate { get; set; }
        public string YSStatus { get; set; }
    }
}
