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

namespace ProTemplate.UserControls
{
    public partial class DateFilterPanel : UserControl
    {
        public event EventHandler<DateSearchResultEventArgs> OnSearch;
        public event EventHandler<EventArgs> OnReset;

        public DateTime? StartDate
        {
            get { return dpStart.SelectedDate; }
            set
            {
                dpStart.SelectedDate = value;
            }
        }

        public DateTime? EndDate
        {
            get { return dpEnd.SelectedDate; }
            set
            {
                dpEnd.SelectedDate = value;
            }
        }

        public string SearchForCodeCaption
        {
            get { return rbCode.Content.ToString(); }
            set
            {
                rbCode.Content = value;
            }
        }

        public DateFilterPanel()
        {
            InitializeComponent();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidationCheck())
                return;

            if (OnSearch != null)
            {
                DateSearchResultEventArgs arg = new DateSearchResultEventArgs();
                if (rbDate.IsChecked == true)
                {
                    arg.SearchType = SearchType.SearchByDate;
                    arg.StartDate = dpStart.SelectedDate;
                    arg.EndDate = ((DateTime)dpEnd.SelectedDate).AddDays(1).AddSeconds(-1);
                }
                else
                {
                    arg.SearchType = SearchType.SearchByCode;
                    arg.Codes = tbDeclarationCodes.Text.Trim();
                }
                OnSearch(this,arg);
            }
        }

        public bool ValidationCheck()
        {
            if (rbDate.IsChecked == true)
            {
                if (dpStart.SelectedDate == null)
                {
                    CommonUIFunction.ShowMessageBox("请选择开始日期");
                    return false;
                }

                if (dpEnd.SelectedDate == null)
                {
                    CommonUIFunction.ShowMessageBox("请选择结束日期");
                    return false;
                }
            }
            else
            {
                if (string.IsNullOrEmpty(tbDeclarationCodes.Text))
                {
                    CommonUIFunction.ShowMessageBox("请输入要查询的海关编码");
                    return false;
                }
            }
            return true;
        }

        private void rbDate_Checked(object sender, RoutedEventArgs e)
        {
            if (dpStart != null)
                dpStart.IsEnabled = true;
            if(dpEnd!=null)
            dpEnd.IsEnabled = true;
            if(tbDeclarationCodes!=null)
            tbDeclarationCodes.IsEnabled = false;
        }

        private void rbDate_Unchecked(object sender, RoutedEventArgs e)
        {
            dpStart.IsEnabled = false;
            dpEnd.IsEnabled = false;
            tbDeclarationCodes.IsEnabled = true;
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            if (OnReset != null)
            {
                OnReset(sender, new EventArgs());
            }
        }
    }

    public class DateSearchResultEventArgs : EventArgs
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public SearchType SearchType { get; set; }
        public string Codes { get; set; }
    }
}
