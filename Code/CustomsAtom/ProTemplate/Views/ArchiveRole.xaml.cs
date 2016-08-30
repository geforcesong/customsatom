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
using Telerik.Windows.Controls;
using ProTemplate.Utility;
using ProTemplate.Models;

namespace ProTemplate.Views
{
    public partial class ArchiveRole : Page
    {
        private string declarationNumber = "123";

        public ArchiveRole()
        {
            InitializeComponent();
            
        }
        private void SetBrowseFilter()
        {
            if (this.RadUpload1 != null)
            {
                this.RadUpload1.Filter = "Image Files (*.gif;*.jpg;*.jpeg;*.png)|*.gif;*.jpg;*.jpeg;*.png|Text Files (*.txt)|*.txt|All Files(*.*)|*.*";
                this.RadUpload1.FilterIndex = 0;
            }
        }
        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            SetBrowseFilter();
            //this.RadUpload1.TargetPhysicalFolder = "c:\\123";
            this.RadUpload1.TargetFolder = "UserUploads/123";
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
        }
        private void RadUpload1_FileUploadStarting(object sender, Telerik.Windows.Controls.FileUploadStartingEventArgs e)
        {
            // Pass a new parameter to the server handler
            e.FileParameters.Add("MyParam1", declarationNumber);
        }

        private void RadUpload1_FileUploaded(object sender, Telerik.Windows.Controls.FileUploadedEventArgs e)
        {
            // Get the value of the returned Parameter from the server
            //ServerReturnedValue.Text = e.HandlerData.CustomData["MyServerParam1"].ToString();
        }

        private void RadUpload1_UploadStarted(object sender, UploadStartedEventArgs e)
        {
            this.declarationNumber = "123";
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            DeclarationInputModel md = new DeclarationInputModel();
            md.ReceivedDate = DateTime.Now;
            ProTemplate.UserControls.RadWindows.TestWindow wnd = new UserControls.RadWindows.TestWindow(md);
            wnd.ShowDialog();
        }
    }
}
