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
using ProTemplate.Models;
using ProTemplate.Utility;
using ProTemplate.ViewModels;
using ProTemplate.Web.DMServices;

namespace ProTemplate.UserControls.RadWindows
{
    public partial class AddUserGroup : RadWindow
    {
        UserGroupDataModel _currentDataModal;
        public AddUserGroup()
        {
            InitializeComponent();
            
            _currentDataModal = new UserGroupDataModel();
            this.DataContext = _currentDataModal;
            PopulateCustomerList();
            this.Header = "新增用户组";
            toolBar.IsNew = true;
        }

        public AddUserGroup(UserGroupDataModel userGroup)
        {
            InitializeComponent();
            
            _currentDataModal = userGroup;
            this.DataContext = _currentDataModal;
            PopulateCustomerList();
            this.Header = "编辑用户组";
            toolBar.IsNew = false;


        }

        private void PopulateCustomerList()
        {
            SystemConfiguration.Instance.DataContext.Load(SystemConfiguration.Instance.DataContext.GetCustomerQuery(), o => 
            {
                CustomerViewModel cvm = App.Current.Resources["CustomerViewModel"] as CustomerViewModel;

                if (cvm != null)
                {
                    cvm.Items.Clear();
                    foreach (var r in SystemConfiguration.Instance.DataContext.Customers.ToList().OrderBy(a=>a.Name))
                    {
                        CustomerDataModel rd = new CustomerDataModel();
                        rd.ID = r.ID;
                        rd.Name = r.Name;
                        rd.IsSelected = false;
                        cvm.Items.Add(rd);
                    }
                    // update role checkbox list
                    if (_currentDataModal.CustomerList != null)
                    {
                        CustomerViewModel rvm = App.Current.Resources["CustomerViewModel"] as CustomerViewModel;
                        if (rvm != null)
                        {
                            var query = (from c in rvm.Items
                                         join u in _currentDataModal.CustomerList on c.ID equals u.ID
                                         select c);
                            if (query != null)
                            {
                                foreach (var role in query)
                                    role.IsSelected = true;

                                if (query.Count() == rvm.Items.Count)
                                    cbAll.IsChecked = true;
                            }
                        }
                    }
                }

            }, null);
            
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
                _currentDataModal.SetErrors("Name", new List<string>() { "用户组名称不能为空" });
            }
            else
            {
                _currentDataModal.ClearErrors("Name");
            }

            CustomerViewModel cvm = ViewModelManager.CustomerViewModelInstance;
            System.Diagnostics.Debug.Assert(cvm != null);
            var query = from a in cvm.Items
                        where a.IsSelected == true
                        select a;
            if (query.Count() == 0)
            {
                CommonUIFunction.ShowMessageText(bdMsgParent, ErrorTexts.ERR002 + MessageTexts.MSG017, true);
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

            

            var currentUserGroup = (from q in SystemConfiguration.Instance.DataContext.UserGroups
                               where q.Id == _currentDataModal.ID
                               select q).SingleOrDefault();

            if (currentUserGroup == null)
            {
                currentUserGroup = new Web.UserGroup();
                currentUserGroup.CreatedDate = DateTime.Now;
                SystemConfiguration.Instance.DataContext.UserGroups.Add(currentUserGroup);
            }

            currentUserGroup.GroupName = tbName.Text;
            currentUserGroup.ModifiedDate = DateTime.Now;

            List<Web.UserGroupCustomer> lstOld = currentUserGroup.UserGroupCustomer.ToList();

            CustomerViewModel customerVM = App.Current.Resources["CustomerViewModel"] as CustomerViewModel;
            if (customerVM != null)
            {
                foreach (var c in customerVM.Items)
                {
                    if (c.IsSelected == false)
                        continue;
                    if (lstOld.Any(o => o.CustomerId == c.ID))
                    {
                        lstOld.Remove(lstOld.First(o => o.CustomerId == c.ID));
                    }
                    else
                    {
                        Web.UserGroupCustomer ur = new Web.UserGroupCustomer();
                        ur.UserGroupId = currentUserGroup.Id;
                        ur.CustomerId = c.ID;
                        currentUserGroup.UserGroupCustomer.Add(ur);
                    }
                }
                foreach (var userGroupCustomer in lstOld)
                {
                    SystemConfiguration.Instance.DataContext.UserGroupCustomers.Remove(userGroupCustomer);
                }
            }

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

                    UpdateViewModel(currentUserGroup);
                    if (IsNeedNew)
                    {
                        CommonUIFunction.ShowMessageText(bdMsgParent, "保存成功，请继续添加！");
                        PopulateCustomerList();
                        _currentDataModal = new UserGroupDataModel();
                        this.DataContext = _currentDataModal;
                    }
                    else
                    {
                        this.Close();
                    }
                }
            }, null);
        }
        private void UpdateViewModel(Web.UserGroup userGroup)
        {
            UserGroupViewModel userGroupVM = App.Current.Resources["UserGroupViewModel"] as UserGroupViewModel;
            if (userGroupVM != null && userGroup!= null)
            {
                //如果是新增的话，添加到viewmodel里面
                if (userGroup.Id != _currentDataModal.ID)
                {
                    userGroupVM.Items.Add(_currentDataModal);
                }

                _currentDataModal.ID = userGroup.Id;
                _currentDataModal.Name = userGroup.GroupName;

                if (userGroup.UserGroupCustomer != null)
                {
                    List<CustomerDataModel> lstCustomer = new List<CustomerDataModel>();
                    foreach (var userGroupCustomer in userGroup.UserGroupCustomer)
                    {
                        CustomerDataModel customerDataModel = new CustomerDataModel();
                        customerDataModel.ID = userGroupCustomer.Customer.ID;
                        customerDataModel.Name = userGroupCustomer.Customer.Name;
                        lstCustomer.Add(customerDataModel);

                    }
                    _currentDataModal.CustomerList = lstCustomer;
                }
                // update index
                userGroupVM.UpdateIndex();
            }
        }
        #endregion

        private void RadWindow_Closed(object sender, WindowClosedEventArgs e)
        {
            if(_currentDataModal!=null)
                _currentDataModal.ClearErrors("Name");
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            CustomerViewModel cvm = App.Current.Resources["CustomerViewModel"] as CustomerViewModel;
            foreach (var a in cvm.Items)
            {
                if (!a.IsSelected)
                    a.IsSelected = true;
            }
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            CustomerViewModel cvm = App.Current.Resources["CustomerViewModel"] as CustomerViewModel;
            foreach (var a in cvm.Items)
            {
                if (a.IsSelected)
                    a.IsSelected = false;
            }
        }
    }
}
