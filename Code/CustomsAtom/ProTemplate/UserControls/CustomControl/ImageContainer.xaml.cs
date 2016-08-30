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

namespace ProTemplate.UserControls.CustomControl
{
    public partial class ImageContainer : UserControl
    {
        public event EventHandler ImageDeleted;
        public event EventHandler DownloadClicked;

        public int Sequence
        {
            get;
            set;
        }

        public string ImageName { get; set; }

        public ImageContainer()
        {
            InitializeComponent();
        }

        public void SetImage(Image img)
        {
            LayoutRoot.Children.Add( img);
        }

        public void SetToReadOnly()
        {
            btnDelete.Visibility = System.Windows.Visibility.Collapsed;
        }

        public void SetDownloadPath(string url)
        {
            downLink.Tag = url;
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = System.Windows.Visibility.Collapsed;
            try
            {
                Web.DeclarationImage realItem = null;
                if (this.Sequence > 0)
                {
                    realItem = (from t in SystemConfiguration.Instance.DataContext.DeclarationImages
                                where t.Sequence == Sequence
                                select t).SingleOrDefault();
                }
                else
                {
                    realItem = (from t in SystemConfiguration.Instance.DataContext.DeclarationImages
                                where t.ScanImageName == ImageName
                                select t).SingleOrDefault();
                }
                if (realItem != null)
                {
                    SystemConfiguration.Instance.DataContext.DeclarationImages.Remove(realItem);
                }
                if (ImageDeleted != null)
                    ImageDeleted(this, null);
            }
            catch
            {
                CommonUIFunction.ShowMessageBox("无法删除图片");
            }
        }

        private void downLink_Click(object sender, RoutedEventArgs e)
        {
            if (DownloadClicked != null)
                DownloadClicked(sender, e);
        }
    }
}
