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

namespace ProTemplate.UserControls.RadWindows
{
    public partial class AddCorporation : RadWindow
    {
        CorporationDataModel _currentDataModal;
        public AddCorporation()
        {
            InitializeComponent();
            _currentDataModal = new CorporationDataModel();
            this.DataContext = _currentDataModal;
            this.Header = "新增经营单位";
            toolBar.IsNew = true;
        }

        public AddCorporation(CorporationDataModel Corporation)
        {
            InitializeComponent();
            _currentDataModal = Corporation;
            this.DataContext = _currentDataModal;
            this.Header = "编辑经营单位";
            toolBar.IsNew = false;
            tbCode.IsReadOnly = true;
        }

        private void EditWindowToolBar_SaveAndClose(object sender, EventArgs e)
        {
            Save(false);
        }

        private void EditWindowToolBar_SaveAndNew(object sender, EventArgs e)
        {
            Save(true);
        }

        private void EditWindowToolBar_Close(object sender, EventArgs e)
        {
            this.Close();
        }

        public bool InputCheck()
        {
            if (tbCode.Text == "")
            {
                _currentDataModal.SetErrors("Code", new List<string>() { "经营单位代码不能为空" });
            }
            else
            {
                _currentDataModal.ClearErrors("Code");
            }
            if (tbName.Text == "")
            {
                _currentDataModal.SetErrors("Name", new List<string>() { "经营单位名称不能为空" });
            }
            else
            {
                _currentDataModal.ClearErrors("Name");
            }
            return !_currentDataModal.HasErrors;
        }

        #region Page Operations
        void Save(bool IsNeedNew)
        {
            //_currentDataModal.Name = tbName.Text;
            if (!InputCheck())
                return;

            //CommonUIFunction.HideMessageText(bdMsgParent);

            

            var currentCorporation = (from q in SystemConfiguration.Instance.DataContext.Corporations
                               where q.CorporationCode == _currentDataModal.Code
                               select q).SingleOrDefault();

            if (currentCorporation == null)
            {
                currentCorporation = new Web.Corporation();
                SystemConfiguration.Instance.DataContext.Corporations.Add(currentCorporation);
            }

            currentCorporation.CorporationName = tbName.Text;
            currentCorporation.CorporationCode = tbCode.Text;
            currentCorporation.Level = tbLevel.Text;

            SystemConfiguration.Instance.DataContext.SubmitChanges((a) =>
            {
                if (a.HasError)
                {
                    a.MarkErrorAsHandled();
                    MessageBox.Show(a.Error.Message);
                    SystemConfiguration.Instance.DataContext.RejectChanges();
                }
                else
                {
                    UpdateViewModel(currentCorporation);

                    if (IsNeedNew)
                    {
                        CommonUIFunction.ShowMessageText(bdMsgParent, "保存成功，请继续添加！");
                        _currentDataModal = new CorporationDataModel();
                        this.DataContext = _currentDataModal;
                    }
                    else
                    {
                        this.Close();
                    }
                }
            }, null);
        }

        private void UpdateViewModel(Web.Corporation Corporation)
        {
            CorporationViewModel CorporationVM = App.Current.Resources["CorporationViewModel"] as CorporationViewModel;
            if (CorporationVM != null)
            {
                if (Corporation.CorporationCode != _currentDataModal.Code)
                {
                    CorporationVM.Items.Add(_currentDataModal);
                }

                _currentDataModal.Code = Corporation.CorporationCode;
                _currentDataModal.Name = Corporation.CorporationName;
                _currentDataModal.Level = Corporation.Level;

                // update index
                CorporationVM.UpdateIndex();
            }
        }

        #endregion

        private void RadWindow_Closed(object sender, WindowClosedEventArgs e)
        {
            if (_currentDataModal != null)
            {
                _currentDataModal.ClearErrors("Name");
                _currentDataModal.ClearErrors("Code");
            }
        }
    }

}
