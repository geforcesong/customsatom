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

namespace ProTemplate.Utility
{
    public class PathManager
    {
        public static string GetIconPath(string iconName)
        {
            return string.Format("/{0};component/Images/Icons/{1}", SystemConfiguration.Instance.AssemblyName, iconName);
        }
    }
}
