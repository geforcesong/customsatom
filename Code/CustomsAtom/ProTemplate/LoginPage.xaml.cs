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
using ProTemplate.Web.DMServices;
using System.ServiceModel.DomainServices.Client;
using ProTemplate.Models;
using ProTemplate.Utility;
using ProTemplate.ViewModels;
using System.IO.IsolatedStorage;

namespace ProTemplate
{
    public partial class LoginPage : Page
    {
        public bool _isShowUs = false;
        IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;

        public LoginPage()
        {
            InitializeComponent();
            VisualStateManager.GoToState(this, "Status1", true);
            if (settings.Contains("UserName"))
            {
                tbName.Text = settings["UserName"].ToString();
            }
            if (settings.Contains("Password"))
            {
                tbPwd.Password = settings["Password"].ToString();
                cbRememberMe.IsChecked = true;
            }
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            
            LoadSettings();
            
            SystemConfiguration.Instance.DataContext.Load(SystemConfiguration.Instance.DataContext.GetUIGroupQuery());
            SystemConfiguration.Instance.DataContext.Load(SystemConfiguration.Instance.DataContext.GetUIPageQuery());
            SystemConfiguration.Instance.DataContext.Load(SystemConfiguration.Instance.DataContext.GetRoleAccessQuery());
            SystemConfiguration.Instance.DataContext.Load(SystemConfiguration.Instance.DataContext.GetRoleQuery());
            SystemConfiguration.Instance.DataContext.Load(SystemConfiguration.Instance.DataContext.GetBossQuery());
            SystemConfiguration.Instance.DataContext.Load(SystemConfiguration.Instance.DataContext.GetUserGroupQuery());
            SystemConfiguration.Instance.DataContext.Load(SystemConfiguration.Instance.DataContext.GetMachineNameIPMappingQuery(), delegate(LoadOperation<Web.MachineNameIPMapping> lp)
            {
                MachineViewModel cvm = ViewModelManager.MachineViewModelInstance;
                if (cvm == null)
                    return;
                cvm.Items.Clear();
                foreach (var q in lp.Entities)
                {
                    ProTemplate.Models.MachineDataModel MachineMD = new Models.MachineDataModel();
                    MachineMD.ID = q.ID;
                    MachineMD.MachineName = q.MachineName;
                    MachineMD.MachineIP = q.MachineIP;
                    cvm.Items.Add(MachineMD);
                }
                cvm.UpdateIndex();
            }, null);
        }

        private void LoadSettings()
        {
            SystemConfiguration.Instance.DataContext.Load(SystemConfiguration.Instance.DataContext.GetSettingsQuery(), delegate(LoadOperation<Web.Setting> lp)
            {
                if (lp.HasError)
                {
                    lp.MarkErrorAsHandled();
                    MessageBox.Show("获取系统配置信息出错，请刷新重试！" + lp.Error.Message);
                }
                else
                {
                    List<SettingDataModel> sList = new List<SettingDataModel>();
                    foreach (var s in lp.Entities)
                    {
                        SettingDataModel sdm = new SettingDataModel();
                        sdm.Name = s.Name;
                        sdm.StringValue = s.StringValue;
                        sdm.DoubleValue = s.DoubleValue;
                        sdm.IntValue = s.IntValue;
                        sdm.DateValue = s.DateValue;
                        sList.Add(sdm);
                    }
                    SystemConfiguration.Instance.UpdateSetting(sList);
                    SaveViewModelsToIsolatedStorage();
                }
                                                                             
            }, null);
        }

        private void SaveViewModelsToIsolatedStorage()
        {
            // 保存CountryViewModel
            CountryViewModel cvm = App.Current.Resources["CountryViewModel"] as CountryViewModel;
            if (cvm != null)
                cvm.SaveToIsolatedStorage();

            // 保存CurrencyViewModel
            CurrencyViewModel cuvm = App.Current.Resources["CurrencyViewModel"] as CurrencyViewModel;
            if (cuvm != null)
                cuvm.SaveToIsolatedStorage();

            // 保存CustomhouseViewModel
            CustomhouseViewModel chvm = App.Current.Resources["CustomhouseViewModel"] as CustomhouseViewModel;
            if (chvm != null)
                chvm.SaveToIsolatedStorage();

            // 保存DistrictViewModel
            DistrictViewModel dvm = App.Current.Resources["DistrictViewModel"] as DistrictViewModel;
            if (dvm != null)
                dvm.SaveToIsolatedStorage();

            // 保存DocumentViewModel
            DocumentViewModel docvm = App.Current.Resources["DocumentViewModel"] as DocumentViewModel;
            if (docvm != null)
                docvm.SaveToIsolatedStorage();

            // 保存DocumentTypeViewModel
            DocumentTypeViewModel docTVM = App.Current.Resources["DocumentTypeViewModel"] as DocumentTypeViewModel;
            if (docTVM != null)
                docTVM.SaveToIsolatedStorage();

            // 保存DutyViewModel
            DutyViewModel dutyVM = App.Current.Resources["DutyViewModel"] as DutyViewModel;
            if (dutyVM != null)
                dutyVM.SaveToIsolatedStorage();

            // 保存FeeMarkViewModel
            FeeMarkViewModel fmVM = App.Current.Resources["FeeMarkViewModel"] as FeeMarkViewModel;
            if (fmVM != null)
                fmVM.SaveToIsolatedStorage();

            // 保存LevyViewModel
            LevyViewModel levyVM = App.Current.Resources["LevyViewModel"] as LevyViewModel;
            if (levyVM != null)
                levyVM.SaveToIsolatedStorage();

            // 保存PayViewModel
            PayViewModel payVM = App.Current.Resources["PayViewModel"] as PayViewModel;
            if (payVM != null)
                payVM.SaveToIsolatedStorage();

            // 保存PortViewModel
            PortViewModel ptVM = App.Current.Resources["PortViewModel"] as PortViewModel;
            if (ptVM != null)
                ptVM.SaveToIsolatedStorage();

            // 保存PurposeViewModel
            PurposeViewModel purVM = App.Current.Resources["PurposeViewModel"] as PurposeViewModel;
            if (purVM != null)
                purVM.SaveToIsolatedStorage();

            // 保存TradeViewModel
            TradeViewModel tradeVM = App.Current.Resources["TradeViewModel"] as TradeViewModel;
            if (tradeVM != null)
                tradeVM.SaveToIsolatedStorage();

            // 保存TransactionViewModel
            TransactionViewModel tsaVM = App.Current.Resources["TransactionViewModel"] as TransactionViewModel;
            if (tsaVM != null)
                tsaVM.SaveToIsolatedStorage();

            // 保存TransportViewModel
            TransportViewModel transVM = App.Current.Resources["TransportViewModel"] as TransportViewModel;
            if (transVM != null)
                transVM.SaveToIsolatedStorage();

            // 保存UnitViewModel
            UnitViewModel utVM = App.Current.Resources["UnitViewModel"] as UnitViewModel;
            if (utVM != null)
                utVM.SaveToIsolatedStorage();

            // 保存WrapViewModel
            WrapViewModel wrapVM = App.Current.Resources["WrapViewModel"] as WrapViewModel;
            if (wrapVM != null)
                wrapVM.SaveToIsolatedStorage();

            
        }

        bool InputCheck()
        {
            if (string.IsNullOrEmpty(tbName.Text))
            {
                CommonUIFunction.ShowMessageText(bdMsg, "请输入登录名称！", true);
                return false;
            }
            if (string.IsNullOrEmpty(tbPwd.Password))
            {
                CommonUIFunction.ShowMessageText(bdMsg, "请输入登录密码！", true);
                return false;
            }
            return true;
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            if (!IsolatedStorageManager.Instance.UpgradeStorageCapacity())
            {
                CommonUIFunction.ShowMessageBox("您必须允许升级本地存储容量才可以使用系统!");
                return;
            }

            // 保存HSCodeDictionaryViewModel
            HSCodeDictionaryViewModel hsdVM = App.Current.Resources["HSCodeDictionaryViewModel"] as HSCodeDictionaryViewModel;
            if (hsdVM != null)
                hsdVM.SaveToIsolatedStorage();

            if (!InputCheck())
                return;
            SystemConfiguration.Instance.DataContext.Load(SystemConfiguration.Instance.DataContext.LoginQuery(tbName.Text, tbPwd.Password), delegate(LoadOperation<Web.User> lp)
                {
                    if (lp.HasError)
                    {
                        lp.MarkErrorAsHandled();
                    }
                    else
                    {
                        if (lp.Entities.Count() == 0)
                        {
                            CommonUIFunction.ShowMessageText(bdMsg, "用户名密码输入错误，请重新输入！", true);
                            return;
                        }
                        else
                        {
                            //登录成功
                            var user = lp.Entities.First();

                            if (settings.Contains("UserName"))
                            {
                                settings["UserName"] = tbName.Text;
                            }
                            else
                            {
                                settings.Add("UserName", tbName.Text);
                            }

                            if (cbRememberMe.IsChecked.Value)
                            {
                                if (settings.Contains("Password"))
                                {
                                    settings["Password"] = tbPwd.Password;
                                }
                                else
                                {
                                    settings.Add("Password", tbPwd.Password);
                                }
                                
                            }
                            else
                            {
                                if (settings.Contains("Password"))
                                {
                                    settings.Remove("Password");
                                }
                            }

                            SystemConfiguration.Instance.DataContext.GetCustomerID(user.Id, lo => 
                            {
                                if (!lo.HasError)
                                {
                                    SystemConfiguration.Instance.CustomerIDList = lo.Value;
                                }
                                else
                                {
                                    SystemConfiguration.Instance.CustomerIDList = new List<int>();
                                }
                            }, null);
                            //记录登录历史

                            Web.LoginHistory lh = new Web.LoginHistory();
                            lh.UserName = user.Alias;
                            lh.LoginIP = Application.Current.Host.InitParams["ClientIP"];
                            lh.LoginDate = DateTime.Now;
                            lh.IsShow = true;

                            SystemConfiguration.Instance.DataContext.LoginHistories.Add(lh);

                            SystemConfiguration.Instance.DataContext.SubmitChanges();

                            UserDataModel udm = new UserDataModel();
                            udm.ID = user.Id;
                            udm.Name = user.Name;
                            udm.Alias = user.Alias;
                            udm.GroupName = user.UserGroup.GroupName;
                            List<RoleDataModel> lstRole = new List<RoleDataModel>();
                            foreach (var r in user.UserRole)
                            {
                                RoleDataModel rd = new RoleDataModel();
                                rd.ID = r.RoleId;
                                rd.Name = r.Role.Name;
                                lstRole.Add(rd);
                            }
                            udm.RoleList = lstRole;
                            ProTemplate.Utility.SystemConfiguration.Instance.SetLoggedOnUser(udm);
                            App app = App.Current as App;
                            if (app != null)
                                app.RootProjectContentFrame.Navigate("/MainPage.xaml");
                        }
                    }
                }, null);
        }

        private void showUs_Click(object sender, RoutedEventArgs e)
        {
            if (!_isShowUs)
                VisualStateManager.GoToState(this, "Status2", true);  
            else
                VisualStateManager.GoToState(this, "Status1", true);  
            _isShowUs = !_isShowUs;
        }

        private void SendEmailHyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Browser.HtmlPage.Window.Invoke("SendEmailTo", ((HyperlinkButton)sender).Content.ToString());
        }

    }
}
