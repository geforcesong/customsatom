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
using System.Windows.Data;
using System.ServiceModel.DomainServices.Client;

namespace ProTemplate.UserControls.RadWindows
{
    public partial class SetDeclarationStatus : RadWindow
    {
        List<Web.Declaration> lstDeclaration = new List<Web.Declaration>();
        public SetDeclarationStatus(int[] declaratinIDs)
        {
            InitializeComponent();
            this.Header = "设置报关状态";
            InitializeDataGrid(declaratinIDs);
        }

        private void InitializeDataGrid(int[] declarationIDs)
        {
            gdCustomAll.Items.Clear();
            List<SetDeclarationStatusDataModel> lst = new List<SetDeclarationStatusDataModel>();
            foreach (int id in declarationIDs)
            {
                SystemConfiguration.Instance.DataContext.Load(SystemConfiguration.Instance.DataContext.GetDeclarationByIDQuery(id), (lo) => 
                {
                    if (lo.HasError)
                    {
                        CommonUIFunction.ShowMessageBox(lo.Error.Message);
                        return;
                    }
                    else
                    {
                        var c = lo.Entities.FirstOrDefault();
                        if (c!=null)
                        {
                            lstDeclaration.Add(c);
                            SetDeclarationStatusDataModel dataModel = new SetDeclarationStatusDataModel();
                            dataModel.ApprovalNumber = c.ApprovalNumber;
                            dataModel.BillNumber = c.BillNumber;
                            dataModel.Conveyance = c.Conveyance;
                            dataModel.DeclarationStatus = c.DeclarationStatus;
                            dataModel.DeclarationNumber = c.DeclarationNumber;
                            dataModel.ID = c.ID;
                            dataModel.TraderName = c.TraderName;
                            dataModel.VoyageNumber = c.VoyageNumber;
                            dataModel.Index = gdCustomAll.Items.Count + 1;

                            gdCustomAll.Items.Insert(0, dataModel);
                        }
                    }
                }, null);
            }

        }

        private void tbDeclarationNumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tbDeclarationNumber.Text.Length == 18)
            {
                SystemConfiguration.Instance.DataContext.Load(SystemConfiguration.Instance.DataContext.GetAllDeclarationByDeclarationCodeResultsFuncQuery(SystemConfiguration.Instance.LoggedOnUser.ID, tbDeclarationNumber.Text), (lo) =>
                {
                    if (lo.HasError)
                    {
                        CommonUIFunction.ShowMessageBox(lo.Error.Message);
                        lo.MarkErrorAsHandled();
                        return;
                    }
                    else
                    {
                        var c = lo.Entities.FirstOrDefault();
                        if (c != null)
                        {
                            bool isExisted = false;
                            foreach (var d in gdCustomAll.Items)
                            {
                                SetDeclarationStatusDataModel a = d as SetDeclarationStatusDataModel;
                                if (a.ID == c.ID)
                                {
                                    isExisted = true;
                                    return;
                                }
                            }
                            if (!isExisted)
                            {
                                SetDeclarationStatusDataModel dataModel = new SetDeclarationStatusDataModel();
                                dataModel.ApprovalNumber = c.ApprovalNumber;
                                dataModel.BillNumber = c.BillNumber;
                                dataModel.Conveyance = c.Conveyance;
                                dataModel.DeclarationNumber = c.DeclarationNumber;
                                dataModel.ID = c.ID;
                                dataModel.TraderName = c.TraderName;
                                dataModel.VoyageNumber = c.VoyageNumber;
                                dataModel.DeclarationStatus = c.DeclarationStatus;
                                dataModel.Index = gdCustomAll.Items.Count + 1;

                                gdCustomAll.Items.Insert(0, dataModel);
                            }
                            tbDeclarationNumber.Text = "";
                        }
                    }
                }, null);
            }
        }
        private void StatusButton_Click(object sender, RoutedEventArgs e)
        {
            string DeclarationStatus = ((Telerik.Windows.Controls.RadButton)sender).Tag.ToString();
            string[] ids = new string[gdCustomAll.Items.Count];
            for (int i = 0; i < ids.Length; i++)
                ids[i] = ((SetDeclarationStatusDataModel)gdCustomAll.Items[i]).ID.ToString();

            if (CommonUIFunction.ShowConfirm("是否将选中记录的报关状态设置为" + DeclarationStatus) != MessageBoxResult.OK)
                return;

            CommonUIFunction.SetApplcationBusyIndicator(true, "正在更新，请稍后");
            SystemConfiguration.Instance.DataContext.UpdateDeclarationStatus(string.Join(",", ids), DeclarationStatus, string.Format("最后更新：{0}", SystemConfiguration.Instance.LoggedOnUser.Name)
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
                        for (int i = 0; i < gdCustomAll.Items.Count; i++)
                        {

                            ((SetDeclarationStatusDataModel)gdCustomAll.Items[i]).DeclarationStatus = DeclarationStatus;
                            gdCustomAll.Rebind();
                        }
                    }
                }, null);
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            if (gdCustomAll.SelectedItem != null)
            {
                gdCustomAll.Items.Remove(gdCustomAll.SelectedItem);
            }
        }
    }
}
