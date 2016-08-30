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
using Telerik.Windows.Controls;
using System.IO;
using ProTemplate.Utility;

namespace ProTemplate.UserControls.RadWindows
{
    public partial class YSLogin : RadWindow
    {
        private static Uri _loginUrl = new Uri("http://localhost:45234/Default.aspx");

        public YSLogin()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SystemConfiguration.Instance.DataContext.YSLogin(textBox1.Text, lp =>
            {
                if (lp.Error == null && lp.Value)
                {
                    MessageBox.Show("登录成功");
                    this.DialogResult = true;
                }
                else
                {
                    MessageBox.Show("登录失败，请重试");
                    this.DialogResult = false;
                }
                this.Close();
            }, null);
        }

        public ImageSource YSValidateImage
        {
            get
            {
                return imgValidate.Source;
            }
            set
            {
                imgValidate.Source = value;
            }
        }

    }
}
