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
using ProTemplate.UserControls.RadWindows;

namespace ProTemplate.Utility.Converters
{
    public class CorporationConverter : System.Windows.Data.IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            BatchAddDeclarationItem dm = value as BatchAddDeclarationItem;
            if (dm != null && dm.IsALevel)
                return 1;
            else 
                return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
