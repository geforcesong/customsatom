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
using ProTemplate.UserControls.RadWindows;
using ProTemplate.ViewModels;
using System.Threading;
using ProTemplate.Utility;
using ProTemplate.Models;
using ProTemplate.Utility.PageObjects;
using System.ServiceModel.DomainServices.Client;

namespace ProTemplate.Views
{
    public partial class Examination : Page
    {
        SearchConditionObject _customsSearchConditionObject;
        public Examination()
        {
            InitializeComponent();
            toolBar.BatchEditButton = true;
            _customsSearchConditionObject = new SearchConditionObject();
        }

        #region 页面事件
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            new Thread(() =>
            {
                this.Dispatcher.BeginInvoke(() =>
                {
                    ViewModelManager.ResetCustomerViewModel();
                });
            }).Start();

            _customsSearchConditionObject.StartDate = _customsSearchConditionObject.StartDate;

            dateFilter.StartDate = _customsSearchConditionObject.StartDate;
            dateFilter.EndDate = _customsSearchConditionObject.EndDate;
            Load(SystemConfiguration.Instance.LoggedOnUser.ID);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            SystemConfiguration.Instance.DataContext.Examinations.Clear();
            ExaminationViewModel vm = ViewModelManager.ExaminationViewModelInstance;
            if (vm != null)
                vm.Items.Clear();
        }
        #endregion

        #region 页面方法
        private void Load(int userID)
        {
            if (_customsSearchConditionObject.SearchType == SearchType.SearchByDate)
            {
                SystemConfiguration.Instance.DataContext.Load(SystemConfiguration.Instance.DataContext.GetExaminationsByReceiveDateQuery(userID, _customsSearchConditionObject.StartDate, _customsSearchConditionObject.EndDate), delegate(LoadOperation<Web.Examination> lp)
                {
                    if (lp.Error != null)
                    {
                        lp.MarkErrorAsHandled();
                        MessageBox.Show(lp.Error.Message);
                    }
                    else
                    {
                        LoadViewModel(lp.Entities);
                    }
                }, null);
            }
            else if (_customsSearchConditionObject.SearchType == SearchType.SearchByCode)
            {
                SystemConfiguration.Instance.DataContext.Load(SystemConfiguration.Instance.DataContext.GetExaminationsByNumberQuery(userID, _customsSearchConditionObject.DeclarationCodes), delegate(LoadOperation<Web.Examination> lp)
                {
                    if (lp.Error != null)
                    {
                        lp.MarkErrorAsHandled();
                        MessageBox.Show(lp.Error.Message);
                    }
                    else
                    {
                        LoadViewModel(lp.Entities);
                    }
                }, null);
            }
        }

        void LoadViewModel(IEnumerable<Web.Examination> entities)
        {
            ExaminationViewModel vm = ViewModelManager.ExaminationViewModelInstance;
            if (vm != null)
            {
                vm.Items.Clear();
                int index = 1;
                foreach (var data in entities)
                {
                    ExaminationDataModel dm = new ExaminationDataModel();
                    dm.Index = index++;
                    dm.ID = data.ID;
                    dm.CustomerID = data.CustomerID;
                    dm.CustomerName = data.Customer.Name;
                    dm.GoodsName = data.GoodsName;
                    dm.Quantity = data.Quantity ?? 0;
                    dm.Amount = data.Amount ?? 0;
                    dm.Password = data.Password;
                    dm.TransferNumber = data.TransferNumber;
                    dm.ReceivedDate = data.ReceiveDate;
                    dm.ExaminationNumber = data.ExaminationNumber;
                    dm.ExaminationStatus = data.ExaminationStatus;
                    dm.RelatedSystemNumber = data.RelatedSystemNumber;
                    dm.Remark = data.Remark;
                    dm.IsRelated = data.IsRelated;
                    dm.ExaminationFee = data.ExaminationFee.HasValue ? data.ExaminationFee.Value : 0;
                    dm.ExaminationCost = data.ExaminationCost.HasValue ? data.ExaminationCost.Value : 0;
                    vm.Items.Add(dm);
                }

                if (vm.Items != null && vm.Items.Count > Constants.CommonGridViewPageSize)
                    gdPager.Visibility = System.Windows.Visibility.Visible;
                else
                    gdPager.Visibility = System.Windows.Visibility.Collapsed;
            }
        }
        #endregion

        #region 工具栏事件
        private void toolBar_NewClick(object sender, EventArgs e)
        {
            AddExamination wnd = new AddExamination();
            wnd.ShowDialog();
        }

        private void toolBar_EditClick(object sender, EventArgs e)
        {
            if (gdExamination.SelectedItems == null || gdExamination.SelectedItems.Count == 0)
            {
                CommonUIFunction.ShowMessageBox(MessageTexts.MSG022);
                return;
            }

            AddExamination wnd = new AddExamination(gdExamination.SelectedItems[0] as ExaminationDataModel);
            wnd.ShowDialog();
        }

        private void dateFilter_OnSearch(object sender, UserControls.DateSearchResultEventArgs e)
        {
            if (e.SearchType == SearchType.SearchByDate)
            {
                _customsSearchConditionObject.SearchType = SearchType.SearchByDate;
                _customsSearchConditionObject.StartDate = (DateTime)e.StartDate;
                _customsSearchConditionObject.EndDate = (DateTime)e.EndDate;
            }
            else
            {
                _customsSearchConditionObject.SearchType = SearchType.SearchByCode;
                _customsSearchConditionObject.DeclarationCodes = e.Codes;
            }
            Load(SystemConfiguration.Instance.LoggedOnUser.ID);
        }

        private void toolBar_DeleteClick(object sender, EventArgs e)
        {
            if (gdExamination.SelectedItems == null || gdExamination.SelectedItems.Count == 0)
            {
                CommonUIFunction.ShowMessageBox(MessageTexts.MSG023);
                return;
            }
            else
            {
                if (CommonUIFunction.ShowConfirm(MessageTexts.MSG024) == MessageBoxResult.Cancel)
                    return;
                bool isDeleted = false;
                foreach (var c in gdExamination.SelectedItems)
                {
                    ExaminationDataModel cdm = c as ExaminationDataModel;
                    if (cdm == null)
                        continue;

                    var queryObj = (from o in SystemConfiguration.Instance.DataContext.Examinations
                                    where o.ID == cdm.ID
                                    select o).SingleOrDefault();
                    if (queryObj != null)
                    {
                        SystemConfiguration.Instance.DataContext.Examinations.Remove(queryObj);
                        isDeleted = true;
                    }
                }

                if (isDeleted)
                {
                    CommonUIFunction.SetApplcationBusyIndicator(true, "正在删除，请稍后");
                    SystemConfiguration.Instance.DataContext.SubmitChanges((a) =>
                    {
                        CommonUIFunction.SetApplcationBusyIndicator(false);
                        if (a.HasError)
                        {
                            a.MarkErrorAsHandled();
                            CommonUIFunction.ShowMessageBox(ErrorTexts.ERR003 + a.Error.Message);
                            SystemConfiguration.Instance.DataContext.RejectChanges();
                        }
                        else
                        {
                            ExaminationViewModel cvm = ViewModelManager.ExaminationViewModelInstance;
                            if (cvm != null)
                            {
                                for (int i = 0; i < gdExamination.SelectedItems.Count; i++)
                                {
                                    ExaminationDataModel customerDM = gdExamination.SelectedItems[i] as ExaminationDataModel;
                                    cvm.Items.Remove(customerDM);
                                    i--;
                                }
                                cvm.UpdateIndex();
                            }
                        }
                    }, null);
                }
            }
        }

        private void toolBar_ChangeExaminationStatusClick(object sender, UserControls.ExaminationStatusEventArgs e)
        {
            if (gdExamination.SelectedItems.Count == 0)
            {
                CommonUIFunction.ShowMessageBox("请先选择您要改变状态的商检单！");
                return;
            }

            if (CommonUIFunction.ShowConfirm("是否将选中商检的商检状态设置为" + e.ExaminationStatus) == MessageBoxResult.Cancel)
                return;

            bool isChanged = false;
            foreach (var c in gdExamination.SelectedItems)
            {
                ExaminationDataModel cdm = c as ExaminationDataModel;
                if (cdm == null)
                    continue;

                var queryObj = (from o in SystemConfiguration.Instance.DataContext.Examinations
                                where o.ID == cdm.ID
                                select o).SingleOrDefault();
                if (queryObj != null)
                {
                    queryObj.ExaminationStatus = e.ExaminationStatus;
                    isChanged = true;
                }
            }

            if (isChanged)
            {
                CommonUIFunction.SetApplcationBusyIndicator(true, "正在更新，请稍后");
                SystemConfiguration.Instance.DataContext.SubmitChanges((io) =>
                {
                    CommonUIFunction.SetApplcationBusyIndicator(false);
                    if (io.HasError)
                    {
                        MessageBox.Show(io.Error.Message);
                        if (io.Error.InnerException != null)
                        {
                            MessageBox.Show(io.Error.InnerException.Message);
                        }
                        io.MarkErrorAsHandled();
                        SystemConfiguration.Instance.DataContext.RejectChanges();
                    }
                    else
                    {
                        //更新ViewModel
                        for (int i = 0; i < gdExamination.SelectedItems.Count; i++)
                            ((ExaminationDataModel)gdExamination.SelectedItems[i]).ExaminationStatus = e.ExaminationStatus;
                    }
                }, null);
            }
        }

        private void toolBar_ExportToExcelClick(object sender, EventArgs e)
        {
            CommonUIFunction.ExportToFile(ExportFileType.Excel, gdExamination);
        }

        private void toolBar_ExportToWordClick(object sender, EventArgs e)
        {
            CommonUIFunction.ExportToFile(ExportFileType.Excel, gdExamination, true);
        }

        private void toolBar_RefreshClick(object sender, EventArgs e)
        {
            Load(SystemConfiguration.Instance.LoggedOnUser.ID);
        }

        private void toolBar_BatchEditClick(object sender, EventArgs e)
        {
            if (gdExamination.SelectedItems.Count > 0)
            {
                List<ExaminationDataModel> ids = new List<ExaminationDataModel>();
                foreach (ExaminationDataModel dm in gdExamination.SelectedItems)
                {
                    ids.Add(dm);
                }
                ExaminationBatchEditForm form = new ExaminationBatchEditForm();
                form.Load(ids);
                form.ShowDialog();
            }
        }
        #endregion


    }
}
