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
using ProTemplate.Utility;
using System.ServiceModel.DomainServices.Client;
using ProTemplate.ViewModels;
using ProTemplate.Models;
using System.Threading;
using ProTemplate.UserControls.RadWindows;
using System.Windows.Browser;
using System.Windows.Printing;
using System.Text;
using ProTemplate.Utility.PageObjects;
using Telerik.Windows.Controls.GridView;
using Telerik.Windows;
using System.Windows.Media.Imaging;

namespace ProTemplate.Views
{
    public partial class CustomsList : Page
    {
        SearchConditionObject _customsSearchConditionObject;
        private bool _isLoggedInForYSValidation = false;
        public CustomsList()
        {
            InitializeComponent();
            toolbar.BatchNewButton = true;
            toolbar.BatchEditButton = true;
            this.toolbar.PreListButton = true;
            _customsSearchConditionObject = new SearchConditionObject();
            gdCustomerAll.AddHandler(GridViewCellBase.CellDoubleClickEvent, new EventHandler<RadRoutedEventArgs>(OnCellDoubleClick), true);
        }

        #region 页面事件
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

            //_customsSearchConditionObject.StartDate = _customsSearchConditionObject.StartDate.AddMonths(-2);

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
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
            GetAllDeclarationByReceiveDateResultViewModel vm = App.Current.Resources["GetAllDeclarationByReceiveDateResultViewModel"] as GetAllDeclarationByReceiveDateResultViewModel;
            if (vm != null)
                vm.Items.Clear();
        }
        #endregion

        #region 页面操作
        private void OnCellDoubleClick(object sender, RadRoutedEventArgs args)
        {
            GridViewCellBase cell = args.OriginalSource as GridViewCellBase;
            TextBlock tb = cell.Content as TextBlock;
            if (tb != null)
            {
                GetAllDeclarationByReceiveDateResultDataModel dm = cell.DataContext as GetAllDeclarationByReceiveDateResultDataModel;
                if (dm != null)
                    OpenEditWindow(dm.ID);
            }
        }

        private void Load(int userID)
        {
            string condition = declarationFilter.ExcuteQuery();
            SystemConfiguration.Instance.DataContext.Load(SystemConfiguration.Instance.DataContext.GetAllDeclarationByReceiveDateResultsFuncQuery(userID, EncryptionUtil.Encrypt(condition)), delegate(LoadOperation<Web.GetAllDeclarationByReceiveDateResult> lp)
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

        private void LoadViewModel(IEnumerable<Web.GetAllDeclarationByReceiveDateResult> entities)
        {
            GetAllDeclarationByReceiveDateResultViewModel vm = new GetAllDeclarationByReceiveDateResultViewModel();
            if (vm != null)
            {
                vm.Items.Clear();
                int index = 1;

                foreach (var data in entities)
                {
                    GetAllDeclarationByReceiveDateResultDataModel dm = new GetAllDeclarationByReceiveDateResultDataModel();
                    dm.Index = index++;
                    dm.ID = data.ID;
                    dm.ReceivedDate = data.ReceivedDate;
                    //dm.PreEntryNumber = data.PreEntryNumber;
                    dm.DeclarationNumber = data.DeclarationNumber;
                    dm.ManualNumber = data.ManualNumber;
                    //dm.ControlNumber = data.ControlNumber;
                    //dm.IEDate = data.IEDate;
                    //dm.TraderCode = data.TraderCode;
                    dm.TraderName = data.TraderName;
                    //dm.OwnerCode = data.OwnerCode;
                    //dm.OwnerName = data.OwnerName;
                    //dm.AgentCode = data.AgentCode;
                    //dm.AgentName = data.AgentName;
                    dm.Conveyance = data.Conveyance;
                    dm.VoyageNumber = data.VoyageNumber;
                    dm.BillNumber = data.BillNumber;
                    //dm.LicenseNumber = data.LicenseNumber;
                    dm.ApprovalNumber = data.ApprovalNumber;
                    //dm.FreightFeeRate = data.FreightFeeRate;
                    //dm.InsuranceFeeRate = data.InsuranceFeeRate;
                    //dm.OtherFeeRate = data.OtherFeeRate;
                    dm.PackageAmount = data.PackageAmount;
                    dm.GrossWeight = data.GrossWeight;
                    //dm.NetWeight = data.NetWeight;
                    //dm.ContainerQuantity = data.ContainerQuantity;
                    //dm.RelatedDeclarationNumber = data.RelatedDeclarationNumber;
                    //dm.RelatedManualNumber = data.RelatedManualNumber;
                    dm.PrerecordWarehouseWarrant = data.PrerecordWarehouseWarrant;
                    //dm.ProductNumber = data.ProductNumber;
                    //dm.Note = data.Note;
                    dm.CreatedDate = data.CreatedDate;
                    dm.DeclarationStatus = data.DeclarationStatus;
                    dm.DeclarationStatusRemark = data.DeclarationStatusRemark;
                    dm.DeclarationDate = data.DeclarationDate;
                    dm.DrawbackStatus = data.DrawbackStatus;
                    dm.DrawbackStatusRemark = data.DrawbackStatusRemark;
                    dm.DrawbackDate = data.DrawbackDate;
                    dm.VerificationStatus = data.VerificationStatus;
                    dm.VerificationStatusDetail = data.VerificationStatusDetail;
                    dm.Dock = data.Dock;
                    dm.ShipLeaveDate = data.ShipLeaveDate;
                    dm.CustomerName = data.CustomerName;
                    //dm.TrasnsportName = data.TransactionName;
                    dm.TradeName = data.TradeName;
                    /*dm.LevyName = data.LevyName;
                    dm.PayName = data.PayName;
                    dm.CountryName = data.CountryName;
                    dm.PortName = data.PortName;
                    dm.DistrictName = data.DistrictName;
                    dm.CustomhouseName = data.CustomhouseName;
                    dm.TransactionName = data.TransactionName;
                    dm.FreightFeeMarkName = data.FreightFeeMarkName;
                    dm.FreightFeeCurrencyName = data.FreightFeeCurrencyName;
                    dm.InsuranceFeeMarkName = data.InsuranceFeeMarkName;
                    dm.InsuranceFeeCurrencyName = data.InsuranceFeeCurrencyName;
                    dm.OtherFeeMarkName = data.OtherFeeMarkName;
                    dm.OtherFeeCurrencyName = data.OtherFeeCurrencyName;
                    dm.WrapName = data.WrapName;*/
                    dm.Remark = data.Remark;
                    dm.TotalItems = data.TotalItems;
                    //dm.TotalContainers = data.TotalContainers;
                    dm.AdmissionStatus = data.AdmissionStatus;
                    dm.LadingStatus = data.LadingStatus;
                    dm.BillCount = (data.TotalItems - 1) / 5 ;
                    dm.ExaminationNumber = data.ExaminationNumber;
                    dm.HasExamination = data.IsExamination == 1 ? "是" : "否";
                    //dm.ContainerNumbers = data.ContainerNumbers;

                    dm.IsLocked = true;
                    vm.Items.Add(dm);
                }

                gdCustomerAll.ItemsSource = vm.Items;

                if (vm.Items != null && vm.Items.Count > Constants.CommonGridViewPageSize)
                    gdPager.Visibility = System.Windows.Visibility.Visible;
                else
                    gdPager.Visibility = System.Windows.Visibility.Collapsed;

                //清除DataContext 中的数据。释放资源。
                if (SystemConfiguration.Instance.DataContext.GetAllDeclarationByReceiveDateResults != null)
                    SystemConfiguration.Instance.DataContext.GetAllDeclarationByReceiveDateResults.Clear();
            }
        }




        #endregion

        #region 工具栏按钮点击事件
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
            gdCustomerAll.FilterDescriptors.Clear();
        }

        private void CommonToolBar_NewClick(object sender, EventArgs e)
        {
            AddDeclaration addWnd = new AddDeclaration();
            addWnd.ShowDialog();
        }


        private void CommonToolBar_PrintReportClick(object sender, EventArgs e)
        {
            if (gdCustomerAll.SelectedItems.Count == 0)
                return;
            
            //string sql = "select * from boss";
            int[] ids = new int[gdCustomerAll.SelectedItems.Count];
            for (int i = 0; i < ids.Length; i++)
                ids[i] = ((GetAllDeclarationByReceiveDateResultDataModel)gdCustomerAll.SelectedItems[i]).ID;

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

            int[] ids = new int[gdCustomerAll.SelectedItems.Count];
            for (int i = 0; i < ids.Length; i++)
                ids[i] = ((GetAllDeclarationByReceiveDateResultDataModel)gdCustomerAll.SelectedItems[i]).ID;

            SetDeclarationStatus form = new SetDeclarationStatus(ids);
            form.ShowDialog();
            form.Closed += new EventHandler<Telerik.Windows.Controls.WindowClosedEventArgs>(form_Closed);
        }

        void form_Closed(object sender, Telerik.Windows.Controls.WindowClosedEventArgs e)
        {
            Load(SystemConfiguration.Instance.LoggedOnUser.ID);
        }

        private void toolbar_ChangeDrawbackStatusClick(object sender, UserControls.DrawbackStatusEventArgs e)
        {
            int[] ids = new int[gdCustomerAll.SelectedItems.Count];
            for (int i = 0; i < ids.Length; i++)
                ids[i] = ((GetAllDeclarationByReceiveDateResultDataModel)gdCustomerAll.SelectedItems[i]).ID;

            SetDrawbackStatus form = new SetDrawbackStatus(ids);
            form.ShowDialog();
            form.Closed += new EventHandler<Telerik.Windows.Controls.WindowClosedEventArgs>(form_Closed);
        }

        private void CommonToolBar_EditClick(object sender, EventArgs e)
        {
            if (gdCustomerAll.SelectedItems == null || gdCustomerAll.SelectedItems.Count == 0)
            {
                CommonUIFunction.ShowMessageBox(MessageTexts.MSG018);
                return;
            }

            var dID = ((GetAllDeclarationByReceiveDateResultDataModel)gdCustomerAll.SelectedItems[0]).ID;
            OpenEditWindow(dID);
        }

        private void toolbar_BatchEditClick(object sender, EventArgs e)
        {

            if (gdCustomerAll.SelectedItems.Count > 0)
            {
                List<GetAllDeclarationByReceiveDateResultDataModel> ids = new List<GetAllDeclarationByReceiveDateResultDataModel>();
                foreach (GetAllDeclarationByReceiveDateResultDataModel dm in gdCustomerAll.SelectedItems)
                {
                    ids.Add(dm);
                }
                DeclarationBatchEditForm form = new DeclarationBatchEditForm();
                form.Declarations = ids;
                form.ShowDialog();
            }
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
            CommonUIFunction.ExportToFile(ExportFileType.Excel, gdCustomerAll);
        }

        private void CommonToolBar_ExportToWordClick(object sender, EventArgs e)
        {
            CommonUIFunction.ExportToFile(ExportFileType.Excel, gdCustomerAll, true);
        }

        private void toolbar_DeleteClick(object sender, EventArgs e)
        {
            if (gdCustomerAll.SelectedItems == null || gdCustomerAll.SelectedItems.Count == 0)
            {
                CommonUIFunction.ShowMessageBox(MessageTexts.MSG020);
                return;
            }
            else
            {
                if (CommonUIFunction.ShowConfirm(MessageTexts.MSG021) == MessageBoxResult.Cancel)
                    return;

                string[] ids = new string[gdCustomerAll.SelectedItems.Count];
                for (int i = 0; i < ids.Length; i++)
                    ids[i] = ((GetAllDeclarationByReceiveDateResultDataModel)gdCustomerAll.SelectedItems[i]).ID.ToString();

                CommonUIFunction.SetApplcationBusyIndicator(true, "正在更新，请稍后");
                SystemConfiguration.Instance.DataContext.DeleteDeclarations(string.Join(",", ids)
                , delegate(InvokeOperation<int> io)
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
                    }
                    else
                    {
                        //更新ViewModel
                        GetAllDeclarationByReceiveDateResultViewModel vm = ViewModelManager.GetAllDeclarationByReceiveDateResultViewModelInstance;
                        if (vm != null)
                        {
                            for (int i = 0; i < gdCustomerAll.SelectedItems.Count; i++)
                            {
                                GetAllDeclarationByReceiveDateResultDataModel dm = gdCustomerAll.SelectedItems[i] as GetAllDeclarationByReceiveDateResultDataModel;
                                if (dm != null)
                                {
                                    vm.Items.Remove(dm);
                                    //i--;
                                }
                            }
                            vm.UpdateIndex();
                        }
                    }
                    Load(SystemConfiguration.Instance.LoggedOnUser.ID);
                }, null);
            }
        }

        private void toolbar_PortCheckClick(object sender, EventArgs e)
        {
            if (gdCustomerAll.SelectedItems == null || gdCustomerAll.SelectedItems.Count == 0)
            {
                CommonUIFunction.ShowMessageBox(MessageTexts.MSG030);
                return;
            }
            List<GetAllDeclarationByReceiveDateResultDataModel> lst = new List<GetAllDeclarationByReceiveDateResultDataModel>();
            foreach (GetAllDeclarationByReceiveDateResultDataModel a in gdCustomerAll.SelectedItems)
            {
                if (a != null)
                    lst.Add(a);
            }

            string dNums = string.Join(",", (from a in lst
                                             select a.DeclarationNumber).ToArray());

            SystemConfiguration.Instance.DataContext.Load(SystemConfiguration.Instance.DataContext.RefreshDeclarationPortCheckQuery(dNums), lp =>
            {
                if (lp.HasError)
                {
                    lp.MarkErrorAsHandled();
                    MessageBox.Show("获取系统信息出错，请重试！");
                }
                else
                {
                    if (lp.Entities != null && lp.Entities.Count() > 0)
                    {
                        ViewPortCheck vpc = new ViewPortCheck(lp.Entities);
                        vpc.ShowDialog();
                    }
                }
            }, null);
        }
        
        private void toolbar_PreListClick(object sender, EventArgs e)
        {
            if (gdCustomerAll.SelectedItems == null || gdCustomerAll.SelectedItems.Count == 0)
            {
                CommonUIFunction.ShowMessageBox(MessageTexts.MSG031);
                return;
            }

            int id = ((GetAllDeclarationByReceiveDateResultDataModel)gdCustomerAll.SelectedItems[0]).ID;
            SystemConfiguration.Instance.DataContext.DoubleCheckDeclarations.Clear();
            SystemConfiguration.Instance.DataContext.DoubleCheckDeclarationItems.Clear();
            SystemConfiguration.Instance.DataContext.Load(SystemConfiguration.Instance.DataContext.GetDoubleCheckDeclarationByDelarationIDQuery(id),
                (io) =>
                {
                    if (io.HasError)
                    {
                        MessageBox.Show(io.Error.Message);
                        if (io.Error.InnerException != null)
                        {
                            MessageBox.Show(io.Error.InnerException.Message);
                        }
                        io.MarkErrorAsHandled();
                    }
                    else
                    {
                        if (io.Entities != null && io.Entities.Count() > 0)
                        {
                            AddDeclaration addWnd = new AddDeclaration(io.Entities.ElementAt(0), (DateTime)((GetAllDeclarationByReceiveDateResultDataModel)gdCustomerAll.SelectedItems[0]).ReceivedDate, ((GetAllDeclarationByReceiveDateResultDataModel)gdCustomerAll.SelectedItems[0]).RelatedSystemNumber);
                            addWnd.ShowDialog();
                        }
                        else
                        {
                            CommonUIFunction.ShowMessageBox(MessageTexts.MSG032);
                        }
                    }
                }, null);
        }

        private void toolbar_YSValidationClick(object sender, EventArgs e)
        {
            if (gdCustomerAll.SelectedItems == null || gdCustomerAll.SelectedItems.Count == 0)
            {
                CommonUIFunction.ShowMessageBox(MessageTexts.MSG035);
                return;
            }

            // 还没有登陆
            if (!_isLoggedInForYSValidation)
            {
                CommonUIFunction.SetApplcationBusyIndicator(true, "正在获取登录信息。。。");
                SystemConfiguration.Instance.DataContext.GetYSValidImage(lp =>
                {
                    CommonUIFunction.SetApplcationBusyIndicator(false);
                    if (lp.Error != null || lp.Value == null || lp.Value.Length == 0)
                    {
                        MessageBox.Show("图片获取失败");
                        return;
                    }
                    else
                    {
                        BitmapImage bitmapimage = new BitmapImage();
                        System.IO.MemoryStream ms = new System.IO.MemoryStream(lp.Value);
                        bitmapimage.SetSource(ms);

                        YSLogin wnd = new YSLogin();
                        wnd.YSValidateImage = bitmapimage;
                        wnd.Closed += new EventHandler<Telerik.Windows.Controls.WindowClosedEventArgs>(wnd_Closed);
                        wnd.ShowDialog();

                    }
                }, null);
                return;
            }
            //已经登陆
            SystemConfiguration.Instance.DataContext.YSExaminationDatas.Clear();
            YSExaminationDataViewModel vm = ViewModelManager.YSExaminationDataViewModelInstance;
            if (vm != null)
                vm.Items.Clear();
            string[] dNumbers = new string[gdCustomerAll.SelectedItems.Count];
            for (int i = 0; i < dNumbers.Length; i++)
                dNumbers[i] = ((GetAllDeclarationByReceiveDateResultDataModel)gdCustomerAll.SelectedItems[i]).DeclarationNumber.ToString();
            SystemConfiguration.Instance.DataContext.Load(SystemConfiguration.Instance.DataContext.YSQueryByDeclarationNumbersQuery(EncryptionUtil.Encrypt(string.Join(",", dNumbers))), lp =>
            {
                CommonUIFunction.SetApplcationBusyIndicator(false);
                if (lp.HasError)
                {
                    Exception exp = lp.Error;
                    lp.MarkErrorAsHandled();
                    MessageBox.Show("获取数据失败,请重试！");
                }
                else if (lp.Entities.Count() > 0)
                {
                    foreach (var data in lp.Entities)
                    {
                        YSExaminationDataDataModel dm = new YSExaminationDataDataModel();
                        dm.ID = data.ID;
                        dm.ApprovalNumber = data.ApprovalNumber;
                        dm.BillNumber = data.BillNumber;
                        dm.Conveyance = data.Conveyance;
                        dm.CustomerName = data.CustomerName;
                        dm.DeclarationNumber = data.DeclarationNumber;
                        dm.DeclarationStatus = data.DeclarationStatus;
                        dm.VoyageNumber = data.VoyageNumber;
                        dm.YSDate = data.YSDate;
                        dm.YSStatus = data.YSStatus;
                        vm.Items.Add(dm);
                    }
                    vm.UpdateIndex();
                    YSValidationWindow wnd = new YSValidationWindow();
                    wnd.ShowDialog();
                }
                else
                {
                    MessageBox.Show("没有查到任何洋山数据,请重试！");
                }
            }, null);
        }

        void wnd_Closed(object sender, Telerik.Windows.Controls.WindowClosedEventArgs e)
        {
            if (e.DialogResult == true)
            {
                _isLoggedInForYSValidation = true;
            }
        }

        private void declarationFilter_ExcuteQueryClick(object sender, EventArgs e)
        {
            Load(SystemConfiguration.Instance.LoggedOnUser.ID);
        }

        private void toolbar_BatchNewClick(object sender, EventArgs e)
        {
            BatchAddDeclarationForm form = new BatchAddDeclarationForm();
            form.ShowDialog();
        }

        private void declarationFilter_ResetClick(object sender, EventArgs e)
        {
            gdCustomerAll.FilterDescriptors.Clear();
        }
        private void declarationFilter_DuplicatedClick(object sender, EventArgs e)
        {
            string condition = declarationFilter.ExcuteQuery();
            SystemConfiguration.Instance.DataContext.Load(SystemConfiguration.Instance.DataContext.GetAllDeclarationDuplicatedResultsFuncQuery(SystemConfiguration.Instance.LoggedOnUser.ID, condition), delegate(LoadOperation<Web.GetAllDeclarationByReceiveDateResult> lp)
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
        #endregion
    }
}
