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
using ProTemplate.Utility;
using Telerik.Windows.Controls;
using ProTemplate.ViewModels;

namespace ProTemplate.UserControls.CustomControl
{
    public partial class ContainerInputForm : UserControl
    {

        DeclarationContainerDataModel _containerDataModel;
        FormState _editState = FormState.Add;
        int _maxSequence = 0;

        private int _currentDeclarationID;
        public int CurrentDeclarationID
        {
            get { return _currentDeclarationID; }
            set
            {
                _currentDeclarationID = value;
                // 设置最大序号
                DeclarationContainerViewModel dvm = ViewModelManager.DeclarationContainerViewModelInstance;
                if (dvm != null && dvm.Items.Count > 0)
                {
                    _maxSequence = (from s in dvm.Items
                                    select s.Sequence).Max();
                }
            }
        }

        FormState DeclarationContainerEditState
        {
            get { return _editState; }
            set
            {
                _editState = value;
                if (_editState == FormState.Add)
                {
                    // clear input
                    _containerDataModel = new DeclarationContainerDataModel();
                    this.DataContext = _containerDataModel;

                    spAddContainer.Visibility = System.Windows.Visibility.Visible;
                    spUpdateContainer.Visibility = System.Windows.Visibility.Collapsed;
                }
                else
                {
                    spAddContainer.Visibility = System.Windows.Visibility.Collapsed;
                    spUpdateContainer.Visibility = System.Windows.Visibility.Visible;
                }
            }
        }

        public void SetToReadOnly()
        {
            btnUpdate.IsEnabled = false;
            btnAdd.IsEnabled = false;
        }

        public ContainerInputForm()
        {
            InitializeComponent();
            _containerDataModel = new DeclarationContainerDataModel();
            this.DataContext = _containerDataModel;
        }

        private bool InputCheck()
        {
            if (tbNumber.Text == "")
            {
                _containerDataModel.SetErrors("Number", new List<string>() { "请输入集装箱号！" });
            }
            else
            {
                _containerDataModel.ClearErrors("Number");
            }
            return !_containerDataModel.HasErrors;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (!InputCheck())
                return;
            // update view model
            DeclarationContainerViewModel divm = ViewModelManager.DeclarationContainerViewModelInstance;
            System.Diagnostics.Debug.Assert(divm != null);
            DeclarationContainerDataModel dcd = new DeclarationContainerDataModel();
            dcd.Index = divm.Items.Count + 1;
            dcd.SortOrder = ++_maxSequence;
            dcd.Number = tbNumber.Text;
            dcd.Model = tbModel.Text;
            dcd.Weight = tbWeight.Text;
            divm.Items.Add(dcd);

            // update DB

            Web.DeclarationContainer dc = new Web.DeclarationContainer();
            dc.SortOrder = dcd.SortOrder;
            dc.DeclarationId = CurrentDeclarationID;
            dc.Number = dcd.Number;
            dc.Model = dcd.Model;
            dc.Weight = dcd.Weight;
            var realItem = (from t in SystemConfiguration.Instance.DataContext.Declarations
                            where t.ID == CurrentDeclarationID
                            select t).SingleOrDefault();
            if (realItem != null)
                realItem.DeclarationContainer.Add(dc);
            // clear input
            DeclarationContainerEditState = FormState.Add;
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (!InputCheck())
                return;

            _containerDataModel.Number = tbNumber.Text;
            _containerDataModel.Model = tbModel.Text;
            _containerDataModel.Weight = tbWeight.Text;

            #region 更新数据库中数据
            var realDeclarationObj = (from t in SystemConfiguration.Instance.DataContext.Declarations
                                      where t.ID == CurrentDeclarationID
                                      select t).SingleOrDefault();
            if (realDeclarationObj != null)
            {
                Web.DeclarationContainer dbItem = null;
                if (_containerDataModel.Sequence > 0)
                {
                    dbItem = (from a in realDeclarationObj.DeclarationContainer
                              where a.Sequence == _containerDataModel.Sequence
                              select a).SingleOrDefault();
                }
                else
                {
                    dbItem = (from a in realDeclarationObj.DeclarationContainer
                              where a.SortOrder == _containerDataModel.SortOrder
                              select a).SingleOrDefault();
                }
                if (dbItem != null)
                {
                    dbItem.Number = _containerDataModel.Number;
                    dbItem.Weight = _containerDataModel.Weight;
                    dbItem.Model = _containerDataModel.Model;
                }
            }
            #endregion
            // set to default
            DeclarationContainerEditState = FormState.Add;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DeclarationContainerEditState = FormState.Add;
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            _containerDataModel = ((RadButton)sender).DataContext as DeclarationContainerDataModel;
            this.DataContext = _containerDataModel;
            DeclarationContainerEditState = FormState.Update;
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            DeclarationContainerDataModel dm = ((RadButton)sender).DataContext as DeclarationContainerDataModel;
            if (dm == null)
                return;
            CommonUIFunction.ShowConfirmYesNo("确定要删除这个集装箱信息么?", 
            (s,arg) => {
                if (arg.DialogResult == true)
                {
                    DeclarationContainerViewModel divm = ViewModelManager.DeclarationContainerViewModelInstance;
                    if (divm != null)
                        divm.Items.Remove(dm);

                    #region 删除数据库中数据
                    var realDeclarationObj = (from t in SystemConfiguration.Instance.DataContext.Declarations
                                              where t.ID == CurrentDeclarationID
                                              select t).SingleOrDefault();
                    if (realDeclarationObj != null)
                    {
                        Web.DeclarationContainer dbItem = null;
                        if (dm.Sequence > 0)
                        {
                            dbItem = (from a in realDeclarationObj.DeclarationContainer
                                      where a.Sequence == dm.Sequence
                                      select a).SingleOrDefault();
                        }
                        else
                        {
                            dbItem = (from a in realDeclarationObj.DeclarationContainer
                                      where a.SortOrder == dm.SortOrder
                                      select a).SingleOrDefault();
                        }
                        if (dbItem != null)
                        {
                            SystemConfiguration.Instance.DataContext.DeclarationContainers.Remove(dbItem);
                        }
                    }
                    #endregion
                }
            });
        }
    }
}
