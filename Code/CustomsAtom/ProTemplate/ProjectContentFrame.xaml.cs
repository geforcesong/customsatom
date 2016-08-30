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

namespace ProTemplate
{
    public partial class ProjectContentFrame : UserControl
    {
        public ProjectContentFrame()
        {
            InitializeComponent();
        }

        public bool IsApplicationBusy
        {
            set
            {
                busyIndicator2.IsBusy = value;
            }
        }

        public string ApplicationBusyText
        {
            set
            {
                busyIndicator2.BusyContent = value;
            }
        }

        public void Navigate(string url)
        {
            ContentFrameRoot.Navigate(new Uri(url, UriKind.Relative));
        }

        public object GetConentPage()
        {
            return ContentFrameRoot.Content;
        }
    }
}
