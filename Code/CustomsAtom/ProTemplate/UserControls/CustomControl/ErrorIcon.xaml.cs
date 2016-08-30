using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace ProTemplate.UserControls.RadWindows
{
    public partial class ErrorIcon : UserControl
    {
        public ErrorIcon()
        {
            InitializeComponent();
        }
        public void SetError(string ErrorMessage)
        {
            Warning.Visibility = Visibility.Visible;
            txtToolTip.Text = ErrorMessage;
        }
        public void Reset()
        {
            Warning.Visibility = Visibility.Collapsed;
            txtToolTip.Text = "";
        }
    }


}
