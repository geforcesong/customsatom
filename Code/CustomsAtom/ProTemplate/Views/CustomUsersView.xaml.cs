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
    public partial class CustomUsersView : Page
    {
        public CustomUsersView()
        {
            InitializeComponent();
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ClearDataContext();
            SystemConfiguration.Instance.DataContext.Load(SystemConfiguration.Instance.DataContext.GetCustomsUserWithScroeQuery(), lp =>
            {
                CommonUIFunction.SetApplcationBusyIndicator(false);
                if (lp.HasError)
                {
                    lp.MarkErrorAsHandled();
                    MessageBox.Show("获取系统信息出错，请重试！");
                }
                else
                {
                    CustomsUserQueryViewModel vm = ViewModelManager.CustomsUserQueryViewModelInstance;
                    if (vm != null )
                    {
                        int count = 1;
                        foreach (var u in lp.Entities)
                        {
                            string []info = u.IdentityNo.Split('@');
                            CustomsUserQueryDataModel cm = new CustomsUserQueryDataModel();
                            cm.Index = count++;
                            cm.Name = u.Name;
                            cm.CustomerNo = u.CustomsNo;
                            cm.IdentityNo = info[0];
                            if (info.Length > 1)
                                cm.Score = info[1];
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
            CustomsUserQueryViewModel vm = ViewModelManager.CustomsUserQueryViewModelInstance;
            if (vm != null)
                vm.Items.Clear();
            SystemConfiguration.Instance.DataContext.CustomsUsers.Clear();
        }
    }
}
