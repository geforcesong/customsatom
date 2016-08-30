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
using System.Windows.Media.Imaging;
using Telerik.Windows.Controls;
using System.Xml.Linq;
using ProTemplate.UserControls;
using ProTemplate.Web.DMServices;

namespace ProTemplate
{
    public partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void ContentFrame_Navigated(object sender, NavigationEventArgs e)
        {
            Page page = ContentFrame.Content as Page;
            if (page != null)
                this.Title = page.Title;
        }

        public object GetContentPage()
        {
            return ContentFrame.Content;
        }

        // If an error occurs during navigation, show an error window
        private void ContentFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            e.Handled = true;
            ProTemplate.UserControls.RadWindows.ErrorWindow errorWin = new ProTemplate.UserControls.RadWindows.ErrorWindow(e.Uri);
            errorWin.ShowDialog();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            LoadNavigationPanel();
        }

        private void LoadNavigationPanel()
        {
            if (SystemConfiguration.Instance.DataContext == null || SystemConfiguration.Instance.DataContext.UIGroups == null || SystemConfiguration.Instance.DataContext.UIGroups.Count == 0)
            {
                App app = App.Current as App;
                if (app != null)
                    app.RootProjectContentFrame.Navigate("/LoginPage.xaml");
                return;
            }

            var query = from q in SystemConfiguration.Instance.DataContext.UIGroups
                        where q.IsActive == true
                        select q;
            foreach (var q in query.OrderBy(o=>o.SortOrder))
                AddNavigationPanel(q);
            if (radPanelBar.Items.Count > 0)
                ((RadPanelBarItem)radPanelBar.Items[0]).IsExpanded = true;
        }

        private void AddNavigationPanel(Web.UIGroup uiGroup)
        {
            if (uiGroup == null)
                return;

            List<int> lstRoleID = (from c in SystemConfiguration.Instance.LoggedOnUser.RoleList select c.ID).ToList();

            List<int> lstUIPageID = (from c in SystemConfiguration.Instance.DataContext.RoleAccesses where lstRoleID.Contains(c.RoleId) select c.UIPageId).Distinct().ToList();
            

            
            System.Windows.Controls.ListBox lstBox = new System.Windows.Controls.ListBox();
            lstBox.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(lstBox_SelectionChanged);
            var lstUIPage = from q in SystemConfiguration.Instance.DataContext.UIPages
                        where q.UIGroupId == uiGroup.Id && lstUIPageID.Contains(q.Id)
                        select q;

            if (lstUIPage.Count() > 0)
            {
                // add header
                RadPanelBarItem rbi = new RadPanelBarItem();
                NavigationPanelHeaderItem nphi = new NavigationPanelHeaderItem();
                nphi.HeaderText = uiGroup.Name;
                nphi.IconName = uiGroup.Icon;
                rbi.Header = nphi;
                radPanelBar.Items.Add(rbi);

                // add content
                foreach (var x in lstUIPage.OrderBy(o => o.SortOrder))
                {
                    System.Windows.Controls.ListBoxItem lbi = new System.Windows.Controls.ListBoxItem();
                    NavigationLink nl = new NavigationLink();
                    nl.LinkText = x.Name;
                    nl.IconName = x.Icon;
                    nl.NavigationURL = x.NavigationURL;
                    lbi.Content = nl;
                    lstBox.Items.Add(lbi);
                }
                rbi.Items.Add(lstBox);
            }
        }

        void lstBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            //清除其他ListBox的选择
            if (sender != null)
            {
                foreach (RadPanelBarItem panel in radPanelBar.Items)
                {
                    foreach(var listbox in panel.Items)
                    {
                        System.Windows.Controls.ListBox lst = listbox as System.Windows.Controls.ListBox;
                        // 取消事件绑定
                        if(lst!= null)
                            lst.SelectionChanged -= new System.Windows.Controls.SelectionChangedEventHandler(lstBox_SelectionChanged);
                        if (lst != sender)
                            lst.SelectedIndex = -1;
                        // 还原事件绑定
                        if (lst != null)
                            lst.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(lstBox_SelectionChanged);
                    }
                }
            }

            System.Windows.Controls.ListBox lb = sender as System.Windows.Controls.ListBox;
            if (lb != null && lb.SelectedItem != null)
            {
                NavigationLink nl = ((System.Windows.Controls.ListBoxItem)lb.SelectedItem).Content as NavigationLink;
                if (nl != null)
                {
                    // Access judgement
                    //if (SystemConfiguration.Instance.IsValidAccessPath("Customer", nl.NavigationURL))
                        ContentFrame.Navigate(new Uri(nl.NavigationURL, UriKind.Relative));
                    //else
                    //{
                    //    MessageBox.Show("您无权访问！");
                    //    lb.SelectedIndex = -1;
                    //}
                }
            }
        }
    }
}
