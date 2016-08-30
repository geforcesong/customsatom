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
using ProTemplate.ViewModels;
using ProTemplate.Utility;
using ProTemplate.Models;

namespace ProTemplate.Views
{
    public partial class LoginHistoryView : Page
    {
        public LoginHistoryView()
        {
            InitializeComponent();
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ClearDataContext();
            dpStart.SelectedDate = DateTime.Today.AddDays(-1);
            dpEnd.SelectedDate = DateTime.Today;
            SystemConfiguration.Instance.DataContext.Load(SystemConfiguration.Instance.DataContext.GetLoginHistoryByDateQuery(DateTime.Today, DateTime.Today.AddDays(1)), lp =>
            {
                CommonUIFunction.SetApplcationBusyIndicator(false);
                if (lp.HasError)
                {
                    lp.MarkErrorAsHandled();
                    MessageBox.Show("获取系统信息出错，请重试！");
                }
                else
                {
                    LoginHistoryViewModel vm = App.Current.Resources["LoginHistoryViewModel"] as LoginHistoryViewModel; 
                    if (vm != null )
                    {
                        int count = 1;
                        foreach (var u in lp.Entities)
                        {
                            LoginHistoryDataModel cm = new LoginHistoryDataModel();
                            cm.Index = count++;
                            cm.UserName = u.UserName;
                            cm.LoginDate = u.LoginDate;
                            cm.LoginIP = u.LoginIP;

                            vm.Items.Add(cm);
                        }
                    }
                }
            }, null);
        }


        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            ClearDataContext();
        }

        private static void ClearDataContext()
        {
            LoginHistoryViewModel vm = App.Current.Resources["LoginHistoryViewModel"] as LoginHistoryViewModel;
            if (vm != null)
                vm.Items.Clear();
            SystemConfiguration.Instance.DataContext.LoginHistories.Clear();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ClearDataContext();
            SystemConfiguration.Instance.DataContext.Load(SystemConfiguration.Instance.DataContext.GetLoginHistoryByDateQuery(dpStart.SelectedDate.Value, dpEnd.SelectedDate.Value.AddDays(1)), lp =>
            {
                CommonUIFunction.SetApplcationBusyIndicator(false);
                if (lp.HasError)
                {
                    lp.MarkErrorAsHandled();
                    MessageBox.Show("获取系统信息出错，请重试！");
                }
                else
                {
                    LoginHistoryViewModel vm = App.Current.Resources["LoginHistoryViewModel"] as LoginHistoryViewModel;
                    if (vm != null)
                    {
                        int count = 1;
                        foreach (var u in lp.Entities)
                        {
                            LoginHistoryDataModel cm = new LoginHistoryDataModel();
                            cm.Index = count++;
                            cm.UserName = u.UserName;
                            cm.LoginDate = u.LoginDate;
                            cm.LoginIP = u.LoginIP;

                            vm.Items.Add(cm);
                        }
                    }
                }
            }, null);
        }
    }
}
