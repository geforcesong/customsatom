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
    public class RoleViewModel
    {
        ObservableCollection<RoleDataModel> _items = new ObservableCollection<RoleDataModel>();
        public ObservableCollection<RoleDataModel> Items
        {
            get { return _items; }
            set
            {
                _items = value;
            }
        }

        public void ResetSelectionStatus()
        {
            if (Items == null || Items.Count == 0)
                return;
            foreach (var a in Items)
                a.IsSelected = false;
        }
    }
}
