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
    public class DeclarationContainerDataModel : DataModel
    {
        public int Sequence { get; set; }
        public int DeclarationId { get; set; }
        public int SortOrder { get; set; }

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

        private string _number;
        public string Number
        {
            get { return _number; }
            set
            {
                _number = value;
                NotifyPropertyChanged("Number");
            }
        }

        private string _model;
        public string Model
        {
            get { return _model; }
            set
            {
                _model = value;
                NotifyPropertyChanged("Model");
            }
        }

        private string _weight;
        public string Weight
        {
            get { return _weight; }
            set
            {
                _weight = value;
                NotifyPropertyChanged("Weight");
            }
        }
    }
}
