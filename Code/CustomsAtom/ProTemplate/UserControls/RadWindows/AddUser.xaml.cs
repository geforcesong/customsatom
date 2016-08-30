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
    public partial class AddUser : RadWindow
    {
        UserDataModel _currentDataModal;
        public AddUser()
        {
            InitializeComponent();
            PopulateRoleList();
            PopulateUserGroupList();
            _currentDataModal = new UserDataModel();
            _currentDataModal.Password = GeneratePassword();
            this.DataContext = _currentDataModal;
            this.Header = "新增用户";
            toolBar.IsNew = true;
        }

        public AddUser(UserDataModel user)
        {
            InitializeComponent();
            PopulateRoleList();
            PopulateUserGroupList();
            tbAlias.IsReadOnly = true;
            _currentDataModal = user;
            this.DataContext = _currentDataModal;
            this.Header = "编辑用户";
            toolBar.IsNew = false;
            // update role checkbox list
            RoleViewModel rvm = App.Current.Resources["RoleViewModel"] as RoleViewModel;
            if (rvm != null)
            {
                var query = from c in rvm.Items
                            join u in _currentDataModal.RoleList on c.ID equals u.ID
                            select c;
                if (query != null)
                {
                    foreach (var role in query)
                        role.IsSelected = true;
                }
            }
            // update user group Combobox list
            foreach (var it in cbGroups.Items)
            {
                UserGroupDataModel dgd = it as UserGroupDataModel;
                if (dgd != null && dgd.Name == _currentDataModal.GroupName)
                {
                    cbGroups.SelectedItem = it;
                    break;
                }
            }
        }

        private string GeneratePassword()
        {
            Random ram = new Random();
            return ram.Next(1000, 9999).ToString();
        }

        private void PopulateRoleList()
        {

            RoleViewModel rvm = App.Current.Resources["RoleViewModel"] as RoleViewModel;
            if (rvm != null)
            {
                rvm.Items.Clear();
                foreach (var r in SystemConfiguration.Instance.DataContext.Roles)
                {
                    RoleDataModel rd = new RoleDataModel();
                    rd.ID = r.Id;
                    rd.Name = r.Name;
                    rd.IsSelected = false;
                    rvm.Items.Add(rd);
                }
            }
        }

        private void PopulateUserGroupList()
        {
            UserGroupViewModel rvm = App.Current.Resources["UserGroupViewModel"] as UserGroupViewModel;
            if (rvm != null)
            {
                rvm.Items.Clear();
                foreach (var r in SystemConfiguration.Instance.DataContext.UserGroups.OrderBy(a=>a.GroupName))
                {
                    UserGroupDataModel rd = new UserGroupDataModel();
                    rd.ID = r.Id;
                    rd.Name = r.GroupName;
                    rd.IsSelected = false;
                    rvm.Items.Add(rd);
                }
            }
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

        private bool InputCheck()
        {
            if (tbName.Text == "")
            {
                _currentDataModal.SetErrors("Name", new List<string>() { "用户名称不能为空" });
            }
            else
            {
                _currentDataModal.ClearErrors("Name");
            }

            if (tbAlias.Text == "")
            {
                _currentDataModal.SetErrors("Alias", new List<string>() { "用户登录名不能为空" });
            }
            else
            {
                _currentDataModal.ClearErrors("Alias");
            }

            if (tbPwd.Text == "")
            {
                _currentDataModal.SetErrors("Password", new List<string>() { "密码不能为空" });
            }
            else
            {
                _currentDataModal.ClearErrors("Password");
            }

            if (cbGroups.SelectedItem == null)
            {
                CommonUIFunction.ShowMessageText(bdMsgParent, ErrorTexts.ERR002 + MessageTexts.MSG011, true);
                return false;
            }
            else
            {
                CommonUIFunction.HideMessageText(bdMsgParent);
            }
            return !_currentDataModal.HasErrors;
        }

        #region Page Operations
        void Save(bool IsNeedNew)
        {
            if (!InputCheck())
                return;
            

            var currentUser = (from q in SystemConfiguration.Instance.DataContext.Users
                               where q.Id == _currentDataModal.ID
                               select q).SingleOrDefault();

            if (currentUser == null)
            {
                currentUser = new Web.User();
                currentUser.CreatedDate = DateTime.Now;

                SystemConfiguration.Instance.DataContext.Users.Add(currentUser);
            }

            currentUser.ModifiedDate = DateTime.Now;
            currentUser.IsActived = true;
            currentUser.Alias = tbAlias.Text;
            currentUser.Password = tbPwd.Text;
            currentUser.Name = tbName.Text;
            currentUser.UserGroupId = ((UserGroupDataModel)cbGroups.SelectedItem).ID;
            currentUser.IsActived = (bool)cbIsActive.IsChecked;
            
            List<Web.UserRole> lstOld = currentUser.UserRole.ToList();
            RoleViewModel rvm = App.Current.Resources["RoleViewModel"] as RoleViewModel;
            if (rvm != null)
            {
                foreach (var c in rvm.Items)
                {
                    if (c.IsSelected == false)
                        continue;
                    if (lstOld.Any(o => o.RoleId == c.ID))
                    {
                        lstOld.Remove(lstOld.First(o => o.RoleId == c.ID));
                    }
                    else
                    {
                        Web.UserRole ur = new Web.UserRole();
                        ur.UserId = currentUser.Id;
                        ur.RoleId = c.ID;
                        currentUser.UserRole.Add(ur);
                    }
                }
                foreach (var userRole in lstOld)
                {
                    SystemConfiguration.Instance.DataContext.UserRoles.Remove(userRole);
                }
            }

            busyIndicator.IsBusy = true;
            SystemConfiguration.Instance.DataContext.SubmitChanges((a) =>
            {
                busyIndicator.IsBusy = false;
                if (a.HasError)
                {
                    a.MarkErrorAsHandled();
                    MessageBox.Show(a.Error.Message);
                    SystemConfiguration.Instance.DataContext.RejectChanges();
                }
                else
                {
                    UpdateViewModel(currentUser);

                    if (IsNeedNew)
                    {
                        CommonUIFunction.ShowMessageText(bdMsgParent, "保存成功，请继续添加！");
                        cbGroups.SelectedIndex = -1;
                        _currentDataModal = new UserDataModel();
                        _currentDataModal.Password = GeneratePassword();
                        this.DataContext = _currentDataModal;
                        PopulateRoleList();
                    }
                    else
                    {
                        this.Close();
                    }
                }
            }, null);
        }

        private void UpdateViewModel(Web.User user)
        {
            UserViewModel userVM = App.Current.Resources["UserViewModel"] as UserViewModel;
            if (userVM != null)
            {
                if (user.Id != _currentDataModal.ID)
                {
                    userVM.Items.Add(_currentDataModal);
                }

                _currentDataModal.ID = user.Id;
                _currentDataModal.Name = user.Name;
                _currentDataModal.Password = user.Password;
                _currentDataModal.Alias = user.Alias;
                _currentDataModal.GroupName = user.UserGroup.GroupName;
                _currentDataModal.IsActive = user.IsActived;
                
                if (user.UserRole != null)
                {
                    List<RoleDataModel> lstRole = new List<RoleDataModel>();
                    foreach (var userRole in user.UserRole)
                    {
                        RoleDataModel roleDM = new RoleDataModel();
                        roleDM.ID = userRole.RoleId;
                        roleDM.Name = userRole.Role.Name;
                        lstRole.Add(roleDM);
                    }
                    _currentDataModal.RoleList = lstRole;
                }
                // update index
                userVM.UpdateIndex();
            }
        }
        #endregion

        private void RadWindow_Closed(object sender, WindowClosedEventArgs e)
        {
            if (_currentDataModal != null)
            {
                _currentDataModal.ClearErrors("Name");
                _currentDataModal.ClearErrors("Alias");
                _currentDataModal.ClearErrors("Password");
            }
        }
    }
}
