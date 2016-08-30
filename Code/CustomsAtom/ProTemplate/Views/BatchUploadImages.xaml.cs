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

namespace ProTemplate.Views
{
    public partial class BatchUploadImages : Page
    {
        public BatchUploadImages()
        {
            InitializeComponent();
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void btnProcessUploadedImages_Click(object sender, RoutedEventArgs e)
        {
            CommonUIFunction.SetApplcationBusyIndicator(true, "正在处理，请稍候...");
            SystemConfiguration.Instance.DataContext.ProcessUploadedImages((a) => {

                CommonUIFunction.SetApplcationBusyIndicator(false);
                if (a.HasError)
                {
                    a.MarkErrorAsHandled();
                    MessageBox.Show(a.Error.Message);
                }
                else
                {
                    CommonUIFunction.ShowMessageBox("图片处理全部成功");
                }
            }, null);
        }

    }
}
