using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using ProTemplate.Web;
using ProTemplate.Utility;
using ProTemplate.ViewModels;
using Telerik.Windows.Controls;
using ProTemplate.Models;
using System.Collections;

namespace ProTemplate.UserControls.CustomControl
{
    public partial class MainFeeForm : UserControl
    {
        public string financialRemark { 
            get 
            {
                return tbRemark.Text;
            }
            set 
            { 
                tbRemark.Text = value == null ? "" : value; 
            }
        }
        FinancialExportDeclarationDataModel _finDataModel;
        FormState _editState = FormState.Add;
        public MainFeeForm()
        {
            InitializeComponent();
            _finDataModel = new FinancialExportDeclarationDataModel();
            this.DataContext = _finDataModel;
        }

        private int _currentDeclarationID;
        public int CurrentDeclarationID
        {
            get { return _currentDeclarationID; }
            set
            {
                _currentDeclarationID = value;
                //// 设置最大序号
                //FinancialExportDeclarationViewModel dvm = ViewModelManager.FinancialExportDeclarationDataModelViewModelInstance;

            }
        }

        public void SetToReadOnly()
        {
            btnAddFeeType.IsEnabled = false;
            btnUpdateFeeType.IsEnabled = false;
            acbFeeType.IsEnabled = false;
            gvFee.Columns[3].IsVisible = false;
            gvFee.Columns[4].IsVisible = false;
        }

        FormState FinancialExportDeclarationEditState
        {
            get { return _editState; }
            set
            {
                _editState = value;
                if (_editState == FormState.Add)
                {
                    // clear input
                    _finDataModel = new FinancialExportDeclarationDataModel();
                    _finDataModel.FeeTypeName = "";
                    _finDataModel.Remark = "";
                    _finDataModel.Amount = 0;
                    _finDataModel.Cost = 0;

                    this.DataContext = _finDataModel;
                    btnAddFeeType.Visibility = System.Windows.Visibility.Visible;
                    spUpdateFeeType.Visibility = System.Windows.Visibility.Collapsed;
                }
                else
                {
                    btnAddFeeType.Visibility = System.Windows.Visibility.Collapsed;
                    spUpdateFeeType.Visibility = System.Windows.Visibility.Visible;
                }
            }
        }

        private void acbFeeType_Populating(object sender, PopulatingEventArgs e)
        {
            FeeTypeViewModel vm = App.Current.Resources["FeeTypeViewModel"] as FeeTypeViewModel;
            if (vm != null)
            {
                var prar = e.Parameter.ToLower();
                var query = from a in vm.Items
                            where a.Code.ToLower().Contains(prar) || a.Name.ToLower().Contains(prar)
                            select a;
                int b = query.Count();
                AutoCompleteBox source = (AutoCompleteBox)sender;
                source.ItemsSource = query.ToList();
            }
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
        private bool InputCheck()
        {
            if (acbFeeType.Text == "" || acbFeeType.SelectedItem == null)
            {
                _finDataModel.SetErrors("FeeTypeName", new List<string>() { "请输入合法的费用类型！" });
            }
            else
            {
                _finDataModel.ClearErrors("FeeTypeName");
            }

            if (!Constants.IsDouble(tbAmount.Text))
            {
                _finDataModel.SetErrors("Amount", new List<string>() { "请输入合法的费用！" });
            }
            else
            {
                _finDataModel.ClearErrors("Amount");
            }

            if (!Constants.IsDouble(tbCost.Text))
            {
                _finDataModel.SetErrors("Cost", new List<string>() { "请输入合法的成本！" });
            }
            else
            {
                _finDataModel.ClearErrors("Cost");
            }

            return !_finDataModel.HasErrors;
        }

        private void btnAddFeeType_Click(object sender, RoutedEventArgs e)
        {
            if (!InputCheck())
                return;


            FinancialExportDeclarationViewModel divm = ViewModelManager.FinancialExportDeclarationViewModelInstance;
            System.Diagnostics.Debug.Assert(divm != null);
            FinancialExportDeclarationDataModel ddd = new FinancialExportDeclarationDataModel();
            ddd.Index = divm.Items.Count+1;
            ddd.FeeTypeCode = acbFeeType.SelectedItem == null ? "" : ((FeeTypeDataModel)acbFeeType.SelectedItem).Code;
            ddd.FeeTypeName = acbFeeType.SelectedItem == null ? "" : ((FeeTypeDataModel)acbFeeType.SelectedItem).Name;
            ddd.Amount = decimal.Parse(tbAmount.Text);
            ddd.Cost = decimal.Parse(tbCost.Text);
            ddd.Remark = "";// tbRemark.Text;
            divm.Items.Add(ddd);

            Web.FinancialExportDeclaration dd = new Web.FinancialExportDeclaration();
            dd.Cost = ddd.Cost.HasValue ? ddd.Cost.Value : 0;
            dd.Amount = ddd.Amount.HasValue ? ddd.Amount.Value : 0;
            dd.DeclarationId = CurrentDeclarationID;
            dd.FeeTypeCode = ddd.FeeTypeCode;
            dd.FinancialRemark = ddd.Remark;
            dd.CreatedDate = DateTime.Now;

            //SystemConfiguration.Instance.DataContext.FinancialExportDeclarations.Add(dd);

            var realItem = (from t in SystemConfiguration.Instance.DataContext.Declarations
                            where t.ID == CurrentDeclarationID
                            select t).SingleOrDefault();
            if (realItem != null)
                realItem.FinancialExportDeclaration.Add(dd);

            _finDataModel = new FinancialExportDeclarationDataModel();
            this.DataContext = _finDataModel;
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            _finDataModel = ((RadButton)sender).DataContext as FinancialExportDeclarationDataModel;
            this.DataContext = _finDataModel;
            FeeTypeDataModel ddm = new FeeTypeDataModel();
            ddm.Code = _finDataModel.FeeTypeCode;
            ddm.Name = _finDataModel.FeeTypeName;
            acbFeeType.SelectedItem = ddm;
            tbAmount.Text = _finDataModel.Amount.ToString();
            tbCost.Text = _finDataModel.Cost.ToString();
            //tbRemark.Text = _finDataModel.Remark == null ? "" : _finDataModel.Remark;
            FinancialExportDeclarationEditState = FormState.Update;
        }

        private void btnCancelFeeType_Click(object sender, RoutedEventArgs e)
        {
            FinancialExportDeclarationEditState = FormState.Add;
        }

        private void btnUpdateFeeType_Click(object sender, RoutedEventArgs e)
        {
            if (!InputCheck())
                return;
            _finDataModel.FeeTypeCode = ((FeeTypeDataModel)acbFeeType.SelectedItem).Code;
            _finDataModel.FeeTypeName = ((FeeTypeDataModel)acbFeeType.SelectedItem).Name;
            _finDataModel.Amount = decimal.Parse(tbAmount.Text);
            _finDataModel.Cost = decimal.Parse(tbCost.Text);
            _finDataModel.Remark = "";// tbRemark.Text;

            #region 更新数据库中数据
            var realDeclarationObj = (from t in SystemConfiguration.Instance.DataContext.Declarations
                                      where t.ID == CurrentDeclarationID
                                      select t).SingleOrDefault();
            if (realDeclarationObj != null)
            {
                Web.FinancialExportDeclaration dbItem = null;
                dbItem = (from a in realDeclarationObj.FinancialExportDeclaration
                          where a.ID == _finDataModel.ID
                          select a).SingleOrDefault();
                
                if (dbItem != null)
                {
                    dbItem.FeeTypeCode = _finDataModel.FeeTypeCode;
                    dbItem.Amount = _finDataModel.Amount.HasValue ? _finDataModel.Amount.Value : 0;
                    dbItem.Cost = _finDataModel.Cost.HasValue ? _finDataModel.Cost.Value : 0;
                    dbItem.CreatedDate = DateTime.Now;
                    dbItem.FinancialRemark = _finDataModel.Remark;
                }
            }
            #endregion
            // set to default
            FinancialExportDeclarationEditState = FormState.Add;
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            FinancialExportDeclarationDataModel dm = ((RadButton)sender).DataContext as FinancialExportDeclarationDataModel;
            if (dm == null)
                return;
            CommonUIFunction.ShowConfirmYesNo("确定要删除这个收费吗?",
            (s, arg) =>
            {
                if (arg.DialogResult == true)
                {
                    FinancialExportDeclarationViewModel divm = ViewModelManager.FinancialExportDeclarationViewModelInstance;
                    if (divm != null)
                        divm.Items.Remove(dm);

                    #region 删除数据库中数据
                    var realDeclarationObj = (from t in SystemConfiguration.Instance.DataContext.Declarations
                                              where t.ID == CurrentDeclarationID
                                              select t).SingleOrDefault();
                    if (realDeclarationObj != null)
                    {
                        Web.FinancialExportDeclaration dbItem = null;
                        dbItem = (from a in realDeclarationObj.FinancialExportDeclaration
                                 where a.ID == dm.ID
                                 select a).SingleOrDefault();

                        if (dbItem != null)
                        {
                            SystemConfiguration.Instance.DataContext.FinancialExportDeclarations.Remove(dbItem);
                        }
                    }
                    #endregion
                }
            });
        }



    }
}
