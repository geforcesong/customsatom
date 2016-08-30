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
    public partial class AddBoss : RadWindow
    {
        BossDataModel _currentDataModal;
        public AddBoss()
        {
            InitializeComponent();
            _currentDataModal = new BossDataModel();
            this.DataContext = _currentDataModal;
            this.Header = "新增老板";
            toolBar.IsNew = true;
        }

        public AddBoss(BossDataModel boss)
        {
            InitializeComponent();
            _currentDataModal = boss;
            this.DataContext = _currentDataModal;
            this.Header = "编辑老板";
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
            if (tbName.Text == "")
            {
                _currentDataModal.SetErrors("Name", new List<string>() { "老板名称不能为空" });
            }
            else
            {
                _currentDataModal.ClearErrors("Name");
            }
            return !_currentDataModal.HasErrors;
        }

        #region Page Operations
        void Save(bool IsNeedNew)
        {
            //_currentDataModal.Name = tbName.Text;
            if (!InputCheck())
                return;

            //CommonUIFunction.HideMessageText(bdMsgParent);

            

            var currentBoss = (from q in SystemConfiguration.Instance.DataContext.Bosses
                               where q.ID == _currentDataModal.ID
                               select q).SingleOrDefault();

            if (currentBoss == null)
            {
                currentBoss = new Web.Boss();
                SystemConfiguration.Instance.DataContext.Bosses.Add(currentBoss);
            }

            currentBoss.Name = tbName.Text;

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
                    UpdateViewModel(currentBoss);

                    if (IsNeedNew)
                    {
                        CommonUIFunction.ShowMessageText(bdMsgParent, "保存成功，请继续添加！");
                        _currentDataModal = new BossDataModel();
                        this.DataContext = _currentDataModal;
                    }
                    else
                    {
                        this.Close();
                    }
                }
            }, null);
        }

        private void UpdateViewModel(Web.Boss boss)
        {
            BossViewModel bossVM = App.Current.Resources["BossViewModel"] as BossViewModel;
            if (bossVM != null)
            {
                if (boss.ID != _currentDataModal.ID)
                {
                    bossVM.Items.Add(_currentDataModal);
                }

                _currentDataModal.ID = boss.ID;
                _currentDataModal.Name = boss.Name;

                // update index
                bossVM.UpdateIndex();
            }
        }

        #endregion

        private void RadWindow_Closed(object sender, WindowClosedEventArgs e)
        {
            if(_currentDataModal!=null)
                _currentDataModal.ClearErrors("Name");
        }
    }

}
