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
using ProTemplate.Utility;
using System.ServiceModel.DomainServices.Client;
using ProTemplate.ViewModels;
using ProTemplate.Models;
using System.IO;
using Telerik.Windows.Controls.GridView;
using Telerik.Windows;

namespace ProTemplate.Views
{
    public partial class ArchiveUser : Page
    {
        public ArchiveUser()
        {
            InitializeComponent();
            commBar.DeleteButton = false;
            gdUsers.AddHandler(GridViewCellBase.CellDoubleClickEvent, new EventHandler<RadRoutedEventArgs>(OnCellDoubleClick), true);
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Refresh();
        }

        private void OnCellDoubleClick(object sender, RadRoutedEventArgs args)
        {
            GridViewCellBase cell = args.OriginalSource as GridViewCellBase;
            TextBlock tb = cell.Content as TextBlock;
            if (tb != null)
            {
                UserDataModel dm = cell.DataContext as UserDataModel;
                if (dm != null)
                {
                    ProTemplate.UserControls.RadWindows.AddUser wnd = new ProTemplate.UserControls.RadWindows.AddUser(gdUsers.SelectedItems[0] as ProTemplate.Models.UserDataModel);
                    wnd.ShowDialog();
                }
            }
        }

        #region Page Operation

        public void Refresh(bool isForceReloadFromDB = false)
        {
            if (SystemConfiguration.Instance.DataContext != null)
            {
                // 如果设置这个参数，会清空当前DB Context的实体对象，这将会从数据库中重新加载数据。
                if (isForceReloadFromDB)
                    SystemConfiguration.Instance.DataContext.Users.Clear();

                SystemConfiguration.Instance.DataContext.Load(SystemConfiguration.Instance.DataContext.GetUserQuery(), delegate(LoadOperation<Web.User> lp)
                {
                    UserViewModel cvm = App.Current.Resources["UserViewModel"] as UserViewModel;
                    if (cvm == null)
                        return;
                    cvm.Items.Clear();
                    foreach (var q in lp.Entities.ToList().OrderBy(a=>a.Name))
                    {
                        ProTemplate.Models.UserDataModel userMD = new Models.UserDataModel();
                        userMD.ID = q.Id;
                        userMD.Name = q.Name;
                        userMD.Alias = q.Alias;
                        userMD.Password = q.Password;
                        userMD.GroupName = q.UserGroup.GroupName;
                        userMD.IsActive = q.IsActived;
                        List<RoleDataModel> lstRole = new List<RoleDataModel>();
                        foreach (var a in q.UserRole)
                        {
                            ProTemplate.Models.RoleDataModel b = new Models.RoleDataModel();
                            b.ID = a.Role.Id;
                            b.Name = a.Role.Name;
                            lstRole.Add(b);
                        }
                        userMD.RoleList = lstRole;
                        cvm.Items.Add(userMD);
                    }
                    cvm.UpdateIndex();
                }, null);
            }
            
        }
        #endregion

        #region Tool bar Events
        private void commBar_NewClick(object sender, EventArgs e)
        {
            ProTemplate.UserControls.RadWindows.AddUser wnd = new ProTemplate.UserControls.RadWindows.AddUser();
            wnd.ShowDialog();
        }

        private void commBar_RefreshClick(object sender, EventArgs e)
        {
            Refresh(true);
        }

        private void commBar_EditClick(object sender, EventArgs e)
        {
            if (gdUsers.SelectedItems == null || gdUsers.SelectedItems.Count == 0)
            {
                CommonUIFunction.ShowMessageBox(MessageTexts.MSG007);
                return;
            }

            ProTemplate.UserControls.RadWindows.AddUser wnd = new ProTemplate.UserControls.RadWindows.AddUser(gdUsers.SelectedItems[0] as ProTemplate.Models.UserDataModel);
            wnd.ShowDialog();
        }

        //private void commBar_DeleteClick(object sender, EventArgs e)
        //{
        //    if (gdUsers.SelectedItems == null || gdUsers.SelectedItems.Count == 0)
        //    {
        //        CommonUIFunction.ShowMessageBox(MessageTexts.MSG008);
        //        return;
        //    }
        //    else
        //    {
        //        if (CommonUIFunction.ShowConfirm(MessageTexts.MSG009) == MessageBoxResult.Cancel)
        //            return;
        //        bool isDeleted = false;
        //        foreach (var c in gdUsers.SelectedItems)
        //        {
        //            UserDataModel cdm = c as UserDataModel;
        //            if (cdm == null)
        //                continue;
        //            var queryObj = (from o in SystemConfiguration.Instance.DataContext.Users
        //                            where o.Id == cdm.ID
        //                            select o).SingleOrDefault();
        //            if (queryObj != null)
        //            {
        //                queryObj.IsActived = false;
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
        //                    UserViewModel cvm = App.Current.Resources["UserViewModel"] as UserViewModel;
        //                    if (cvm != null)
        //                    {
        //                        for (int i = 0; i < gdUsers.SelectedItems.Count; i++)
        //                        {
        //                            UserDataModel customerDM = gdUsers.SelectedItems[i] as UserDataModel;
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
        

        private void CommonToolBar_ExportToExcelClick(object sender, EventArgs e)
        {
            CommonUIFunction.ExportToFile(ExportFileType.Excel, gdUsers);
        }

        private void CommonToolBar_ExportToWordClick(object sender, EventArgs e)
        {
            CommonUIFunction.ExportToFile(ExportFileType.Excel, gdUsers, true);
        }
        #endregion

    }
}
