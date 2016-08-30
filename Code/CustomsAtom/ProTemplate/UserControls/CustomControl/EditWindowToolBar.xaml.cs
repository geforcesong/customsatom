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

namespace ProTemplate.UserControls
{
    public partial class EditWindowToolBar : UserControl
    {
        public EditWindowToolBar()
        {
            InitializeComponent();
        }

        public bool IsNew
        {
            get { return btnSaveAndNew.Visibility == Visibility.Visible; }
            set
            {
                if (!value)
                {
                    btnSaveAndNew.Visibility = Visibility.Collapsed;
                }
            }
        }

        public bool CanSave
        {
            set
            {
                btnSaveAndClose.IsEnabled = btnSaveAndNew.IsEnabled = value;
            }
        }

        public event EventHandler SaveAndNew;
        public event EventHandler SaveAndClose;
        public event EventHandler Close;

        private void btnSaveAndNew_Click(object sender, RoutedEventArgs e)
        {
            if (SaveAndNew != null)
            {
                SaveAndNew(this, new EventArgs());
            }
        }

        private void btnSaveAndClose_Click(object sender, RoutedEventArgs e)
        {
            if (SaveAndClose != null)
            {
                SaveAndClose(this, new EventArgs());
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            if (Close != null)
            {
                Close(this, new EventArgs());
            }
        }
    }
}
