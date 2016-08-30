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
using System.Linq;

namespace ProTemplate.Utility.Converters
{
    public class DataRowButtonIsEnabledConverter : System.Windows.Data.IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            // 如果是客户禁止编辑datarow
            var qCustomer = from a in SystemConfiguration.Instance.LoggedOnUser.RoleList
                        where a.Name.Contains("客户")
                        select a.Name;
            if (qCustomer.Count() > 0)
                return false;
            else
                return true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
