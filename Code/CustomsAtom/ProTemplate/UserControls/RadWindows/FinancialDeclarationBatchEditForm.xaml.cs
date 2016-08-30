using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using Telerik.Windows.Controls;
using ProTemplate.Models;
using ProTemplate.ViewModels;
using ProTemplate.Utility;

namespace ProTemplate.UserControls.RadWindows
{
    /// <summary>
    /// Interaction logic for FinancialDeclarationBatchEditForm.xaml
    /// </summary>
    public partial class FinancialDeclarationBatchEditForm
    {
        public FinancialDeclarationBatchEditForm()
        {
            InitializeComponent();
        }
        public List<GetAllFinancialExportDeclarationDataModel> FinancialDeclarations { get; set; }

        private void RadWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Load(SystemConfiguration.Instance.LoggedOnUser.ID);
        }
        private void EditWindowToolBar_SaveAndClose(object sender, EventArgs e)
        {
            foreach (GetAllFinancialExportDeclarationDataModel dm in gdDeclaration.Items)
            {
                var declaration = (from d in SystemConfiguration.Instance.DataContext.Declarations
                                   where d.ID == dm.ID
                                   select d).SingleOrDefault();
                if (declaration != null)
                {
                    CustomerViewModel vmCustomer = App.Current.Resources["CustomerViewModel"] as CustomerViewModel;

                    var customer = (from c in vmCustomer.Items where c.Name == dm.CustomerName select c).FirstOrDefault();

                    if (customer != null)
                    {
                        declaration.CustomerID = customer.ID;
                    }
                    else
                    {
                        continue;
                    }

                    declaration.BillNumber = dm.BillNumber;
                    declaration.DeclarationNumber = dm.DeclarationNumber;
                    declaration.ApprovalNumber = dm.ApprovalNumber;
                    declaration.Remark = dm.Remark;
                    declaration.FinancialRemark = dm.FinancialRemark;

                    Web.FinancialExportDeclaration billFee = declaration.FinancialExportDeclaration.FirstOrDefault(o => o.FeeTypeCode == "106");
                    if (billFee != null)
                    {
                        billFee.Amount = dm.BillFeeAmount.HasValue ? dm.BillFeeAmount.Value : 0;
                    }
                    else
                    {
                        if (dm.BillFeeAmount.HasValue && Constants.IsDouble(dm.BillFeeAmount.Value.ToString()))
                        {
                            billFee = new Web.FinancialExportDeclaration();
                            billFee.FeeTypeCode = "106";
                            billFee.Amount = dm.BillFeeAmount.HasValue ? dm.BillFeeAmount.Value : 0;
                            billFee.DeclarationId = declaration.ID;
                            billFee.CreatedDate = DateTime.Now;
                            SystemConfiguration.Instance.DataContext.FinancialExportDeclarations.Add(billFee);
                        }
                    }
                    Web.FinancialExportDeclaration declarationFee = declaration.FinancialExportDeclaration.FirstOrDefault(o => o.FeeTypeCode == "101" || o.FeeTypeCode == "102" || o.FeeTypeCode == "103" || o.FeeTypeCode == "104" || o.FeeTypeCode == "105");
                    if (declarationFee != null)
                    {
                        declarationFee.Amount = dm.DeclarationFeeAmount.HasValue ? dm.DeclarationFeeAmount.Value : 0;
                        declarationFee.Cost = dm.DeclarationFeeCost.HasValue ? dm.DeclarationFeeCost.Value : 0;
                    }
                    else
                    {
                        if (dm.DeclarationFeeAmount.HasValue || dm.DeclarationFeeCost.HasValue)
                        {
                            declarationFee = new Web.FinancialExportDeclaration();
                            declarationFee.FeeTypeCode = "101";
                            declarationFee.Amount = dm.DeclarationFeeAmount.HasValue ? (Constants.IsDouble(dm.DeclarationFeeAmount.Value.ToString()) ? dm.DeclarationFeeAmount.Value : 0) : 0;
                            declarationFee.Cost = dm.DeclarationFeeCost.HasValue ? (Constants.IsDouble(dm.DeclarationFeeCost.Value.ToString()) ? dm.DeclarationFeeCost.Value : 0) : 0;
                            declarationFee.DeclarationId = declaration.ID;
                            declarationFee.CreatedDate = DateTime.Now;
                            SystemConfiguration.Instance.DataContext.FinancialExportDeclarations.Add(declarationFee);
                        }
                    }
                    Web.FinancialExportDeclaration examinationFee = declaration.FinancialExportDeclaration.FirstOrDefault(o => o.FeeTypeCode == "107");
                    if (examinationFee != null)
                    {
                        examinationFee.Amount = dm.ExaminationFeeAmount.HasValue ? dm.ExaminationFeeAmount.Value : 0;
                        examinationFee.Cost = dm.ExaminationFeeCost.HasValue ? dm.ExaminationFeeCost.Value : 0;
                    }
                    else
                    {
                        if (dm.ExaminationFeeAmount.HasValue || dm.ExaminationFeeCost.HasValue)
                        {
                            examinationFee = new Web.FinancialExportDeclaration();
                            examinationFee.FeeTypeCode = "107";
                            examinationFee.Amount = dm.ExaminationFeeAmount.HasValue ? (Constants.IsDouble(dm.ExaminationFeeAmount.Value.ToString()) ? dm.ExaminationFeeAmount.Value : 0) : 0;
                            examinationFee.Cost = dm.ExaminationFeeCost.HasValue ? (Constants.IsDouble(dm.ExaminationFeeCost.Value.ToString()) ? dm.ExaminationFeeCost.Value : 0) : 0;
                            examinationFee.DeclarationId = declaration.ID;
                            examinationFee.CreatedDate = DateTime.Now;
                            SystemConfiguration.Instance.DataContext.FinancialExportDeclarations.Add(examinationFee);
                        }
                    }
                    Web.FinancialExportDeclaration checkFee = declaration.FinancialExportDeclaration.FirstOrDefault(o => o.FeeTypeCode == "108");
                    if (checkFee != null)
                    {
                        checkFee.Amount = dm.CheckFeeAmount.HasValue ? dm.CheckFeeAmount.Value : 0;
                        checkFee.Cost = dm.CheckFeeCost.HasValue ? dm.CheckFeeCost.Value : 0;
                    }
                    else
                    {
                        if (dm.CheckFeeAmount.HasValue || dm.CheckFeeCost.HasValue)
                        {
                            checkFee = new Web.FinancialExportDeclaration();
                            checkFee.FeeTypeCode = "108";
                            checkFee.Amount = dm.CheckFeeAmount.HasValue ? (Constants.IsDouble(dm.CheckFeeAmount.Value.ToString()) ? dm.CheckFeeAmount.Value : 0) : 0;
                            checkFee.Cost = dm.CheckFeeCost.HasValue ? (Constants.IsDouble(dm.CheckFeeCost.Value.ToString()) ? dm.CheckFeeCost.Value : 0) : 0;
                            checkFee.DeclarationId = declaration.ID;
                            checkFee.CreatedDate = DateTime.Now;
                            SystemConfiguration.Instance.DataContext.FinancialExportDeclarations.Add(checkFee);
                        }
                    }

                    Web.FinancialExportDeclaration commissionFee = declaration.FinancialExportDeclaration.FirstOrDefault(o => o.FeeTypeCode == "109");
                    if (commissionFee != null)
                    {
                        commissionFee.Amount = dm.CommissionFeeAmount.HasValue ? dm.CommissionFeeAmount.Value : 0;
                        commissionFee.Cost = dm.CommissionFeeCost.HasValue ? dm.CommissionFeeCost.Value : 0;
                    }
                    else
                    {
                        if (dm.CommissionFeeAmount.HasValue || dm.CommissionFeeCost.HasValue)
                        {
                            commissionFee = new Web.FinancialExportDeclaration();
                            commissionFee.FeeTypeCode = "109";
                            commissionFee.Amount = dm.CommissionFeeAmount.HasValue ? (Constants.IsDouble(dm.CommissionFeeAmount.Value.ToString()) ? dm.CommissionFeeAmount.Value : 0) : 0;
                            commissionFee.Cost = dm.CommissionFeeCost.HasValue ? (Constants.IsDouble(dm.CommissionFeeCost.Value.ToString()) ? dm.CommissionFeeCost.Value : 0) : 0;
                            commissionFee.DeclarationId = declaration.ID;
                            commissionFee.CreatedDate = DateTime.Now;
                            SystemConfiguration.Instance.DataContext.FinancialExportDeclarations.Add(commissionFee);
                        }
                    }
                }
            }

            SystemConfiguration.Instance.DataContext.SubmitChanges((a) =>
            {
                busyIndicator.IsBusy = false;
                if (a.HasError)
                {
                    a.MarkErrorAsHandled();
                    CommonUIFunction.ShowMessageText(bdMsgParent, a.Error.Message, true);
                    SystemConfiguration.Instance.DataContext.RejectChanges();
                }
                else
                {
                    CommonUIFunction.ShowMessageText(bdMsgParent, "保存成功");
                    foreach (GetAllFinancialExportDeclarationDataModel dm in gdDeclaration.Items)
                    {
                        var declaration = (from c in FinancialDeclarations where c.ID == dm.ID select c).FirstOrDefault();
                        if (declaration != null)
                        {
                            //有问题，为什么赋值赋不进去
                            declaration.IsLocked = false;
                            declaration.BillNumber = dm.BillNumber;
                            declaration.ApprovalNumber = dm.ApprovalNumber;
                            declaration.DeclarationNumber = dm.DeclarationNumber;
                            declaration.Remark = dm.Remark;
                            declaration.CustomerName = dm.CustomerName;
                            declaration.BillFeeAmount = dm.BillFeeAmount;
                            declaration.DeclarationFeeAmount = dm.DeclarationFeeAmount;
                            declaration.DeclarationFeeCost = dm.DeclarationFeeCost;
                            declaration.ExaminationFeeAmount = dm.ExaminationFeeAmount;
                            declaration.ExaminationFeeCost = dm.ExaminationFeeCost;
                            declaration.CheckFeeAmount = dm.CheckFeeAmount;
                            declaration.CheckFeeCost = dm.CheckFeeCost;
                            declaration.CommissionFeeAmount = dm.CommissionFeeAmount;
                            declaration.CommissionFeeCost = dm.CommissionFeeCost;
                            declaration.FinancialRemark = dm.FinancialRemark;
                            declaration.IsLocked = true;
                        }

                    }

                    this.Close();
                }
            }, null);
        }

        private void EditWindowToolBar_SaveAndNew(object sender, EventArgs e)
        {
            //Save(true);
        }

        private void EditWindowToolBar_Close(object sender, EventArgs e)
        {
            this.Close();
        }

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

        private void Load(int userID)
        {
            busyIndicator.IsBusy = true;
            SystemConfiguration.Instance.DataContext.Declarations.Clear();
            SystemConfiguration.Instance.DataContext.FinancialExportDeclarations.Clear();
            SystemConfiguration.Instance.DataContext.Load(SystemConfiguration.Instance.DataContext.GetDeclarationByIDsQuery(FinancialDeclarations.Select(o => o.ID)), (p) => { busyIndicator.IsBusy = false; }, null);
            
            List<GetAllFinancialExportDeclarationDataModel> lstSource = new List<GetAllFinancialExportDeclarationDataModel>();
            foreach (GetAllFinancialExportDeclarationDataModel d in FinancialDeclarations)
            {
                GetAllFinancialExportDeclarationDataModel dm = new GetAllFinancialExportDeclarationDataModel();

                dm.Index = lstSource.Count + 1;
                dm.ID = d.ID;
                dm.CustomerName = d.CustomerName;
                dm.BillNumber = d.BillNumber;
                dm.DeclarationNumber = d.DeclarationNumber;
                dm.ApprovalNumber = d.ApprovalNumber;
                dm.BillFeeAmount = d.BillFeeAmount;
                dm.DeclarationFeeAmount = d.DeclarationFeeAmount;
                dm.DeclarationFeeCost = d.DeclarationFeeCost;
                dm.CheckFeeAmount = d.CheckFeeAmount;
                dm.CheckFeeCost = d.CheckFeeCost;
                dm.ExaminationFeeAmount = d.ExaminationFeeAmount;
                dm.ExaminationFeeCost = d.ExaminationFeeCost;
                dm.CommissionFeeAmount = d.CommissionFeeAmount;
                dm.CommissionFeeCost = d.CommissionFeeCost;
                //dm.RelatedSystemNumber = d.RelatedSystemNumber;
                dm.Remark = d.Remark;
                dm.FinancialRemark = d.FinancialRemark;
                //gdDeclaration.Items.Add(dm);
                lstSource.Add(dm);
            }
            gdDeclaration.ItemsSource = null;
            gdDeclaration.ItemsSource = lstSource;
        }
    }
}
