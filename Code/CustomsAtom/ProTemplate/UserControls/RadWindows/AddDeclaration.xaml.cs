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
using System.ServiceModel.DomainServices.Client;

namespace ProTemplate.UserControls.RadWindows
{
    public partial class AddDeclaration : RadWindow
    {
        List<HSInputModel> _hsAddList = new List<HSInputModel>();
        DeclarationInputModel _inputDataModel;
        private bool isNewForm = true;
        int _currentEditHS = -1;
        Web.DoubleCheckDeclaration doubleCheckDeclarationSource = null;
        List<Control> allInputControls = new List<Control>();

        #region Constructors
        public AddDeclaration()
        {
            InitializeComponent();
            _inputDataModel = new DeclarationInputModel();
            _inputDataModel.ReceivedDate = DateTime.Now;
            this.DataContext = _inputDataModel;
            isNewForm = true;
            
            //SetPermission();
        }


        void EnterKeyDownToTab(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                GoToNextControl(sender);
            }
        }


        void GoToNextControl(object sender)
        {
            var self = sender as Control;
            if (self == null)
            {
                return;
            }

            var selfTabIndex = self.TabIndex;
            //找出下一个控件
            var nextControl = allInputControls.FirstOrDefault(c => c.TabIndex > selfTabIndex);
            if (nextControl != null)
            {
                nextControl.Focus();
            }
            else
            {
                allInputControls[0].Focus();//最后一个控件时，再跳到第一个(可选处理)
            }
        }


        /// <summary>
        /// 查找所有子元素(递归)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parent"></param>
        /// <returns></returns>
        public static IEnumerable<T> FindChildren<T>(DependencyObject parent) where T : class
        {
            int count = VisualTreeHelper.GetChildrenCount(parent);
            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(parent, i);
                    var t = child as T;
                    if (t != null)
                        yield return t;

                    IEnumerable<T> children = FindChildren<T>(child);
                    foreach (T item in children)
                        yield return item;
                }
            }
        }

        private void DropDownClosedToNext(object sender, EventArgs e)
        {
            GoToNextControl(sender);
        }

        void AutoCompleteDropDownClosed(object sender, RoutedPropertyChangedEventArgs<bool> e)
        {
            AutoCompleteBox box = sender as AutoCompleteBox;
            if (box != null && box.SelectedItem != null)
            {
                string txtName = box.Text;
                Type t = box.SelectedItem.GetType();
                string s = t.GetProperty("Name").GetValue(box.SelectedItem, null).ToString();
                if (txtName == s)
                    GoToNextControl(sender);
            }
        }


        public AddDeclaration(Web.DoubleCheckDeclaration dcd, DateTime receiveDate, string relatedSystemNumber)
        {
            InitializeComponent();
            // 更新UI
            this.Header = "编辑预录单";
            dpReceiveDate.TabIndex = 20000;
            toolBar.IsNew = false;
            dpReceiveDate.IsEnabled = false;
            tbRelatedSystemNumber.IsEnabled = false;
            isNewForm = false;
            doubleCheckDeclarationSource = dcd;
            // 更新预录单
            _inputDataModel = new DeclarationInputModel();
            _inputDataModel.ID = dcd.ID;
            _inputDataModel.DeclarationId = dcd.DeclarationId;
            _inputDataModel.ReceivedDate = receiveDate;
            _inputDataModel.CustomerName = dcd.CustomerName;
            _inputDataModel.DeclarationNumber = dcd.DeclarationNumber;
            _inputDataModel.ApprovalNumber = dcd.ApprovalNumber;
            _inputDataModel.TransactionName = dcd.TransactionName;
            foreach (var it in cbTransaction.Items)
            {
                TransactionDataModel dgd = it as TransactionDataModel;
                if (dgd != null && dgd.Name == _inputDataModel.TransactionName)
                {
                    cbTransaction.SelectedItem = it;
                    break;
                }
            }
            _inputDataModel.InsureFeeCurrencyName = dcd.InsuranceFeeCurrencyName;
            _inputDataModel.FreightFeeCurrencyName = dcd.FreightFeeCurrencyName;
            
            _inputDataModel.RelatedSystemNumber = relatedSystemNumber;
            _inputDataModel.DistrictName = dcd.DistrictName;
            _inputDataModel.TradeName = dcd.TradeName;
            _inputDataModel.FeightFeeRate = dcd.FeightFeeRate;
            _inputDataModel.InsuranceFeeRate = dcd.InsuranceFeeRate;
            _inputDataModel.CustomhouseName = dcd.CustomhouseName;
            _inputDataModel.CountryName = dcd.CountryName;
            _inputDataModel.ManualNumber = dcd.ManualNumber;
            _inputDataModel.LicenseNumber = dcd.LicenseNumber;
            _inputDataModel.PackageAmount = dcd.PackageAmount;
            _inputDataModel.ContractNumber = dcd.ContractNumber;
            _inputDataModel.GrossWeight = dcd.GrossWeight;
            _inputDataModel.NetWeight = dcd.NetWeight;
            _inputDataModel.ContainerNumbers = dcd.ContainerNumbers;
            _inputDataModel.DocumentCodes = dcd.DocumentCodes;
            _inputDataModel.ExaminationNumber = dcd.ExaminationNumber;
            _inputDataModel.PayName = dcd.PayName;
            
            this.DataContext = _inputDataModel;
            // 更新预录单Item
            foreach (var di in dcd.DoubleCheckDeclarationItem.OrderBy(d=>d.SortOrder))
            {
                HSInputModel hs = new HSInputModel();
                hs.ID = di.ID;
                hs.DoubleCheckDeclarationId = di.DoubleCheckDeclarationId;
                hs.ControlNumber = di.ControlNumber == null ? "" : di.ControlNumber;
                hs.HSCode = di.HSCode;
                hs.Name = di.Name;
                hs.SortOrder = di.SortOrder;
                hs.FirstUnitName = di.FirstUnitName;
                hs.FirstQuantity = di.FirstQuantity;
                hs.SecondUnitName = di.SecondUnitName;
                hs.SecondQuantity = di.SecondQuantity;
                hs.DeclaredUnitName = di.DeclaredUnitName;
                hs.DeclaredQuantity = di.DeclaredQuantity;
                hs.TotalAmount = di.TotalAmount;
                hs.CurrencyName = di.CurrencyName;
                _hsAddList.Add(hs);
            }
            gdHSCode.ItemsSource = _hsAddList;
            SetPermission();
        }
        #endregion

        private void SetPermission()
        {
            var qUser = from a in SystemConfiguration.Instance.LoggedOnUser.RoleList
                        where a.Name == "管理员"
                        select a.Name;
            if (qUser.Count() > 0)
                toolBar.CanSave = true;
            else
                toolBar.CanSave = false;
        }

        #region Page Events
        private void RadWindow_Activated(object sender, EventArgs e)
        {
            acbCustomer.Focus();
        }   

        private void RadWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //把界面上的TextBox,RadioButton,ComboBox,CheckBox都加入列表
            //注：一般业务录入界面上只有这4种类型的输入控件，如果还有其实类型，可自行扩展
            allInputControls.AddRange(FindChildren<TextBox>(MyScrolls).Cast<Control>());
            allInputControls.AddRange(FindChildren<RadioButton>(MyScrolls).Cast<Control>());
            allInputControls.AddRange(FindChildren<ComboBox>(MyScrolls).Cast<Control>());
            allInputControls.AddRange(FindChildren<CheckBox>(MyScrolls).Cast<Control>());
            allInputControls.AddRange(FindChildren<AutoCompleteBox>(MyScrolls).Cast<Control>());
            //allInputControls.AddRange(FindChildren<DatePicker>(MyScrolls).Cast<Control>());
            //按TabIndex排序
            allInputControls = allInputControls.OrderBy(c => c.TabIndex).ToList();
            foreach (Control c in allInputControls)
            {
                c.KeyDown += EnterKeyDownToTab;
                if (c is ComboBox)
                {
                    //ComboBox要特殊处理
                    (c as ComboBox).DropDownClosed += DropDownClosedToNext;
                }
                if (c is AutoCompleteBox)
                {
                    (c as AutoCompleteBox).DropDownClosed += AutoCompleteDropDownClosed;
                }
            }

            //acbCustomer.Focus();
        }

        private void cbTransaction_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (cbTransaction.SelectedItem == null)
                return;
            else
            {
                TransactionDataModel tdm = cbTransaction.SelectedItem as TransactionDataModel;
                if (tdm != null)
                {
                    if (tdm.Name.ToUpper() == "FOB")
                    {
                        tbShipFee.Text = tbInsureFee.Text = string.Empty;
                        tbInsureFee.IsEnabled = tbShipFee.IsEnabled = false;
                        tbPackageAmount.Focus();
                    }
                    else
                    {
                        tbInsureFee.IsEnabled = tbShipFee.IsEnabled = true;
                    }
                }
            }
        }

        private void btnAddHS_Click(object sender, RoutedEventArgs e)
        {
            if (!HSInputCheck())
                return;
            if (IsManagementHS(tbHS.Text))
            {
                HSInputModel md = new HSInputModel();
                md.ID = _hsAddList.Count + 1;
                md.ControlNumber = tbControlNumber.Text;
                md.HSCode = tbHS.Text;
                md.Name = tbProductName.Text;
                md.FirstUnitName = tbFirstUnitName.Text.Trim();
                md.FirstQuantity = tbFirstQuantity.Text;
                md.SecondUnitName = tbSecondUnitName.Text;
                md.SecondQuantity = tbSecondQuantity.Text;
                md.DeclaredUnitName = tbDeclaredUnitName.Text;
                md.DeclaredQuantity = tbDeclaredQuantity.Text;
                md.TotalAmount = tbTotalAmount.Text;
                md.CurrencyName = tbBZ.Text;//acbCurrency.SelectedItem == null ? acbCurrency.Text : ((CurrencyDataModel)acbCurrency.SelectedItem).Name;
                int insPos = 0;
                while (insPos < _hsAddList.Count && IsManagementHS(_hsAddList[insPos].HSCode))
                    insPos++;
                _hsAddList.Insert(insPos, md);
                for (int i = 0; i < _hsAddList.Count; i++)
                    _hsAddList[i].SortOrder = i + 1;
            }
            else
            {
                HSInputModel md = new HSInputModel();
                md.ID = _hsAddList.Count + 1;
                md.ControlNumber = tbControlNumber.Text;
                md.HSCode = tbHS.Text;
                md.Name = tbProductName.Text;
                md.SortOrder = _hsAddList.Count + 1;
                md.FirstUnitName = tbFirstUnitName.Text.Trim();
                md.FirstQuantity = tbFirstQuantity.Text;
                md.SecondUnitName = tbSecondUnitName.Text;
                md.SecondQuantity = tbSecondQuantity.Text;
                md.DeclaredUnitName = tbDeclaredUnitName.Text;
                md.DeclaredQuantity = tbDeclaredQuantity.Text;
                md.TotalAmount = tbTotalAmount.Text;
                md.CurrencyName = tbBZ.Text; //acbCurrency.SelectedItem == null ? acbCurrency.Text : ((CurrencyDataModel)acbCurrency.SelectedItem).Name;
                _hsAddList.Add(md);
            }
            gdHSCode.ItemsSource = null;
            gdHSCode.ItemsSource = _hsAddList;
            ClearHSInput();
        }

        private void ClearHSInput()
        {
            tbControlNumber.Text = "";
            tbHS.Text = string.Empty; 
            tbProductName.Text = string.Empty;
            tbFirstUnitName.Text = "";
            tbFirstQuantity.Text = "";
            tbSecondQuantity.Text = "";
            tbSecondUnitName.Text = "";
            tbDeclaredQuantity.Text = "";
            tbDeclaredUnitName.Text = "";
            tbTotalAmount.Text = "";
            tbBZ.Text = "";
            //acbCurrency.Text = "";
            //acbCurrency.SelectedItem = null;

            hsMsg.Inlines.Clear();
            hsMsg.Text = "没有相关HS信息";
            _currentEditHS = -1;
        }

        private bool IsManagementHS(string hsCode)
        {
            HSCodeDictionaryViewModel cvm = ViewModelManager.HSCodeDictionaryViewModelInstance as HSCodeDictionaryViewModel;
            if (cvm == null)
                return false;
            else
            {
                var qurey = (from a in cvm.Items
                             where a.Code == hsCode
                            select a).FirstOrDefault();
                if (qurey != null && qurey.ManagementName != null && qurey.ManagementName.Contains("B"))
                    return true;
                else
                    return false;
            }
        }

        private void tbHS_LostFocus(object sender, RoutedEventArgs e)
        {
            HSCodeDictionaryViewModel cvm = ViewModelManager.HSCodeDictionaryViewModelInstance as HSCodeDictionaryViewModel;
            if (cvm != null)
            {
                var qurey = from a in cvm.Items
                            where a.Code == tbHS.Text
                            select a;
                if (qurey.Count() > 0)
                {
                    var d = qurey.ElementAt(0);
                    hsMsg.Text = "";
                    var line1 = string.Format("商品描述：{0},     监管条件：{1}",
                        d.Name, d.ManagementName);
                    hsMsg.Inlines.Add(new Run() { Text = line1 });
                    hsMsg.Inlines.Add(new LineBreak());
                    var line2 = string.Format("法定第一单位：{0}， 法定第二单位：{1}", d.FirstUnitName, d.SecondUnitName);
                    hsMsg.Inlines.Add(new Run() { Text = line2 });
                    hsMsg.Inlines.Add(new LineBreak());
                    hsMsg.Inlines.Add(new Run() { Text = "申报要素：" + d.DeclarationFactor });
                    hsMsg.Inlines.Add(new LineBreak());
                    hsMsg.Inlines.Add(new Run()
                    {
                        Text = string.Format("出口税率：{0}, 退税率:{1}", string.IsNullOrEmpty(d.ExportRate) ? "无" : d.ExportRate,
                                                                          string.IsNullOrEmpty(d.DrawbackRate) ? "无" : d.DrawbackRate)
                    });
                    tbFirstUnitName.Text = d.FirstUnitName;
                    if (!string.IsNullOrEmpty(d.SecondUnitName) && d.SecondUnitName != "无")
                        tbSecondUnitName.Text = d.SecondUnitName;
                }
                else
                {
                    hsMsg.Inlines.Clear();
                    hsMsg.Text = "没有相关HS信息";
                    tbFirstUnitName.Text = string.Empty;
                }
            }
        }
        #endregion

        #region Input Check
        private bool HSInputCheck()
        {
            if (!string.IsNullOrEmpty(tbManualNumber.Text) && string.IsNullOrEmpty(tbControlNumber.Text))
            {
                CommonUIFunction.ShowMessageBox("备案号不为空时项号为必填！");
                return false;
            }

            if (string.IsNullOrEmpty(tbHS.Text))
            {
                CommonUIFunction.ShowMessageBox("请输入商品编号");
                return false;
            }

            if (string.IsNullOrEmpty(tbProductName.Text))
            {
                CommonUIFunction.ShowMessageBox("请输入商品名称");
                return false;
            }
            return true;
        }

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

            if (tbHGBH.Text == "" )
            {
                _inputDataModel.SetErrors("DeclarationNumber", new List<string>() { "海关编号不能为空" });
                //_inputDataModel.SetErrors("ApprovalNumber", new List<string>() { "海关编号，批准文号不能同时为空" });
            }
            else
            {
                _inputDataModel.ClearErrors("DeclarationNumber");
                //_inputDataModel.ClearErrors("ApprovalNumber");
            }

            if (dpReceiveDate.SelectedDate == null)
                _inputDataModel.SetErrors("ReceivedDate", new List<string>() { "请输入接收日期！" });
            else
                _inputDataModel.ClearErrors("ReceivedDate");

            if (_hsAddList != null && _hsAddList.Count > 0)
            {
                var query = from q in _hsAddList
                            where IsManagementHS(q.HSCode)
                            select q;
                if (query.Count() > 0 && !tbDocumentCodes.Text.ToUpper().Contains("B"))
                {
                    _inputDataModel.SetErrors("DocumentCodes", new List<string>() { "如果商品中含有管理HS编码，则隋付文档必须含B！" });
                }
                else
                    _inputDataModel.ClearErrors("DocumentCodes");
            }
            else
                _inputDataModel.ClearErrors("DocumentCodes");

            if (tbDocumentCodes.Text != null && tbDocumentCodes.Text.ToUpper().Contains("B") && tbExaminationNumber.Text == "")
            {
                _inputDataModel.SetErrors("ExaminationNumber", new List<string>() { "如果单证中含有B则商检编号不能为空！" });
            }
            else
                _inputDataModel.ClearErrors("ExaminationNumber");

            //bool isMachineSelected = true;
            //if (cbMachines.SelectedItem==null)
            //{
            //    CommonUIFunction.ShowMessageBox("打单机器不能为空");
            //    isMachineSelected = false;
            //}

            return !_inputDataModel.HasErrors ;//&& isMachineSelected;
        }
        #endregion

        #region GridView Button Events
        private void btnMoveUp_Click(object sender, RoutedEventArgs e)
        {
            RadButton button = sender as RadButton;
            if (button != null)
            {
                var current = (from q in _hsAddList
                                   where q.ID == (int)button.Tag
                                   select q).SingleOrDefault();
                if (current == null)
                    return;
                else
                {
                    if (current.SortOrder <= 1)
                        return;
                    else
                    {
                        var next = (from q in _hsAddList
                                    where q.SortOrder == current.SortOrder - 1
                                    select q).SingleOrDefault();
                        next.SortOrder++;
                        current.SortOrder--;
                        gdHSCode.ItemsSource = null;
                        gdHSCode.ItemsSource = _hsAddList.OrderBy(o=>o.SortOrder).ToList();
                    }
                }
            }
        }

        private void btnMoveDown_Click(object sender, RoutedEventArgs e)
        {
            RadButton button = sender as RadButton;
            if (button != null)
            {
                var current = (from q in _hsAddList
                               where q.ID == (int)button.Tag
                               select q).SingleOrDefault();
                if (current == null)
                    return;
                else
                {
                    if (current.SortOrder >= _hsAddList.Count)
                        return;
                    else
                    {
                        var next = (from q in _hsAddList
                                    where q.SortOrder == current.SortOrder + 1
                                    select q).SingleOrDefault();
                        next.SortOrder--;
                        current.SortOrder++;
                        gdHSCode.ItemsSource = null;
                        gdHSCode.ItemsSource = _hsAddList.OrderBy(o => o.SortOrder).ToList();
                    }
                }
            }
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            RadButton button = sender as RadButton;
            if (button != null)
            {
                var current = (from q in _hsAddList
                               where q.ID == (int)button.Tag
                               select q).SingleOrDefault();
                if (current == null)
                    return;
                tbControlNumber.Text = current.ControlNumber;
                tbHS.Text = current.HSCode;
                tbProductName.Text = current.Name;
                tbFirstUnitName.Text = current.FirstUnitName;
                tbFirstQuantity.Text = current.FirstQuantity;
                tbSecondQuantity.Text = current.SecondQuantity;
                tbSecondUnitName.Text = current.SecondUnitName;
                tbDeclaredQuantity.Text = current.DeclaredQuantity;
                tbDeclaredUnitName.Text = current.DeclaredUnitName;
                tbTotalAmount.Text = current.TotalAmount;
                //acbCurrency.Text = current.CurrencyName;
                var currency = (from c in ViewModelManager.CurrencyViewModelInstance.Items
                                where c.Name == current.CurrencyName
                                select c.Name).FirstOrDefault();
                //acbCurrency.SelectedItem = currency;
                tbBZ.Text = currency;
                btnAddHS.Visibility = System.Windows.Visibility.Collapsed;
                spUpdateHS.Visibility = System.Windows.Visibility.Visible;
                _currentEditHS = current.ID;
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            RadButton button = sender as RadButton;
            if (button != null)
            {
                var current = (from q in _hsAddList
                               where q.ID == (int)button.Tag
                               select q).SingleOrDefault();
                if (current != null)
                {
                    CommonUIFunction.ShowConfirmYesNo("是否要删除改商品项目?",
                    (s, arg) =>
                    {
                        if (arg.DialogResult == true)
                        {
                            _hsAddList.Remove(current);
                            for (int i = 0; i < _hsAddList.Count; i++)
                                _hsAddList[i].SortOrder = i + 1;
                            gdHSCode.ItemsSource = null;
                            gdHSCode.ItemsSource = _hsAddList;
                        }
                    });
                }
            }
        }

        private void btnUpdateHS_Click(object sender, RoutedEventArgs e)
        {
            if (!HSInputCheck())
                return;
            var hs = (from q in _hsAddList
                      where q.ID == _currentEditHS
                     select q).FirstOrDefault();
            if (hs != null)
            {
                hs.ControlNumber = tbControlNumber.Text;
                hs.HSCode = tbHS.Text;
                hs.Name = tbProductName.Text;
                hs.FirstUnitName = tbFirstUnitName.Text.Trim();
                hs.FirstQuantity = tbFirstQuantity.Text;
                hs.SecondUnitName = tbSecondUnitName.Text;
                hs.SecondQuantity = tbSecondQuantity.Text;
                hs.DeclaredUnitName = tbDeclaredUnitName.Text;
                hs.DeclaredQuantity = tbDeclaredQuantity.Text;
                hs.TotalAmount = tbTotalAmount.Text;
                hs.CurrencyName = tbBZ.Text; //acbCurrency.SelectedItem == null ? acbCurrency.Text : ((CurrencyDataModel)acbCurrency.SelectedItem).Name;
                
                gdHSCode.ItemsSource = null;
                gdHSCode.ItemsSource = _hsAddList;
                ClearHSInput();
                btnAddHS.Visibility = System.Windows.Visibility.Visible;
                spUpdateHS.Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        private void btnCancelHS_Click(object sender, RoutedEventArgs e)
        {
            ClearHSInput();
            btnAddHS.Visibility = System.Windows.Visibility.Visible;
            spUpdateHS.Visibility = System.Windows.Visibility.Collapsed;
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
                            where a.PinYin.ToLower().Contains(prar) || a.Name.ToLower().Contains(prar) || a.Code.ToLower().Contains(prar)
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
                            where a.PinYin.ToLower().Contains(prar) || a.Name.ToLower().Contains(prar) || a.Code.ToLower().Contains(prar)
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
                            where a.PinYin.ToLower().Contains(prar) || a.Name.ToLower().Contains(prar) || a.Code.ToLower().Contains(prar)
                            select a;
                AutoCompleteBox source = (AutoCompleteBox)sender;
                source.ItemsSource = query.ToList();
            }
        }

        private void acbPay_Populating(object sender, PopulatingEventArgs e)
        {
            PayViewModel vm = ViewModelManager.PayViewModelInstance;
            if (vm != null)
            {
                var prar = e.Parameter.ToLower();
                var query = from a in vm.Items
                            where a.PinYin.ToLower().Contains(prar) || a.Name.ToLower().Contains(prar) || a.Code.Contains(prar)
                            select a;
                AutoCompleteBox source = (AutoCompleteBox)sender;
                source.ItemsSource = query.ToList();
            }
        }

        private void acbCountry_Populating(object sender, PopulatingEventArgs e)
        {
            CountryViewModel vm = ViewModelManager.CountryViewModelInstance as CountryViewModel;
            if (vm != null)
            {
                var prar = e.Parameter.ToLower();
                var query = from a in vm.Items
                            where a.PinYin.ToLower().Contains(prar) || a.Name.ToLower().Contains(prar) || a.Code.ToLower().Contains(prar)
                            select a;
                AutoCompleteBox source = (AutoCompleteBox)sender;
                source.ItemsSource = query.ToList();
            }
        }

        private void acbCurrency_Populating(object sender, PopulatingEventArgs e)
        {
            CurrencyViewModel vm = ViewModelManager.CurrencyViewModelInstance;
            if (vm != null)
            {
                var prar = e.Parameter.ToLower();
                var query = from a in vm.Items
                            where a.PinYin.ToLower().Contains(prar) || a.Name.ToLower().Contains(prar) || a.Symbol.ToLower().Contains(prar)
                            select a;
                AutoCompleteBox source = (AutoCompleteBox)sender;
                source.ItemsSource = query.ToList();
            }
        }
        #endregion

        #region ToolBar Events
        private void toolBar_SaveAndClose(object sender, EventArgs e)
        {
            if (!InputCheck())
                return;
            // 如果是新增窗口
            if (isNewForm)
            {
                string declarationNumber = tbHGBH.Text;
                string approvalNumber = tbHXDH.Text;
                busyIndicator.IsBusy = true;
                SystemConfiguration.Instance.DataContext.CheckExsitingDeclaration(declarationNumber
                    , approvalNumber
                    , (p) =>
                    {
                        if (p.HasError)
                        {
                            busyIndicator.IsBusy = false;
                            p.MarkErrorAsHandled();
                            MessageBox.Show(p.Error.Message);
                        }
                        else
                        {
                            if (p.Value && CommonUIFunction.ShowConfirm(MessageTexts.MSG029) == MessageBoxResult.Cancel)
                            {
                                busyIndicator.IsBusy = false;
                                return;
                            }
                            Save(false);
                        }
                    }, null);
            }
            else // 编辑窗口
            {
                SaveForEdit();
            }
        }

        private void SaveForEdit()
        {
            // 更新DoubleCheckDeclaration
            var doubleCheckDeclaration = (from d in SystemConfiguration.Instance.DataContext.DoubleCheckDeclarations
                                          where d.ID == _inputDataModel.ID
                                          select d).FirstOrDefault();
            if (doubleCheckDeclaration == null)
                return;
            doubleCheckDeclaration.CustomerName = ((CustomerDataModel)acbCustomer.SelectedItem).Name;
            doubleCheckDeclaration.DeclarationNumber = tbHGBH.Text;//海关编号
            doubleCheckDeclaration.ApprovalNumber = tbHXDH.Text;//核销单号
            doubleCheckDeclaration.TransactionName = cbTransaction.SelectedItem == null ? "" : ((TransactionDataModel)cbTransaction.SelectedItem).Name;
            doubleCheckDeclaration.DistrictName = tbYNHYD.Text;// acbDistrict.SelectedItem == null ? acbDistrict.Text : ((DistrictDataModel)acbDistrict.SelectedItem).Name;
            doubleCheckDeclaration.TradeName = tbMYFS.Text;
            doubleCheckDeclaration.FeightFeeRate = tbShipFee.Text;
            doubleCheckDeclaration.InsuranceFeeRate = tbInsureFee.Text;
            doubleCheckDeclaration.CustomhouseName = tbCKKA.Text;
            doubleCheckDeclaration.CountryName = tbYDG.Text;//acbCountry.SelectedItem == null ? acbCountry.Text : ((CountryDataModel)acbCountry.SelectedItem).Name;
            doubleCheckDeclaration.ManualNumber = tbManualNumber.Text;
            doubleCheckDeclaration.LicenseNumber = tbLicenseNumber.Text;
            doubleCheckDeclaration.PackageAmount = tbPackageAmount.Text;
            doubleCheckDeclaration.ContractNumber = tbContractNumber.Text;
            doubleCheckDeclaration.GrossWeight = tbGrossWeight.Text;
            doubleCheckDeclaration.NetWeight = tbNetWeight.Text;
            doubleCheckDeclaration.ContainerNumbers = tbContainerNumbers.Text;
            doubleCheckDeclaration.DocumentCodes = tbDocumentCodes.Text;
            doubleCheckDeclaration.ExaminationNumber = tbExaminationNumber.Text;
            doubleCheckDeclaration.PayName = tbJHFS.Text;// acbPay.SelectedItem == null ? acbPay.Text : ((PayDataModel)acbPay.SelectedItem).Name;
            doubleCheckDeclaration.InsuranceFeeCurrencyName = tbBF.Text;// acbInsureFeeCurrency.SelectedItem == null ? acbInsureFeeCurrency.Text : ((CurrencyDataModel)acbInsureFeeCurrency.SelectedItem).Name;
            doubleCheckDeclaration.FreightFeeCurrencyName = tbYF.Text;// acbFeightFeeCurrency.SelectedItem == null ? acbFeightFeeCurrency.Text : ((CurrencyDataModel)acbFeightFeeCurrency.SelectedItem).Name;

            // 更新DoubleCheckDeclarationItems
            foreach (var dd in doubleCheckDeclarationSource.DoubleCheckDeclarationItem)
            {
                var queryNew = (from n in _hsAddList
                                where n.ID == dd.ID
                                select n).SingleOrDefault();
                if (queryNew != null)
                {
                    dd.SortOrder = queryNew.SortOrder;
                    dd.Name = queryNew.Name;
                    dd.HSCode = queryNew.HSCode;
                    dd.FirstUnitName = queryNew.FirstUnitName;
                    dd.FirstQuantity = queryNew.FirstQuantity;
                    dd.SecondQuantity = queryNew.SecondQuantity;
                    dd.SecondUnitName = queryNew.SecondUnitName;
                    dd.ControlNumber = queryNew.ControlNumber;
                    dd.DeclaredQuantity = queryNew.DeclaredQuantity;
                    dd.DeclaredUnitName = queryNew.DeclaredUnitName;
                    dd.TotalAmount = queryNew.TotalAmount;
                    dd.CurrencyName = queryNew.CurrencyName;
                }
            }
            // 添加
            List<int> dItemIDDBs = doubleCheckDeclarationSource.DoubleCheckDeclarationItem.Select(a => a.ID).ToList();
            var queryAdd = from d in _hsAddList
                           where !dItemIDDBs.Contains(d.ID)
                           select d;
            foreach (var hs in queryAdd)
            {
                Web.DoubleCheckDeclarationItem dd = new Web.DoubleCheckDeclarationItem();
                dd.DoubleCheckDeclarationId = doubleCheckDeclarationSource.ID;
                dd.SortOrder = hs.SortOrder;
                dd.Name = hs.Name;
                dd.HSCode = hs.HSCode;
                dd.FirstUnitName = hs.FirstUnitName;
                dd.FirstQuantity = hs.FirstQuantity;
                dd.SecondQuantity = hs.SecondQuantity;
                dd.SecondUnitName = hs.SecondUnitName;
                dd.ControlNumber = hs.ControlNumber;
                dd.DeclaredQuantity = hs.DeclaredQuantity;
                dd.DeclaredUnitName = hs.DeclaredUnitName;
                dd.TotalAmount = hs.TotalAmount;
                dd.CurrencyName = hs.CurrencyName;
                SystemConfiguration.Instance.DataContext.DoubleCheckDeclarationItems.Add(dd);
            }
            // 删除
            List<int> dItemIDs = _hsAddList.Select(a => a.ID).ToList();
            var queryDelete = from t in doubleCheckDeclarationSource.DoubleCheckDeclarationItem
                              where !dItemIDs.Contains(t.ID)
                              select t;
            for (int i = 0; i < queryDelete.Count(); i++)
            {
                for (int j = 0; j < SystemConfiguration.Instance.DataContext.DoubleCheckDeclarationItems.Count(); j++)
                {
                    if (SystemConfiguration.Instance.DataContext.DoubleCheckDeclarationItems.ElementAt(j) == queryDelete.ElementAt(i))
                    {
                        //int testID = queryDelete.ElementAt(i).ID;
                        SystemConfiguration.Instance.DataContext.DoubleCheckDeclarationItems.Remove(queryDelete.ElementAt(i));
                        i--;
                        break;
                    }
                }
            }

            //
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
                    this.Close();
                }
            }, null);
        }

        private void Save(bool isNew)
        {
            // add declaration
            Web.Declaration declaration = new Web.Declaration();
            declaration.ReceivedDate = (DateTime)dpReceiveDate.SelectedDate;
            declaration.CreatedDate = DateTime.Now;
            declaration.CustomerID = ((CustomerDataModel)acbCustomer.SelectedItem).ID;
            declaration.DeclarationNumber = tbHGBH.Text;
            declaration.ApprovalNumber = tbHXDH.Text;
            declaration.DeclarationStatus = "正在审理中";
            declaration.DeclarationStatusRemark = "最后更新：" + SystemConfiguration.Instance.LoggedOnUser.Name;
            declaration.RelatedSystemNumber = tbRelatedSystemNumber.Text;
            //declaration.CountryCode = tbYDG.Text;//acbCountry.SelectedItem == null ? "" : ((CountryDataModel)acbCountry.SelectedItem).Code;
            declaration.DeclarationDate = DateTime.Now;
            SystemConfiguration.Instance.DataContext.Declarations.Add(declaration);
            // add DoubleCheckDeclaration:
            Web.DoubleCheckDeclaration dcd = new Web.DoubleCheckDeclaration();
            dcd.DeclarationId = declaration.ID;
            dcd.CustomerName = ((CustomerDataModel)acbCustomer.SelectedItem).Name;
            dcd.DeclarationNumber = declaration.DeclarationNumber;
            dcd.ApprovalNumber = declaration.ApprovalNumber;
            dcd.TransactionName = cbTransaction.SelectedItem==null ?"":((TransactionDataModel)cbTransaction.SelectedItem).Name;
            dcd.DistrictName = tbYNHYD.Text;// acbDistrict.SelectedItem == null ? acbDistrict.Text : ((DistrictDataModel)acbDistrict.SelectedItem).Name;
            dcd.TradeName = tbMYFS.Text;//acbTrade.SelectedItem==null?acbTrade.Text:((TradeDataModel)acbTrade.SelectedItem).Name;
            dcd.FeightFeeRate = tbShipFee.Text;
            dcd.InsuranceFeeRate = tbInsureFee.Text;
            dcd.CustomhouseName = tbCKKA.Text;
            dcd.CountryName = tbYDG.Text;// acbCountry.SelectedItem == null ? acbCountry.Text : ((CountryDataModel)acbCountry.SelectedItem).Name;
            dcd.ManualNumber = tbManualNumber.Text;
            dcd.LicenseNumber = tbLicenseNumber.Text;
            dcd.PackageAmount = tbPackageAmount.Text;
            dcd.ContractNumber = tbContractNumber.Text;
            dcd.GrossWeight = tbGrossWeight.Text;
            dcd.NetWeight = tbNetWeight.Text;
            dcd.ContainerNumbers = tbContainerNumbers.Text;
            dcd.DocumentCodes = tbDocumentCodes.Text;
            dcd.ExaminationNumber = tbExaminationNumber.Text;
            dcd.PayName = tbJHFS.Text;// acbPay.SelectedItem == null ? acbPay.Text : ((PayDataModel)acbPay.SelectedItem).Name;
            dcd.FreightFeeCurrencyName = tbYF.Text;// acbFeightFeeCurrency.SelectedItem == null ? acbFeightFeeCurrency.Text : ((CurrencyDataModel)acbFeightFeeCurrency.SelectedItem).Name;
            dcd.InsuranceFeeCurrencyName = tbBF.Text;// acbInsureFeeCurrency.SelectedItem == null ? acbInsureFeeCurrency.Text : ((CurrencyDataModel)acbInsureFeeCurrency.SelectedItem).Name;
            

            SystemConfiguration.Instance.DataContext.DoubleCheckDeclarations.Add(dcd);
            // add DoubleCheckDeclarationItem

            if (_hsAddList != null && _hsAddList.Count > 0)
            {
                foreach (var hs in _hsAddList)
                {
                    Web.DoubleCheckDeclarationItem dd = new Web.DoubleCheckDeclarationItem();
                    dd.DoubleCheckDeclarationId = dcd.ID;
                    dd.SortOrder = hs.SortOrder;
                    dd.Name = hs.Name;
                    dd.HSCode = hs.HSCode;
                    dd.FirstUnitName = hs.FirstUnitName;
                    dd.FirstQuantity = hs.FirstQuantity;
                    dd.SecondQuantity = hs.SecondQuantity;
                    dd.SecondUnitName = hs.SecondUnitName;
                    dd.ControlNumber = hs.ControlNumber;
                    dd.DeclaredQuantity = hs.DeclaredQuantity;
                    dd.DeclaredUnitName = hs.DeclaredUnitName;
                    dd.TotalAmount = hs.TotalAmount;
                    dd.CurrencyName = hs.CurrencyName;
                    SystemConfiguration.Instance.DataContext.DoubleCheckDeclarationItems.Add(dd);
                }
            }

            
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
                    //update ViewModel
                    GetAllDeclarationByReceiveDateResultViewModel vm = ViewModelManager.GetAllDeclarationByReceiveDateResultViewModelInstance;
                    if (vm != null)
                    {
                        GetAllDeclarationByReceiveDateResultDataModel dm = new GetAllDeclarationByReceiveDateResultDataModel();
                        dm.ID = declaration.ID;
                        dm.ReceivedDate = declaration.ReceivedDate;
                        dm.DeclarationNumber = declaration.DeclarationNumber;
                        dm.ApprovalNumber = declaration.ApprovalNumber;
                        dm.RelatedSystemNumber = declaration.RelatedSystemNumber;
                        dm.CustomerName = dcd.CustomerName;
                        vm.Items.Add(dm);
                        vm.UpdateIndex();
                    }

                    if (isNew)
                    {
                        CommonUIFunction.ShowMessageText(bdMsgParent, "保存成功，请继续添加！");
                        _inputDataModel = new DeclarationInputModel();
                        _inputDataModel.ReceivedDate = DateTime.Now;
                        this.DataContext = _inputDataModel;
                        cbTransaction.SelectedIndex = -1;
                        gdHSCode.ItemsSource = null;
                        _hsAddList.Clear();
                    }
                    else
                    {
                        this.Close();
                    }
                }
            }, null);
        }

        private void toolBar_SaveAndNew(object sender, EventArgs e)
        {
            if (!InputCheck())
                return;
            string declarationNumber = tbHGBH.Text;
            string approvalNumber = tbHXDH.Text;
            busyIndicator.IsBusy = true;
            SystemConfiguration.Instance.DataContext.CheckExsitingDeclaration(declarationNumber
                , approvalNumber
                , (p) =>
                {
                    if (p.HasError)
                    {
                        busyIndicator.IsBusy = false;
                        p.MarkErrorAsHandled();
                        MessageBox.Show(p.Error.Message);
                    }
                    else
                    {
                        if(p.Value && CommonUIFunction.ShowConfirm(MessageTexts.MSG029) == MessageBoxResult.Cancel)
                        {
                            busyIndicator.IsBusy = false;
                            return;
                        }
                        Save(true);
                    }
                }, null);
        }

        private void toolBar_Close(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        private void tbFirstQuantity_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void tbCKKA_LostFocus(object sender, RoutedEventArgs e)
        {
            CustomhouseViewModel vm = App.Current.Resources["CustomhouseViewModel"] as CustomhouseViewModel;
            if (vm != null)
            {
                var prar = tbCKKA.Text.ToLower();
                if (!string.IsNullOrEmpty(prar))
                {
                    var query = (from a in vm.Items
                                where a.PinYin.ToLower() == (prar) || a.Name.ToLower() == (prar) || a.Code.ToLower() == (prar)
                                select a.Name).FirstOrDefault();
                    if (!string.IsNullOrEmpty(query))
                    {
                        tbCKKA.Text = query;
                    }
                }
            }
        }

        private void tbMYFS_LostFocus(object sender, RoutedEventArgs e)
        {
            TradeViewModel vm = App.Current.Resources["TradeViewModel"] as TradeViewModel;
            if (vm != null)
            {
                var prar = tbMYFS.Text.ToLower();
                var query = (from a in vm.Items
                            where a.PinYin.ToLower() == (prar) || a.Name.ToLower() == (prar) || a.Code.ToLower() == (prar)
                            select a.Name).FirstOrDefault();
                if (!string.IsNullOrEmpty(query))
                {
                    tbMYFS.Text = query;
                }
            }
        }

        private void tbYDG_LostFocus(object sender, RoutedEventArgs e)
        {
            CountryViewModel vm = ViewModelManager.CountryViewModelInstance as CountryViewModel;
            if (vm != null)
            {
                var prar = tbYDG.Text.ToLower();
                var query = (from a in vm.Items
                            where a.PinYin.ToLower() == (prar) || a.Name.ToLower() == (prar) || a.Code.ToLower() == (prar)
                            select a.Name).FirstOrDefault();
                if (!string.IsNullOrEmpty(query))
                {
                    tbYDG.Text = query;
                }
            }
        }

        private void tbYNHYD_LostFocus(object sender, RoutedEventArgs e)
        {
            DistrictViewModel vm = App.Current.Resources["DistrictViewModel"] as DistrictViewModel;
            if (vm != null)
            {
                var prar = tbYNHYD.Text.ToLower();
                var query = (from a in vm.Items
                            where a.PinYin.ToLower() == (prar) || a.Name.ToLower() == (prar) || a.Code.ToLower() == (prar)
                            select a.Name).FirstOrDefault();
                if (!string.IsNullOrEmpty(query))
                {
                    tbYNHYD.Text = query;
                }
            }
        }

        private void tbYF_LostFocus(object sender, RoutedEventArgs e)
        {
            CurrencyViewModel vm = ViewModelManager.CurrencyViewModelInstance;
            if (vm != null)
            {
                var prar = tbYF.Text.ToLower();
                var query = (from a in vm.Items
                            where a.PinYin.ToLower() == (prar) || a.Name.ToLower() == (prar) || a.Symbol.ToLower() == (prar)
                            select a.Name).FirstOrDefault();

                if (!string.IsNullOrEmpty(query))
                {
                    tbYF.Text = query;
                }
            }
        }

        private void tbBF_LostFocus(object sender, RoutedEventArgs e)
        {
            CurrencyViewModel vm = ViewModelManager.CurrencyViewModelInstance;
            if (vm != null)
            {
                var prar = tbBF.Text.ToLower();
                var query = (from a in vm.Items
                             where a.PinYin.ToLower() == (prar) || a.Name.ToLower() == (prar) || a.Symbol.ToLower() == (prar)
                             select a.Name).FirstOrDefault();

                if (!string.IsNullOrEmpty(query))
                {
                    tbBF.Text = query;
                }
            }
        }

        private void tbJHFS_LostFocus(object sender, RoutedEventArgs e)
        {
            PayViewModel vm = ViewModelManager.PayViewModelInstance;
            if (vm != null)
            {
                var prar = tbJHFS.Text.ToLower();
                var query = (from a in vm.Items
                            where a.PinYin.ToLower() == (prar) || a.Name.ToLower() == (prar) || a.Code == (prar)
                            select a.Name).FirstOrDefault();
                if (!string.IsNullOrEmpty(query))
                {
                    tbJHFS.Text = query;
                }
            }
        }

        private void tbBZ_LostFocus(object sender, RoutedEventArgs e)
        {
            CurrencyViewModel vm = ViewModelManager.CurrencyViewModelInstance;
            if (vm != null)
            {
                var prar = tbBZ.Text.ToLower();
                var query = (from a in vm.Items
                             where a.PinYin.ToLower() == (prar) || a.Name.ToLower() == (prar) || a.Symbol.ToLower() == (prar)
                             select a.Name).FirstOrDefault();

                if (!string.IsNullOrEmpty(query))
                {
                    tbBZ.Text = query;
                }
            }
        }

   

    }
}
