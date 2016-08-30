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
using ProTemplate.UserControls.RadWindows;
using ProTemplate.Models;

namespace ProTemplate.Views
{
    public partial class ArchiveUserGroup : Page
    {
        public ArchiveUserGroup()
        {
            InitializeComponent();
            commBar.DeleteButton = false;
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Refresh(true);
        }

        #region Page Operation
        public void Refresh(bool isForceReloadFromDB = false)
        {
            if (SystemConfiguration.Instance.DataContext != null)
            {
                // 如果设置这个参数，会清空当前DB Context的实体对象，这将会从数据库中重新加载数据。
                if (isForceReloadFromDB)
                    SystemConfiguration.Instance.DataContext.UserGroups.Clear();

                SystemConfiguration.Instance.DataContext.Load(SystemConfiguration.Instance.DataContext.GetUserGroupQuery(), delegate(LoadOperation<Web.UserGroup> lp)
                {
                    UserGroupViewModel cvm = App.Current.Resources["UserGroupViewModel"] as UserGroupViewModel;
                    if (cvm == null)
                        return;
                    cvm.Items.Clear();
                    foreach (Web.UserGroup userGroup in lp.Entities.ToList().OrderBy(a=>a.GroupName))
                    {
                        ProTemplate.Models.UserGroupDataModel userGroupMD = new Models.UserGroupDataModel();
                        userGroupMD.ID = userGroup.Id;
                        userGroupMD.Name = userGroup.GroupName;

                        List<CustomerDataModel> lstCustomer = new List<CustomerDataModel>();
                        foreach (var userGroupCustomer in userGroup.UserGroupCustomer)
                        {
                            ProTemplate.Models.CustomerDataModel customerDM = new Models.CustomerDataModel();
                            customerDM.ID = userGroupCustomer.Customer.ID;
                            customerDM.Name = userGroupCustomer.Customer.Name;
                            lstCustomer.Add(customerDM);
                        }
                        userGroupMD.CustomerList = lstCustomer;

                        cvm.Items.Add(userGroupMD);
                    }
                    cvm.UpdateIndex();
                }, null);
            }

        }
        #endregion

        #region Tool bar Events
        private void commBar_NewClick(object sender, EventArgs e)
        {
            AddUserGroup wnd = new AddUserGroup();
            wnd.ShowDialog();
        }

        private void commBar_RefreshClick(object sender, EventArgs e)
        {
            Refresh(true);
        }

        private void commBar_EditClick(object sender, EventArgs e)
        {
            if (gdUserGroup.SelectedItems == null || gdUserGroup.SelectedItems.Count == 0)
            {
                CommonUIFunction.ShowMessageBox(MessageTexts.MSG016);
                return;
            }

            AddUserGroup wnd = new AddUserGroup(gdUserGroup.SelectedItems[0] as ProTemplate.Models.UserGroupDataModel);
            wnd.ShowDialog();
        }

        #endregion
    }
}
