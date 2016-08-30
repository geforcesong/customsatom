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
    public partial class ArchiveMachine : Page
    {
        public ArchiveMachine()
        {
            InitializeComponent();
            //commBar.DeleteButton = false;
        }

        // Executes when the Machine navigates to this page.
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
                    SystemConfiguration.Instance.DataContext.MachineNameIPMappings.Clear();

                SystemConfiguration.Instance.DataContext.Load(SystemConfiguration.Instance.DataContext.GetMachineNameIPMappingQuery(), delegate(LoadOperation<Web.MachineNameIPMapping> lp)
                {
                    MachineViewModel cvm = ViewModelManager.MachineViewModelInstance;
                    if (cvm == null)
                        return;
                    cvm.Items.Clear();
                    foreach (var q in lp.Entities)
                    {
                        ProTemplate.Models.MachineDataModel MachineMD = new Models.MachineDataModel();
                        MachineMD.ID = q.ID;
                        MachineMD.MachineName = q.MachineName;
                        MachineMD.MachineIP = q.MachineIP;
                        cvm.Items.Add(MachineMD);
                    }
                    cvm.UpdateIndex();
                }, null);
            }

        }
        #endregion

        #region Tool bar Events
        private void commBar_NewClick(object sender, EventArgs e)
        {
            AddMachine wnd = new AddMachine();
            wnd.ShowDialog();
        }

        private void commBar_RefreshClick(object sender, EventArgs e)
        {
            Refresh(true);
        }

        private void commBar_EditClick(object sender, EventArgs e)
        {
            if (gdMachine.SelectedItems == null || gdMachine.SelectedItems.Count == 0)
            {
                CommonUIFunction.ShowMessageBox(MessageTexts.MSG033);
                return;
            }

            AddMachine wnd = new AddMachine(gdMachine.SelectedItems[0] as MachineDataModel);
            wnd.ShowDialog();
        }
        private void commBar_DeleteClick(object sender, EventArgs e)
        {
            if (gdMachine.SelectedItems == null || gdMachine.SelectedItems.Count == 0)
            {
                CommonUIFunction.ShowMessageBox(MessageTexts.MSG033);
                return;
            }



            CommonUIFunction.ShowConfirmYesNo(MessageTexts.MSG034, (o, args) =>
            {
                if (args.DialogResult.HasValue && args.DialogResult.Value)
                {
                    var Machine = (from c in SystemConfiguration.Instance.DataContext.MachineNameIPMappings where c.ID == ((MachineDataModel)gdMachine.SelectedItems[0]).ID select c).FirstOrDefault();
                    if (Machine != null)
                    {
                        SystemConfiguration.Instance.DataContext.MachineNameIPMappings.Remove(Machine);
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
                            MachineViewModel cvm = App.Current.Resources["MachineViewModel"] as MachineViewModel;
                            if (cvm == null)
                                return;
                            cvm.Items.Remove((MachineDataModel)gdMachine.SelectedItems[0]);
                        }
                    }, null);
                }
            });
        }

        #endregion



    }
    
}
