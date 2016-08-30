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
    public partial class FeeAll : Page
    {
        public FeeAll()
        {
            InitializeComponent();
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void btnFeeReport_Click(object sender, RoutedEventArgs e)
        {
            HtmlPopupWindowOptions options = new HtmlPopupWindowOptions();
            options.Resizeable = true;
            options.Left = 0;
            options.Top = 0;
            options.Toolbar = true;
            options.Width = 1000;
            options.Height = 800;
            options.Menubar = true;
            options.Directories = true;
            HtmlPage.Window.Invoke("OpenNormalWindow", new Uri("http://" + App.Current.Host.Source.Host + ":" + App.Current.Host.Source.Port + "/Report/FinancialReport.aspx?Report=ExportFeeNotPaidReport"));
        }

        private void btnCostReport_Click(object sender, RoutedEventArgs e)
        {
            HtmlPopupWindowOptions options = new HtmlPopupWindowOptions();
            options.Resizeable = true;
            options.Left = 0;
            options.Top = 0;
            options.Toolbar = true;
            options.Width = 1000;
            options.Height = 800;
            options.Menubar = true;
            options.Directories = true;
            HtmlPage.Window.Invoke("OpenNormalWindow", new Uri("http://" + App.Current.Host.Source.Host + ":" + App.Current.Host.Source.Port + "/Report/FinancialReport.aspx?Report=ExportFeeCostReport"));

        }

        private void btnBillReport_Click(object sender, RoutedEventArgs e)
        {
            HtmlPopupWindowOptions options = new HtmlPopupWindowOptions();
            options.Resizeable = true;
            options.Left = 0;
            options.Top = 0;
            options.Toolbar = true;
            options.Width = 1000;
            options.Height = 800;
            options.Menubar = true;
            options.Directories = true;
            HtmlPage.Window.Invoke("OpenNormalWindow", new Uri("http://" + App.Current.Host.Source.Host + ":" + App.Current.Host.Source.Port + "/Report/FinancialReport.aspx?Report=ExportDeclarationCountReport"));

        }

    }
}
