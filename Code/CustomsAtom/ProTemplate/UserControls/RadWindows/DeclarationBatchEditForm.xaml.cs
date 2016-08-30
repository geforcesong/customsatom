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
using ProTemplate.Utility;
using System.ServiceModel.DomainServices.Client;
using ProTemplate.Models;
using ProTemplate.ViewModels;

namespace ProTemplate.UserControls.RadWindows
{
    /// <summary>
    /// Interaction logic for DeclarationBatchEditForm.xaml
    /// </summary>
    public partial class DeclarationBatchEditForm
    {
        public DeclarationBatchEditForm()
        {
            InitializeComponent();

        }
        public List<GetAllDeclarationByReceiveDateResultDataModel> Declarations { get; set; }

        private void RadWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Load(SystemConfiguration.Instance.LoggedOnUser.ID);
        }
        private void EditWindowToolBar_SaveAndClose(object sender, EventArgs e)
        {
            foreach (GetAllDeclarationByReceiveDateResultDataModel dm in gdDeclaration.Items)
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
                    declaration.RelatedSystemNumber = dm.RelatedSystemNumber;
                    


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
                    GetAllDeclarationByReceiveDateResultViewModel vm = App.Current.Resources["GetAllDeclarationByReceiveDateResultViewModel"] as GetAllDeclarationByReceiveDateResultViewModel;
                    foreach (GetAllDeclarationByReceiveDateResultDataModel dm in gdDeclaration.Items)
                    {
                        var declaration = (from c in vm.Items where c.ID == dm.ID select c).FirstOrDefault();
                        if (declaration != null)
                        {
                            //有问题，为什么赋值赋不进去
                            declaration.IsLocked = false;
                            declaration.BillNumber = dm.BillNumber;
                            declaration.ApprovalNumber = dm.ApprovalNumber;
                            declaration.DeclarationNumber = dm.DeclarationNumber;
                            declaration.RelatedSystemNumber = dm.RelatedSystemNumber;
                            declaration.Remark = dm.Remark;
                            declaration.CustomerName = dm.CustomerName;
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
            
            SystemConfiguration.Instance.DataContext.Load(SystemConfiguration.Instance.DataContext.GetDeclarationByIDsQuery(Declarations.Select(o => o.ID)), (p) => { 
                List<GetAllDeclarationByReceiveDateResultDataModel> lstSource = new List<GetAllDeclarationByReceiveDateResultDataModel>();
                foreach (GetAllDeclarationByReceiveDateResultDataModel d in Declarations)
                {
                    GetAllDeclarationByReceiveDateResultDataModel dm = new GetAllDeclarationByReceiveDateResultDataModel();
                    var declarationTemp = (from c in SystemConfiguration.Instance.DataContext.Declarations where c.ID == d.ID select c).FirstOrDefault();
                    dm.Index = lstSource.Count + 1;
                    dm.ID = d.ID;
                    dm.CustomerName = d.CustomerName;
                    dm.BillNumber = declarationTemp == null ? "" : declarationTemp.BillNumber; 
                    dm.DeclarationNumber = d.DeclarationNumber;
                    dm.ApprovalNumber = d.ApprovalNumber;
                    dm.RelatedSystemNumber = declarationTemp == null ? "" : declarationTemp.RelatedSystemNumber;
                    dm.Remark = declarationTemp == null ? "" : declarationTemp.Remark;

                    //gdDeclaration.Items.Add(dm);
                    lstSource.Add(dm);

                    //SystemConfiguration.Instance.DataContext.Load(SystemConfiguration.Instance.DataContext.GetDeclarationByIDQuery(d.ID));

                    gdDeclaration.ItemsSource = null;
                    gdDeclaration.ItemsSource = lstSource;
                    busyIndicator.IsBusy = false;
                }

            }, null);

        }

    }
}
