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
using System.ServiceModel.DomainServices.Client;
using ProTemplate.UserControls.RadWindows;

namespace ProTemplate.Views
{
    public partial class ArchiveFeeType : Page
    {
        public ArchiveFeeType()
        {
            InitializeComponent();
        }

        #region Page Operation
        public void Refresh(bool isForceReloadFromDB = false)
        {
            if (SystemConfiguration.Instance.DataContext != null)
            {
                // 如果设置这个参数，会清空当前DB Context的实体对象，这将会从数据库中重新加载数据。
                if (isForceReloadFromDB)
                    SystemConfiguration.Instance.DataContext.FeeTypes.Clear();

                SystemConfiguration.Instance.DataContext.Load(SystemConfiguration.Instance.DataContext.GetFeeTypeQuery(), delegate(LoadOperation<Web.FeeType> lp)
                {
                    FeeTypeViewModel cvm = App.Current.Resources["FeeTypeViewModel"] as FeeTypeViewModel;
                    if (cvm == null)
                        return;
                    cvm.Items.Clear();
                    foreach (var q in lp.Entities)
                    {
                        ProTemplate.Models.FeeTypeDataModel FeeTypeMD = new Models.FeeTypeDataModel();
                        FeeTypeMD.Code = q.Code;
                        FeeTypeMD.Name = q.Name;
                        FeeTypeMD.Amount = q.Amount;
                        FeeTypeMD.Cost = q.Cost;
                        cvm.Items.Add(FeeTypeMD);
                    }
                    cvm.UpdateIndex();
                }, null);
            }

        }
        #endregion

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Refresh();
        }

        //private bool Validate()
        //{
        //    bool bret = true;
        //    if (tbName.Text.Trim() == "")
        //    {
        //        CommonUIFunction.ShowMessageBox("费用名不能为空！");
        //        bret = false;
        //    }
        //    if (tbName.Text.Trim() == "")
        //    {
        //        CommonUIFunction.ShowMessageBox("费用代码不能为空！");
        //        bret = false;
        //    }
        //    if (!Constants.IsDouble(tbAmount.Text))
        //    {
        //        CommonUIFunction.ShowMessageBox("费用只能是数字！");
        //        bret = false;
        //    }
        //    if (!Constants.IsDouble(tbCost.Text))
        //    {
        //        CommonUIFunction.ShowMessageBox("成本只能是数字！");
        //        bret = false;
        //    }
        //    return bret;
        //}
        #region Tool bar Events
        private void commBar_NewClick(object sender, EventArgs e)
        {
            AddFeeType wnd = new AddFeeType();
            wnd.ShowDialog();
        }

        private void commBar_RefreshClick(object sender, EventArgs e)
        {
            Refresh(true);
        }

        private void commBar_EditClick(object sender, EventArgs e)
        {
            if (gdFeeType.SelectedItems == null || gdFeeType.SelectedItems.Count == 0)
            {
                CommonUIFunction.ShowMessageBox(MessageTexts.MSG026);
                return;
            }

            AddFeeType wnd = new AddFeeType(gdFeeType.SelectedItems[0] as ProTemplate.Models.FeeTypeDataModel);
            wnd.ShowDialog();
        }

        #endregion
    }
}
