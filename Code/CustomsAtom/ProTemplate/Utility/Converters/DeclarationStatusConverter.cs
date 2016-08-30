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
    public class DeclarationStatusConverter : System.Windows.Data.IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int status = 0;
            GetAllDeclarationByReceiveDateResultDataModel md = value as GetAllDeclarationByReceiveDateResultDataModel;
            if (md != null)
            {
                switch (md.DeclarationStatus)
                {
                    case "正在报关":
                        status = 1;
                        break;
                    case "退单":
                        status = 2;
                        break;
                    case "报关完成":
                        status = 3;
                        if (md.Remark != null && md.Remark.Contains("无纸"))
                        {
                            status = 5;
                        }
                        break;
                    case "查验":
                        status = 4;
                        break;
                    default:
                        status = 0;
                        break;
                }

            }
            else
            {
                GetAllFinancialExportDeclarationDataModel mdf = value as GetAllFinancialExportDeclarationDataModel;
                if (mdf != null)
                {
                    switch (mdf.DeclarationStatus)
                    {
                        case "正在报关":
                            status = 1;
                            break;
                        case "退单":
                            status = 2;
                            break;
                        case "报关完成":
                            status = 3;
                            if (mdf.Remark != null && mdf.Remark.Contains("无纸"))
                            {
                                status = 5;
                            }
                            break;
                        case "查验":
                            status = 4;
                            break;
                        default:
                            status = 0;
                            break;
                    }

                }
                else
                {
                    return false;
                }
            }
            return status;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
