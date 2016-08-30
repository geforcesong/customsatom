using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Navigation;
using ProTemplate.Web.DMServices;
using System.ServiceModel.DomainServices.Client;
using ProTemplate.Utility;
using ProTemplate.ViewModels;
using ProTemplate.Models;

namespace ProTemplate.Views
{
    public partial class ArchiveCustomer : Page
    {
        public ArchiveCustomer()
        {
            InitializeComponent();
            commBar.DeleteButton = false;
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Refresh();
        }

        #region Page Operation 
        public void Refresh(bool isForceReloadFromDB = false)
        {
            if (SystemConfiguration.Instance.DataContext != null)
            {
                // 如果设置这个参数，会清空当前DB Context的实体对象，这将会从数据库中重新加载数据。
                if (isForceReloadFromDB)
                    SystemConfiguration.Instance.DataContext.Customers.Clear();
                
                SystemConfiguration.Instance.DataContext.Load(SystemConfiguration.Instance.DataContext.GetCustomerQuery(), delegate(LoadOperation<Web.Customer> lp)
                {
                    CustomerViewModel cvm = App.Current.Resources["CustomerViewModel"] as CustomerViewModel;
                    if (cvm == null)
                        return;
                    cvm.Items.Clear();
                    foreach (var q in lp.Entities.ToList().OrderBy(a=>a.Name))
                    {
                        ProTemplate.Models.CustomerDataModel customerMD = new Models.CustomerDataModel();
                        customerMD.ID = q.ID;
                        customerMD.Name = q.Name;
                        customerMD.PinYin = q.PinYin;
                        customerMD.PhoneNumber = q.PhoneNumber;
                        customerMD.Address = q.Address;
                        customerMD.BossName = q.Boss.Name;
                        customerMD.IsActive = q.IsActive;
                        cvm.Items.Add(customerMD);
                    }
                    cvm.UpdateIndex();
                }, null);
            }
        }

        public void Add()
        {
            ProTemplate.UserControls.RadWindows.AddCustomer wnd = new ProTemplate.UserControls.RadWindows.AddCustomer();
            wnd.ShowDialog();
        }

        //public void Delete()
        //{
        //    if (gdCustomers.SelectedItems == null || gdCustomers.SelectedItems.Count == 0)
        //    {
        //        CommonUIFunction.ShowMessageBox(MessageTexts.MSG003);
        //        return;
        //    }
        //    else
        //    {
        //        if (CommonUIFunction.ShowConfirm(MessageTexts.MSG005) == MessageBoxResult.Cancel)
        //            return;
        //        bool isDeleted = false;
        //        foreach (var c in gdCustomers.SelectedItems)
        //        {
        //            CustomerDataModel cdm = c as CustomerDataModel;
        //            if (cdm == null)
        //                continue;
        //            int a = SystemConfiguration.Instance.DataContext.Customers.Count;
        //            var queryObj = (from o in SystemConfiguration.Instance.DataContext.Customers
        //                            where o.ID == cdm.ID
        //                            select o).SingleOrDefault();
        //            if (queryObj != null)
        //            {
        //                SystemConfiguration.Instance.DataContext.Customers.Remove(queryObj);
        //                isDeleted = true;
        //            }
        //        }

        //        if (isDeleted)
        //        {
        //            SystemConfiguration.Instance.DataContext.SubmitChanges((a) =>
        //            {
        //                if (a.HasError)
        //                {
        //                    a.MarkErrorAsHandled();
        //                    CommonUIFunction.ShowMessageBox(ErrorTexts.ERR003 + a.Error.Message);
        //                    SystemConfiguration.Instance.DataContext.RejectChanges();
        //                }
        //                else
        //                {
        //                    CustomerViewModel cvm = App.Current.Resources["CustomerViewModel"] as CustomerViewModel;
        //                    if (cvm != null)
        //                    {
        //                        for (int i = 0; i < gdCustomers.SelectedItems.Count;i++ )
        //                        {
        //                            CustomerDataModel customerDM = gdCustomers.SelectedItems[i] as CustomerDataModel;
        //                            cvm.Items.Remove(customerDM);
        //                            i--;
        //                        }
        //                        cvm.UpdateIndex();
        //                    }
        //                    //CommonUIFunction.ShowMessageBox("删除成功！");
        //                }
        //            }, null);
        //        }
        //    }
        //}


        public void Update()
        {
            if (gdCustomers.SelectedItems == null || gdCustomers.SelectedItems.Count == 0)
            {
                CommonUIFunction.ShowMessageBox(MessageTexts.MSG004);
                return;
            }

            ProTemplate.UserControls.RadWindows.AddCustomer wnd = new ProTemplate.UserControls.RadWindows.AddCustomer(gdCustomers.SelectedItems[0] as ProTemplate.Models.CustomerDataModel);
            wnd.ShowDialog();
        }
        #endregion

        #region Tool bar Events
        private void commBar_NewClick(object sender, EventArgs e)
        {
            Add();
        }

        private void commBar_RefreshClick(object sender, EventArgs e)
        {
            Refresh(true);
        }

        private void commBar_EditClick(object sender, EventArgs e)
        {
            Update();
        }

        //private void commBar_DeleteClick(object sender, EventArgs e)
        //{
        //    Delete();
        //}
        
        private void CommonToolBar_ExportToExcelClick(object sender, EventArgs e)
        {
            CommonUIFunction.ExportToFile(ExportFileType.Excel, gdCustomers);
        }

        private void CommonToolBar_ExportToWordClick(object sender, EventArgs e)
        {
            CommonUIFunction.ExportToFile(ExportFileType.Excel, gdCustomers, true);
        }
        #endregion
    }
}
