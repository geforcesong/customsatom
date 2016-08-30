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
using System.Windows.Navigation;
using System.Windows.Browser;

namespace ProTemplate.Views
{
    public partial class PrintCheckNotification : Page
    {
        public PrintCheckNotification()
        {
            InitializeComponent();
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            //HtmlPopupWindowOptions options = new HtmlPopupWindowOptions();
            //HtmlPage.(new Uri("http://" + App.Current.Host.Source.Host + ":" + App.Current.Host.Source.Port + "/Report/CheckNotificationForm.aspx"), "", options);
            HtmlPage.Window.Invoke("OpenNormalWindow", "http://" + App.Current.Host.Source.Host + ":" + App.Current.Host.Source.Port + "/Report/CheckNotificationForm.aspx");
        }

    }
}
