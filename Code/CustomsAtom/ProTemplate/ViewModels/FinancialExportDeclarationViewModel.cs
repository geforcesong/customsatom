using System;
using System.Linq;
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
using System.Collections.ObjectModel;

namespace ProTemplate.ViewModels
{
    public class FinancialExportDeclarationViewModel
    {
        ObservableCollection<FinancialExportDeclarationDataModel> _items = new ObservableCollection<FinancialExportDeclarationDataModel>();
        public ObservableCollection<FinancialExportDeclarationDataModel> Items
        {
            get { return _items; }
            set
            {
                _items = value;
            }
        }
        public string GetFeeTypeName(string code)
        {
            if (_items == null)
                return "";
            else
            {
                var query = (from c in _items
                             where c.FeeTypeCode == code
                             select c).SingleOrDefault();
                if (query != null)
                    return query.FeeTypeName;
                else
                    return ""; ;
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
