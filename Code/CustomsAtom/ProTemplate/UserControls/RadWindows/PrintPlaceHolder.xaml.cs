using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using Telerik.Windows.Controls;

namespace ProTemplate.UserControls.RadWindows
{
    /// <summary>
    /// Interaction logic for PrintPlaceHolder.xaml
    /// </summary>
    public partial class PrintPlaceHolder
    {
        public PrintPlaceHolder()
        {
            InitializeComponent();
            
        }
        public void FormLoad(object sender, EventArgs e)
        {
            hp.SourceUrl = new Uri("/Report/ReportForm.aspx?Report=ExportDeclarationForm&content=" + SQL, UriKind.Relative);
        }
        public string SQL { get; set; }
    }
}
