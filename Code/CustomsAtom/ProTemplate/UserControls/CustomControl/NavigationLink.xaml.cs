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

namespace ProTemplate.UserControls
{
    public partial class NavigationLink : UserControl
    {
        public NavigationLink()
        {
            InitializeComponent();
        }

        public string LinkText
        {
            get { return tbText.Text; }
            set { tbText.Text = value; }
        }

        public string NavigationURL { get; set; }

        public string IconName
        {
            set
            {
                var path = ProTemplate.Utility.PathManager.GetIconPath(value);
                img.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri(path, UriKind.Relative));
            }
        }
    }
}
