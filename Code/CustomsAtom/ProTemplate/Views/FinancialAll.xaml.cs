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
using ProTemplate.ViewModels;
using ProTemplate.Utility.PageObjects;
using System.Threading;
using ProTemplate.Utility;
using ProTemplate.Models;
using System.ServiceModel.DomainServices.Client;
using ProTemplate.UserControls.RadWindows;
using System.Windows.Browser;
using Telerik.Windows;
using Telerik.Windows.Controls.GridView;

namespace ProTemplate.Views
{
    public partial class FinancialAll : Page
    {
        SearchConditionObject _customsSearchConditionObject;
        public FinancialAll()
        {
            InitializeComponent();
            _customsSearchConditionObject = new SearchConditionObject();
            toolbar.NewButton = false;
            toolbar.DeleteButton = false;
            toolbar.BatchEditButton = true;
            toolbar.IsShowSetDeclarationStatusGroup = true;
            if (SystemConfiguration.Instance.LoggedOnUser.RoleList.FirstOrDefault(o => o.Name.Contains("客户")) != null)
            {
                gdFinancialAll.Columns[18].IsVisible = false;
                gdFinancialAll.Columns[22].IsVisible = false;
                gdFinancialAll.Columns[24].IsVisible = false;
                gdFinancialAll.Columns[27].IsVisible = false;
                gdFinancialAll.Columns[29].IsVisible = false;
                gdFinancialAll.Columns[31].IsVisible = false;
                gdFinancialAll.Columns[33].IsVisible = false;
                gdFinancialAll.Columns[34].IsVisible = false;
            }
            gdFinancialAll.AddHandler(GridViewCellBase.CellDoubleClickEvent, new EventHandler<RadRoutedEventArgs>(OnCellDoubleClick), true);
        }

        private void OnCellDoubleClick(object sender, RadRoutedEventArgs args)
        {
            GridViewCellBase cell = args.OriginalSource as GridViewCellBase;
            TextBlock tb = cell.Content as TextBlock;
            if (tb != null)
            {
                GetAllFinancialExportDeclarationDataModel dm = cell.DataContext as GetAllFinancialExportDeclarationDataModel;
                if (dm != null)
                    OpenEditWindow(dm.ID);
            }
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {


            new Thread(() =>
            {
                this.Dispatcher.BeginInvoke(() =>
                {
                    ViewModelManager.ResetCustomerViewModel();
                    ViewModelManager.ResetTransactionViewModel();
                    ViewModelManager.ResetHSCodeDictionaryViewModelViewModel();
                    ViewModelManager.ResetCustomhouseViewModel();
                    ViewModelManager.ResetDistrictViewModel();
                    ViewModelManager.ResetTradeViewModel();
                    ViewModelManager.ResetCurrencyViewModel();
                    ViewModelManager.ResetTransportViewModel();
                    ViewModelManager.ResetFeeMarkViewModel();
                    ViewModelManager.ResetWrapViewModel();
                    ViewModelManager.ResetPayViewModel();
                    ViewModelManager.ResetLevyViewModel();
                    ViewModelManager.ResetCountryViewModel();
                    ViewModelManager.ResetPortViewModel();
                    ViewModelManager.ResetDutyViewModel();
                    ViewModelManager.ResetUnitViewModel();
                    ViewModelManager.ResetDocumentViewModel();
                    ViewModelManager.ResetFeeTypeViewModel();
                });
            }).Start();

            Load(SystemConfiguration.Instance.LoggedOnUser.ID);
            //SetPermission();
        }
        private void SetPermission()
        {
            var qUser = from a in SystemConfiguration.Instance.LoggedOnUser.RoleList
                        where a.Name == "管理员"
                        select a.Name;
            if (qUser.Count() > 0)
            {
                toolbar.IsShowSetDeclarationStatusGroup = true;
                toolbar.CanAddDelete = true;
            }
            else
            {
                toolbar.IsShowSetDeclarationStatusGroup = false;
                toolbar.CanAddDelete = false;
            }

            //if (SystemConfiguration.Instance.LoggedOnUser.RoleList.FirstOrDefault(o => o.Name.Contains("客户")) != null)
            //{
            //    colCheckCost.IsVisible = false;
            //    colCommissionCost.IsVisible = false;
            //    colDeclarationCost.IsVisible = false;
            //    colExaminationCost.IsVisible = false;
            //    colOtherCost.IsVisible = false;
            //}
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
            GetAllFinancialExportDeclarationViewModel vm = App.Current.Resources["GetAllFinancialExportDeclarationViewModel"] as GetAllFinancialExportDeclarationViewModel;
            if (vm != null)
                vm.Items.Clear();
        }

        private void Load(int userID)
        {
            string condition = declarationFilter.ExcuteQuery();
            SystemConfiguration.Instance.DataContext.Load(SystemConfiguration.Instance.DataContext.GetAllFinancialExportDeclarationByReceiveDateQuery(userID, EncryptionUtil.Encrypt(condition)), delegate(LoadOperation<Web.GetAllFinancialDeclaration> lp)
            {
                if (lp.Error != null)
                {
                    lp.MarkErrorAsHandled();
                    MessageBox.Show(lp.Error.Message);
                }
                else
                {
                    //gdPager.ItemCount = lp.TotalEntityCount;
                    //gdCustomerAll.p
                    LoadViewModel(lp.Entities);
                }
            }, null);
        }
        private void LoadViewModel(IEnumerable<Web.GetAllFinancialDeclaration> entities)
        {
            List<GetAllFinancialExportDeclarationDataModel> vm = new List<GetAllFinancialExportDeclarationDataModel>();
            if (vm != null)
            {
               // vm.Items.Clear();
                int index = 1;

                foreach (var data in entities)
                {
                    GetAllFinancialExportDeclarationDataModel dm = new GetAllFinancialExportDeclarationDataModel();
                    dm.Index = index++;
                    dm.ID = data.ID;
                    dm.CustomerName = data.CustomerName;
                    dm.ReceivedDate = data.ReceivedDate;
                    dm.DeclarationNumber = data.DeclarationNumber;
                    dm.TraderName = data.TraderName;
                    dm.Conveyance = data.Conveyance;
                    dm.VoyageNumber = data.VoyageNumber;
                    dm.BillNumber = data.BillNumber;
                    dm.ApprovalNumber = data.ApprovalNumber;
                    dm.DeclarationStatus = data.DeclarationStatus;
                    dm.DrawbackStatus = data.DrawbackStatus;
                    dm.ShipLeaveDate = data.ShipLeaveDate;
                    dm.BillCount = (data.TotalItems - 1) / 5;
                    dm.Remark = data.Remark;
                    dm.ExaminationNumber = data.CertificateNumber;
                    dm.HasExamination = data.IsExamination == 1 ? "是" : "否";
                    dm.DeclarationFeeAmount = data.DeclarationFeeAmount;
                    dm.DeclarationFeeCost = data.DeclarationFeeCost;
                    dm.BillFeeAmount = data.BillFeeAmount;
                    dm.ExaminationFeeAmount = data.ExaminationFeeAmount;
                    dm.ExaminationFeeCost = data.ExaminationFeeCost;
                    dm.CheckFeeAmount = data.CheckFeeAmount;
                    dm.CheckFeeCost = data.CheckFeeCost;
                    dm.CommissionFeeAmount = data.CommissionFeeAmount;
                    dm.CommissionFeeCost = data.CommissionFeeCost;
                    dm.OtherFeeAmount = data.OtherFeeAmount;
                    dm.OtherFeeCost = data.OtherFeeCost;
                    dm.PackageAmount = data.PackageAmount;
                    dm.GrossWeight = data.GrossWeight;
                    dm.DeclarationDate = data.DeclarationDate;
                    dm.Dock = data.Dock;
                    dm.RelatedSystemNumber = data.RelatedSystemNumber;
                    dm.ContainerNumbers = data.ContainerNumbers;
                    dm.FinancialRemark = data.FinancialRemark;
                    dm.IsLocked = true;
                    vm.Add(dm);
                }

                gdFinancialAll.ItemsSource = vm;

                if (vm != null && vm.Count > Constants.CommonGridViewPageSize)
                    gdPager.Visibility = System.Windows.Visibility.Visible;
                else
                    gdPager.Visibility = System.Windows.Visibility.Collapsed;

                //清除DataContext 中的数据。释放资源。
                if (SystemConfiguration.Instance.DataContext.GetAllFinancialDeclarations != null)
                    SystemConfiguration.Instance.DataContext.GetAllFinancialDeclarations.Clear();
            }
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

        private void dateFilter_OnReset(object sender, EventArgs e)
        {
            gdFinancialAll.FilterDescriptors.Clear();
        }

        private void CommonToolBar_PrintReportClick(object sender, EventArgs e)
        {
                        //string sql = "select * from boss";
            int[] ids = new int[gdFinancialAll.SelectedItems.Count];
            for (int i = 0; i < ids.Length; i++)
                ids[i] = ((GetAllFinancialExportDeclarationDataModel)gdFinancialAll.SelectedItems[i]).ID;

            string sql = EncryptionUtil.Encrypt(string.Format(Constants.PrintSQL, string.Join(",", ids)));

            HtmlPopupWindowOptions options = new HtmlPopupWindowOptions();
            options.Resizeable = true;
            options.Left = 0;
            options.Top = 0;
            options.Toolbar = true;
            options.Width = 1000;
            options.Height = 800;
            options.Menubar = true;
            options.Directories = true;
            HtmlPage.Window.Invoke("OpenNormalWindow", new Uri("http://" + App.Current.Host.Source.Host + ":" + App.Current.Host.Source.Port + "/Report/ReportForm.aspx?Report=ExportDeclarationForm&content=" + sql));
        }

        private void CommonToolBar_RefreshClick(object sender, EventArgs e)
        {
            Load(SystemConfiguration.Instance.LoggedOnUser.ID);
        }
        private void CommonToolBar_ChangeDeclarationStatusClick(object sender, EventArgs e)
        {

            int[] ids = new int[gdFinancialAll.SelectedItems.Count];
            for (int i = 0; i < ids.Length; i++)
                ids[i] = ((GetAllFinancialExportDeclarationDataModel)gdFinancialAll.SelectedItems[i]).ID;

            SetDeclarationStatus form = new SetDeclarationStatus(ids);
            form.ShowDialog();
            form.Closed += new EventHandler<Telerik.Windows.Controls.WindowClosedEventArgs>(form_Closed);
        }

        void form_Closed(object sender, Telerik.Windows.Controls.WindowClosedEventArgs e)
        {
            Load(SystemConfiguration.Instance.LoggedOnUser.ID);
        }

        private void CommonToolBar_EditClick(object sender, EventArgs e)
        {
            if (gdFinancialAll.SelectedItems == null || gdFinancialAll.SelectedItems.Count == 0)
            {
                CommonUIFunction.ShowMessageBox(MessageTexts.MSG018);
                return;
            }

            var dID = ((GetAllFinancialExportDeclarationDataModel)gdFinancialAll.SelectedItems[0]).ID;
            OpenEditWindow(dID);
        }

        private static void OpenEditWindow(int dID)
        {
            SystemConfiguration.Instance.DataContext.Declarations.Clear();
            SystemConfiguration.Instance.DataContext.FinancialExportDeclarations.Clear();

            SystemConfiguration.Instance.DataContext.Load(SystemConfiguration.Instance.DataContext.GetDeclarationByIDQuery(dID),
                (arg) =>
                {
                    if (arg.HasError)
                    {
                        arg.MarkErrorAsHandled();
                        MessageBox.Show(arg.Error.Message);
                    }
                    else
                    {
                        if (arg.Entities.Count() > 0)
                        {
                            DeclarationEditFormDataModel dataModel = new DeclarationEditFormDataModel();
                            var declaration = arg.Entities.ElementAt(0);
                            dataModel.ID = declaration.ID;
                            dataModel.CustomerID = declaration.CustomerID;
                            CustomerViewModel cvm = ViewModelManager.CustomerViewModelInstance;
                            if (cvm != null)
                                dataModel.CustomerName = cvm.GetCustomerName(declaration.CustomerID);
                            //基本tab
                            dataModel.ReceivedDate = declaration.ReceivedDate;
                            dataModel.PreEntryNumber = declaration.PreEntryNumber;
                            dataModel.DeclarationNumber = declaration.DeclarationNumber;
                            dataModel.BillNumber = declaration.BillNumber;
                            dataModel.ApprovalNumber = declaration.ApprovalNumber;
                            dataModel.Dock = declaration.Dock;
                            dataModel.RelatedSystemNumber = declaration.RelatedSystemNumber;
                            dataModel.ShipLeaveDate = declaration.ShipLeaveDate;
                            dataModel.Remark = declaration.Remark;
                            dataModel.DeclarationStatus = declaration.DeclarationStatus;
                            dataModel.DeclarationStatusRemark = declaration.DeclarationStatusRemark;
                            dataModel.DeclarationDate = declaration.DeclarationDate;
                            dataModel.DrawbackStatus = declaration.DrawbackStatus;
                            dataModel.DrawbackStatusRemark = declaration.DrawbackStatusRemark;
                            dataModel.DrawbackDate = declaration.DrawbackDate;
                            dataModel.VerificationStatus = declaration.VerificationStatus;
                            dataModel.VerificationStatusDetail = declaration.VerificationStatusDetail;
                            dataModel.AdmissionStatus = declaration.AdmissionStatus;
                            dataModel.LadingStatus = declaration.LadingStatus;
                            dataModel.OnBoardingStatus = declaration.OnBoardingStatus;
                            // 报关
                            /// Customerhouse
                            CustomhouseViewModel chvm = ViewModelManager.CustomhouseViewModelInstance;
                            if (chvm != null)
                                dataModel.CustomhouseName = chvm.GetCustomhouseName(declaration.CustomhouseCode);
                            // IEDate
                            dataModel.IEDate = declaration.IEDate;
                            /// Transport
                            TransportViewModel tpvm = ViewModelManager.TransportViewModelInstance;
                            if (tpvm != null)
                                dataModel.TransportName = tpvm.GetTransportName(declaration.TransportCode);
                            // 航次号
                            dataModel.VoyageNumber = declaration.VoyageNumber;
                            //贸易方式
                            TradeViewModel tvm = ViewModelManager.TradeViewModelInstance;
                            if (tvm != null)
                                dataModel.TradeName = tvm.GetTradeName(declaration.TradeCode);
                            // levy
                            LevyViewModel lvm = ViewModelManager.LevyViewModelInstance;
                            if (lvm != null)
                                dataModel.LevyName = lvm.GetLevyName(declaration.LevyCode);
                            // 结汇方式
                            PayViewModel pvm = ViewModelManager.PayViewModelInstance;
                            if (pvm != null)
                                dataModel.PayName = pvm.GetPayName(declaration.PayCode);

                            // 运抵国
                            CountryViewModel ctrvm = ViewModelManager.CountryViewModelInstance;
                            if (cvm != null)
                                dataModel.CountryName = ctrvm.GetCountryName(declaration.CountryCode);

                            // 抵运港
                            PortViewModel ptvm = ViewModelManager.PortViewModelInstance;
                            if (ptvm != null)
                                dataModel.PortName = ptvm.GetPortName(declaration.PortCode);

                            // 境内货源地
                            DistrictViewModel dvm = ViewModelManager.DistrictViewModelInstance;
                            if (dvm != null)
                                dataModel.DistrictName = dvm.GetDistrictName(declaration.DistrictCode);
                            //成交方式
                            TransactionViewModel tavm = ViewModelManager.TransactionViewModelInstance;
                            if (tavm != null)
                                dataModel.TransactionName = tavm.GetTransactionName(declaration.TransactionCode);

                            // 运费
                            dataModel.FeeTrans = declaration.FreightFeeRate ?? 0;
                            dataModel.FeeInsure = declaration.InsuranceFeeRate ?? 0;
                            dataModel.FeeOther = declaration.OtherFeeRate ?? 0;
                            FeeMarkViewModel uvm = ViewModelManager.FeeMarkViewModelInstance;
                            if (uvm != null)
                            {
                                dataModel.FeeMarkTransName = uvm.GetFeeMarkName(declaration.FreightFeeMarkCode);
                                dataModel.FeeMarkInsureName = uvm.GetFeeMarkName(declaration.InsuranceFeeMarkCode);
                                dataModel.FeeMarkOtherName = uvm.GetFeeMarkName(declaration.OtherFeeMarkCode);
                            }
                            CurrencyViewModel cyvm = ViewModelManager.CurrencyViewModelInstance;
                            if (cyvm != null)
                            {
                                dataModel.CurrencyTransName = cyvm.GetCurrencyName(declaration.FreightFeeCurrencyCode);
                                dataModel.CurrencyInsureName = cyvm.GetCurrencyName(declaration.InsuranceFeeCurrencyCode);
                                dataModel.CurrencyOtherName = cyvm.GetCurrencyName(declaration.OtherFeeCurrencyCode);
                            }
                            dataModel.CreateDate = declaration.CreatedDate;
                            dataModel.NetWeight = declaration.NetWeight ?? 0;
                            dataModel.GrossWeight = declaration.GrossWeight ?? 0;
                            dataModel.ContainerQuantity = declaration.ContainerQuantity;
                            dataModel.PackageAmount = declaration.PackageAmount ?? 0;
                            dataModel.ManualNumber = declaration.ManualNumber;
                            dataModel.ControlNumber = declaration.ControlNumber;
                            dataModel.Conveyance = declaration.Conveyance;
                            dataModel.Note = declaration.Note;
                            dataModel.TraderCode = declaration.TraderCode;
                            dataModel.TraderName = declaration.TraderName;
                            dataModel.OwnerCode = declaration.OwnerCode;
                            dataModel.OwnerName = declaration.OwnerName;
                            dataModel.AgentCode = declaration.AgentCode;
                            dataModel.AgentName = declaration.AgentName;
                            dataModel.LicenseNumber = declaration.LicenseNumber;
                            dataModel.FinancialRemark = declaration.FinancialRemark;
                            WrapViewModel wvm = ViewModelManager.WrapViewModelInstance;
                            if (wvm != null)
                                dataModel.WrapName = wvm.GetWrapName(declaration.WrapCode);


                            // 打开窗体
                            DeclarationEditForm def = new DeclarationEditForm(dataModel);
                            def.ShowDialog();
                        }
                        else
                            CommonUIFunction.ShowMessageBox(MessageTexts.MSG019);
                    }
                }, null);
        }

        private void CommonToolBar_ExportToExcelClick(object sender, EventArgs e)
        {
            CommonUIFunction.ExportToFile(ExportFileType.Excel, gdFinancialAll);
        }

        private void CommonToolBar_ExportToWordClick(object sender, EventArgs e)
        {
            CommonUIFunction.ExportToFile(ExportFileType.Excel, gdFinancialAll, true);
        }

        private void toolbar_BatchEditClick(object sender, EventArgs e)
        {
            if (gdFinancialAll.SelectedItems.Count > 0)
            {
                List<GetAllFinancialExportDeclarationDataModel> ids = new List<GetAllFinancialExportDeclarationDataModel>();
                foreach (GetAllFinancialExportDeclarationDataModel dm in gdFinancialAll.SelectedItems)
                {
                    ids.Add(dm);
                }
                FinancialDeclarationBatchEditForm form = new FinancialDeclarationBatchEditForm();
                form.FinancialDeclarations = ids;
                form.ShowDialog();
            }
            
        }

        private void declarationFilter_ExcuteQueryClick(object sender, EventArgs e)
        {
            Load(SystemConfiguration.Instance.LoggedOnUser.ID);
        }

        private void declarationFilter_ResetClick(object sender, EventArgs e)
        {
            gdFinancialAll.FilterDescriptors.Clear();
        }
    }
}
