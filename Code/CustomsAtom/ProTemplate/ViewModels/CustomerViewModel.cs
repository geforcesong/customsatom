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
using System.Linq;
namespace ProTemplate.ViewModels
{
    public class CustomerViewModel
    {
        ObservableCollection<CustomerDataModel> _items = new ObservableCollection<CustomerDataModel>();
        public ObservableCollection<CustomerDataModel> Items
        {
            get { return _items; }
            set
            {
                _items = value;
            }
        }

        public void UpdateIndex()
        {
            if (_items == null)
                return;
            for (int i = 0; i < Items.Count; i++)
                Items[i].Index = i + 1;
        }

        public string GetCustomerName(int customerID)
        {
            if (_items == null)
                return "";
            else
            {
                var query = (from c in _items
                             where c.ID == customerID
                             select c).SingleOrDefault();
                if (query != null)
                    return query.Name;
                else
                    return ""; ;
            }
        }
    }
}
