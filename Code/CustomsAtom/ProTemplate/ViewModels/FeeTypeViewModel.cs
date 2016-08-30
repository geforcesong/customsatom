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
using ProTemplate.Models;
using System.Linq;
using System.Collections.ObjectModel;

namespace ProTemplate.ViewModels
{
    public class FeeTypeViewModel
    {
        ObservableCollection<FeeTypeDataModel> _items = new ObservableCollection<FeeTypeDataModel>();
        public ObservableCollection<FeeTypeDataModel> Items
        {
            get { return _items; }
            set
            {
                _items = value;
            }
        }
        public string GetFeeTypeName(string code)
        {
            var lstName = (from c in _items where c.Code == code select c.Name);
            return lstName.FirstOrDefault();
        }

        public void UpdateIndex()
        {
            if (_items == null)
                return;
            for (int i = 0; i < Items.Count; i++)
                Items[i].Index = i + 1;
        }
    }
}
