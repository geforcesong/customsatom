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
    public class DeclarationDocumentDataModel : DataModel
    {
        public int Sequence { get; set; }
        public int DeclarationId { get; set; }
        public int SortOrder { get; set; }
        public string DocumentCode { get; set; }

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

        private string _documentName;
        public string DocumentName
        {
            get { return _documentName; }
            set
            {
                _documentName = value;
                NotifyPropertyChanged("DocumentName");
            }
        }

        private string _certificateNumber;
        public string CertificateNumber
        {
            get { return _certificateNumber; }
            set
            {
                _certificateNumber = value;
                NotifyPropertyChanged("CertificateNumber");
            }
        }

    }
}
