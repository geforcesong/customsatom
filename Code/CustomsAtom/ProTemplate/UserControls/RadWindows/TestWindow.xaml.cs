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
using ProTemplate.Models;
using ProTemplate.ViewModels;
using ProTemplate.Utility;

namespace ProTemplate.UserControls.RadWindows
{
    public partial class TestWindow : RadWindow
    {

        public TestWindow(DeclarationInputModel dataModel)
        {
            InitializeComponent();

        }
        

        private void toolBar_SaveAndClose(object sender, EventArgs e)
        {
        }

        private void toolBar_Close(object sender, EventArgs e)
        {
            this.Close();
        }

        
    }

}
