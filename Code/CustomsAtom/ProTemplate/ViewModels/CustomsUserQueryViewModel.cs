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
using System.Collections.ObjectModel;
using ProTemplate.Models;

namespace ProTemplate.ViewModels
{
    public class CustomsUserQueryViewModel
    {
        ObservableCollection<CustomsUserQueryDataModel> _items = new ObservableCollection<CustomsUserQueryDataModel>();

        public ObservableCollection<CustomsUserQueryDataModel> Items
        {
            get { return _items; }
            set
            {
                _items = value;
            }
        }
    }
}
