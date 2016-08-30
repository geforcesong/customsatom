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
    public partial class ArchiveBoss : Page
    {
        public ArchiveBoss()
        {
            
            InitializeComponent();
            commBar.DeleteButton = false;
        }

        // Executes when the Boss navigates to this page.
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
                    SystemConfiguration.Instance.DataContext.Bosses.Clear();

                SystemConfiguration.Instance.DataContext.Load(SystemConfiguration.Instance.DataContext.GetBossQuery(), delegate(LoadOperation<Web.Boss> lp)
                {
                    BossViewModel cvm = App.Current.Resources["BossViewModel"] as BossViewModel;
                    if (cvm == null)
                        return;
                    cvm.Items.Clear();
                    foreach (var q in lp.Entities)
                    {
                        ProTemplate.Models.BossDataModel BossMD = new Models.BossDataModel();
                        BossMD.ID = q.ID;
                        BossMD.Name = q.Name;
                        cvm.Items.Add(BossMD);
                    }
                    cvm.UpdateIndex();
                }, null);
            }
            
        }
        #endregion

        #region Tool bar Events
        private void commBar_NewClick(object sender, EventArgs e)
        {
            AddBoss wnd = new AddBoss();
            wnd.ShowDialog();
        }

        private void commBar_RefreshClick(object sender, EventArgs e)
        {
            Refresh(true);
        }

        private void commBar_EditClick(object sender, EventArgs e)
        {
            if (gdBoss.SelectedItems == null || gdBoss.SelectedItems.Count == 0)
            {
                CommonUIFunction.ShowMessageBox(MessageTexts.MSG007);
                return;
            }

            ProTemplate.UserControls.RadWindows.AddBoss wnd = new ProTemplate.UserControls.RadWindows.AddBoss(gdBoss.SelectedItems[0] as ProTemplate.Models.BossDataModel);
            wnd.ShowDialog();
        }

        #endregion

    }
}
