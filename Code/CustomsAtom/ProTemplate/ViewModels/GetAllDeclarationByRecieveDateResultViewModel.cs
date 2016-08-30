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
using System.Collections.Generic;

namespace ProTemplate.ViewModels
{
    public class GetAllDeclarationByReceiveDateResultViewModel
    {
        List<GetAllDeclarationByReceiveDateResultDataModel> _items = new List<GetAllDeclarationByReceiveDateResultDataModel>();
        public List<GetAllDeclarationByReceiveDateResultDataModel> Items
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
    }
}
