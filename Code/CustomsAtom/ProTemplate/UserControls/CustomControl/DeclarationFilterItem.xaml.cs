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
using System.ServiceModel.DomainServices.Client;
using ProTemplate.Utility;
using ProTemplate.Web;

namespace ProTemplate.UserControls.CustomControl
{
    public partial class DeclarationFilterItem : UserControl
    {
        public DeclarationFilterItem()
        {
            InitializeComponent();

            //cbbContentBoss.Items.Clear();
            if (cbbContentBoss.Items.Count == 0)
            {
                cbbContentBoss.DisplayMemberPath = "Name";
                cbbContentBoss.ItemsSource = SystemConfiguration.Instance.DataContext.Bosses.OrderBy(o => o.Name);
            }
            //cbbContentCustomer.Items.Clear();
            if (cbbContentCustomer.Items.Count == 0)
            {
                cbbContentCustomer.DisplayMemberPath = "Name";
                cbbContentCustomer.ItemsSource = SystemConfiguration.Instance.DataContext.Customers.OrderBy(o => o.Name);
            }
            //InitialFilterCondition();
        }

        public void SetDefault(string value)
        {
            foreach (var item in cbbCondition.Items)
            {
                if (((ConditionContent)item).ValueField == value)
                {
                    cbbCondition.SelectedItem = item;
                    break;
                }
            }
        }

        public void InitialFilterCondition()
        {
            dpStart.SelectedDate = DateTime.Today;
            dpEnd.SelectedDate = DateTime.Today.AddDays(1);
            cbbCondition.Items.Clear();

            //All 
            ConditionContent conditionAll = new ConditionContent();
            conditionAll.DisplayField = "(全部)";
            conditionAll.ValueField = "";
            cbbCondition.Items.Add(conditionAll);
            //DeclarationNumber 
            ConditionContent conditionDeclarationNumber = new ConditionContent();
            conditionDeclarationNumber.DisplayField = "海关编号";
            conditionDeclarationNumber.ValueField = "DeclarationNumber";
            cbbCondition.Items.Add(conditionDeclarationNumber);
            //ApprovalNumber 
            ConditionContent conditionApprovalNumber = new ConditionContent();
            conditionApprovalNumber.DisplayField = "批准文号";
            conditionApprovalNumber.ValueField = "ApprovalNumber";
            cbbCondition.Items.Add(conditionApprovalNumber);
            //Customer 
            ConditionContent conditionCustomer = new ConditionContent();
            conditionCustomer.DisplayField = "客户名称";
            conditionCustomer.ValueField = "Customer";
            cbbCondition.Items.Add(conditionCustomer);
            //Customer 
            ConditionContent conditionCustomerText = new ConditionContent();
            conditionCustomerText.DisplayField = "客户名称(文本)";
            conditionCustomerText.ValueField = "CustomerText";
            cbbCondition.Items.Add(conditionCustomerText);
            //ReceivedDate 
            ConditionContent conditionReceivedDate = new ConditionContent();
            conditionReceivedDate.DisplayField = "接收日期";
            conditionReceivedDate.ValueField = "ReceivedDate";
            cbbCondition.Items.Add(conditionReceivedDate);
            //DeclarationDate 
            ConditionContent conditionDeclarationDate = new ConditionContent();
            conditionDeclarationDate.DisplayField = "报关日期";
            conditionDeclarationDate.ValueField = "DeclarationDate";
            cbbCondition.Items.Add(conditionDeclarationDate);
            //ManualNumber 
            ConditionContent conditionManualNumber = new ConditionContent();
            conditionManualNumber.DisplayField = "备案号";
            conditionManualNumber.ValueField = "ManualNumber";
            cbbCondition.Items.Add(conditionManualNumber);
            //TraderName 
            ConditionContent conditionTraderName = new ConditionContent();
            conditionTraderName.DisplayField = "经营单位";
            conditionTraderName.ValueField = "TraderName";
            cbbCondition.Items.Add(conditionTraderName);
            //Conveyance 
            ConditionContent conditionConveyance = new ConditionContent();
            conditionConveyance.DisplayField = "运输工具";
            conditionConveyance.ValueField = "Conveyance";
            cbbCondition.Items.Add(conditionConveyance);
            //VoyageNumber 
            ConditionContent conditionVoyageNumber = new ConditionContent();
            conditionVoyageNumber.DisplayField = "航次号";
            conditionVoyageNumber.ValueField = "VoyageNumber";
            cbbCondition.Items.Add(conditionVoyageNumber);
            //BillNumber 
            ConditionContent conditionBillNumber = new ConditionContent();
            conditionBillNumber.DisplayField = "提运单号";
            conditionBillNumber.ValueField = "BillNumber";
            cbbCondition.Items.Add(conditionBillNumber);
            //BillNumber 
            ConditionContent conditionDock = new ConditionContent();
            conditionDock.DisplayField = "码头";
            conditionDock.ValueField = "Dock";
            cbbCondition.Items.Add(conditionDock);
            //DeclarationStatus 
            ConditionContent conditionDeclarationStatus = new ConditionContent();
            conditionDeclarationStatus.DisplayField = "报关状态";
            conditionDeclarationStatus.ValueField = "DeclarationStatus";
            cbbCondition.Items.Add(conditionDeclarationStatus);
            if (SystemConfiguration.Instance.LoggedOnUser.RoleList.Any(o => o.Name == "客户经理" || o.Name == "客户财务人员" || o.Name == "客户操作人员"))
            {
                //DrawbackStatus 
                ConditionContent conditionDrawbackStatus = new ConditionContent();
                conditionDrawbackStatus.DisplayField = "退税状态";
                conditionDrawbackStatus.ValueField = "DrawbackStatus";
                cbbCondition.Items.Add(conditionDrawbackStatus);
                //DrawbackStatusRemark 
                ConditionContent conditionDrawbackStatusRemark = new ConditionContent();
                conditionDrawbackStatusRemark.DisplayField = "退税情况";
                conditionDrawbackStatusRemark.ValueField = "DrawbackStatusRemark";
                cbbCondition.Items.Add(conditionDrawbackStatusRemark);
                //DrawbackDate 
                ConditionContent conditionDrawbackDate = new ConditionContent();
                conditionDrawbackDate.DisplayField = "退税日期";
                conditionDrawbackDate.ValueField = "DrawbackDate";
                cbbCondition.Items.Add(conditionDrawbackDate);
            }
            //ContainerNumber 
            ConditionContent conditionContainerNumber = new ConditionContent();
            conditionContainerNumber.DisplayField = "箱号";
            conditionContainerNumber.ValueField = "ContainerNumber";
            cbbCondition.Items.Add(conditionContainerNumber);
            //ItemName 
            ConditionContent conditionItemName = new ConditionContent();
            conditionItemName.DisplayField = "品名";
            conditionItemName.ValueField = "ItemName";
            cbbCondition.Items.Add(conditionItemName);
            //HasExamination 
            ConditionContent conditionHasExamination = new ConditionContent();
            conditionHasExamination.DisplayField = "商检";
            conditionHasExamination.ValueField = "HasExamination";
            cbbCondition.Items.Add(conditionHasExamination);
            //ExaminationNumber 
            ConditionContent conditionExaminationNumber = new ConditionContent();
            conditionExaminationNumber.DisplayField = "商检编号";
            conditionExaminationNumber.ValueField = "ExaminationNumber";
            cbbCondition.Items.Add(conditionExaminationNumber);
            //RelatedSystemNumber 
            ConditionContent conditionRelatedSystemNumber = new ConditionContent();
            conditionRelatedSystemNumber.DisplayField = "运编号";
            conditionRelatedSystemNumber.ValueField = "RelatedSystemNumber";
            cbbCondition.Items.Add(conditionRelatedSystemNumber);
            //RelatedSystemNumber 
            ConditionContent conditionBossName = new ConditionContent();
            conditionBossName.DisplayField = "老板名称";
            conditionBossName.ValueField = "Boss";
            cbbCondition.Items.Add(conditionBossName);
            //Remark 
            ConditionContent conditionRemark = new ConditionContent();
            conditionRemark.DisplayField = "注释";
            conditionRemark.ValueField = "Remark";
            cbbCondition.Items.Add(conditionRemark);
            if (SystemConfiguration.Instance.LoggedOnUser.RoleList.Any(o => o.Name == "财务人员" || o.Name == "管理员" || o.Name == "出口经理"))
            {
                //FinancialFinancialRemark 
                ConditionContent conditionFinancialRemark = new ConditionContent();
                conditionFinancialRemark.DisplayField = "财务注释";
                conditionFinancialRemark.ValueField = "FinancialRemark";
                cbbCondition.Items.Add(conditionFinancialRemark);
            }
            cbbCondition.SelectedIndex = 0;

        }

        public string Query()
        {
            string conditionResult = "";
            ConditionContent condition = (ConditionContent)cbbCondition.SelectedItem;
            bool hasCondition = false;
            if (condition != null)
            {
                switch (condition.ValueField)
                {
                    case "DeclarationNumber":
                        //string strDeclarationNumberCondition = string.Empty;
                        List<string> declarationNumberList = new List<string>();
                        declarationNumberList = tbContent.Text.Split(',').ToList();
                        for (int i = 0; i < declarationNumberList.Count; i ++ )
                        {
                            if (!string.IsNullOrEmpty(declarationNumberList[i]))
                            {
                                hasCondition = true;
                                declarationNumberList[i] = " d.DeclarationNumber like '%" + declarationNumberList[i] + "%' ";
                            }
                        }
                        if (hasCondition)
                        {
                            conditionResult += " And (" + string.Join("OR", declarationNumberList) + ")";
                        }
                        break;
                    case "ApprovalNumber":
                        //string strApprovalNumberCondition = string.Empty;
                        List<string> approvalNumberList = new List<string>();
                        approvalNumberList = tbContent.Text.Split(',').ToList();
                        for (int i = 0; i < approvalNumberList.Count; i++)
                        {
                            if (!string.IsNullOrEmpty(approvalNumberList[i]))
                            {
                                hasCondition = true;
                                approvalNumberList[i] = " d.approvalNumber like '%" + approvalNumberList[i] + "%' ";
                            }
                        }
                        if (hasCondition)
                        {
                            conditionResult += " And (" + string.Join("OR", approvalNumberList) + ")";
                        }
                        break;
                    case "Customer":
                        conditionResult += " And c.Name like '%" + ((Customer)cbbContentCustomer.SelectedValue).Name + "%'";
                        break;
                    case "CustomerText":
                        conditionResult += " And c.Name like '%" + tbContent.Text + "%'";
                        break;
                    case "ReceivedDate":
                        conditionResult += " And d.ReceivedDate BETWEEN '" + dpStart.SelectedDate + "' AND '" + dpEnd.SelectedDate + "'";
                        break;
                    case "DeclarationDate":
                        conditionResult += " And d.DeclarationDate BETWEEN '" + dpStart.SelectedDate + "' AND '" + dpEnd.SelectedDate + "'";
                        break;
                    case "ManualNumber":
                        List<string> ManualNumberList = new List<string>();
                        ManualNumberList = tbContent.Text.Split(',').ToList();
                        for (int i = 0; i < ManualNumberList.Count; i++)
                        {
                            if (!string.IsNullOrEmpty(ManualNumberList[i]))
                            {
                                hasCondition = true;
                                ManualNumberList[i] = " d.ManualNumber like '%" + ManualNumberList[i] + "%' ";
                            }
                        }
                        if (hasCondition)
                        {
                            conditionResult += " And (" + string.Join("OR", ManualNumberList) + ")";
                        }
                        break;
                    case "TraderName":
                        conditionResult += " And d.TraderName like '%" + tbContent.Text + "%'";
                        break;
                    case "Conveyance":
                        conditionResult += " And d.Conveyance like '%" + tbContent.Text + "%'";
                        break;
                    case "VoyageNumber":
                        conditionResult += " And d.VoyageNumber like '%" + tbContent.Text + "%'";
                        break;
                    case "BillNumber":
                        List<string> BillNumberList = new List<string>();
                        BillNumberList = tbContent.Text.Split(',').ToList();
                        for (int i = 0; i < BillNumberList.Count; i++)
                        {
                            if (!string.IsNullOrEmpty(BillNumberList[i]))
                            {
                                hasCondition = true;
                                BillNumberList[i] = " d.BillNumber like '%" + BillNumberList[i] + "%' ";
                            }
                        }
                        if (hasCondition)
                        {
                            conditionResult += " And (" + string.Join("OR", BillNumberList) + ")";
                        }
                        break;
                    case "DeclarationStatus":
                        conditionResult += " And d.DeclarationStatus like '%" + ((ComboBoxItem)cbbContentDeclarationStatus.SelectedValue).Content + "%'";
                        break;
                    case "DrawbackStatus":
                        conditionResult += " And d.DrawbackStatus like '%" + ((ComboBoxItem)cbbContentDrawbackStatus.SelectedValue).Content + "%'";
                        break;
                    case "DrawbackStatusRemark":
                        conditionResult += " And d.DrawbackStatusRemark like '%" + tbContent.Text + "%'";
                        break;
                    case "DrawbackDate":
                        conditionResult += " And d.DrawbackDate BETWEEN '" + dpStart.SelectedDate + "' AND '" + dpEnd.SelectedDate + "'";
                        break;
                    case "HasExamination":
                        if (((ComboBoxItem)cbbContentExamination.SelectedValue).Content == "是")
                        {
                            conditionResult += " And CertificateNumber IS NOT NULL";
                        }
                        else
                        {
                            conditionResult += " And CertificateNumber IS NULL";
                        }
                        break;
                    case "RelatedSystemNumber":
                        List<string> RelatedSystemNumberList = new List<string>();
                        RelatedSystemNumberList = tbContent.Text.Split(',').ToList();
                        for (int i = 0; i < RelatedSystemNumberList.Count; i++)
                        {
                            if (!string.IsNullOrEmpty(RelatedSystemNumberList[i]))
                            {
                                hasCondition = true;
                                RelatedSystemNumberList[i] = " d.RelatedSystemNumber like '%" + RelatedSystemNumberList[i] + "%' ";
                            }
                        }
                        if (hasCondition)
                        {
                            conditionResult += " And (" + string.Join("OR", RelatedSystemNumberList) + ")";
                        }
                        break;
                    case "Remark":
                        conditionResult += " And d.Remark like '%" + tbContent.Text + "%'";
                        break;
                    case "Dock":
                        conditionResult += " And d.Dock like '%" + tbContent.Text + "%'";
                        break;
                    case "Boss":
                        conditionResult += " And b.Name like '%" + ((Boss)cbbContentBoss.SelectedValue).Name + "%'";
                        break;
                    case "FinancialRemark":
                        conditionResult += " And d.FinancialRemark like '%" + tbContent.Text + "%'";
                        break;
                    case "ContainerNumber":
                        List<string> ContainerNumberList = new List<string>();
                        ContainerNumberList = tbContent.Text.Split(',').ToList();
                        for (int i = 0; i < ContainerNumberList.Count; i++)
                        {
                            if (!string.IsNullOrEmpty(ContainerNumberList[i]))
                            {
                                hasCondition = true;
                                ContainerNumberList[i] = " dc.Number like '%" + ContainerNumberList[i] + "%' ";
                            }
                        }
                        if (hasCondition)
                        {
                            conditionResult += " And (" + string.Join("OR", ContainerNumberList) + ")";
                        }
                        break;
                    case "ItemName":
                        conditionResult += " And di.Name like '%" + tbContent.Text + "%'";
                        break;
                    case "ExaminationNumber":
                        List<string> ExaminationNumberList = new List<string>();
                        ExaminationNumberList = tbContent.Text.Split(',').ToList();
                        for (int i = 0; i < ExaminationNumberList.Count; i++)
                        {
                            if (!string.IsNullOrEmpty(ExaminationNumberList[i]))
                            {
                                hasCondition = true;
                                ExaminationNumberList[i] = " CertificateNumber like '%" + ExaminationNumberList[i] + "%' ";
                            }
                        }
                        if (hasCondition)
                        {
                            conditionResult += " And (" + string.Join("OR", ExaminationNumberList) + ")";
                        }
                        break;

                }
                                    
            }
            return conditionResult;
        }

        private void cbbCondition_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ConditionContent condition = (ConditionContent)cbbCondition.SelectedItem;
            if (condition != null)
            {
                if (condition.ValueField == "")
                {
                    dpEnd.Visibility = System.Windows.Visibility.Collapsed;
                    dpStart.Visibility = System.Windows.Visibility.Collapsed;
                    tbTo.Visibility = System.Windows.Visibility.Collapsed;
                    tbContent.Visibility = System.Windows.Visibility.Collapsed;
                    cbbContentDeclarationStatus.Visibility = System.Windows.Visibility.Collapsed;
                    cbbContentDrawbackStatus.Visibility = System.Windows.Visibility.Collapsed;
                    cbbContentExamination.Visibility = System.Windows.Visibility.Collapsed;
                    cbbContentBoss.Visibility = System.Windows.Visibility.Collapsed;
                    cbbContentCustomer.Visibility = System.Windows.Visibility.Collapsed;
                }
                else if (condition.ValueField == "ReceivedDate" || condition.ValueField == "DeclarationDate" || condition.ValueField == "DrawbackDate")
                {
                    dpEnd.Visibility = System.Windows.Visibility.Visible;
                    dpStart.Visibility = System.Windows.Visibility.Visible;
                    tbTo.Visibility = System.Windows.Visibility.Visible;
                    tbContent.Visibility = System.Windows.Visibility.Collapsed;
                    cbbContentDeclarationStatus.Visibility = System.Windows.Visibility.Collapsed;
                    cbbContentDrawbackStatus.Visibility = System.Windows.Visibility.Collapsed;
                    cbbContentExamination.Visibility = System.Windows.Visibility.Collapsed;
                    cbbContentBoss.Visibility = System.Windows.Visibility.Collapsed;
                    cbbContentCustomer.Visibility = System.Windows.Visibility.Collapsed;
                }
                else if (condition.ValueField == "DeclarationStatus")
                {
                    dpEnd.Visibility = System.Windows.Visibility.Collapsed;
                    dpStart.Visibility = System.Windows.Visibility.Collapsed;
                    tbTo.Visibility = System.Windows.Visibility.Collapsed;
                    tbContent.Visibility = System.Windows.Visibility.Collapsed;
                    cbbContentDeclarationStatus.Visibility = System.Windows.Visibility.Visible;
                    cbbContentDrawbackStatus.Visibility = System.Windows.Visibility.Collapsed;
                    cbbContentExamination.Visibility = System.Windows.Visibility.Collapsed;
                    cbbContentBoss.Visibility = System.Windows.Visibility.Collapsed;
                    cbbContentCustomer.Visibility = System.Windows.Visibility.Collapsed;
                }
                else if (condition.ValueField == "DrawbackStatus")
                {
                    dpEnd.Visibility = System.Windows.Visibility.Collapsed;
                    dpStart.Visibility = System.Windows.Visibility.Collapsed;
                    tbTo.Visibility = System.Windows.Visibility.Collapsed;
                    tbContent.Visibility = System.Windows.Visibility.Collapsed;
                    cbbContentDeclarationStatus.Visibility = System.Windows.Visibility.Collapsed;
                    cbbContentDrawbackStatus.Visibility = System.Windows.Visibility.Visible;
                    cbbContentExamination.Visibility = System.Windows.Visibility.Collapsed;
                    cbbContentBoss.Visibility = System.Windows.Visibility.Collapsed;
                    cbbContentCustomer.Visibility = System.Windows.Visibility.Collapsed;
                }
                else if (condition.ValueField == "HasExamination")
                {
                    dpEnd.Visibility = System.Windows.Visibility.Collapsed;
                    dpStart.Visibility = System.Windows.Visibility.Collapsed;
                    tbTo.Visibility = System.Windows.Visibility.Collapsed;
                    tbContent.Visibility = System.Windows.Visibility.Collapsed;
                    cbbContentDeclarationStatus.Visibility = System.Windows.Visibility.Collapsed;
                    cbbContentDrawbackStatus.Visibility = System.Windows.Visibility.Collapsed;
                    cbbContentExamination.Visibility = System.Windows.Visibility.Visible;
                    cbbContentBoss.Visibility = System.Windows.Visibility.Collapsed;
                    cbbContentCustomer.Visibility = System.Windows.Visibility.Collapsed;
                } else if (condition.ValueField == "Boss")
                {
                    dpEnd.Visibility = System.Windows.Visibility.Collapsed;
                    dpStart.Visibility = System.Windows.Visibility.Collapsed;
                    tbTo.Visibility = System.Windows.Visibility.Collapsed;
                    tbContent.Visibility = System.Windows.Visibility.Collapsed;
                    cbbContentDeclarationStatus.Visibility = System.Windows.Visibility.Collapsed;
                    cbbContentDrawbackStatus.Visibility = System.Windows.Visibility.Collapsed;
                    cbbContentExamination.Visibility = System.Windows.Visibility.Collapsed;
                    cbbContentBoss.Visibility = System.Windows.Visibility.Visible;
                    cbbContentCustomer.Visibility = System.Windows.Visibility.Collapsed;
                }
                else if (condition.ValueField == "Customer")
                {
                    dpEnd.Visibility = System.Windows.Visibility.Collapsed;
                    dpStart.Visibility = System.Windows.Visibility.Collapsed;
                    tbTo.Visibility = System.Windows.Visibility.Collapsed;
                    tbContent.Visibility = System.Windows.Visibility.Collapsed;
                    cbbContentDeclarationStatus.Visibility = System.Windows.Visibility.Collapsed;
                    cbbContentDrawbackStatus.Visibility = System.Windows.Visibility.Collapsed;
                    cbbContentExamination.Visibility = System.Windows.Visibility.Collapsed;
                    cbbContentBoss.Visibility = System.Windows.Visibility.Collapsed;
                    cbbContentCustomer.Visibility = System.Windows.Visibility.Visible;
                }
                else
                {
                    dpEnd.Visibility = System.Windows.Visibility.Collapsed;
                    dpStart.Visibility = System.Windows.Visibility.Collapsed;
                    tbTo.Visibility = System.Windows.Visibility.Collapsed;
                    tbContent.Visibility = System.Windows.Visibility.Visible;
                    cbbContentDeclarationStatus.Visibility = System.Windows.Visibility.Collapsed;
                    cbbContentDrawbackStatus.Visibility = System.Windows.Visibility.Collapsed;
                    cbbContentExamination.Visibility = System.Windows.Visibility.Collapsed;
                    cbbContentBoss.Visibility = System.Windows.Visibility.Collapsed;
                    cbbContentCustomer.Visibility = System.Windows.Visibility.Collapsed;
                }
            }
        }
    }
    public class ConditionContent
    {
        public string DisplayField;
        public string ValueField;
        public override string ToString()
        {
            return DisplayField;
        }
    }
}
