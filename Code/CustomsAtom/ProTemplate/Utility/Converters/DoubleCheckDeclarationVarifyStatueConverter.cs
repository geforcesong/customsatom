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

namespace ProTemplate.Utility.Converters
{
    public class DoubleCheckDeclarationVarifyStatueConverter : System.Windows.Data.IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            DoubleCheckDeclarationVarifyDataModel dm = value as DoubleCheckDeclarationVarifyDataModel;
            if (dm == null || dm.VarifyFlag == "未校验")
                return 0;
            else if (dm.VarifyFlag == "失败")
                return 1;
            else
                return 2;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
