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
using ProTemplate.Utility;
using System.ServiceModel.DomainServices.Client;
using ProTemplate.Models;

namespace ProTemplate.UserControls.RadWindows
{
    public partial class BatchAddDeclarationForm : RadWindow
    {
        List<string> lstTraderCode = new List<string>();
        public BatchAddDeclarationForm()
        {
            InitializeComponent();
            SystemConfiguration.Instance.DataContext.Corporations.Clear();
            SystemConfiguration.Instance.DataContext.Load(SystemConfiguration.Instance.DataContext.GetCorporationQuery(), delegate(LoadOperation<Web.Corporation> lp)
            {
                foreach (var q in lp.Entities)
                {
                    lstTraderCode.Add(q.CorporationCode);
                }
            }, null);
        }

        private void EditWindowToolBar_SaveAndClose(object sender, EventArgs e)
        {
            List<BatchAddDeclarationItem> items = gdDeclaration.ItemsSource as List<BatchAddDeclarationItem>;
            if (items == null || items.Count == 0)
                return;
            bool isSaved = false;
            foreach (var a in items)
            {
                if (a.CustomerNameID == 0 || string.IsNullOrEmpty(a.CustomerName))
                    continue;
                Web.Declaration declaration = new Web.Declaration();
                declaration.DeclarationStatus = "正在审理中";
                declaration.TraderCode = a.TraderCode;
                declaration.CustomerID = a.CustomerNameID;
                declaration.ReceivedDate = a.ReceivedDate;
                declaration.DeclarationNumber = a.DeclarationNumber;
                declaration.BillNumber = a.BillNumber;
                declaration.ApprovalNumber = a.ApprovalNumber;
                declaration.RelatedSystemNumber = a.RelatedSystemNumber;
                declaration.Remark = a.Remark;
                declaration.DeclarationDate = DateTime.Now;
                SystemConfiguration.Instance.DataContext.Declarations.Add(declaration);
                isSaved = true;
            }
            // 有数据添加，提交更新
            if (isSaved)
            {
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
                        CommonUIFunction.ShowMessageBox("保存成功！");
                        this.Close();
                    }
                }, null);
            }
            else
            {
                CommonUIFunction.ShowMessageBox("没有数据被保存！");
            }
        }

        private void EditWindowToolBar_Close(object sender, EventArgs e)
        {
            this.Close();
        }

        private void RadWindow_Loaded(object sender, RoutedEventArgs e)
        {
            SystemConfiguration.Instance.DataContext.Declarations.Clear();
            List<BatchAddDeclarationItem> lst = new List<BatchAddDeclarationItem>();
            for (int i = 0; i < 20; i++)
            {
                BatchAddDeclarationItem b = new BatchAddDeclarationItem();
                b.Index = i + 1;
                b.ReceivedDate = DateTime.Now;
                lst.Add(b);
            }
            gdDeclaration.ItemsSource = lst;
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

        private void acbCustomer_DropDownClosed(object sender, RoutedPropertyChangedEventArgs<bool> e)
        {
            AutoCompleteBox box = sender as AutoCompleteBox;
            if (box != null)
            {
                BatchAddDeclarationItem item = box.DataContext as BatchAddDeclarationItem;
                if (item != null && box.SelectedItem != null)
                    item.CustomerNameID = ((ProTemplate.Models.CustomerDataModel)box.SelectedItem).ID;
            }
        }

        private void toolBar_SaveAndNew(object sender, EventArgs e)
        {
            foreach (var declaration in gdDeclaration.Items)
            {
                BatchAddDeclarationItem item = declaration as BatchAddDeclarationItem;
                if (lstTraderCode.Contains(item.TraderCode))
                {
                    item.IsALevel = true;
                }
                else
                {
                    item.IsALevel = false;
                }
            }
        }
    }


    public class BatchAddDeclarationItem : DataModel
    {
        public int Index { get; set; }
        public int CustomerNameID { get; set; }
        public string CustomerName { get; set; }
        public DateTime ReceivedDate { get; set; }
        public string DeclarationNumber { get; set; }
        public string BillNumber { get; set; }
        public string ApprovalNumber { get; set; }
        public string RelatedSystemNumber { get; set; }
        public string Remark { get; set; }
        public string TraderCode { get; set; }
        //public bool IsALevel { get; set; }
        // Pinyin
        private bool _isALevel;
        public bool IsALevel
        {
            get
            {
                return _isALevel;
            }
            set
            {
                _isALevel = value;
                NotifyPropertyChanged("IsALevel");
            }
        }
        ////报关费
        //public string DeclarationFeeAmount { get; set; }
        ////商检费
        //public string ExaminationFeeAmount { get; set; }
        ////商检费成本
        //public string ExaminationFeeCost { get; set; }
        ////查验费
        //public string CheckFeeAmount { get; set; }
        ////查验费成本
        //public string CheckFeeCost { get; set; }
        ////代办费
        //public string CommissionFeeAmount { get; set; }
        ////代办费成本
        //public string CommissionFeeCost { get; set; }
        ////财务情况
        //public string FinancialRemark { get; set; }
    }
}
