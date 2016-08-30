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
    public partial class AddCustomsUser : RadWindow
    {
        CustomsUserDataModel _currentDataModal;
        public AddCustomsUser()
        {
            InitializeComponent();
            _currentDataModal = new CustomsUserDataModel();
            this.DataContext = _currentDataModal;
            this.Header = "新增报关员";
            toolBar.IsNew = true;
        }

        public AddCustomsUser(CustomsUserDataModel customsUser)
        {
            InitializeComponent();
            _currentDataModal = customsUser;
            this.DataContext = _currentDataModal;
            this.Header = "编辑报关员";
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
                _currentDataModal.SetErrors("Name", new List<string>() { "报关员名称不能为空" });
            }
            else
            {
                _currentDataModal.ClearErrors("Name");
            }
            if (tbCustomsNo.Text == "")
            {
                _currentDataModal.SetErrors("CustomsNo", new List<string>() { "报关员编号不能为空" });
            }
            else
            {
                _currentDataModal.ClearErrors("CustomsNo");
            }
            if (tbIdentityNo.Text == "")
            {
                _currentDataModal.SetErrors("IdentityNo", new List<string>() { "身份证不能为空" });
            }
            else
            {
                _currentDataModal.ClearErrors("IdentityNo");
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

            

            var currentCustomsUser = (from q in SystemConfiguration.Instance.DataContext.CustomsUsers
                               where q.ID == _currentDataModal.ID
                               select q).SingleOrDefault();

            if (currentCustomsUser == null)
            {
                currentCustomsUser = new Web.CustomsUser();
                SystemConfiguration.Instance.DataContext.CustomsUsers.Add(currentCustomsUser);
            }

            currentCustomsUser.Name = tbName.Text;
            currentCustomsUser.CustomsNo = tbCustomsNo.Text;
            currentCustomsUser.IdentityNo = tbIdentityNo.Text;

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
                    UpdateViewModel(currentCustomsUser);

                    if (IsNeedNew)
                    {
                        CommonUIFunction.ShowMessageText(bdMsgParent, "保存成功，请继续添加！");
                        _currentDataModal = new CustomsUserDataModel();
                        this.DataContext = _currentDataModal;
                    }
                    else
                    {
                        this.Close();
                    }
                }
            }, null);
        }

        private void UpdateViewModel(Web.CustomsUser customsUser)
        {
            CustomsUserViewModel customsUserVM = App.Current.Resources["CustomsUserViewModel"] as CustomsUserViewModel;
            if (customsUserVM != null)
            {
                if (customsUser.ID != _currentDataModal.ID)
                {
                    customsUserVM.Items.Add(_currentDataModal);
                }

                _currentDataModal.ID = customsUser.ID;
                _currentDataModal.Name = customsUser.Name;
                _currentDataModal.CustomsNo = customsUser.CustomsNo;
                _currentDataModal.IdentityNo = customsUser.IdentityNo;

                // update index
                customsUserVM.UpdateIndex();
            }
        }

        #endregion

        private void RadWindow_Closed(object sender, WindowClosedEventArgs e)
        {
            if (_currentDataModal != null)
            {
                _currentDataModal.ClearErrors("Name");
                _currentDataModal.ClearErrors("CustomsNo");
                _currentDataModal.ClearErrors("IdentityNo");
            }
        }
    }

}
