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

namespace ProTemplate.UserControls.RadWindows
{
    public partial class AddFeeType : RadWindow
    {
        FeeTypeDataModel _currentDataModal;
        public AddFeeType()
        {
            InitializeComponent();
            _currentDataModal = new FeeTypeDataModel();
            this.DataContext = _currentDataModal;
            this.Header = "新增费用类型";
            toolBar.IsNew = true;
        }

        public AddFeeType(FeeTypeDataModel feeType)
        {
            InitializeComponent();
            _currentDataModal = feeType;
            this.DataContext = _currentDataModal;
            this.Header = "编辑费用类型";
            tbCode.IsReadOnly = true;
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
            if (tbCode.Text.Trim() == "")
            {
                _currentDataModal.SetErrors("Code", new List<string>() { "费用类型代码不能为空" });
            }
            else
            {
                if (toolBar.IsNew == true)
                {
                    FeeTypeViewModel cvm = App.Current.Resources["FeeTypeViewModel"] as FeeTypeViewModel;
                    if (cvm.Items.FirstOrDefault(o => o.Code == tbCode.Text) != null)
                    {
                        _currentDataModal.SetErrors("Code", new List<string>() { "费用类型代码已存在，不能新建重复的费用类型。" });
                    }
                    else
                    {
                        _currentDataModal.ClearErrors("Code");
                    }
                }
                else
                {
                    _currentDataModal.ClearErrors("Code");
                }
            }

            if (tbName.Text == "")
            {
                _currentDataModal.SetErrors("Name", new List<string>() { "费用类型名称不能为空" });
            }
            else
            {
                _currentDataModal.ClearErrors("Name");
            }

            if (!Constants.IsDouble(tbAmount.Text))
            {
                _currentDataModal.SetErrors("Amount", new List<string>() { "默认费用只能是数字" });
            }
            else
            {
                _currentDataModal.ClearErrors("Amount");
            }

            if (!Constants.IsDouble(tbCost.Text))
            {
                _currentDataModal.SetErrors("Cost", new List<string>() { "默认成本只能是数字" });
            }
            else
            {
                _currentDataModal.ClearErrors("Cost");
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

            var currentFeeType = (from q in SystemConfiguration.Instance.DataContext.FeeTypes
                               where q.Code == _currentDataModal.Code
                               select q).SingleOrDefault();

            if (currentFeeType == null)
            {
                currentFeeType = new Web.FeeType();
                SystemConfiguration.Instance.DataContext.FeeTypes.Add(currentFeeType);
            }

            currentFeeType.Code = tbCode.Text;
            currentFeeType.Name = tbName.Text;
            currentFeeType.Amount = decimal.Parse(tbAmount.Text);
            currentFeeType.Cost = decimal.Parse(tbCost.Text);

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
                    UpdateViewModel(currentFeeType);

                    if (IsNeedNew)
                    {
                        CommonUIFunction.ShowMessageText(bdMsgParent, "保存成功，请继续添加！");
                        _currentDataModal = new FeeTypeDataModel();
                        this.DataContext = _currentDataModal;
                    }
                    else
                    {
                        this.Close();
                    }
                }
            }, null);
        }

        private void UpdateViewModel(Web.FeeType feeType)
        {
            FeeTypeViewModel feeTypeVM = App.Current.Resources["FeeTypeViewModel"] as FeeTypeViewModel;
            if (feeTypeVM != null)
            {
                if (feeType.Code != _currentDataModal.Code)
                {
                    feeTypeVM.Items.Add(_currentDataModal);
                }

                _currentDataModal.Code = feeType.Code;
                _currentDataModal.Name = feeType.Name;
                _currentDataModal.Amount = feeType.Amount;
                _currentDataModal.Cost = feeType.Cost;

                // update index
                feeTypeVM.UpdateIndex();
            }
        }

        #endregion

        private void RadWindow_Closed(object sender, WindowClosedEventArgs e)
        {
            if (_currentDataModal != null)
            {
                _currentDataModal.ClearErrors("Name");
                _currentDataModal.ClearErrors("Code");
                _currentDataModal.ClearErrors("Amount");
                _currentDataModal.ClearErrors("Cost");
            }
        }
    }
}
