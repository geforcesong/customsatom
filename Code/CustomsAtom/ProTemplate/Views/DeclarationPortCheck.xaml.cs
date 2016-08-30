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
using ProTemplate.ViewModels;
using ProTemplate.Utility;
using ProTemplate.Models;

namespace ProTemplate.Views
{
    public partial class DeclarationPortCheck : Page
    {
        public DeclarationPortCheck()
        {
            InitializeComponent();
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            SystemConfiguration.Instance.DataContext.Declarations.Clear();
            DeclarationPortCheckViewModel vm = ViewModelManager.DeclarationPortCheckViewModelInstance;
            if (vm != null)
                vm.Items.Clear();
        }

        

    }
}
