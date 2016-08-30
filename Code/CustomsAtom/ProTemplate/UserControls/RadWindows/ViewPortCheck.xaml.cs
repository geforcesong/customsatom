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

namespace ProTemplate.UserControls.RadWindows
{
    public partial class ViewPortCheck : RadWindow
    {
        public ViewPortCheck(IEnumerable<Web.Declaration> lst)
        {
            InitializeComponent();
            if (lst != null)
                dpCheckUser.SetInitialViewModal(lst);
        }
    }
}
