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
using Telerik.Windows.Controls;
using ProTemplate.Utility;
using ProTemplate.ViewModels;
using ProTemplate.Models;
using System.Windows.Data;

namespace ProTemplate.UserControls.RadWindows
{
    public partial class AddMachine : RadWindow
    {
        MachineDataModel _currentDataModal;
        public AddMachine()
        {
            InitializeComponent();
            _currentDataModal = new MachineDataModel();
            this.DataContext = _currentDataModal;
            this.Header = "新增打单机器";
            toolBar.IsNew = true;
        }

        public AddMachine(MachineDataModel Machine)
        {
            InitializeComponent();
            _currentDataModal = Machine;
            this.DataContext = _currentDataModal;
            this.Header = "编辑打单机器";
            toolBar.IsNew = false;
        }

        private void EditWindowToolBar_SaveAndClose(object sender, EventArgs e)
        {
            Save(false);
        }

        private void EditWindowToolBar_SaveAndNew(object sender, EventArgs e)
        {
            Save(true);
        }

        private void EditWindowToolBar_Close(object sender, EventArgs e)
        {
            this.Close();
        }

        public bool InputCheck()
        {
            if (tbMachineName.Text == "")
            {
                _currentDataModal.SetErrors("MachineName", new List<string>() { "机器名不能为空" });
            }
            else
            {
                _currentDataModal.ClearErrors("MachineName");
            }
            if (tbMachineIP.Text == "")
            {
                _currentDataModal.SetErrors("MachineIP", new List<string>() { "机器IP不能为空" });
            }
            else
            {
                _currentDataModal.ClearErrors("MachineIP");
            }
            return !_currentDataModal.HasErrors;
        }

        #region Page Operations
        void Save(bool IsNeedNew)
        {
            //_currentDataModal.Name = tbMachineName.Text;
            if (!InputCheck())
                return;

            //CommonUIFunction.HideMessageText(bdMsgParent);

            

            var currentMachine = (from q in SystemConfiguration.Instance.DataContext.MachineNameIPMappings
                               where q.ID == _currentDataModal.ID
                               select q).SingleOrDefault();

            if (currentMachine == null)
            {
                currentMachine = new Web.MachineNameIPMapping();
                SystemConfiguration.Instance.DataContext.MachineNameIPMappings.Add(currentMachine);
            }

            currentMachine.MachineName = tbMachineName.Text;
            currentMachine.MachineIP = tbMachineIP.Text;

            SystemConfiguration.Instance.DataContext.SubmitChanges((a) =>
            {
                if (a.HasError)
                {
                    a.MarkErrorAsHandled();
                    MessageBox.Show(a.Error.Message);
                    SystemConfiguration.Instance.DataContext.RejectChanges();
                }
                else
                {
                    UpdateViewModel(currentMachine);

                    if (IsNeedNew)
                    {
                        CommonUIFunction.ShowMessageText(bdMsgParent, "保存成功，请继续添加！");
                        _currentDataModal = new MachineDataModel();
                        this.DataContext = _currentDataModal;
                    }
                    else
                    {
                        this.Close();
                    }
                }
            }, null);
        }

        private void UpdateViewModel(Web.MachineNameIPMapping Machine)
        {
            MachineViewModel MachineVM = App.Current.Resources["MachineViewModel"] as MachineViewModel;
            if (MachineVM != null)
            {
                if (Machine.ID != _currentDataModal.ID)
                {
                    MachineVM.Items.Add(_currentDataModal);
                }

                _currentDataModal.ID = Machine.ID;
                _currentDataModal.MachineName = Machine.MachineName;
                _currentDataModal.MachineIP = Machine.MachineIP;

                // update index
                MachineVM.UpdateIndex();
            }
        }

        #endregion

        private void RadWindow_Closed(object sender, WindowClosedEventArgs e)
        {
            if (_currentDataModal != null)
            {
                _currentDataModal.ClearErrors("MachineName");
                _currentDataModal.ClearErrors("MachineIP");
            }
        }
    }

}
