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
using ProTemplate.ViewModels;
using ProTemplate.Models;
using Telerik.Windows.Controls.GridView;
using Telerik.Windows;
using ProTemplate.UserControls.RadWindows;

namespace ProTemplate.Views
{
    public partial class Varify : Page
    {
        public Varify()
        {
            InitializeComponent();
            gdBoss.AddHandler(GridViewCellBase.CellDoubleClickEvent, new EventHandler<RadRoutedEventArgs>(OnCellDoubleClick), true);
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            //SystemConfiguration.Instance.DataContext.DoubleCheckDeclarations.Clear();
            //SystemConfiguration.Instance.DataContext.DoubleCheckDeclarationItems.Clear();
            SystemConfiguration.Instance.DataContext.DoubleCheckDeclarationVarifyResults.Clear();
            DoubleCheckDeclarationVarifyViewModel vmcheck = ViewModelManager.DoubleCheckDeclarationVarifyViewModelInstance;
            if (vmcheck != null)
                vmcheck.Items.Clear();
        }

        private void OnCellDoubleClick(object sender, RadRoutedEventArgs args)
        {
            GridViewCellBase cell = args.OriginalSource as GridViewCellBase;
            TextBlock tb = cell.Content as TextBlock;
            if (tb != null)
            {
                DoubleCheckDeclarationVarifyDataModel dm = cell.DataContext as DoubleCheckDeclarationVarifyDataModel;
                if (dm != null)
                {
                    if (!dm.VarifyMsg.Contains("。") || dm.VarifyFlag == "成功")
                    {
                        CommonUIFunction.ShowMessageBox(dm.VarifyMsg);
                        return;
                    }
                    else
                    {
                        VerifyDetailInfoWindow vdw = new VerifyDetailInfoWindow(dm.VarifyMsg);
                        vdw.ShowDialog();
                    }
                }
            }
        }

        private void tbNumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tbNumber.Text.Length == 18)
            {
                DoubleCheckDeclarationVarifyViewModel vmcheck = ViewModelManager.DoubleCheckDeclarationVarifyViewModelInstance;
                if (vmcheck != null)
                {
                    var hasQuery = from q in vmcheck.Items
                                   where q.DeclarationNumber == tbNumber.Text
                                   select q;
                    if (hasQuery.Count() > 0)
                    {
                        tbNumber.Text = "";
                        return;
                    }
                }

                //CommonUIFunction.SetApplcationBusyIndicator(true, "正在获取数据，请稍后...");
                SystemConfiguration.Instance.DataContext.Load(SystemConfiguration.Instance.DataContext.GetDoubleCheckDeclarationByDelarationNumberQuery(tbNumber.Text), lp =>
                {
                    //CommonUIFunction.SetApplcationBusyIndicator(false);
                    if (lp.HasError)
                    {
                        lp.MarkErrorAsHandled();
                        MessageBox.Show("获取系统信息出错，请重试！");
                    }
                    else
                    {
                        DoubleCheckDeclarationVarifyViewModel vm = ViewModelManager.DoubleCheckDeclarationVarifyViewModelInstance;
                        if (lp.Entities.Count() > 0 && vm != null)
                        {
                            var ele = lp.Entities.First();
                            DoubleCheckDeclarationVarifyDataModel dm = new DoubleCheckDeclarationVarifyDataModel();

                            dm.ID = ele.ID;
                            dm.DeclarationId = ele.DeclarationId;
                            dm.CustomerName = ele.CustomerName;
                            dm.DeclarationNumber = ele.DeclarationNumber;
                            dm.ApprovalNumber = ele.ApprovalNumber;
                            dm.TransactionName = ele.TransactionName;
                            dm.PrimaryColumn = "DeclarationNumber";
                            dm.VarifyFlag = "未校验";
                            vm.Items.Add(dm);
                            vm.UpdateIndex();
                        }
                    }
                    tbNumber.Text = "";
                }, null);

            }
        }

        //private void tbApproveNumber_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    if (tbApproveNumber.Text.Length == 9)
        //    {
        //        DoubleCheckDeclarationVarifyViewModel vmcheck = ViewModelManager.DoubleCheckDeclarationVarifyViewModelInstance;
        //        if (vmcheck != null)
        //        {
        //            var hasQuery = from q in vmcheck.Items
        //                           where q.ApprovalNumber == tbApproveNumber.Text
        //                           select q;
        //            if (hasQuery.Count() > 0)
        //            {
        //                tbApproveNumber.Text = "";
        //                return;
        //            }
        //        }

        //        //CommonUIFunction.SetApplcationBusyIndicator(true, "正在获取数据，请稍后...");
        //        SystemConfiguration.Instance.DataContext.Load(SystemConfiguration.Instance.DataContext.GetDoubleCheckDeclarationByApproveNumberQuery(tbApproveNumber.Text), lp =>
        //        {
        //            //CommonUIFunction.SetApplcationBusyIndicator(false);
        //            if (lp.HasError)
        //            {
        //                lp.MarkErrorAsHandled();
        //                MessageBox.Show("获取系统信息出错，请重试！");
        //            }
        //            else
        //            {
        //                DoubleCheckDeclarationVarifyViewModel vm = ViewModelManager.DoubleCheckDeclarationVarifyViewModelInstance;
        //                if (lp.Entities.Count() > 0 && vm != null)
        //                {
        //                    var ele = lp.Entities.First();
        //                    DoubleCheckDeclarationVarifyDataModel dm = new DoubleCheckDeclarationVarifyDataModel();

        //                    dm.ID = ele.ID;
        //                    dm.DeclarationId = ele.DeclarationId;
        //                    dm.CustomerName = ele.CustomerName;
        //                    dm.DeclarationNumber = ele.DeclarationNumber;
        //                    dm.ApprovalNumber = ele.ApprovalNumber;
        //                    dm.TransactionName = ele.TransactionName;
        //                    dm.PrimaryColumn = "ApprovalNumber";
        //                    dm.VarifyFlag = "未校验";
        //                    vm.Items.Add(dm);
        //                    vm.UpdateIndex();
        //                }
        //            }
        //            tbApproveNumber.Text = "";
        //        }, null);

        //    }
        //}

        private void btnVarify_Click(object sender, RoutedEventArgs e)
        {
            DoubleCheckDeclarationVarifyViewModel vm = ViewModelManager.DoubleCheckDeclarationVarifyViewModelInstance;
            if (vm == null || vm.Items.Count == 0)
                return;

            if (cbMachines.SelectedItem == null)
            {
                MessageBox.Show("请选择打单机器。");
                return;
            }

            MachineDataModel machine = (MachineDataModel)cbMachines.SelectedItem;

            List<int> lstIds = (from a in vm.Items select a.DeclarationId).ToList();

            //保存打单机器
            SystemConfiguration.Instance.DataContext.SetDoubleCheckMachineByIds(lstIds, machine.MachineName, lp =>
            {

                SystemConfiguration.Instance.DataContext.DoubleCheckDeclarationVarifyResults.Clear();

                var query = (from a in vm.Items
                             select string.Format("{0}:{1}:{2}", a.ID, a.DeclarationNumber, a.ApprovalNumber)).ToArray();

                //CommonUIFunction.SetApplcationBusyIndicator(true, "正在验证，请稍后...");
                SystemConfiguration.Instance.DataContext.Load(SystemConfiguration.Instance.DataContext.DoubleCheckDeclarationVarifyResultsQuery(string.Join(",", query)), lp1 =>
                {
                    //CommonUIFunction.SetApplcationBusyIndicator(false);
                    if (lp1.HasError)
                    {
                        lp1.MarkErrorAsHandled();
                        MessageBox.Show("获取系统信息出错，请重试！");
                    }
                    else
                    {
                        foreach (var item in lp1.Entities)
                        {
                            var vmItem = (from q in vm.Items
                                          where q.ID == item.ID
                                          select q).FirstOrDefault();
                            if (vmItem != null)
                            {
                                vmItem.VarifyFlag = item.VarifyFlag ? "成功" : "失败";
                                vmItem.VarifyMsg = item.VarifyMsg;
                            }
                        }
                    }
                }, null);
            }, null);
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            DoubleCheckDeclarationVarifyViewModel vm = ViewModelManager.DoubleCheckDeclarationVarifyViewModelInstance;
            vm.Items.Clear();
        }
    }
}
