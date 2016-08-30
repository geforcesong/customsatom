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
using ProTemplate.Models;

namespace ProTemplate.UserControls
{
    public partial class CommonToolBar : UserControl
    {
        private bool hasAccessShowSetDeclarationStatusGroup = true;
        private bool hasAccessShowSetDrawbackStatusGroup = true;
        private bool hasAccessShowSetExaminationStatusGroup = true;
        private bool hasAccessShowPortCheckButton = true;
        private bool hasAccessShowYSValidationButton = true;
        private bool hasAccessPreListButton = true;
        private bool hasAccessNewButton = true;
        private bool hasAccessEditButton = true;
        private bool hasAccessBatchEditButton = true;
        private bool hasAccessDeleteButton = true;
        public CommonToolBar()
        {
            CheckUserRoleAccess();
            InitializeComponent();
            ProTemplate.Utility.SystemConfiguration.Instance.SystemToolBar = this;
            NewButton = true;
            EditButton = true;
            DeleteButton = true;
            PreListButton = false;

            IsShowSetDeclarationStatusGroup = false;
            IsShowSetExaminationStatusGroup = false;
            IsShowSetDrawbackStatusGroup = false;
            IsShowPortCheckButton = false;
            IsShowYSValidationButton = false;
            BatchEditButton = false;
        }



        private void CheckUserRoleAccess()
        {
            string lstRoles = string.Join(";", SystemConfiguration.Instance.LoggedOnUser.RoleList.Select(o=>o.Name));

            if (lstRoles.Contains("客户"))
            {
                hasAccessShowSetDeclarationStatusGroup = false;
                hasAccessShowSetDrawbackStatusGroup = false;
                hasAccessShowSetExaminationStatusGroup = false;
                hasAccessNewButton = false;
                hasAccessEditButton = true;
                hasAccessBatchEditButton = false;
                hasAccessDeleteButton = false;
                hasAccessShowPortCheckButton = false;
                hasAccessPreListButton = false;
                hasAccessShowYSValidationButton = false;
            }
            else
            {
                if (lstRoles.Contains("出口退税人员"))
                {
                    hasAccessShowSetDeclarationStatusGroup = true;
                    hasAccessShowSetDrawbackStatusGroup = true;
                    hasAccessShowSetExaminationStatusGroup = false;
                    hasAccessNewButton = false;
                    hasAccessEditButton = true;
                    hasAccessBatchEditButton = false;
                    hasAccessDeleteButton = false;
                    hasAccessShowPortCheckButton = false;
                    hasAccessPreListButton = false;
                    hasAccessShowYSValidationButton = false;
                }

                else
                {
                    if (lstRoles.Contains("财务人员"))
                    {
                        hasAccessShowSetDeclarationStatusGroup = true;
                        hasAccessShowSetDrawbackStatusGroup = false;
                        hasAccessShowSetExaminationStatusGroup = false;
                        hasAccessNewButton = true;
                        hasAccessEditButton = true;
                        hasAccessBatchEditButton = true;
                        hasAccessDeleteButton = true;
                        hasAccessShowPortCheckButton = false;
                        hasAccessPreListButton = false;
                        hasAccessShowYSValidationButton = false;
                    }
                    else
                    {
                        if (lstRoles.Contains("出口清单人员"))
                        {
                            hasAccessShowSetDeclarationStatusGroup = true;
                            hasAccessShowSetDrawbackStatusGroup = false;
                            hasAccessShowSetExaminationStatusGroup = false;
                            hasAccessNewButton = true;
                            hasAccessEditButton = true;
                            hasAccessBatchEditButton = true;
                            hasAccessDeleteButton = true;
                            hasAccessShowPortCheckButton = true;
                            hasAccessPreListButton = true;
                            hasAccessShowYSValidationButton = true;
                        } else
                        if (lstRoles.Contains("出口操作人员") || lstRoles.Contains("出口人员") || lstRoles.Contains("出口审核人员"))
                        {
                            hasAccessShowSetDeclarationStatusGroup = false;
                            hasAccessShowSetDrawbackStatusGroup = false;
                            hasAccessShowSetExaminationStatusGroup = false;
                            hasAccessNewButton = true;
                            hasAccessEditButton = true;
                            hasAccessBatchEditButton = false;
                            hasAccessDeleteButton = false;
                            hasAccessShowPortCheckButton = true;
                            hasAccessPreListButton = true;
                            hasAccessShowYSValidationButton = true;
                        }
                        else
                        {
                            if (lstRoles.Contains("管理员") || lstRoles.Contains("出口经理"))
                            {
                                hasAccessShowSetDeclarationStatusGroup = true;
                                hasAccessShowSetDrawbackStatusGroup = true;
                                hasAccessShowSetExaminationStatusGroup = true;
                                hasAccessNewButton = true;
                                hasAccessEditButton = true;
                                hasAccessBatchEditButton = true;
                                hasAccessDeleteButton = true;
                                hasAccessShowPortCheckButton = true;
                                hasAccessPreListButton = true;
                                hasAccessShowYSValidationButton = true;
                            }
                        }
                    }

                }
            }
        }

        public bool NewButton
        {
            set 
            { 
                if (!value || !hasAccessNewButton) btnNew.Visibility = Visibility.Collapsed; 
            }
        }

        public bool BatchNewButton
        {
            set
            {
                if (value) btnBatchNew.Visibility = Visibility.Visible;
                else btnBatchNew.Visibility = Visibility.Collapsed;
            }
        }

        public bool EditButton
        {
            set { if (!value || !hasAccessEditButton) btnEdit.Visibility = Visibility.Collapsed; }
        }

        public bool BatchEditButton
        {
            set
            {
                if (!value || !hasAccessBatchEditButton) btnBatchEdit.Visibility = Visibility.Collapsed;
                else btnBatchEdit.Visibility = Visibility.Visible;
            }
        }

        public bool DeleteButton
        {
            set { if (!value || !hasAccessDeleteButton) btnDelete.Visibility = Visibility.Collapsed; else btnDelete.Visibility = Visibility.Visible; }
        }

        public bool RefreshButton
        {
            set { if (!value) btnRefresh.Visibility = Visibility.Collapsed; else btnRefresh.Visibility = Visibility.Visible; }
        }


        public bool PreListButton
        {
            set { if (!value || !hasAccessPreListButton) btnPreListEdit.Visibility = Visibility.Collapsed; else btnPreListEdit.Visibility = Visibility.Visible; }
        }

        public bool ShowPrintButton
        {
            get { return btnPrintReport.Visibility == System.Windows.Visibility.Visible; }
            set
            {
                if (value)
                    btnPrintReport.Visibility = System.Windows.Visibility.Visible;
                else
                    btnPrintReport.Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        public bool IsShowSetDeclarationStatusGroup
        {
            get { return StatusSeparator.Visibility == System.Windows.Visibility.Visible; }
            set
            {
                if (value && hasAccessShowSetDeclarationStatusGroup)
                {
                    StatusSeparator.Visibility = System.Windows.Visibility.Visible;

                    StatusButton.Visibility = System.Windows.Visibility.Visible;
                    //StatusButton2.Visibility = System.Windows.Visibility.Visible;
                    //StatusButton3.Visibility = System.Windows.Visibility.Visible;
                    //StatusButton4.Visibility = System.Windows.Visibility.Visible;
                    //StatusButton5.Visibility = System.Windows.Visibility.Visible;
                    //StatusButton6.Visibility = System.Windows.Visibility.Visible;
                    //StatusButton7.Visibility = System.Windows.Visibility.Visible;
                    //StatusButton8.Visibility = System.Windows.Visibility.Visible;
                    //StatusButton9.Visibility = System.Windows.Visibility.Visible;
                }
                else
                {
                    StatusSeparator.Visibility = System.Windows.Visibility.Collapsed;
                    StatusButton.Visibility = System.Windows.Visibility.Collapsed;
                    //StatusButton2.Visibility = System.Windows.Visibility.Collapsed;
                    //StatusButton3.Visibility = System.Windows.Visibility.Collapsed;
                    //StatusButton4.Visibility = System.Windows.Visibility.Collapsed;
                    //StatusButton5.Visibility = System.Windows.Visibility.Collapsed;
                    //StatusButton6.Visibility = System.Windows.Visibility.Collapsed;
                    //StatusButton7.Visibility = System.Windows.Visibility.Collapsed;
                    //StatusButton8.Visibility = System.Windows.Visibility.Collapsed;
                    //StatusButton9.Visibility = System.Windows.Visibility.Collapsed;
                }
            }
        }

        public bool IsShowSetExaminationStatusGroup
        {
            get { return ExaminationSeparator.Visibility == System.Windows.Visibility.Visible; }
            set
            {
                if (value && hasAccessShowSetExaminationStatusGroup)
                {
                    ExaminationSeparator.Visibility = System.Windows.Visibility.Visible;
                    btnSetExamination.Visibility = System.Windows.Visibility.Visible;
                    btnSetNOExamination.Visibility = System.Windows.Visibility.Visible;
                }
                else
                {
                    ExaminationSeparator.Visibility = System.Windows.Visibility.Collapsed;
                    btnSetExamination.Visibility = System.Windows.Visibility.Collapsed;
                    btnSetNOExamination.Visibility = System.Windows.Visibility.Collapsed;
                }
            }
        }

        public bool IsShowSetDrawbackStatusGroup
        {
            get { return DrawbackStatusSeparator.Visibility == System.Windows.Visibility.Visible; }
            set
            {
                if (value && hasAccessShowSetDrawbackStatusGroup)
                {
                    DrawbackStatusSeparator.Visibility = System.Windows.Visibility.Visible;
                    btnDrawback.Visibility = System.Windows.Visibility.Visible;
                    //btnDrawbacked.Visibility = System.Windows.Visibility.Visible;
                    //btnDrawbacking.Visibility = System.Windows.Visibility.Visible;
                    //btnDrawbackNormal.Visibility = System.Windows.Visibility.Visible;
                    //btnDrawbackOther.Visibility = System.Windows.Visibility.Visible;
                }
                else
                {
                    DrawbackStatusSeparator.Visibility = System.Windows.Visibility.Collapsed;
                    btnDrawback.Visibility = System.Windows.Visibility.Collapsed;
                    //btnDrawbacked.Visibility = System.Windows.Visibility.Collapsed;
                    //btnDrawbacking.Visibility = System.Windows.Visibility.Collapsed;
                    //btnDrawbackNormal.Visibility = System.Windows.Visibility.Collapsed;
                    //btnDrawbackOther.Visibility = System.Windows.Visibility.Collapsed;
                }
            }
        }

        public bool IsShowPortCheckButton
        {
            get { return PortCheckSeparator.Visibility == System.Windows.Visibility.Visible; }
            set
            {
                if (value && hasAccessShowPortCheckButton)
                {
                    PortCheckSeparator.Visibility = System.Windows.Visibility.Visible;
                    btnPortCheck.Visibility = System.Windows.Visibility.Visible;
                }
                else
                {
                    PortCheckSeparator.Visibility = System.Windows.Visibility.Collapsed;
                    btnPortCheck.Visibility = System.Windows.Visibility.Collapsed;
                }
            }
        }

        public bool IsShowYSValidationButton
        {
            get { return YSValidationSeparator.Visibility == System.Windows.Visibility.Visible; }
            set
            {
                if (value && hasAccessShowYSValidationButton)
                {
                    YSValidationSeparator.Visibility = System.Windows.Visibility.Visible;
                    btnYSValidation.Visibility = System.Windows.Visibility.Visible;
                }
                else
                {
                    YSValidationSeparator.Visibility = System.Windows.Visibility.Collapsed;
                    btnYSValidation.Visibility = System.Windows.Visibility.Collapsed;
                }
            }
        }

        public bool CanAddDelete
        {
            set
            {
                btnDelete.IsEnabled = btnNew.IsEnabled = value;
            }
        }

        public event EventHandler NewClick;
        public event EventHandler BatchNewClick;
        public event EventHandler RefreshClick;
        public event EventHandler BatchEditClick;
        public event EventHandler EditClick;
        public event EventHandler DeleteClick;
        public event EventHandler ExportToExcelClick;
        public event EventHandler ExportToWordClick;
        public event EventHandler PrintReportClick;
        public event EventHandler PortCheckClick;
        public event EventHandler<DeclarationStatusEventArgs> ChangeDeclarationStatusClick;
        public event EventHandler<DrawbackStatusEventArgs> ChangeDrawbackStatusClick;
        public event EventHandler<ExaminationStatusEventArgs> ChangeExaminationStatusClick;
        //public event EventHandler<EventArgs> ChangeDeclarationStatusClick;
        public event EventHandler PreListClick;
        public event EventHandler YSValidationClick;

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            if (NewClick != null)
                NewClick(this, new EventArgs());
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            if (RefreshClick != null)
                RefreshClick(this, new EventArgs());
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (EditClick != null)
                EditClick(this, new EventArgs());
        }

        private void btnBatchEdit_Click(object sender, RoutedEventArgs e)
        {
            if (BatchEditClick != null)
                BatchEditClick(this, new EventArgs());
        }



        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (DeleteClick != null)
                DeleteClick(this, new EventArgs());
        }

        private void btnFullScreen_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Host.Content.IsFullScreen = !Application.Current.Host.Content.IsFullScreen;
            if (Application.Current.Host.Content.IsFullScreen)
                ToolTipService.SetToolTip(imgFullScreen, "退出全屏");
            else
                ToolTipService.SetToolTip(imgFullScreen, "全屏");
        }

        public void SetFullScreenToolTip(string tip)
        {
            ToolTipService.SetToolTip(imgFullScreen, tip);
        }

        private void btnExportToExcel_Click(object sender, RoutedEventArgs e)
        {
            if (ExportToExcelClick != null)
                ExportToExcelClick(this, new EventArgs());
        }

        private void btnExportToWord_Click(object sender, RoutedEventArgs e)
        {
            if (ExportToWordClick != null)
                ExportToWordClick(this, new EventArgs());
        }

        private void btnPrintReport_Click(object sender, RoutedEventArgs e)
        {
            if (PrintReportClick != null)
                PrintReportClick(this, new EventArgs());
        }

        private void StatusButton_Click(object sender, RoutedEventArgs e)
        {
            //if (ChangeDeclarationStatusClick != null)
            //{
            //    DeclarationStatusEventArgs arg = new DeclarationStatusEventArgs();
            //    arg.DeclarationStatus = ((Telerik.Windows.Controls.RadButton)sender).Tag.ToString();
            //    ChangeDeclarationStatusClick(this, arg);
            //}
            if (ChangeDeclarationStatusClick != null)
            {
                //DeclarationStatusEventArgs arg = new DeclarationStatusEventArgs();
                //arg.DeclarationStatus = ((Telerik.Windows.Controls.RadButton)sender).Tag.ToString();
                ChangeDeclarationStatusClick(this, new DeclarationStatusEventArgs());
            }
        }

        //private void SetStatusButton_Click(object sender, RoutedEventArgs e)
        //{
        //    if (ChangeDeclarationStatusClick != null)
        //    {
        //        //DeclarationStatusEventArgs arg = new DeclarationStatusEventArgs();
        //        //arg.DeclarationStatus = ((Telerik.Windows.Controls.RadButton)sender).Tag.ToString();
        //        ChangeDeclarationStatusClick(this, new DeclarationStatusEventArgs());
        //    }
        //}

        private void SetExamination_Click(object sender, RoutedEventArgs e)
        {
            if (ChangeExaminationStatusClick != null)
            {
                ExaminationStatusEventArgs arg = new ExaminationStatusEventArgs();
                arg.ExaminationStatus = ((Telerik.Windows.Controls.RadButton)sender).Tag.ToString();
                ChangeExaminationStatusClick(this, arg);
            }
        }

        private void DrawbackStatus_Click(object sender, RoutedEventArgs e)
        {
            if (ChangeDrawbackStatusClick != null)
            {
                //DrawbackStatusEventArgs arg = new DrawbackStatusEventArgs();
                //arg.DrawbackStatus = ((Telerik.Windows.Controls.RadButton)sender).Tag.ToString();
                ChangeDrawbackStatusClick(this, new DrawbackStatusEventArgs());
            }
        }

        private void btnPortCheck_Click(object sender, RoutedEventArgs e)
        {
            if (PortCheckClick != null)
                PortCheckClick(this, new EventArgs());
        }

        private void btnPreListEdit_Click(object sender, RoutedEventArgs e)
        {
            if (PreListClick != null)
                PreListClick(this, new EventArgs());
        }

        private void btnYSValidation_Click(object sender, RoutedEventArgs e)
        {
            if (YSValidationClick != null)
                YSValidationClick(this, new EventArgs());
        }

        private void btnBatchNew_Click(object sender, RoutedEventArgs e)
        {
            if (BatchNewClick != null)
                BatchNewClick(this, new EventArgs());
        }


    }

    public class DeclarationStatusEventArgs : EventArgs
    {
        public string DeclarationStatus { get; set; }
    }

    public class ExaminationStatusEventArgs : EventArgs
    {
        public string ExaminationStatus { get; set; }
    }

    public class DrawbackStatusEventArgs : EventArgs
    {
        public string DrawbackStatus { get; set; }
    }
}
