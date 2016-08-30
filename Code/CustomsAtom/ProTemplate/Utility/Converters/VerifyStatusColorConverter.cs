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

namespace ProTemplate.Utility.Converters
{
    public class VerifyStatusColorConverter : System.Windows.Data.IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (parameter!=null && parameter.ToString() == "校验失败")
                    return new SolidColorBrush(Colors.Red);
            else
            {
                if (value == null)
                    return new SolidColorBrush(Colors.Black);
                else
                {
                    string status = value.ToString();
                    if(status == "校验成功")
                        return new SolidColorBrush(Colors.Green);
                    else if (status == "校验失败")
                        return new SolidColorBrush(Colors.Red);
                    else
                        return new SolidColorBrush(Colors.Black);
                }
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class VerifyDockColorConverter : System.Windows.Data.IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return new SolidColorBrush(Colors.Black);
            else
            {
                string status = value.ToString();
                if (status == "校验失败")
                    return new SolidColorBrush(Colors.Red);
                else
                    return new SolidColorBrush(Colors.Black);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
