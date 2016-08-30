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
using ProTemplate.Models;
using Telerik.Windows.Controls;
using ProTemplate.Utility;
using ProTemplate.ViewModels;

namespace ProTemplate.UserControls.RadWindows
{
    public partial class ExaminationBatchEditForm : RadWindow
    {
        public ExaminationBatchEditForm()
        {
            InitializeComponent();
        }

        private void EditWindowToolBar_SaveAndClose(object sender, EventArgs e)
        {
            foreach (ExaminationDataModel dm in gdDeclaration.Items)
            {
                var examination = (from d in SystemConfiguration.Instance.DataContext.Examinations
                                   where d.ID == dm.ID
                                   select d).FirstOrDefault();
                if (examination != null)
                {
                    examination.ExaminationCost = dm.ExaminationCost;
                    examination.ExaminationFee = dm.ExaminationFee;

                    var declaration = (from d in SystemConfiguration.Instance.DataContext.DeclarationDocuments
                                       where d.CertificateNumber == examination.ExaminationNumber
                                       select d).SingleOrDefault();

                    if (declaration != null)
                    {
                        var billFee = (from d in SystemConfiguration.Instance.DataContext.DeclarationDocuments
                                           from fd in SystemConfiguration.Instance.DataContext.FinancialExportDeclarations
                                           where d.DeclarationId == fd.DeclarationId && d.CertificateNumber == examination.ExaminationNumber
                                           select fd).SingleOrDefault();

                        if (billFee != null)
                        {
                            billFee.Amount = dm.ExaminationFee;
                            billFee.Cost = dm.ExaminationCost;
                        }
                        else
                        {
                            billFee = new Web.FinancialExportDeclaration();
                            billFee.FeeTypeCode = "107";
                            billFee.Amount = dm.ExaminationFee;
                            billFee.Cost = dm.ExaminationCost;
                            billFee.DeclarationId = declaration.DeclarationId;
                            billFee.CreatedDate = DateTime.Now;
                            SystemConfiguration.Instance.DataContext.FinancialExportDeclarations.Add(billFee);
                        }
                    }
                }
            }

            busyIndicator.IsBusy = true;
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
                    ExaminationViewModel vm = ViewModelManager.ExaminationViewModelInstance;
                    foreach (ExaminationDataModel dm in gdDeclaration.Items)
                    {
                        var examination = (from c in vm.Items where c.ID == dm.ID select c).FirstOrDefault();
                        if (examination != null)
                        {
                            examination.ExaminationFee = dm.ExaminationFee;
                            examination.ExaminationCost = dm.ExaminationCost;
                        }
                    }
                    //var declaration = (from d in SystemConfiguration.Instance.DataContext.Declaration
                    //                   dd in SystemConfiguration.Instance.DataContext.DeclarationDocuents
                    //                       fd in SystemConfiguration.Instance.DataContext.FinancialExpomrtDeclarations
                    //                   where (d.CertificateNumber == examination.ExaminationNumber)
                    //                   select d).SingleOrDefault();
                    //SystemConfiguration.Instance.DataContext.Load()

                    this.Close();
                }
            }, null);
        }

        private void EditWindowToolBar_Close(object sender, EventArgs e)
        {
            this.Close();
        }

        public void Load(List<ExaminationDataModel> examinations)
        {
            if (examinations == null || examinations.Count == 0)
                return;
            SystemConfiguration.Instance.DataContext.Examinations.Clear();
            SystemConfiguration.Instance.DataContext.Load(SystemConfiguration.Instance.DataContext.GetExaminationByIDsQuery(examinations.Select(o => o.ID)), (p) => { }, null);

            List<ExaminationDataModel> lstSource = new List<ExaminationDataModel>();
            foreach (ExaminationDataModel d in examinations)
            {
                ExaminationDataModel dm = new ExaminationDataModel();

                dm.Index = lstSource.Count + 1;
                dm.ID = d.ID;
                dm.ExaminationNumber = d.ExaminationNumber;
                dm.ExaminationFee = d.ExaminationFee;
                dm.ExaminationCost = d.ExaminationCost;
                lstSource.Add(dm);
            }
            gdDeclaration.ItemsSource = null;
            gdDeclaration.ItemsSource = lstSource;

            SystemConfiguration.Instance.DataContext.FinancialExportDeclarations.Clear();
            SystemConfiguration.Instance.DataContext.Load(SystemConfiguration.Instance.DataContext.GetFinancialDeclarationByExaminationIDsQuery(examinations.Select(o => o.ID)), (p) => { }, null);

            SystemConfiguration.Instance.DataContext.DeclarationDocuments.Clear();
            SystemConfiguration.Instance.DataContext.Load(SystemConfiguration.Instance.DataContext.GetDeclarationDocumentByExaminationIDsQuery(examinations.Select(o => o.ID)), (p) => { }, null);
        }
    }
}
