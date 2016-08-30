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
using System.Windows.Data;
using ProTemplate.Web;

namespace ProTemplate.UserControls.RadWindows
{
    public partial class DeclarationEditForm : RadWindow
    {
        DeclarationEditFormDataModel _dataModel;
        DeclarationItemDataModel _declarationItemDataModel;
        FormState _declarationItemEditState = FormState.Add;
        int _maxDeclarationItemSequence = 0;

        //static void ResetAutoCompleteBinding(AutoCompleteBox acb, DeclarationItemDataModel dm, string pathName)
        //{
        //    if (acb == null)
        //        return;
        //    acb.Text = "";
        //    Binding binding = new Binding();
        //    binding.Mode = BindingMode.OneWay;
        //    binding.NotifyOnValidationError = true;
        //    binding.ValidatesOnExceptions = true;
        //    binding.Path = new PropertyPath(pathName);
        //    binding.Source = dm;
        //    acb.SetBinding(AutoCompleteBox.TextProperty, binding);
        //}

        #region Initial Form
        public DeclarationEditForm(DeclarationEditFormDataModel dataModel)
        {
            InitializeComponent();
            _dataModel = dataModel;
            this.DataContext = dataModel;
            _declarationItemDataModel = new DeclarationItemDataModel();
            gridDeclarationItem.DataContext = _declarationItemDataModel;
            
            InitDeclarationItemViewModel();
            
            documentInputForm.CurrentDeclarationID = _dataModel.ID;
            containerInputForm.CurrentDeclarationID = _dataModel.ID;
            delarationImageUploader.CurrentDeclarationID = _dataModel.ID;
            mainFeeForm.CurrentDeclarationID = _dataModel.ID;
            mainFeeForm.financialRemark = _dataModel.FinancialRemark;

            if (SystemConfiguration.Instance.LoggedOnUser.DisplayRoles.Contains("清单") || SystemConfiguration.Instance.LoggedOnUser.DisplayRoles.Contains("退税"))
            {
                tabFee.Visibility = System.Windows.Visibility.Collapsed;
            }

            SetPermission();
        }

        private void SetPermission()
        {
            // 只有管理员可以保存数据
            var qUser = from a in SystemConfiguration.Instance.LoggedOnUser.RoleList
                        where a.Name == "管理员" || a.Name == "出口退税人员" || a.Name == "出口经理" || a.Name == "出口清单人员" || a.Name == "财务人员"
                        select a.Name;
            if (qUser.Count() > 0)
                toolBar.CanSave = true;
            else
                toolBar.CanSave = false;

            //如果登陆的是客户，则所有数据都是只读
            var qCustomer = from a in SystemConfiguration.Instance.LoggedOnUser.RoleList
                        where a.Name.Contains("客户") || a.Name.Contains("操作")
                        select a.Name;
            if (qCustomer.Count() > 0)
            {
                //acbCustomer.IsEnabled = false;
                //acbCountry.IsEnabled = false;
                //acbCurrencyInsure.IsEnabled = false;
                //acbCurrencyOther.IsEnabled = false;
                //acbCurrencyProduct.IsEnabled = false;
                //acbCurrencyTrans.IsEnabled = false;
                //acbCustomer.IsEnabled = false;
                //acbCustomhouse.IsEnabled = false;
                //acbDistrict.IsEnabled = false;
                //acbDuty.IsEnabled = false;
                //acbFeeMarkInsure.IsEnabled = false;
                //acbFeeMarkOther.IsEnabled = false;
                //acbFeeMarkTrans.IsEnabled = false;
                //acbLevy.IsEnabled = false;
                //acbPay.IsEnabled = false;
                //acbPort.IsEnabled = false;
                //acbTrade.IsEnabled = false;
                //acbTransaction.IsEnabled = false;
                //acbTransport.IsEnabled = false;
                //acbUnitDeclaration.IsEnabled = false;
                //acbUnitLaw.IsEnabled = false;
                //acbUnitLaw2.IsEnabled = false;
                //acbWrap.IsEnabled = false;
                btnAddDeclarationItem.IsEnabled = false;
                btnUpdateDeclarationItem.IsEnabled = false;
                
                containerInputForm.SetToReadOnly();
                delarationImageUploader.SetToReadOnly();
                documentInputForm.SetToReadOnly();
                mainFeeForm.SetToReadOnly();
            }
        }

        FormState DeclarationItemEditState
        {
            get { return _declarationItemEditState; }
            set
            {
                _declarationItemEditState = value;
                if (_declarationItemEditState == FormState.Add)
                {
                    // clear input
                    _declarationItemDataModel = new DeclarationItemDataModel();
                    _declarationItemDataModel.CurrencyName = "";
                    _declarationItemDataModel.DeclaredUnitName = "";
                    _declarationItemDataModel.LegalUnitName = "";
                    _declarationItemDataModel.SecondUnitName = "";
                    _declarationItemDataModel.DutyName = "";
                    //ResetAutoCompleteBinding(acbCurrencyProduct, _declarationItemDataModel, "CurrencyName");
                    //ResetAutoCompleteBinding(acbUnitDeclaration, _declarationItemDataModel, "DeclaredUnitName");
                    //ResetAutoCompleteBinding(acbUnitLaw, _declarationItemDataModel, "LegalUnitName");
                    //ResetAutoCompleteBinding(acbUnitLaw2, _declarationItemDataModel, "SecondUnitName");
                    //ResetAutoCompleteBinding(acbDuty, _declarationItemDataModel, "DutyName");

                    gridDeclarationItem.DataContext = _declarationItemDataModel;
                    spAddItem.Visibility = System.Windows.Visibility.Visible;
                    spUpdateItem.Visibility = System.Windows.Visibility.Collapsed;
                }
                else
                {
                    spAddItem.Visibility = System.Windows.Visibility.Collapsed;
                    spUpdateItem.Visibility = System.Windows.Visibility.Visible;
                }
            }
        }

        void InitDeclarationItemViewModel()
        {
            DeclarationItemViewModel divm = ViewModelManager.DeclarationItemViewModelInstance;
            if (divm != null)
            {
                divm.Items.Clear();

                var realItem = (from t in SystemConfiguration.Instance.DataContext.Declarations
                                where t.ID == _dataModel.ID
                                select t).SingleOrDefault();
                if (realItem != null)
                {
                    foreach (var item in realItem.DeclarationItem)
                    {
                        DeclarationItemDataModel itemDM = new DeclarationItemDataModel();
                        itemDM.Sequence = item.Sequence;
                        itemDM.DeclarationId = _dataModel.ID;
                        itemDM.SortOrder = item.SortOrder;
                        itemDM.Number = item.Number;
                        itemDM.SubNumber = item.SubNumber;
                        itemDM.Name = item.Name;
                        itemDM.Model = item.Model;
                        itemDM.CurrencyCode = item.CurrencyCode;
                        itemDM.CurrencyName = ViewModelManager.CurrencyViewModelInstance.GetCurrencyName(itemDM.CurrencyCode);
                        itemDM.DeclaredQuantity = item.DeclaredQuantity??0;
                        itemDM.DeclaredUnitCode = item.DeclaredUnitCode;
                        UnitViewModel unitVM = ViewModelManager.UnitViewModelInstance;
                        itemDM.DeclaredUnitName = unitVM.GetUnitName(itemDM.DeclaredUnitCode);
                        itemDM.DeclaredPrice = item.DeclaredPrice??0;
                        itemDM.DeclaredTotalPrice = item.DeclaredTotalPrice??0;
                        itemDM.LegalQuantity = item.LegalQuantity??0;
                        itemDM.LegalUnitCode = item.LegalUnitCode;
                        itemDM.LegalUnitName = unitVM.GetUnitName(itemDM.LegalUnitCode);
                        itemDM.SecondQuantity = item.SecondQuantity??0;
                        itemDM.SecondUnitCode = item.SecondUnitCode;
                        itemDM.SecondUnitName = unitVM.GetUnitName(itemDM.SecondUnitCode);
                        itemDM.VersionNumber = item.VersionNumber;
                        itemDM.Purpose = item.Purpose;
                        itemDM.CountryCode = item.CountryCode;
                        itemDM.ProductNumber = item.ProductNumber;
                        itemDM.DutyCode = item.DutyCode;
                        itemDM.DutyName = ViewModelManager.DutyViewModelInstance.GetDutyName(itemDM.DutyCode);
                        divm.Items.Add(itemDM);
                    }
                }
                divm.UpdateIndex();
                //更新_maxDeclarationItemSequence
                if (divm.Items != null && divm.Items.Count > 0)
                {
                    _maxDeclarationItemSequence = (from s in divm.Items
                                                   select s.Sequence).Max();
                }
                //
                InitDeclarationDocumentViewModel(realItem);
                InitDeclarationContainerViewModel(realItem);
                InitDeclarationFeeViewModel(realItem);
            }
        }

        void InitDeclarationDocumentViewModel(Web.Declaration declaration)
        {
            DeclarationDocumentViewModel vm = ViewModelManager.DeclarationDocumentViewModelInstance;
            if (declaration != null && vm !=null)
            {
                vm.Items.Clear();
                foreach (var item in declaration.DeclarationDocument)
                {
                    DeclarationDocumentDataModel docDM = new DeclarationDocumentDataModel();
                    docDM.DeclarationId = item.DeclarationId;
                    docDM.DocumentCode = item.Document;
                    docDM.DocumentName = ViewModelManager.DocumentViewModelInstance.GetDocumentName(docDM.DocumentCode);
                    docDM.CertificateNumber = item.CertificateNumber;
                    docDM.Sequence = item.Sequence;
                    docDM.SortOrder = item.SortOrder;
                    vm.Items.Add(docDM);
                }
                vm.UpdateIndex();
            }
        }

        void InitDeclarationFeeViewModel(Web.Declaration declaration)
        {
            FinancialExportDeclarationViewModel vm = ViewModelManager.FinancialExportDeclarationViewModelInstance;
            if (declaration != null && vm != null)
            {
                vm.Items.Clear();
                foreach (var item in declaration.FinancialExportDeclaration)
                {
                    FinancialExportDeclarationDataModel docDM = new FinancialExportDeclarationDataModel();
                    docDM.ID = item.ID;
                    docDM.FeeTypeCode = item.FeeTypeCode;
                    docDM.FeeTypeName = ViewModelManager.FeeTypeViewModelInstance.GetFeeTypeName(docDM.FeeTypeCode); ;
                    docDM.Amount = item.Amount;
                    docDM.Cost = item.Cost;
                    docDM.Remark = item.FinancialRemark;
                    vm.Items.Add(docDM);
                }
                vm.UpdateIndex();
            }
            SystemConfiguration.Instance.DataContext.FinancialExportDeclarations.Clear();
        }

        void InitDeclarationContainerViewModel(Web.Declaration declaration)
        {
            DeclarationContainerViewModel vm = ViewModelManager.DeclarationContainerViewModelInstance;
            if (declaration != null && vm != null)
            {
                vm.Items.Clear();
                foreach (var item in declaration.DeclarationContainer)
                {
                    DeclarationContainerDataModel containerDM = new DeclarationContainerDataModel();
                    containerDM.DeclarationId = item.DeclarationId;
                    containerDM.Number = item.Number;
                    containerDM.Weight = item.Weight;
                    containerDM.Model = item.Model;
                    containerDM.Sequence = item.Sequence;
                    containerDM.SortOrder = item.SortOrder;
                    vm.Items.Add(containerDM);
                }
                vm.UpdateIndex();
            }
        }
        #endregion

        #region AutoCompleteBox Populating Events
        private void acbCustomer_Populating(object sender, PopulatingEventArgs e)
        {
            CustomerViewModel vm = App.Current.Resources["CustomerViewModel"] as CustomerViewModel;
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

        private void acbCustomhouse_Populating(object sender, PopulatingEventArgs e)
        {
            CustomhouseViewModel vm = App.Current.Resources["CustomhouseViewModel"] as CustomhouseViewModel;
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

        private void acbDistrict_Populating(object sender, PopulatingEventArgs e)
        {
            DistrictViewModel vm = App.Current.Resources["DistrictViewModel"] as DistrictViewModel;
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

        private void acbPay_Populating(object sender, PopulatingEventArgs e)
        {
            PayViewModel vm = App.Current.Resources["PayViewModel"] as PayViewModel;
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

        private void acbLevy_Populating(object sender, PopulatingEventArgs e)
        {
            LevyViewModel vm = App.Current.Resources["LevyViewModel"] as LevyViewModel;
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

        private void acbCountry_Populating(object sender, PopulatingEventArgs e)
        {
            CountryViewModel vm = App.Current.Resources["CountryViewModel"] as CountryViewModel;
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

        private void acbPort_Populating(object sender, PopulatingEventArgs e)
        {
            PortViewModel vm = App.Current.Resources["PortViewModel"] as PortViewModel;
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

        private void acbDuty_Populating(object sender, PopulatingEventArgs e)
        {
            DutyViewModel vm = App.Current.Resources["DutyViewModel"] as DutyViewModel;
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

        private void acbTrade_Populating(object sender, PopulatingEventArgs e)
        {
            TradeViewModel vm = App.Current.Resources["TradeViewModel"] as TradeViewModel;
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

        private void acbCurrency_Populating(object sender, PopulatingEventArgs e)
        {
            CurrencyViewModel vm = App.Current.Resources["CurrencyViewModel"] as CurrencyViewModel;
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

        private void acbTransport_Populating(object sender, PopulatingEventArgs e)
        {
            TransportViewModel vm = App.Current.Resources["TransportViewModel"] as TransportViewModel;
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

        private void acbFeeMark_Populating(object sender, PopulatingEventArgs e)
        {
            FeeMarkViewModel vm = App.Current.Resources["FeeMarkViewModel"] as FeeMarkViewModel;
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

        private void acbWrap_Populating(object sender, PopulatingEventArgs e)
        {
            WrapViewModel vm = App.Current.Resources["WrapViewModel"] as WrapViewModel;
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

        private void acbTransaction_Populating(object sender, PopulatingEventArgs e)
        {
            TransactionViewModel vm = App.Current.Resources["TransactionViewModel"] as TransactionViewModel;
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



        private void acbUnit_Populating(object sender, PopulatingEventArgs e)
        {
            UnitViewModel vm = App.Current.Resources["UnitViewModel"] as UnitViewModel;
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

        #region Toolbar Events
        private void toolBar_SaveAndClose(object sender, EventArgs e)
        {
            if (!InputCheck())
                return;
            var declaration = (from d in SystemConfiguration.Instance.DataContext.Declarations
                               where d.ID == _dataModel.ID
                               select d).SingleOrDefault();
            if (declaration != null)
            {
                //更新基本信息
                declaration.CustomerID = ((CustomerDataModel)acbCustomer.SelectedItem).ID;
                declaration.ReceivedDate = (DateTime)dpReceiveDate.SelectedDate;
                declaration.DeclarationNumber = tbDeclarationNumber.Text;
                declaration.PreEntryNumber = tbPreEntryNumber.Text;
                declaration.BillNumber = tbBillNumber.Text;
                declaration.ApprovalNumber = tbApprovalNumber.Text;
                declaration.Dock = tbDock.Text;
                declaration.Remark = tbRemark.Text;
                declaration.RelatedSystemNumber = tbRelatedSystemNumber.Text;
                declaration.DeclarationStatusRemark = tbDeclarationStatusRemark.Text;
                declaration.DeclarationDate = dpDeclarationStatusDate.SelectedDate;
                declaration.DrawbackStatusRemark = tbDrawbackStatusRemark.Text;
                declaration.DrawbackDate = dpDrawbackStatusDate.SelectedDate;
                declaration.VerificationStatusDetail = tbVerificationStatusDetail.Text;
                declaration.ShipLeaveDate = dpLeaveDate.SelectedDate;
                // 更新报关
                declaration.CustomhouseCode = ViewModelManager.CustomhouseViewModelInstance.GetCustomhouseCode(_dataModel.CustomhouseName);
                declaration.IEDate = _dataModel.IEDate;
                declaration.TransportCode = ViewModelManager.TransportViewModelInstance.GetTransportCode(_dataModel.TransportName);
                declaration.VoyageNumber = _dataModel.VoyageNumber;
                declaration.TradeCode = ViewModelManager.TradeViewModelInstance.GetTradeCode(_dataModel.TradeName);
                declaration.LevyCode = ViewModelManager.LevyViewModelInstance.GetLevyCode(_dataModel.LevyName);
                declaration.PayCode = ViewModelManager.PayViewModelInstance.GetPayCode(_dataModel.PayName);
                declaration.CountryCode = ViewModelManager.CountryViewModelInstance.GetCountryCode(_dataModel.CountryName);
                declaration.PortCode = ViewModelManager.PortViewModelInstance.GetPortCode(_dataModel.PortName);
                declaration.DistrictCode = ViewModelManager.DistrictViewModelInstance.GetDistrictCode(_dataModel.DistrictName);
                declaration.TransactionCode = ViewModelManager.TransactionViewModelInstance.GetTransactionCode(_dataModel.TransactionName);
                declaration.FreightFeeCurrencyCode = ViewModelManager.CurrencyViewModelInstance.GetCurrencyCode(_dataModel.CurrencyTransName);
                declaration.FreightFeeMarkCode = ViewModelManager.FeeMarkViewModelInstance.GetFeeMarkCode(_dataModel.FeeMarkTransName);
                declaration.FreightFeeRate = _dataModel.FeeTrans;
                declaration.InsuranceFeeCurrencyCode = ViewModelManager.CurrencyViewModelInstance.GetCurrencyCode(_dataModel.CurrencyInsureName);
                declaration.InsuranceFeeMarkCode = ViewModelManager.FeeMarkViewModelInstance.GetFeeMarkCode(_dataModel.FeeMarkInsureName);
                declaration.InsuranceFeeRate = _dataModel.FeeInsure;
                declaration.OtherFeeCurrencyCode = ViewModelManager.CurrencyViewModelInstance.GetCurrencyCode(_dataModel.CurrencyOtherName);
                declaration.OtherFeeMarkCode = ViewModelManager.FeeMarkViewModelInstance.GetFeeMarkCode(_dataModel.FeeMarkOtherName);
                declaration.OtherFeeRate = _dataModel.FeeOther;

                declaration.PackageAmount = _dataModel.PackageAmount;
                declaration.CreatedDate = _dataModel.CreateDate;
                declaration.GrossWeight = _dataModel.GrossWeight;
                declaration.NetWeight = _dataModel.NetWeight;
                declaration.ContainerQuantity = _dataModel.ContainerQuantity;
                declaration.WrapCode = ViewModelManager.WrapViewModelInstance.GetWrapCode(_dataModel.WrapName);

                declaration.ManualNumber = _dataModel.ManualNumber;
                declaration.Conveyance = _dataModel.Conveyance;
                declaration.ControlNumber = _dataModel.ControlNumber;
                declaration.Note = _dataModel.Note;
                declaration.TraderCode = _dataModel.TraderCode;
                declaration.TraderName = _dataModel.TraderName;
                declaration.OwnerCode = _dataModel.OwnerCode;
                declaration.OwnerName = _dataModel.OwnerName;
                declaration.AgentName = _dataModel.AgentName;
                declaration.AgentCode = _dataModel.AgentCode;
                declaration.LicenseNumber = _dataModel.LicenseNumber;
                declaration.FinancialRemark = mainFeeForm.financialRemark;

                // 开始提交
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
                        delarationImageUploader.HasImageUpdated = false;
                        GetAllDeclarationByReceiveDateResultViewModel vm = ViewModelManager.GetAllDeclarationByReceiveDateResultViewModelInstance;
                        var curDM = (from d in vm.Items
                                    where d.ID == declaration.ID
                                    select d).FirstOrDefault();
                        if(curDM!=null)
                        {
                            curDM.IsLocked = false;
                            curDM.DeclarationNumber = declaration.DeclarationNumber;
                            curDM.ReceivedDate = declaration.ReceivedDate;
                            curDM.ManualNumber = declaration.ManualNumber;
                            curDM.TraderName = declaration.TraderName;
                            curDM.Conveyance = declaration.Conveyance;
                            curDM.VoyageNumber = declaration.VoyageNumber;
                            curDM.BillNumber = declaration.BillNumber;
//                            curDM.TradeName = declaration.tra
                            curDM.ApprovalNumber = declaration.ApprovalNumber;
                            curDM.IsLocked = true;
                        }

                        this.Close();
                    }
                }, null);
            }
            else
            {
                delarationImageUploader.HasImageUpdated = false;
                this.Close();
            }
        }

        private void toolBar_Close(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion

        #region RadWindow Events

        private void RadWindow_Closed(object sender, WindowClosedEventArgs e)
        {
            SystemConfiguration.Instance.DataContext.Declarations.Clear();
            SystemConfiguration.Instance.DataContext.DeclarationItems.Clear();
            SystemConfiguration.Instance.DataContext.DeclarationDocuments.Clear();
            SystemConfiguration.Instance.DataContext.DeclarationImages.Clear();
            SystemConfiguration.Instance.DataContext.DeclarationContainers.Clear();
            SystemConfiguration.Instance.DataContext.FinancialExportDeclarations.Clear();
        }

        private void RadWindow_PreviewClosed(object sender, WindowPreviewClosedEventArgs e)
        {
            if (delarationImageUploader.HasImageUpdated)
            {
                e.Cancel = true;
                CommonUIFunction.ShowConfirmYesNo("图片已经更改，你确定放弃保存么?", new EventHandler<WindowClosedEventArgs>(ConfirmClosed));
            }
        }

        void ConfirmClosed(object sender, WindowClosedEventArgs e)
        {
            if (e.DialogResult == true)
            {
                delarationImageUploader.HasImageUpdated = false;
                SystemConfiguration.Instance.DataContext.RejectChanges();
                this.Close();
            }
        }
        #endregion

        #region Input Check
        private bool InputCheck()
        {
            if (acbCustomer.Text == "" || acbCustomer.SelectedItem == null)
            {
                _dataModel.SetErrors("CustomerName", new List<string>() { "请输入合法客户" });
            }
            else
            {
                _dataModel.ClearErrors("CustomerName");
            }

            if (tbDeclarationNumber.Text == "")
            {
                _dataModel.SetErrors("DeclarationNumber", new List<string>() { "海关编号不能为空" });
                //_dataModel.SetErrors("ApprovalNumber", new List<string>() { "海关编号，批准文号不能同时为空" });
            }
            else
            {
                _dataModel.ClearErrors("DeclarationNumber");
                //_dataModel.ClearErrors("ApprovalNumber");
            }

            if (dpReceiveDate.SelectedDate == null)
                _dataModel.SetErrors("ReceivedDate", new List<string>() { "请输入接收日期！" });
            else
                _dataModel.ClearErrors("ReceivedDate");

            return !_dataModel.HasErrors;
        }

        private bool AddDeclarationItemInputCheck()
        {
            if (tbItemNumber.Text == "" )
            {
                _declarationItemDataModel.SetErrors("Number", new List<string>() { "请输入商品编号！" });
            }
            else
            {
                _declarationItemDataModel.ClearErrors("Number");
            }

            if (tbItemName.Text == "")
            {
                _declarationItemDataModel.SetErrors("Name", new List<string>() { "请输入商品名称！" });
            }
            else
            {
                _declarationItemDataModel.ClearErrors("Name");
            }
            return !_declarationItemDataModel.HasErrors;
        }

        #endregion

        #region 关于编辑商品的事件
        private void btnAddDeclarationItem_Click(object sender, RoutedEventArgs e)
        {
            if (!AddDeclarationItemInputCheck())
                return;
            DeclarationItemViewModel divm = ViewModelManager.DeclarationItemViewModelInstance;
            System.Diagnostics.Debug.Assert(divm != null);
            // update data model
            _declarationItemDataModel.SortOrder = ++_maxDeclarationItemSequence;
            _declarationItemDataModel.Index = _declarationItemDataModel.SortOrder;
            _declarationItemDataModel.Number = tbItemNumber.Text;
            _declarationItemDataModel.SubNumber = tbItemSubNumber.Text;
            _declarationItemDataModel.Name = tbItemName.Text;
            _declarationItemDataModel.Model = tbItemModel.Text;
            _declarationItemDataModel.CurrencyCode = acbCurrencyProduct.SelectedItem == null ? "" : ((CurrencyDataModel)acbCurrencyProduct.SelectedItem).Code;
            _declarationItemDataModel.CurrencyName = acbCurrencyProduct.SelectedItem == null ? "" : ((CurrencyDataModel)acbCurrencyProduct.SelectedItem).Name;
            decimal val;
            decimal.TryParse(tbItemDeclaredQuantity.Text, out val);
            _declarationItemDataModel.DeclaredQuantity = val;
            _declarationItemDataModel.DeclaredUnitCode = acbUnitDeclaration.SelectedItem == null ? "" : ((UnitDataModel)acbUnitDeclaration.SelectedItem).Code;
            _declarationItemDataModel.DeclaredUnitName = acbUnitDeclaration.SelectedItem == null ? "" : ((UnitDataModel)acbUnitDeclaration.SelectedItem).Name;
            // 单价
            decimal.TryParse(tbItemDeclaredPrice.Text, out val);
            _declarationItemDataModel.DeclaredPrice = val;
            // 总价
            decimal.TryParse(tbItemDeclaredTotalPrice.Text, out val);
            _declarationItemDataModel.DeclaredTotalPrice = val;
            // 法定数量
            decimal.TryParse(tbItemLegalQuantity.Text, out val);
            _declarationItemDataModel.LegalQuantity = val;
            _declarationItemDataModel.LegalUnitCode = acbUnitLaw.SelectedItem == null ? "" : ((UnitDataModel)acbUnitLaw.SelectedItem).Code;
            _declarationItemDataModel.LegalUnitName = acbUnitLaw.SelectedItem == null ? "" : ((UnitDataModel)acbUnitLaw.SelectedItem).Name;
            //第二数量
            decimal.TryParse(tbItemSecondQuantity.Text, out val);
            _declarationItemDataModel.SecondQuantity = val;
            _declarationItemDataModel.SecondUnitCode = acbUnitLaw2.SelectedItem == null ? "" : ((UnitDataModel)acbUnitLaw2.SelectedItem).Code;
            _declarationItemDataModel.SecondUnitName = acbUnitLaw2.SelectedItem == null ? "" : ((UnitDataModel)acbUnitLaw2.SelectedItem).Name;
            _declarationItemDataModel.DutyCode = acbDuty.SelectedItem == null ? "" : ((DutyDataModel)acbDuty.SelectedItem).Code;
            _declarationItemDataModel.DutyName = acbDuty.SelectedItem == null ? "" : ((DutyDataModel)acbDuty.SelectedItem).Name;
            // update view model
            divm.Items.Add(_declarationItemDataModel);
            // update real data model
            Web.DeclarationItem item = new Web.DeclarationItem();
            item.DeclarationId = _dataModel.ID;
            item.SortOrder = _declarationItemDataModel.SortOrder;
            item.Number = _declarationItemDataModel.Number;
            item.SubNumber = _declarationItemDataModel.SubNumber;
            item.Name = _declarationItemDataModel.Name;
            item.Model = _declarationItemDataModel.Model;
            item.CurrencyCode = _declarationItemDataModel.CurrencyCode;
            item.DeclaredQuantity = _declarationItemDataModel.DeclaredQuantity;
            item.DeclaredUnitCode = _declarationItemDataModel.DeclaredUnitCode;
            item.DeclaredPrice = _declarationItemDataModel.DeclaredPrice;
            item.DeclaredTotalPrice = _declarationItemDataModel.DeclaredTotalPrice;
            item.LegalQuantity = _declarationItemDataModel.LegalQuantity;
            item.LegalUnitCode = _declarationItemDataModel.LegalUnitCode;
            item.SecondQuantity = _declarationItemDataModel.SecondQuantity;
            item.SecondUnitCode = _declarationItemDataModel.SecondUnitCode;
            //item.VersionNumber = _declarationItemDataModel.VersionNumber;
            //item.Purpose = _declarationItemDataModel.Purpose;
            //item.CountryCode = _declarationItemDataModel.CountryCode;
            //item.ProductNumber = _declarationItemDataModel.ProductNumber;
            item.DutyCode = _declarationItemDataModel.DutyCode;

            var realItem = (from t in SystemConfiguration.Instance.DataContext.Declarations
                            where t.ID == _dataModel.ID
                            select t).SingleOrDefault();
            if (realItem != null)
                realItem.DeclarationItem.Add(item);

            DeclarationItemEditState = FormState.Add;
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            _declarationItemDataModel = ((RadButton)sender).DataContext as DeclarationItemDataModel;
            if (_declarationItemDataModel != null)
            {
                //acbCurrencyProduct.Text = _declarationItemDataModel.CurrencyName;
                //acbUnitDeclaration.Text = _declarationItemDataModel.DeclaredUnitName;
                //acbUnitLaw.Text = _declarationItemDataModel.LegalUnitName;
                //acbUnitLaw2.Text = _declarationItemDataModel.SecondUnitName;
                //acbDuty.Text = _declarationItemDataModel.DutyName;
                CurrencyDataModel cdm = new CurrencyDataModel();
                cdm.Code = _declarationItemDataModel.CurrencyCode;
                cdm.Name = _declarationItemDataModel.CurrencyName;
                acbCurrencyProduct.SelectedItem = cdm;

                UnitDataModel udm = new UnitDataModel();
                udm.Code = _declarationItemDataModel.DeclaredUnitCode;
                udm.Name = _declarationItemDataModel.DeclaredUnitName;
                acbUnitDeclaration.SelectedItem = udm;

                UnitDataModel udmLaw = new UnitDataModel();
                udmLaw.Code = _declarationItemDataModel.LegalUnitCode;
                udmLaw.Name = _declarationItemDataModel.LegalUnitName;
                acbUnitLaw.SelectedItem = udmLaw;

                UnitDataModel udmLaw2 = new UnitDataModel();
                udmLaw2.Code = _declarationItemDataModel.SecondUnitCode;
                udmLaw2.Name = _declarationItemDataModel.SecondUnitName;
                acbUnitLaw2.SelectedItem = udmLaw2;

                DutyDataModel ddm = new DutyDataModel();
                ddm.Code = _declarationItemDataModel.DutyCode;
                ddm.Name = _declarationItemDataModel.DutyName;
                acbDuty.SelectedItem = ddm;

                gridDeclarationItem.DataContext = _declarationItemDataModel;
                DeclarationItemEditState = FormState.Update;
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            DeclarationItemDataModel dm = ((RadButton)sender).DataContext as DeclarationItemDataModel;
            if (dm == null)
                return;
            CommonUIFunction.ShowConfirmYesNo("确定要删除这个文档信息么?",
            (s, arg) =>
            {
                if (arg.DialogResult == true)
                {
                    DeclarationItemViewModel divm = ViewModelManager.DeclarationItemViewModelInstance;
                    if (divm != null)
                        divm.Items.Remove(dm);

                    #region 删除数据库中数据
                    var realDeclarationObj = (from t in SystemConfiguration.Instance.DataContext.Declarations
                                              where t.ID == _dataModel.ID
                                              select t).SingleOrDefault();
                    if (realDeclarationObj != null)
                    {
                        Web.DeclarationItem dbItem = null;
                        if (dm.Sequence > 0)
                        {
                            dbItem = (from a in realDeclarationObj.DeclarationItem
                                      where a.Sequence == dm.Sequence
                                      select a).SingleOrDefault();
                        }
                        else
                        {
                            dbItem = (from a in realDeclarationObj.DeclarationItem
                                      where a.SortOrder == dm.SortOrder
                                      select a).SingleOrDefault();
                        }
                        if (dbItem != null)
                        {
                            SystemConfiguration.Instance.DataContext.DeclarationItems.Remove(dbItem);
                        }
                    }
                    #endregion
                }
            });
        }

        private void btnUpdateDeclarationItem_Click(object sender, RoutedEventArgs e)
        {
            if (!AddDeclarationItemInputCheck())
                return;
            _declarationItemDataModel.Number = tbItemNumber.Text;
            _declarationItemDataModel.SubNumber = tbItemSubNumber.Text;
            _declarationItemDataModel.Name = tbItemName.Text;
            _declarationItemDataModel.Model = tbItemModel.Text;
            _declarationItemDataModel.CurrencyCode = acbCurrencyProduct.SelectedItem == null ? "" : ((CurrencyDataModel)acbCurrencyProduct.SelectedItem).Code;
            _declarationItemDataModel.CurrencyName = acbCurrencyProduct.SelectedItem == null ? "" : ((CurrencyDataModel)acbCurrencyProduct.SelectedItem).Name;
            decimal val;
            decimal.TryParse(tbItemDeclaredQuantity.Text, out val);
            _declarationItemDataModel.DeclaredQuantity = val;
            _declarationItemDataModel.DeclaredUnitCode = acbUnitDeclaration.SelectedItem == null ? "" : ((UnitDataModel)acbUnitDeclaration.SelectedItem).Code;
            _declarationItemDataModel.DeclaredUnitName = acbUnitDeclaration.SelectedItem == null ? "" : ((UnitDataModel)acbUnitDeclaration.SelectedItem).Name;
            // 单价
            decimal.TryParse(tbItemDeclaredPrice.Text, out val);
            _declarationItemDataModel.DeclaredPrice = val;
            // 总价
            decimal.TryParse(tbItemDeclaredTotalPrice.Text, out val);
            _declarationItemDataModel.DeclaredTotalPrice = val;
            // 法定数量
            decimal.TryParse(tbItemLegalQuantity.Text, out val);
            _declarationItemDataModel.LegalQuantity = val;
            _declarationItemDataModel.LegalUnitCode = acbUnitLaw.SelectedItem == null ? "" : ((UnitDataModel)acbUnitLaw.SelectedItem).Code;
            _declarationItemDataModel.LegalUnitName = acbUnitLaw.SelectedItem == null ? "" : ((UnitDataModel)acbUnitLaw.SelectedItem).Name;
            //第二数量
            decimal.TryParse(tbItemSecondQuantity.Text, out val);
            _declarationItemDataModel.SecondQuantity = val;
            _declarationItemDataModel.SecondUnitCode = acbUnitLaw2.SelectedItem == null ? "" : ((UnitDataModel)acbUnitLaw2.SelectedItem).Code;
            _declarationItemDataModel.SecondUnitName = acbUnitLaw2.SelectedItem == null ? "" : ((UnitDataModel)acbUnitLaw2.SelectedItem).Name;
            _declarationItemDataModel.DutyCode = acbDuty.SelectedItem == null ? "" : ((DutyDataModel)acbDuty.SelectedItem).Code;
            _declarationItemDataModel.DutyName = acbDuty.SelectedItem == null ? "" : ((DutyDataModel)acbDuty.SelectedItem).Name;

            #region 更新数据库中数据
            //update DB
            var realDeclarationObj = (from t in SystemConfiguration.Instance.DataContext.Declarations
                                where t.ID == _dataModel.ID
                                select t).SingleOrDefault();
            if(realDeclarationObj!=null)
            {
                Web.DeclarationItem dbItem = null;
                if (_declarationItemDataModel.Sequence > 0)
                {
                    dbItem = (from a in realDeclarationObj.DeclarationItem
                              where a.Sequence == _declarationItemDataModel.Sequence
                              select a).SingleOrDefault();
                }
                else
                {
                    dbItem = (from a in realDeclarationObj.DeclarationItem
                              where a.SortOrder == _declarationItemDataModel.SortOrder
                              select a).SingleOrDefault();
                }

                if (dbItem != null)
                {
                    dbItem.Number = _declarationItemDataModel.Number;
                    dbItem.SubNumber = _declarationItemDataModel.SubNumber;
                    dbItem.Name = _declarationItemDataModel.Name;
                    dbItem.Model = _declarationItemDataModel.Model;
                    dbItem.CurrencyCode = _declarationItemDataModel.CurrencyCode;
                    dbItem.DeclaredQuantity = _declarationItemDataModel.DeclaredQuantity;
                    dbItem.DeclaredUnitCode = _declarationItemDataModel.DeclaredUnitCode;
                    dbItem.DeclaredPrice = _declarationItemDataModel.DeclaredPrice;
                    dbItem.DeclaredTotalPrice = _declarationItemDataModel.DeclaredTotalPrice;
                    dbItem.LegalQuantity = _declarationItemDataModel.LegalQuantity;
                    dbItem.LegalUnitCode = _declarationItemDataModel.LegalUnitCode;
                    dbItem.SecondQuantity = _declarationItemDataModel.SecondQuantity;
                    dbItem.SecondUnitCode = _declarationItemDataModel.SecondUnitCode;
                    dbItem.DutyCode = _declarationItemDataModel.DutyCode;
                }
            }
            #endregion
            // set to default
            DeclarationItemEditState = FormState.Add;
        }

        private void btnCancelDeclarationItem_Click(object sender, RoutedEventArgs e)
        {
            DeclarationItemEditState = FormState.Add;
        }
        #endregion

        
    }
}
