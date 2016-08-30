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
using ProTemplate.ViewModels;
using System.Windows.Media.Imaging;
using ProTemplate.UserControls.RadWindows;

namespace ProTemplate.UserControls.CustomControl
{
    public partial class YSValidationUserControl : UserControl
    {
        public YSValidationUserControl()
        {
            InitializeComponent();
        }

        public bool CanLogin
        {
            set
            {
                btnLogin.IsEnabled = value;
            }
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            CommonUIFunction.SetApplcationBusyIndicator(true, "正在获取登录信息。。。");
            SystemConfiguration.Instance.DataContext.GetYSValidImage(lp =>
            {
                CommonUIFunction.SetApplcationBusyIndicator(false);
                if (lp.Error != null || lp.Value == null || lp.Value.Length == 0)
                {
                    MessageBox.Show("图片获取失败");
                    return;
                }
                else
                {
                    BitmapImage bitmapimage = new BitmapImage();
                    System.IO.MemoryStream ms = new System.IO.MemoryStream(lp.Value);
                    bitmapimage.SetSource(ms);

                    YSLogin wnd = new YSLogin();
                    wnd.YSValidateImage = bitmapimage;
                    wnd.Closed += new EventHandler<Telerik.Windows.Controls.WindowClosedEventArgs>(wnd_Closed);
                    wnd.ShowDialog();

                }
            }, null);
        }

        void wnd_Closed(object sender, Telerik.Windows.Controls.WindowClosedEventArgs e)
        {
            if (e.DialogResult == true)
            {
                btnLogin.Visibility = System.Windows.Visibility.Collapsed;
                btnClear.IsEnabled = true;
                rbDays.IsEnabled = true;
                rbConf.IsEnabled = true;
                rbDays.IsChecked = true;
            }
        }

        private void rbDays_Checked(object sender, RoutedEventArgs e)
        {
            ClearData();
            if (sender == rbDays)
            {
                btnToday.IsEnabled = true;
                btnYesterday.IsEnabled = true;
                btnTheDayBeforeYesterday.IsEnabled = true;
                tbInputNumber.IsEnabled = false;
            }
            else
            {
                btnToday.IsEnabled = false;
                btnYesterday.IsEnabled = false;
                btnTheDayBeforeYesterday.IsEnabled = false;
                tbInputNumber.IsEnabled = true;
            }
        }

        private void tbInputNumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tbInputNumber.Text.Length == 18)
            {
                YSExaminationDataViewModel vm = ViewModelManager.YSExaminationDataViewModelInstance;
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


                SystemConfiguration.Instance.DataContext.YSExaminationDatas.Clear();
                CommonUIFunction.SetApplcationBusyIndicator(true, "正在获取数据，请稍后...");
                SystemConfiguration.Instance.DataContext.Load(SystemConfiguration.Instance.DataContext.YSQueryByDeclarationNumbersQuery(EncryptionUtil.Encrypt(tbInputNumber.Text)), lp =>
                {
                    CommonUIFunction.SetApplcationBusyIndicator(false);
                    if (lp.HasError)
                    {
                        lp.MarkErrorAsHandled();
                        MessageBox.Show("获取系统信息出错，请重试！");
                    }
                    else if (lp.Entities.Count() > 0)
                    {
                        var data = lp.Entities.ElementAt(0);
                        YSExaminationDataDataModel dm = new YSExaminationDataDataModel();
                        dm.ID = data.ID;
                        dm.ApprovalNumber = data.ApprovalNumber;
                        dm.BillNumber = data.BillNumber;
                        dm.Conveyance = data.Conveyance;
                        dm.CustomerName = data.CustomerName;
                        dm.DeclarationNumber = data.DeclarationNumber;
                        dm.DeclarationStatus = data.DeclarationStatus;
                        dm.VoyageNumber = data.VoyageNumber;
                        dm.YSDate = data.YSDate;
                        dm.YSStatus = data.YSStatus;
                        vm.Items.Add(dm);
                        vm.UpdateIndex();
                    }
                    tbInputNumber.Text = "";
                }, null);
            }
        }

        private void ClearData()
        {
            YSExaminationDataViewModel vm = ViewModelManager.YSExaminationDataViewModelInstance;
            if (vm != null)
                vm.Items.Clear();
            SystemConfiguration.Instance.DataContext.YSExaminationDatas.Clear();
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            ClearData();
        }

        private void LoadDataByDate(DateTime start, DateTime end)
        {
            ClearData();
            YSExaminationDataViewModel vm = ViewModelManager.YSExaminationDataViewModelInstance;
            if (vm == null)
                return;
            SystemConfiguration.Instance.DataContext.Load(SystemConfiguration.Instance.DataContext.YSQueryByDateQuery(start, end), lp =>
            {
                CommonUIFunction.SetApplcationBusyIndicator(false);
                if (lp.HasError)
                {
                    Exception exp = lp.Error;
                    lp.MarkErrorAsHandled();
                    MessageBox.Show("获取数据失败,请重试！");
                }
                else if (lp.Entities.Count() > 0)
                {
                    foreach (var data in lp.Entities)
                    {
                        YSExaminationDataDataModel dm = new YSExaminationDataDataModel();
                        dm.ID = data.ID;
                        dm.ApprovalNumber = data.ApprovalNumber;
                        dm.BillNumber = data.BillNumber;
                        dm.Conveyance = data.Conveyance;
                        dm.CustomerName = data.CustomerName;
                        dm.DeclarationNumber = data.DeclarationNumber;
                        dm.DeclarationStatus = data.DeclarationStatus;
                        dm.VoyageNumber = data.VoyageNumber;
                        dm.YSDate = data.YSDate;
                        dm.YSStatus = data.YSStatus;
                        vm.Items.Add(dm);
                    }
                    vm.UpdateIndex();
                }
            }, null);
        }

        private void btnToday_Click(object sender, RoutedEventArgs e)
        {
            DateTime end = DateTime.Today.AddDays(1);
            DateTime start = DateTime.Today;
            LoadDataByDate(start, end);
        }

        private void btnYesterday_Click(object sender, RoutedEventArgs e)
        {
            DateTime end = DateTime.Today;
            DateTime start = DateTime.Today.AddDays(-1);
            LoadDataByDate(start, end);
        }

        private void btnTheDayBeforeYesterday_Click(object sender, RoutedEventArgs e)
        {
            DateTime end = DateTime.Today.AddDays(-1);
            DateTime start = DateTime.Today.AddDays(-2);
            LoadDataByDate(start, end);
        }

        private void btnSetDeclarationStatus_Click(object sender, RoutedEventArgs e)
        {
            if (gdYS.SelectedItems.Count > 0)
            {
                List<int> ids = new List<int>();
                foreach (YSExaminationDataDataModel item in gdYS.SelectedItems)
                {
                    ids.Add(item.ID);
                }

                SetDeclarationStatus form = new SetDeclarationStatus(ids.ToArray());
                form.ShowDialog();
            }
        }
    }
}
