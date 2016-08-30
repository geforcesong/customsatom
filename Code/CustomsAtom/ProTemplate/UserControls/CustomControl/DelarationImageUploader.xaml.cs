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
using System.Windows.Media.Imaging;
using System.Windows.Browser;

namespace ProTemplate.UserControls.CustomControl
{
    public partial class DelarationImageUploader : UserControl
    {
        public DelarationImageUploader()
        {
            InitializeComponent();
            HasImageUpdated = false;
        }

        public bool HasImageUpdated { get;set; }

        private int _currentDeclarationID;
        public int CurrentDeclarationID
        {
            get { return _currentDeclarationID; }
            set
            {
                _currentDeclarationID = value;
                imgUploader.TargetFolder = string.Format("UserUploads/{0}", _currentDeclarationID);
                LoadExistingImages();
            }
        }

        public void SetToReadOnly()
        {
            imgUploader.IsEnabled = false;
            foreach (var a in rpImages.Children)
            {
                ((ImageContainer)a).SetToReadOnly();
            }
        }

        void LoadExistingImages()
        {
            var realItem = (from t in SystemConfiguration.Instance.DataContext.Declarations
                            where t.ID == CurrentDeclarationID
                            select t).SingleOrDefault();
            if (realItem != null)
            {
                foreach (var img in realItem.DeclarationImage)
                {
                    AddImageToGallary(img.Sequence,img.ScanImageName);
                }
            }
        }

        private void RadUpload1_FileUploaded(object sender, Telerik.Windows.Controls.FileUploadedEventArgs e)
        {
            //设置标志位
            if (!HasImageUpdated)
                HasImageUpdated = true;

            Web.DeclarationImage imgObj = new Web.DeclarationImage();
            imgObj.DeclarationId = CurrentDeclarationID;
            imgObj.ScanImageName = e.SelectedFile.Name;
            var realItem = (from t in SystemConfiguration.Instance.DataContext.Declarations
                            where t.ID == CurrentDeclarationID
                            select t).SingleOrDefault();
            if (realItem != null)
                realItem.DeclarationImage.Add(imgObj);

            AddImageToGallary(imgObj.Sequence, e.SelectedFile.Name);
        }

        void AddImageToGallary(int sequence, string fileName)
        {
            Uri uri = ConstructAbsoluteUri(new Uri(this.imgUploader.UploadServiceUrl, UriKind.RelativeOrAbsolute));
            string imageURL = uri.AbsoluteUri.Remove(uri.AbsoluteUri.LastIndexOf("/")) +
                "/" + imgUploader.TargetFolder + "/" + fileName;
            Image imgObj = new Image();
            BitmapImage bmp = new BitmapImage(new Uri(imageURL, UriKind.RelativeOrAbsolute));
            imgObj.Source = bmp;
            imgObj.Stretch = Stretch.Fill;
            imgObj.Cursor = Cursors.Hand;
            imgObj.Tag = imgUploader.TargetFolder + "/" + fileName;
            imgObj.MouseLeftButtonDown += new MouseButtonEventHandler(imgObj_MouseLeftButtonDown);
            ToolTipService.SetToolTip(imgObj,string.Format("点击下载{0}",fileName));
            ImageContainer ic = new CustomControl.ImageContainer();
            ic.SetDownloadPath(imgObj.Tag.ToString());
            ic.Sequence = sequence;
            ic.ImageName = fileName;
            ic.SetImage(imgObj);
            ic.ImageDeleted += (a, b) =>
                {
                    HasImageUpdated = true;
                };

            ic.DownloadClicked += (a, b) =>
                                      {
                                          HyperlinkButton hb = a as HyperlinkButton;
                                          if(hb!=null)
                                          {
                                              string url = string.Format("../ashx/DownloadImages.ashx?FilePath={0}", hb.Tag.ToString());
                                              HtmlPage.Window.Invoke("OpenWindow", url);
                                          }
                                      };
            rpImages.Children.Add(ic);
        }

        void imgObj_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 1)
            {
                string url = string.Format("../ashx/DownloadImages.ashx?FilePath={0}", ((Image)sender).Tag.ToString());
                HtmlPage.Window.Invoke("OpenWindow", url);
            }
        }

        private static Uri ConstructAbsoluteUri(Uri url)
        {
            if (!url.IsAbsoluteUri)
            {
                System.Uri source = System.Windows.Application.Current.Host.Source;
                string server = source.AbsoluteUri.Remove(source.AbsoluteUri.Length - source.AbsolutePath.Length);
                int serverLen = server.Length;
                string relativePath = url.OriginalString;
                const string PathSeparator = "/";

                if (relativePath.StartsWith(PathSeparator, StringComparison.OrdinalIgnoreCase))
                {
                    //// ; nothing to do - just continue!
                }
                else if (relativePath.StartsWith("~/", StringComparison.OrdinalIgnoreCase))
                {
                    relativePath = relativePath.Substring(1);
                }
                else if (relativePath.StartsWith("./", StringComparison.OrdinalIgnoreCase))
                {
                    relativePath = relativePath.Remove(0, 1);
                    server = source.AbsoluteUri.Remove(source.AbsoluteUri.LastIndexOf(PathSeparator, StringComparison.OrdinalIgnoreCase));
                }
                else if (relativePath.StartsWith("../", StringComparison.OrdinalIgnoreCase))
                {
                    server = RemoveLastNode(source.AbsoluteUri, PathSeparator, serverLen);
                    while (relativePath.StartsWith("../", StringComparison.OrdinalIgnoreCase))
                    {
                        relativePath = relativePath.Remove(0, 3);
                        server = RemoveLastNode(server, PathSeparator, serverLen);
                    }
                    server += PathSeparator;
                }
                else
                {
                    server += PathSeparator;
                }
                url = new Uri(server + relativePath, UriKind.Absolute);
            }
            return url;
        }

        private static string RemoveLastNode(string path, string separator, int stopAt)
        {
            int i = path.LastIndexOf(separator, StringComparison.OrdinalIgnoreCase);

            if (i < stopAt)
            {
                i = stopAt;
            }

            if (i < path.Length)
            {
                if (i <= 0)
                {
                    path = string.Empty;
                }
                else if (i > 0)
                {
                    path = path.Remove(i);
                }
            }

            return path;
        }

    }
}
