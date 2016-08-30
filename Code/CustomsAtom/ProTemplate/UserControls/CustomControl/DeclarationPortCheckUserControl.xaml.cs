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
using ProTemplate.Utility;
using ProTemplate.ViewModels;
using ProTemplate.Models;
using System.ServiceModel.DomainServices.Client;
using ProTemplate.UserControls.RadWindows;

namespace ProTemplate.UserControls.CustomControl
{
    public partial class DeclarationPortCheckUserControl : UserControl
    {
        public DeclarationPortCheckUserControl()
        {
            InitializeComponent();
        }

        #region 输入更新
        private void tbInputNumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tbInputNumber.Text.Length == 18)
            {
                DeclarationPortCheckViewModel vm = ViewModelManager.DeclarationPortCheckViewModelInstance;
                if (vm == null)
                    return;
                var hasQuery = from q in vm.Items
                               where q.DeclarationNumber == tbInputNumber.Text
                               select q;
                if (hasQuery.Count() > 0)
                {
                    tbInputNumber.Text = "";
                    return;
                }

                SystemConfiguration.Instance.DataContext.Declarations.Clear();
                busyIndicator.IsBusy = true;
                SystemConfiguration.Instance.DataContext.Load(SystemConfiguration.Instance.DataContext.GetDeclarationPortCheckByDeclarationNumberQuery(tbInputNumber.Text), lp =>
                {
                    busyIndicator.IsBusy = false;
                    if (lp.HasError)
                    {
                        lp.MarkErrorAsHandled();
                        MessageBox.Show("获取系统信息出错，请重试！");
                    }
                    else if (lp.Entities.Count() > 0)
                    {
                        var data = lp.Entities.ElementAt(0);
                        UpdateDeclarationPortCheckViewModel(data);
                    }
                    tbInputNumber.Text = "";
                }, null);
            }
        }

        private void UpdateDeclarationPortCheckViewModel(Web.Declaration data)
        {
            DeclarationPortCheckViewModel vm = ViewModelManager.DeclarationPortCheckViewModelInstance;
            if (data == null || vm == null)
                return;
            else
            {
                DeclarationPortCheckDataModel dm = new DeclarationPortCheckDataModel();
                dm.ID = data.ID;
                dm.CustomerName = data.Customer.Name;
                dm.BillNumber = data.BillNumber;
                dm.ApprovalNumber = data.ApprovalNumber;
                dm.DeclarationNumber = data.DeclarationNumber;
                dm.VoyageNumber = data.VoyageNumber;
                // 港区
                dm.Dock = data.Dock;
                if (VerifyDock(data.DeclarationNumber, dm.Dock))
                    dm.VerifyDock = "校验成功";
                else
                    dm.VerifyDock = "校验失败";
                //预配仓单
                dm.PrerecordWarehouseWarrant = data.PrerecordWarehouseWarrant;

                //箱号,用逗号分隔
                if (data.DeclarationContainer != null)
                {
                    dm.BoxNumber = string.Join(",", (from a in data.DeclarationContainer
                                                     select a.Number).OrderBy(a=>a).ToArray());
                }
                // 件数
                dm.PackageNumber = string.Format("{0}", data.PackageAmount ?? 0);
                dm.GrossWeight = string.Format("{0:0}", data.GrossWeight ?? 0);
                dm.Conveyance = data.Conveyance;
                // 箱量
                dm.BoxCount = data.DeclarationContainer == null ? "0" : data.DeclarationContainer.Count.ToString();
                vm.Items.Add(dm);
                vm.UpdateIndex();
            }
        }
        #endregion

        #region Button Clicks
        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            string declarationNumbers = GetSelectedDeclarationNumbers();
            if (string.IsNullOrEmpty(declarationNumbers))
                return;
            SystemConfiguration.Instance.DataContext.Declarations.Clear();
            busyIndicator.IsBusy = true;
            SystemConfiguration.Instance.DataContext.Load(SystemConfiguration.Instance.DataContext.RefreshDeclarationPortCheckQuery(declarationNumbers), lp =>
            {
                busyIndicator.IsBusy = false;
                if (lp.HasError)
                {
                    lp.MarkErrorAsHandled();
                    MessageBox.Show("获取系统信息出错，请重试！");
                }
                else
                {
                    DeclarationPortCheckViewModel vm = ViewModelManager.DeclarationPortCheckViewModelInstance;
                    foreach (var declaration in lp.Entities)
                    {
                        var dm = (from d in vm.Items
                                  where d.DeclarationNumber == declaration.DeclarationNumber
                                  select d).FirstOrDefault();
                        if (dm != null)
                        {
                            dm.CustomerName = declaration.Customer.Name;
                            dm.BillNumber = declaration.BillNumber;
                            dm.DeclarationNumber = declaration.DeclarationNumber;
                            dm.VoyageNumber = declaration.VoyageNumber;
                            // 港区
                            dm.Dock = declaration.Dock;
                            if (VerifyDock(declaration.DeclarationNumber, dm.Dock))
                                dm.VerifyDock = "校验成功";
                            else
                                dm.VerifyDock = "校验失败";
                            dm.PrerecordWarehouseWarrant = declaration.PrerecordWarehouseWarrant;
                            //箱号,用逗号分隔
                            if (declaration.DeclarationContainer != null)
                            {
                                dm.BoxNumber = string.Join(",", (from a in declaration.DeclarationContainer
                                                                 select a.Number).OrderBy(a=>a).ToArray());
                            }
                            // 件数
                            dm.PackageNumber = string.Format("{0}", declaration.PackageAmount ?? 0);
                            dm.GrossWeight = string.Format("{0:0}", declaration.GrossWeight ?? 0);
                            dm.Conveyance = declaration.Conveyance;
                            // 箱量
                            dm.BoxCount = declaration.DeclarationContainer == null ? "0" : declaration.DeclarationContainer.Count.ToString();
                        }
                    }
                }
            }, null);
        }

        private void btnInport_Click(object sender, RoutedEventArgs e)
        {
            string declarationNumbers = GetSelectedDeclarationNumbers();
            if (string.IsNullOrEmpty(declarationNumbers))
                return;
            SystemConfiguration.Instance.DataContext.DeclarationPortCheckResults.Clear();
            busyIndicator.IsBusy = true;
            SystemConfiguration.Instance.DataContext.Load(SystemConfiguration.Instance.DataContext.CheckLandingResultsQuery(declarationNumbers), lp =>
            {
                busyIndicator.IsBusy = false;
                if (lp.HasError)
                {
                    lp.MarkErrorAsHandled();
                    MessageBox.Show("获取系统信息出错，请重试！");
                }
                else
                {
                    DeclarationPortCheckViewModel vm = ViewModelManager.DeclarationPortCheckViewModelInstance;
                    foreach (var el in lp.Entities)
                    {
                        var dm = (from d in vm.Items
                                  where d.DeclarationNumber == el.DeclarationNumber
                                  select d).SingleOrDefault();
                        if (dm != null)
                        {

                            dm.NetConveyance = el.ConveyanceOnline;
                            dm.NetVoyageNumber = el.VoyageNumberOnline;
                            dm.NetGrossWeight = el.GrossWeightOnline;
                            dm.NetPackageNumber = el.PackageAmountOnline;
                            dm.NetBoxCount = el.OnlineContainerCount.ToString();
                            if (!string.IsNullOrEmpty(el.OnlineContainerNumber))
                                dm.NetBoxNumber = string.Join(",", el.OnlineContainerNumber.Split(',').OrderBy(p=>p));
                            else
                                dm.NetBoxNumber = "";
                            dm.UpdateVerifyStatus();
                        }
                    }
                    SystemConfiguration.Instance.DataContext.DeclarationPortCheckResults.Clear();
                }
            }, null);
        }

        private void btn2Fang_Click(object sender, RoutedEventArgs e)
        {
            string declarationNumbers = GetSelectedDeclarationNumbers();
            if (string.IsNullOrEmpty(declarationNumbers))
                return;
            SystemConfiguration.Instance.DataContext.DeclarationPortCheckResults.Clear();
            busyIndicator.IsBusy = true;
            SystemConfiguration.Instance.DataContext.Load(SystemConfiguration.Instance.DataContext.CheckContainerAdmissionStatusResultsQuery(declarationNumbers), lp =>
            {
                busyIndicator.IsBusy = false;
                if (lp.HasError)
                {
                    lp.MarkErrorAsHandled();
                    MessageBox.Show("获取系统信息出错，请重试！");
                }
                else
                {
                    DeclarationPortCheckViewModel vm = ViewModelManager.DeclarationPortCheckViewModelInstance;
                    foreach (var el in lp.Entities)
                    {
                        var dm = (from d in vm.Items
                                  where d.DeclarationNumber == el.DeclarationNumber
                                  select d).FirstOrDefault();
                        if (dm != null)
                        {
                            dm.ErFangInfomation = "";
                            if (string.IsNullOrEmpty(el.ContainerAdmissionStatus))
                                dm.ErFangInfomation = "没有放行信息";
                            else
                            {
                                string[] asInfo = el.ContainerAdmissionStatus.Split(',');
                                if (asInfo.Distinct().Count() == 1)
                                    dm.ErFangInfomation = asInfo[0];
                                else
                                    dm.ErFangInfomation = "部分放行";
                            }
                        }
                    }
                    SystemConfiguration.Instance.DataContext.DeclarationPortCheckResults.Clear();
                }
            }, null);
        }

        private void btnLeavePort_Click(object sender, RoutedEventArgs e)
        {
            string declarationNumbers = GetSelectedDeclarationNumbers();
            if (string.IsNullOrEmpty(declarationNumbers))
                return;
            SystemConfiguration.Instance.DataContext.DeclarationPortCheckResults.Clear();
            busyIndicator.IsBusy = true;
            SystemConfiguration.Instance.DataContext.Load(SystemConfiguration.Instance.DataContext.CheckLeaveDockDateResultsQuery(declarationNumbers), lp =>
            {
                busyIndicator.IsBusy = false;
                if (lp.HasError)
                {
                    lp.MarkErrorAsHandled();
                    MessageBox.Show("获取系统信息出错，请重试！");
                }
                else
                {
                    DeclarationPortCheckViewModel vm = ViewModelManager.DeclarationPortCheckViewModelInstance;
                    foreach (var el in lp.Entities)
                    {
                        var dm = (from d in vm.Items
                                  where d.DeclarationNumber == el.DeclarationNumber
                                  select d).FirstOrDefault();
                        if (dm != null)
                        {
                            dm.LeaveTime = el.LeaveDockDate;
                        }
                    }
                    SystemConfiguration.Instance.DataContext.DeclarationPortCheckResults.Clear();
                }
            }, null);
        }
        #endregion

        string GetSelectedDeclarationNumbers()
        {
            DeclarationPortCheckViewModel vm = ViewModelManager.DeclarationPortCheckViewModelInstance;
            if (vm == null || vm.Items.Count == 0)
                return null;
            else
            {
                var query = string.Join(",", (from a in vm.Items
                                              select a.DeclarationNumber));
                return query;
            }
        }

        public bool VerifyDock(string strDeclaration, string strDock)
        {
            bool bRet = false;

            if (string.IsNullOrEmpty(strDeclaration) || string.IsNullOrEmpty(strDock))
            {
                return bRet;
            }

            if (strDeclaration.Substring(9, 1) == "7" && strDock.Contains("外"))
            {
                bRet = true;
            }

            if (strDeclaration.Substring(9, 1) == "8" && strDock.Contains("洋山"))
            {
                bRet = true;
            }

            if (strDeclaration.Substring(9, 1) == "5" && !strDock.Contains("外") && !strDock.Contains("洋山"))
            {
                bRet = true;
            }

            return bRet;
        }

        public void SetInitialViewModal(IEnumerable<Web.Declaration> lst)
        {
            DeclarationPortCheckViewModel vm = ViewModelManager.DeclarationPortCheckViewModelInstance;
            if (lst == null || vm == null)
                return;
            else
            {
                vm.Items.Clear();
                foreach (var data in lst)
                {
                    DeclarationPortCheckDataModel dm = new DeclarationPortCheckDataModel();
                    dm.ID = data.ID;
                    dm.CustomerName = data.Customer.Name;
                    dm.BillNumber = data.BillNumber;
                    dm.ApprovalNumber = data.ApprovalNumber;
                    dm.DeclarationNumber = data.DeclarationNumber;
                    dm.VoyageNumber = data.VoyageNumber;
                    // 港区
                    dm.Dock = data.Dock;
                    if (VerifyDock(data.DeclarationNumber, dm.Dock))
                        dm.VerifyDock = "校验成功";
                    else
                        dm.VerifyDock = "校验失败";
                    //预配仓单
                    dm.PrerecordWarehouseWarrant = data.PrerecordWarehouseWarrant;

                    //箱号,用逗号分隔
                    if (data.DeclarationContainer!= null && data.DeclarationContainer.Count>0)
                    {
                        string[] containers = data.DeclarationContainer.Select(o => o.Number).Distinct().ToArray();
                        dm.BoxNumber = string.Join(",", containers.OrderBy(p=>p));
                        dm.BoxCount = containers.Length.ToString();
                    }
                    else
                    {
                        dm.BoxCount = "0";
                        dm.BoxNumber = "";
                    }
                    // 件数
                    dm.PackageNumber = string.Format("{0}", data.PackageAmount ?? 0);
                    dm.GrossWeight = string.Format("{0:0}", data.GrossWeight ?? 0);
                    dm.Conveyance = data.Conveyance;
                    // 箱量
                    //dm.BoxCount = data.DeclarationContainer == null ? "0" : data.DeclarationContainer.Count.ToString();
                    vm.Items.Add(dm);
                    vm.UpdateIndex();
                }
            }
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            SystemConfiguration.Instance.DataContext.Declarations.Clear();
            DeclarationPortCheckViewModel vm = ViewModelManager.DeclarationPortCheckViewModelInstance;
            if (vm != null)
                vm.Items.Clear();
        }

        private void btnClearInput_Click(object sender, RoutedEventArgs e)
        {
            SystemConfiguration.Instance.DataContext.Declarations.Clear();
            DeclarationPortCheckViewModel vm = ViewModelManager.DeclarationPortCheckViewModelInstance;
            if (vm != null)
            {
                List<DeclarationPortCheckDataModel> lst = (from c in vm.Items where c.CheckedItem == true select c).ToList();
                if (lst != null && lst.Count > 0)
                {
                    foreach (var a in lst)
                    {
                        vm.Items.Remove(a);
                    }
                    vm.UpdateIndex();
                }
            }
        }

        private void btnSetDeclarationStatus_Click(object sender, RoutedEventArgs e)
        {
            DeclarationPortCheckViewModel vm = ViewModelManager.DeclarationPortCheckViewModelInstance;
            if (vm != null)
            {
                List<DeclarationPortCheckDataModel> lst = (from c in vm.Items where c.CheckedItem == true select c).ToList();

                int[] ids = (from c in lst select c.ID).ToArray();

                SetDeclarationStatus form = new SetDeclarationStatus(ids);
                form.ShowDialog();
            }
        }

        private void dgData_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.MouseLeftButtonUp += (a, b) =>
                {
                    TimeSpan t = DateTime.Now.TimeOfDay;
                    if (dgData.Tag != null)
                    {
                        TimeSpan oldT = (TimeSpan)dgData.Tag;
                        if ((t - oldT) < TimeSpan.FromMilliseconds(300))
                        {
                            DeclarationPortCheckDataModel data = e.Row.DataContext as DeclarationPortCheckDataModel;
                            if (data != null)
                                Clipboard.SetText(data.NetBoxNumber);
                        }
                    }
                    dgData.Tag = t;
                };
        }

        private void cbSelectAll_Click(object sender, RoutedEventArgs e)
        {
            CheckBox cb = e.OriginalSource as CheckBox;
            if (cb.IsChecked.HasValue)
            {
                DeclarationPortCheckViewModel vm = ViewModelManager.DeclarationPortCheckViewModelInstance;
                foreach (var a in vm.Items)
                {
                    a.CheckedItem = cb.IsChecked.Value;
                }
            }
        }

        private void dgData_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgData.SelectedItem != null)
            {
                DeclarationPortCheckDataModel model = (DeclarationPortCheckDataModel)dgData.SelectedItem;
                tbNetContainerNumber.Text = model.NetBoxNumber;
            }
        }
    }
}
