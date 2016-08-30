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
using ProTemplate.Web.DMServices;
using ProTemplate.Utility;
using ProTemplate.Models;
using ProTemplate.ViewModels;
using System.Windows.Data;
using System.ServiceModel.DomainServices.Client;
using System.Collections;

namespace ProTemplate.UserControls.RadWindows
{
    public partial class AddCustomer : RadWindow
    {
        private CustomerDataModel _currentDataModal;

        public AddCustomer()
        {
            InitializeComponent();
            ViewModelManager.ResetFeeTypeViewModel();
            _currentDataModal = new CustomerDataModel();
            PopulateBossList();
            this.DataContext = _currentDataModal;
            this.Header = "新增客户";
            toolBar.IsNew = true;
        }

        public AddCustomer(CustomerDataModel customer)
        {
            InitializeComponent();

            PopulateBossList();
            ViewModelManager.ResetFeeTypeViewModel();
            _currentDataModal = customer;
            this.DataContext = _currentDataModal;
            this.Header = "编辑客户";
            toolBar.IsNew = false;

            // update boss Combobox list
            foreach (var it in cbBoss.Items)
            {
                BossDataModel dgd = it as BossDataModel;
                if (dgd != null && dgd.Name == _currentDataModal.BossName)
                {
                    cbBoss.SelectedItem = it;
                    break;
                }
            }

            PopulateCustomerFeeSetting();
        }

        private void PopulateCustomerFeeSetting()
        {
            lvwFeeType.Items.Clear();
            SystemConfiguration.Instance.DataContext.Load(SystemConfiguration.Instance.DataContext.GetCustomerFeeSettingByCustomerIDQuery(_currentDataModal.ID), delegate(LoadOperation<Web.CustomerFeeSetting> lo)
            {
                if (lo.HasError)
                {
                    MessageBox.Show(lo.Error.Message);
                    return;
                }
                else
                {
                    foreach (var v in lo.Entities)
                    {
                        lvwFeeType.Items.Add(new FeeTypeDataModel() { Index = lvwFeeType.Items.Count + 1, Name = v.FeeType.Name, Code = v.FeeType.Code, Cost = v.Cost, Amount = v.Amount });
                    }
                }
            }, null);
        }

        private void acbFeeType_Populating(object sender, PopulatingEventArgs e)
        {
            try
            {
                FeeTypeViewModel vm = App.Current.Resources["FeeTypeViewModel"] as FeeTypeViewModel;
                if (vm != null)
                {
                    var prar = e.Parameter.ToLower();
                    var query = from a in vm.Items
                                where a.Code.ToLower().Contains(prar) || a.Name.ToLower().Contains(prar)
                                select a;
                    AutoCompleteBox source = (AutoCompleteBox)sender;
                    source.ItemsSource = query.ToList();
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void PopulateBossList()
        {
            BossViewModel rvm = App.Current.Resources["BossViewModel"] as BossViewModel;
            if (rvm != null)
            {
                rvm.Items.Clear();
                foreach (var r in SystemConfiguration.Instance.DataContext.Bosses)
                {
                    BossDataModel rd = new BossDataModel();
                    rd.ID = r.ID;
                    rd.Name = r.Name;
                    rvm.Items.Add(rd);
                }
            }
        }

        public bool InputCheck()
        {
            if (string.IsNullOrEmpty(tbName.Text))
            {
                _currentDataModal.SetErrors("Name", new List<string>() { "客户名称不能为空。" });
            }
            else
            {
                _currentDataModal.ClearErrors("Name");
            }

            if (cbBoss.SelectedItem == null)
            {
                CommonUIFunction.ShowMessageText(bdMsgParent, ErrorTexts.ERR002 + MessageTexts.MSG013, true);
                return false;
            }
            else
            {
                CommonUIFunction.HideMessageText(bdMsgParent);
            }
            decimal d = 0;
            //if (!decimal.TryParse(tbNormalFee.Text, out d) || !decimal.TryParse(tbCheckFee.Text, out d) || !decimal.TryParse(tbDrawbackFee.Text, out d) 
            //    || !decimal.TryParse(tbCancelFee.Text, out d) || !decimal.TryParse(tbSealFee.Text, out d) || !decimal.TryParse(tbLianDanFee.Text, out d)
            //    || !decimal.TryParse(tbCostFee.Text, out d))
            //{
            //    CommonUIFunction.ShowMessageText(bdMsgParent, ErrorTexts.ERR001 + MessageTexts.MSG025, true);
            //    return false;
            //}
            //else
            //{
            //    CommonUIFunction.HideMessageText(bdMsgParent);
            //}

            return !_currentDataModal.HasErrors;
        }

        private void Save(bool IsNeedNew)
        {
            if (!InputCheck())
                return;

            var currentCustomer = (from q in SystemConfiguration.Instance.DataContext.Customers
                               where q.ID == _currentDataModal.ID
                               select q).SingleOrDefault();

            if (currentCustomer == null)
            {
                currentCustomer = new Web.Customer();

                SystemConfiguration.Instance.DataContext.Customers.Add(currentCustomer);

                //增加相应用户组
                Web.UserGroup userGroup = new Web.UserGroup();
                userGroup.GroupName = tbName.Text + "组";
                userGroup.CreatedDate = DateTime.Now;
                userGroup.ModifiedDate = DateTime.Now;
                userGroup.ModifiedBy = SystemConfiguration.Instance.LoggedOnUser.ID;
                SystemConfiguration.Instance.DataContext.UserGroups.Add(userGroup);

                Web.UserGroupCustomer userGroupCustomer = new Web.UserGroupCustomer();
                userGroupCustomer.Customer = currentCustomer;
                userGroupCustomer.UserGroup = userGroup;
                SystemConfiguration.Instance.DataContext.UserGroupCustomers.Add(userGroupCustomer);

                //增加费用
                foreach (var v in lvwFeeType.Items)
                {
                    FeeTypeDataModel feeType = v as FeeTypeDataModel;
                    Web.CustomerFeeSetting customerFeeSetting = new Web.CustomerFeeSetting();
                    customerFeeSetting.Customer = currentCustomer;
                    customerFeeSetting.FeeTypeCode = feeType.Code;
                    customerFeeSetting.Amount = feeType.Amount.HasValue ? feeType.Amount.Value : 0;
                    customerFeeSetting.Cost = feeType.Cost.HasValue ? feeType.Cost.Value : 0;
                    SystemConfiguration.Instance.DataContext.CustomerFeeSettings.Add(customerFeeSetting);
                }
            }
            else
            {
                
                //修改费用
                List<Web.CustomerFeeSetting> oldFeeTypeList = currentCustomer.CustomerFeeSetting.ToList();
                foreach (Web.CustomerFeeSetting s in oldFeeTypeList)
                {
                    SystemConfiguration.Instance.DataContext.CustomerFeeSettings.Remove(s);
                }

                foreach (var v in lvwFeeType.Items)
                {
                    FeeTypeDataModel feeType = v as FeeTypeDataModel;
                    Web.CustomerFeeSetting customerFeeSetting = new Web.CustomerFeeSetting();
                    customerFeeSetting.Customer = currentCustomer;
                    customerFeeSetting.FeeTypeCode = feeType.Code;
                    customerFeeSetting.Amount = feeType.Amount.HasValue ? feeType.Amount.Value : 0;
                    customerFeeSetting.Cost = feeType.Cost.HasValue ? feeType.Cost.Value : 0;
                    SystemConfiguration.Instance.DataContext.CustomerFeeSettings.Add(customerFeeSetting);
                }

            }
            
            currentCustomer.Name = tbName.Text;
            currentCustomer.Address = tbAddress.Text;
            currentCustomer.PhoneNumber = tbPhone.Text;
            currentCustomer.IsActive = (bool)cbIsValid.IsChecked;
            currentCustomer.BossId = ((BossDataModel)cbBoss.SelectedItem).ID;
            

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
                    UpdateViewModel(currentCustomer);

                    if (IsNeedNew)
                    {
                        CommonUIFunction.ShowMessageText(bdMsgParent, "保存成功，请继续添加！");
                        _currentDataModal = new CustomerDataModel();

                        this.DataContext = _currentDataModal;
                    }
                    else
                    {
                        this.Close();
                    }
                }
            }, null);            
        }

        private void UpdateViewModel(Web.Customer customer)
        {
            CustomerViewModel customerVM = ViewModelManager.CustomerViewModelInstance;
            if (customerVM != null)
            {
                if (customer.ID != _currentDataModal.ID)
                {
                    customerVM.Items.Add(_currentDataModal);
                }

                _currentDataModal.ID = customer.ID;
                _currentDataModal.Name = customer.Name;
                if (!string.IsNullOrEmpty(_currentDataModal.Name))
                {
                    _currentDataModal.ClearErrors("Name");
                }
                _currentDataModal.PhoneNumber = customer.PhoneNumber;
                _currentDataModal.Address = customer.Address;
                _currentDataModal.PinYin = customer.PinYin;
                _currentDataModal.BossName = customer.Boss.Name;
                _currentDataModal.IsActive = customer.IsActive;
            }

            customerVM.UpdateIndex();
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

        private void RadWindow_Closed(object sender, WindowClosedEventArgs e)
        {
            var currentCustomer = (from q in SystemConfiguration.Instance.DataContext.Customers
                                   where q.ID == _currentDataModal.ID
                                   select q).SingleOrDefault();
            if (currentCustomer != null)
                UpdateViewModel(currentCustomer);
        }

        private void btnAddFeeType_Click(object sender, RoutedEventArgs e)
        {
            if (acbFeeType.SelectedItem == null)
            {
                return;
            }
            if (!Constants.IsDouble(tbAmount.Text))
            {
                MessageBox.Show("费用必须为数字.");
                return;
            }
            if (!Constants.IsDouble(tbCost.Text))
            {
                MessageBox.Show("成本必须为数字.");
                return;
            }

            FeeTypeDataModel dm = new FeeTypeDataModel();
            dm.Index = lvwFeeType.Items.Count + 1;
            dm.Code = ((FeeTypeDataModel)acbFeeType.SelectedItem).Code;
            dm.Name = ((FeeTypeDataModel)acbFeeType.SelectedItem).Name;
            dm.Amount = decimal.Parse(tbAmount.Text);
            dm.Cost = decimal.Parse(tbCost.Text);

            lvwFeeType.Items.Add(dm);

            acbFeeType.SelectedItem = null;
            tbAmount.Text = "";
            tbCost.Text = "";
        }

        private void acbFeeType_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            IList items = e.AddedItems;
            if (items != null && items.Count > 0)
            {
                tbAmount.Text = ((FeeTypeDataModel)items[0]).Amount.ToString();
                tbCost.Text = ((FeeTypeDataModel)items[0]).Cost.ToString();
            }
        }

        private void btnUpdateFeeType_Click(object sender, RoutedEventArgs e)
        {
            if (lvwFeeType.SelectedItem == null)
            {
                return;
            }
            if (acbFeeType.SelectedItem == null)
            {
                return;
            }
            if (!Constants.IsDouble(tbAmount.Text))
            {
                MessageBox.Show("费用必须为数字.");
                return;
            }
            if (!Constants.IsDouble(tbCost.Text))
            {
                MessageBox.Show("成本必须为数字.");
                return;
            }
            FeeTypeDataModel feeType = lvwFeeType.SelectedItem as FeeTypeDataModel;
            feeType.Code = ((FeeTypeDataModel)acbFeeType.SelectedItem).Code;
            feeType.Name = ((FeeTypeDataModel)acbFeeType.SelectedItem).Name;
            feeType.Amount = decimal.Parse(tbAmount.Text);
            feeType.Cost = decimal.Parse(tbCost.Text);

            lvwFeeType.SelectedItem = feeType;
            lvwFeeType.Rebind();
        }

        private void btnDeleteFeeType_Click(object sender, RoutedEventArgs e)
        {
            if (lvwFeeType.SelectedItem == null)
            {
                return;
            }

            lvwFeeType.Items.Remove(lvwFeeType.SelectedItem);
            lvwFeeType.Rebind();
        }

        private void lvwFeeType_SelectionChanged(object sender, SelectionChangeEventArgs e)
        {
            if (lvwFeeType.SelectedItem == null)
            {
                return;
            }

            FeeTypeDataModel feeType = lvwFeeType.SelectedItem as FeeTypeDataModel;
            acbFeeType.Text = feeType.Name;
            tbAmount.Text = feeType.Amount.ToString();
            tbCost.Text = feeType.Cost.ToString();
        }

    }
}
