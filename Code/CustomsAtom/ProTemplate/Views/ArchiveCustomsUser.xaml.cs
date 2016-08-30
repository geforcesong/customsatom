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
using ProTemplate.UserControls.RadWindows;

namespace ProTemplate.Views
{
    public partial class ArchiveCustomsUser : Page
    {
        public ArchiveCustomsUser()
        {
            InitializeComponent();
            //commBar.DeleteButton = false;
        }

        // Executes when the CustomsUser navigates to this page.
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
                    SystemConfiguration.Instance.DataContext.CustomsUsers.Clear();

                SystemConfiguration.Instance.DataContext.Load(SystemConfiguration.Instance.DataContext.GetCustomsUserQuery(), delegate(LoadOperation<Web.CustomsUser> lp)
                {
                    CustomsUserViewModel cvm = App.Current.Resources["CustomsUserViewModel"] as CustomsUserViewModel;
                    if (cvm == null)
                        return;
                    cvm.Items.Clear();
                    foreach (var q in lp.Entities)
                    {
                        ProTemplate.Models.CustomsUserDataModel CustomsUserMD = new Models.CustomsUserDataModel();
                        CustomsUserMD.ID = q.ID;
                        CustomsUserMD.Name = q.Name;
                        CustomsUserMD.CustomsNo = q.CustomsNo;
                        CustomsUserMD.IdentityNo = q.IdentityNo;
                        cvm.Items.Add(CustomsUserMD);
                    }
                    cvm.UpdateIndex();
                }, null);
            }

        }
        #endregion

        #region Tool bar Events
        private void commBar_NewClick(object sender, EventArgs e)
        {
            AddCustomsUser wnd = new AddCustomsUser();
            wnd.ShowDialog();
        }

        private void commBar_RefreshClick(object sender, EventArgs e)
        {
            Refresh(true);
        }

        private void commBar_EditClick(object sender, EventArgs e)
        {
            if (gdCustomsUser.SelectedItems == null || gdCustomsUser.SelectedItems.Count == 0)
            {
                CommonUIFunction.ShowMessageBox(MessageTexts.MSG027);
                return;
            }

            AddCustomsUser wnd = new AddCustomsUser(gdCustomsUser.SelectedItems[0] as CustomsUserDataModel);
            wnd.ShowDialog();
        }
        private void commBar_DeleteClick(object sender, EventArgs e)
        {
            if (gdCustomsUser.SelectedItems == null || gdCustomsUser.SelectedItems.Count == 0)
            {
                CommonUIFunction.ShowMessageBox(MessageTexts.MSG027);
                return;
            }



            CommonUIFunction.ShowConfirmYesNo(MessageTexts.MSG028, (o, args) =>
            {
                if (args.DialogResult.HasValue && args.DialogResult.Value)
                {
                    var customsUser = (from c in SystemConfiguration.Instance.DataContext.CustomsUsers where c.ID == ((CustomsUserDataModel)gdCustomsUser.SelectedItems[0]).ID select c).FirstOrDefault();
                    if (customsUser != null)
                    {
                        SystemConfiguration.Instance.DataContext.CustomsUsers.Remove(customsUser);
                    }

                    SystemConfiguration.Instance.DataContext.SubmitChanges((so) => 
                    {
                        if (so.HasError)
                        {
                            CommonUIFunction.ShowMessageBox(so.Error.Message);
                            SystemConfiguration.Instance.DataContext.RejectChanges();
                        }
                        else
                        {
                            CustomsUserViewModel cvm = App.Current.Resources["CustomsUserViewModel"] as CustomsUserViewModel;
                            if (cvm == null)
                                return;
                            cvm.Items.Remove((CustomsUserDataModel)gdCustomsUser.SelectedItems[0]);
                        }
                    }, null);
                }
            });
        }

        #endregion



    }
    
}
