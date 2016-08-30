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
using ProTemplate.ViewModels;
using ProTemplate.Models;
using ProTemplate.Utility;

namespace ProTemplate.UserControls.RadWindows
{
    public partial class AddExamination : RadWindow
    {
        ExaminationDataModel _inputDataModel;
        string [] _existingTransferNumber;
        public AddExamination()
        {
            InitializeComponent();

            _inputDataModel = new ExaminationDataModel();
            _inputDataModel.ExaminationNumber = "31005221";
            _inputDataModel.ReceivedDate = DateTime.Now;
            this.DataContext = _inputDataModel;
            this.Header = "新增商检";
            toolBar.IsNew = true;
            ExaminationEditState = FormState.Add;
        }

        FormState ExaminationEditState { get; set; }

        public AddExamination(ExaminationDataModel ex)
        {
            InitializeComponent();

            _inputDataModel = ex;
            this.DataContext = _inputDataModel;
            this.Header = "编辑商检";
            toolBar.IsNew = false;
            ExaminationEditState = FormState.Update;
        }

        #region AutoCompleteBox Populating Events
        private void acbCustomer_Populating(object sender, PopulatingEventArgs e)
        {
            CustomerViewModel vm = ViewModelManager.CustomerViewModelInstance;
            if (vm != null)
            {
                var prar = e.Parameter.ToLower();
                var query = from a in vm.Items
                            where a.PinYin.ToLower().Contains(prar) || a.Name.ToLower().Contains(prar)
                            select a;
                AutoCompleteBox source = (AutoCompleteBox)sender;
                source.ItemsSource = query.ToList();
            }
        }
        #endregion

        #region ToolBar Events
        private void toolBar_SaveAndClose(object sender, EventArgs e)
        {
            SystemConfiguration.Instance.DataContext.GetExistingTransferNumbers((b) => {
                if (b.Error == null)
                {
                    _existingTransferNumber = b.Value as string[];
                    if (!InputCheck())
                        return;
                    Save(false);
                }
                else
                {
                    MessageBox.Show("获取以存在的转单号失败，请重试！");
                    return;
                }
            }, null);
        }

        private void toolBar_SaveAndNew(object sender, EventArgs e)
        {
            SystemConfiguration.Instance.DataContext.GetExistingTransferNumbers((b) => {
                if (b.Error == null)
                {
                    _existingTransferNumber = b.Value as string[];
                    if (!InputCheck())
                        return;
                    Save(true);
                }
                else
                {
                    MessageBox.Show("获取以存在的转单号失败，请重试！");
                    return;
                }
            },null);
        }

        private void toolBar_Close(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Save(bool isNew)
        {
            var currentExamination = (from q in SystemConfiguration.Instance.DataContext.Examinations
                                      where q.ID == _inputDataModel.ID
                                      select q).SingleOrDefault();

            if (currentExamination == null)
            {
                currentExamination = new Web.Examination();
                SystemConfiguration.Instance.DataContext.Examinations.Add(currentExamination);
            }

            
            currentExamination.ReceiveDate = (DateTime)dpReceiveDate.SelectedDate;
            currentExamination.CustomerID = ((CustomerDataModel)acbCustomer.SelectedItem).ID;
            currentExamination.GoodsName = tbProductName.Text;
            decimal val;
            decimal.TryParse(tbQuantity.Text, out val);
            currentExamination.Quantity = val;
            decimal.TryParse(tbAmount.Text, out val);
            currentExamination.Amount = val;
            currentExamination.Password = tbPassword.Text;
            currentExamination.TransferNumber = tbTransferNumber.Text;
            currentExamination.ExaminationNumber = tbExaminationNumber.Text;
            currentExamination.Remark = tbRemark.Text;
            currentExamination.RelatedSystemNumber = tbRelatedSystemNumber.Text;
            currentExamination.ExaminationStatus = "未检";
            decimal.TryParse(tbFee.Text, out val);
            currentExamination.ExaminationFee = val;
            decimal.TryParse(tbExaminationCost.Text, out val);
            currentExamination.ExaminationCost = val;

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
                        ExaminationViewModel evm = ViewModelManager.ExaminationViewModelInstance;
                        if (evm != null)
                        {
                            if (ExaminationEditState == FormState.Add)
                            {
                                ExaminationDataModel dm = new ExaminationDataModel();
                                dm.ID = currentExamination.ID;
                                dm.CustomerID = currentExamination.CustomerID;
                                dm.CustomerName = ((CustomerDataModel)acbCustomer.SelectedItem).Name;
                                dm.GoodsName = currentExamination.GoodsName;
                                dm.Quantity = currentExamination.Quantity ?? 0;
                                dm.Amount = currentExamination.Amount ?? 0;
                                dm.Password = currentExamination.Password;
                                dm.TransferNumber = currentExamination.TransferNumber;
                                dm.ReceivedDate = currentExamination.ReceiveDate;
                                dm.ExaminationNumber = currentExamination.ExaminationNumber;
                                dm.ExaminationStatus = currentExamination.ExaminationStatus;
                                dm.Remark = currentExamination.Remark;
                                dm.RelatedSystemNumber = currentExamination.RelatedSystemNumber;
                                dm.ExaminationCost = currentExamination.ExaminationCost??0;
                                evm.Items.Add(dm);
                                evm.UpdateIndex();
                            }
                            else
                            {
                                var dm = (from d in evm.Items
                                          where d.ID == currentExamination.ID
                                          select d).SingleOrDefault();
                                if (dm != null)
                                {
                                    dm.CustomerID = currentExamination.CustomerID;
                                    dm.CustomerName = ((CustomerDataModel)acbCustomer.SelectedItem).Name;
                                    dm.GoodsName = currentExamination.GoodsName;
                                    dm.Quantity = currentExamination.Quantity ?? 0;
                                    dm.Amount = currentExamination.Amount ?? 0;
                                    dm.Password = currentExamination.Password;
                                    dm.TransferNumber = currentExamination.TransferNumber;
                                    dm.ReceivedDate = currentExamination.ReceiveDate;
                                    dm.ExaminationNumber = currentExamination.ExaminationNumber;
                                    dm.ExaminationStatus = currentExamination.ExaminationStatus;
                                    dm.Remark = currentExamination.Remark;
                                    dm.RelatedSystemNumber = currentExamination.RelatedSystemNumber;
                                    dm.ExaminationCost = currentExamination.ExaminationCost ?? 0;
                                }
                            }

                        }

                        if (isNew)
                        {
                            CommonUIFunction.ShowMessageText(bdMsgParent, "保存成功，请继续添加！");
                            _inputDataModel = new ExaminationDataModel();
                            _inputDataModel.ReceivedDate = DateTime.Now;
                            this.DataContext = _inputDataModel;
                        }
                        else
                        {
                            this.Close();
                        }
                    }
                }, null);
        }
        #endregion

        bool InputCheck()
        {
            if (acbCustomer.Text == "" || acbCustomer.SelectedItem == null)
            {
                _inputDataModel.SetErrors("CustomerName", new List<string>() { "请输入合法客户" });
            }
            else
            {
                _inputDataModel.ClearErrors("CustomerName");
            }

            if (dpReceiveDate.SelectedDate == null)
                _inputDataModel.SetErrors("ReceivedDate", new List<string>() { "请输入接收日期！" });
            else
                _inputDataModel.ClearErrors("ReceivedDate");

            if (!Constants.IsDouble(tbFee.Text))
            {
                _inputDataModel.SetErrors("ExaminationFee", new List<string>() { "费用为数字格式" });
            }
            else
            {
                _inputDataModel.ClearErrors("ExaminationFee");
            }

            if (!Constants.IsDouble(tbQuantity.Text))
            {
                _inputDataModel.SetErrors("Quantity", new List<string>() { "数量为数字格式" });
            }
            else
            {
                _inputDataModel.ClearErrors("Quantity");
            }

            if (!Constants.IsDouble(tbAmount.Text))
            {
                _inputDataModel.SetErrors("Amount", new List<string>() { "金额为数字格式" });
            }
            else
            {
                _inputDataModel.ClearErrors("Amount");
            }

            string[] existingTransferNumbers = null;
            if (ExaminationEditState == FormState.Update)
                existingTransferNumbers = _existingTransferNumber.Where(a => a != _inputDataModel.TransferNumber).ToArray();
            else
                existingTransferNumbers = _existingTransferNumber;

            if (tbTransferNumber.Text != null && existingTransferNumbers != null)
            {
                if (existingTransferNumbers.Contains(tbTransferNumber.Text))
                {
                    _inputDataModel.SetErrors("TransferNumber", new List<string>() { "转单号不能重复！" });
                }
                else
                    _inputDataModel.ClearErrors("TransferNumber");
            }
            else
            {
                _inputDataModel.ClearErrors("TransferNumber");
            }
            return !_inputDataModel.HasErrors;// && isTransactionSelected;
        }

    }
}
