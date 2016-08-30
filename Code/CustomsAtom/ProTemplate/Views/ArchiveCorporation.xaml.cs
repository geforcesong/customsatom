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
    public partial class ArchiveCorporation : Page
    {
        public ArchiveCorporation()
        {
            
            InitializeComponent();
            //commBar.DeleteButton = false;
        }

        // Executes when the Corporation navigates to this page.
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
                    SystemConfiguration.Instance.DataContext.Corporations.Clear();

                SystemConfiguration.Instance.DataContext.Load(SystemConfiguration.Instance.DataContext.GetCorporationQuery(), delegate(LoadOperation<Web.Corporation> lp)
                {
                    CorporationViewModel cvm = App.Current.Resources["CorporationViewModel"] as CorporationViewModel;
                    if (cvm == null)
                        return;
                    cvm.Items.Clear();
                    foreach (var q in lp.Entities)
                    {
                        ProTemplate.Models.CorporationDataModel CorporationMD = new Models.CorporationDataModel();
                        CorporationMD.Code = q.CorporationCode;
                        CorporationMD.Name = q.CorporationName;
                        CorporationMD.Level = q.Level;
                        cvm.Items.Add(CorporationMD);
                    }
                    cvm.UpdateIndex();
                }, null);
            }
            
        }
        #endregion

        #region Tool bar Events
        private void commBar_NewClick(object sender, EventArgs e)
        {
            AddCorporation wnd = new AddCorporation();
            wnd.ShowDialog();
        }

        private void commBar_RefreshClick(object sender, EventArgs e)
        {
            Refresh(true);
        }

        private void commBar_EditClick(object sender, EventArgs e)
        {
            if (gdCorporation.SelectedItems == null || gdCorporation.SelectedItems.Count == 0)
            {
                CommonUIFunction.ShowMessageBox(MessageTexts.MSG007);
                return;
            }

            ProTemplate.UserControls.RadWindows.AddCorporation wnd = new ProTemplate.UserControls.RadWindows.AddCorporation(gdCorporation.SelectedItems[0] as ProTemplate.Models.CorporationDataModel);
            wnd.ShowDialog();
        }

        private void commBar_DeleteClick(object sender, EventArgs e)
        {
            if (gdCorporation.SelectedItems == null || gdCorporation.SelectedItems.Count == 0)
            {
                CommonUIFunction.ShowMessageBox(MessageTexts.MSG007);
                return;
            }

            CorporationDataModel c = gdCorporation.SelectedItems[0] as ProTemplate.Models.CorporationDataModel;

            SystemConfiguration.Instance.DataContext.DeleteCorproationByCode(c.Code, lp => 
            {
                if (lp.HasError)
                {
                    MessageBox.Show("删除出错，请联系管理员。");
                    return;
                }
                else
                {
                    CorporationViewModel cvm = App.Current.Resources["CorporationViewModel"] as CorporationViewModel;
                    cvm.Items.Remove(c);
                    cvm.UpdateIndex();
                    MessageBox.Show("删除成功。");
                }
            }, null);
        }

        #endregion



    }
}
