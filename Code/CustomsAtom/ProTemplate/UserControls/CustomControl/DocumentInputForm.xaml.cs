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
using ProTemplate.ViewModels;
using ProTemplate.Models;
using ProTemplate.Utility;
using Telerik.Windows.Controls;

namespace ProTemplate.UserControls.CustomControl
{
    public partial class DocumentInputForm : UserControl
    {
        DeclarationDocumentDataModel _docDataModel;
        FormState _editState = FormState.Add;
        int _maxSequence = 0;
        public DocumentInputForm()
        {
            InitializeComponent();
            _docDataModel = new DeclarationDocumentDataModel();
            this.DataContext = _docDataModel;
            
        }

        private int _currentDeclarationID;
        public int CurrentDeclarationID
        {
            get { return _currentDeclarationID; }
            set
            {
                _currentDeclarationID = value;
                // 设置最大序号
                DeclarationDocumentViewModel dvm = ViewModelManager.DeclarationDocumentViewModelInstance;
                if (dvm != null && dvm.Items.Count > 0)
                {
                    _maxSequence = (from s in dvm.Items
                                   select s.Sequence).Max();
                }
            }
        }

        public void SetToReadOnly()
        {
            btnAddDocument.IsEnabled = false;
            btnUpdateDocument.IsEnabled = false;
            //acbDocument.IsEnabled = false;
        }

        FormState DeclarationDocumentEditState
        {
            get { return _editState; }
            set
            {
                _editState = value;
                if (_editState == FormState.Add)
                {
                    // clear input
                    _docDataModel = new DeclarationDocumentDataModel();
                    _docDataModel.DocumentName = "";

                    this.DataContext = _docDataModel;
                    btnAddDocument.Visibility = System.Windows.Visibility.Visible;
                    spUpdateDocument.Visibility = System.Windows.Visibility.Collapsed;
                }
                else
                {
                    btnAddDocument.Visibility = System.Windows.Visibility.Collapsed;
                    spUpdateDocument.Visibility = System.Windows.Visibility.Visible;
                }
            }
        }

        private void acbDocument_Populating(object sender, PopulatingEventArgs e)
        {
            DocumentViewModel vm = App.Current.Resources["DocumentViewModel"] as DocumentViewModel;
            if (vm != null)
            {
                var prar = e.Parameter.ToLower();
                var query = from a in vm.Items
                            where a.PinYin.ToLower().Contains(prar) || a.Name.ToLower().Contains(prar)
                            select a;
                int b = query.Count();
                AutoCompleteBox source = (AutoCompleteBox)sender;
                source.ItemsSource = query.ToList();
            }
        }

        private bool InputCheck()
        {
            if (acbDocument.Text == "" || acbDocument.SelectedItem == null)
            {
                _docDataModel.SetErrors("DocumentName", new List<string>() { "请输入合法单证！" });
            }
            else
            {
                _docDataModel.ClearErrors("DocumentName");
            }

            //if (tbCertificateNumber.Text == "")
            //{
            //    _docDataModel.SetErrors("CertificateNumber", new List<string>() { "请输入单证编号" });
            //}
            //else
            //{
            //    _docDataModel.ClearErrors("CertificateNumber");
            //}
            return !_docDataModel.HasErrors;
        }

        private void btnAddDocument_Click(object sender, RoutedEventArgs e)
        {
            if (!InputCheck())
                return;
            DeclarationDocumentViewModel divm = ViewModelManager.DeclarationDocumentViewModelInstance;
            System.Diagnostics.Debug.Assert(divm != null);
            DeclarationDocumentDataModel ddd = new DeclarationDocumentDataModel();
            ddd.Index = divm.Items.Count+1;
            ddd.SortOrder = (++_maxSequence);
            ddd.DocumentCode = acbDocument.SelectedItem == null ? "" : ((DocumentDataModel)acbDocument.SelectedItem).Code;
            ddd.DocumentName = acbDocument.SelectedItem == null ? "" : ((DocumentDataModel)acbDocument.SelectedItem).Name;
            ddd.CertificateNumber = tbCertificateNumber.Text;
            divm.Items.Add(ddd);

            Web.DeclarationDocument dd = new Web.DeclarationDocument();
            dd.SortOrder = ddd.SortOrder;
            dd.DeclarationId = CurrentDeclarationID;
            dd.Document = ddd.DocumentCode;
            dd.CertificateNumber = ddd.CertificateNumber;
            var realItem = (from t in SystemConfiguration.Instance.DataContext.Declarations
                            where t.ID == CurrentDeclarationID
                            select t).SingleOrDefault();
            if (realItem != null)
                realItem.DeclarationDocument.Add(dd);

            _docDataModel = new DeclarationDocumentDataModel();
            this.DataContext = _docDataModel;
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            _docDataModel = ((RadButton)sender).DataContext as DeclarationDocumentDataModel;
            this.DataContext = _docDataModel;
            DocumentDataModel ddm = new DocumentDataModel();
            ddm.Code = _docDataModel.DocumentCode;
            ddm.Name = _docDataModel.DocumentName;
            acbDocument.SelectedItem = ddm;
            DeclarationDocumentEditState = FormState.Update;
        }

        private void btnCancelDocument_Click(object sender, RoutedEventArgs e)
        {
            DeclarationDocumentEditState = FormState.Add;
        }

        private void btnUpdateDocument_Click(object sender, RoutedEventArgs e)
        {
            if (!InputCheck())
                return;
            _docDataModel.DocumentCode = ((DocumentDataModel)acbDocument.SelectedItem).Code;
            _docDataModel.DocumentName = ((DocumentDataModel)acbDocument.SelectedItem).Name;
            _docDataModel.CertificateNumber = tbCertificateNumber.Text;

            #region 更新数据库中数据
            var realDeclarationObj = (from t in SystemConfiguration.Instance.DataContext.Declarations
                                      where t.ID == CurrentDeclarationID
                                      select t).SingleOrDefault();
            if (realDeclarationObj != null)
            {
                Web.DeclarationDocument dbItem = null;
                if (_docDataModel.Sequence > 0)
                {
                    dbItem = (from a in realDeclarationObj.DeclarationDocument
                              where a.Sequence == _docDataModel.Sequence
                              select a).SingleOrDefault();
                }
                else
                {
                    dbItem = (from a in realDeclarationObj.DeclarationDocument
                              where a.SortOrder == _docDataModel.SortOrder
                              select a).SingleOrDefault();
                }
                if (dbItem != null)
                {
                    dbItem.Document = _docDataModel.DocumentCode;
                    dbItem.CertificateNumber = _docDataModel.CertificateNumber;
                }
            }
            #endregion
            // set to default
            DeclarationDocumentEditState = FormState.Add;
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            DeclarationDocumentDataModel dm = ((RadButton)sender).DataContext as DeclarationDocumentDataModel;
            if (dm == null)
                return;
            CommonUIFunction.ShowConfirmYesNo("确定要删除这个文档信息么?",
            (s, arg) =>
            {
                if (arg.DialogResult == true)
                {
                    DeclarationDocumentViewModel divm = ViewModelManager.DeclarationDocumentViewModelInstance;
                    if (divm != null)
                        divm.Items.Remove(dm);

                    #region 删除数据库中数据
                    var realDeclarationObj = (from t in SystemConfiguration.Instance.DataContext.Declarations
                                              where t.ID == CurrentDeclarationID
                                              select t).SingleOrDefault();
                    if (realDeclarationObj != null)
                    {
                        Web.DeclarationDocument dbItem = null;
                        if (dm.Sequence > 0)
                        {
                            dbItem = (from a in realDeclarationObj.DeclarationDocument
                                      where a.Sequence == dm.Sequence
                                      select a).SingleOrDefault();
                        }
                        else
                        {
                            dbItem = (from a in realDeclarationObj.DeclarationDocument
                                      where a.SortOrder == dm.SortOrder
                                      select a).SingleOrDefault();
                        }
                        if (dbItem != null)
                        {
                            SystemConfiguration.Instance.DataContext.DeclarationDocuments.Remove(dbItem);
                        }
                    }
                    #endregion
                }
            });
        }


    }
}
