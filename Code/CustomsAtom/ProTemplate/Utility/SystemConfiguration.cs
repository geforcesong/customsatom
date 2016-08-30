using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Reflection;
using System.Xml.Linq;
using System.Linq;
using System.IO;
using ProTemplate.Web.DMServices;
using ProTemplate.UserControls;
using System.Collections.Generic;
using ProTemplate.Models;
using System.Text;
using System.Runtime.Serialization.Json;

namespace ProTemplate.Utility
{
    public class SystemConfiguration
    {
        private SystemConfiguration(string assemblyName) 
        {
            _assemblyName = assemblyName;
            _dataContext = App.Current.Resources["CustomsAtomContext"] as CustomsAtomContext;
        }

        private static readonly SystemConfiguration _instance;
        public static SystemConfiguration Instance { get { return _instance; } }

        static SystemConfiguration()
        {
            string [] info =Assembly.GetExecutingAssembly().FullName.Split(',');
            _instance = new SystemConfiguration(info[0]);
            _instance._appName = "Customer Atom";
            //设置RadGridView本地化
            Telerik.Windows.Controls.LocalizationManager.Manager = new RadGridViewResourceManager();
        }

        private string _assemblyName = "";
        public string AssemblyName { get { return _assemblyName; } }

        public IEnumerable<int> CustomerIDList = null;

        private CustomsAtomContext _dataContext = null;
        public CustomsAtomContext DataContext { get { return _dataContext; } }

        private string _appName = "";
        public string ApplicationName { get { return _appName; } }

        public CommonToolBar SystemToolBar { get; set; }

        // 登录用户
        private ProTemplate.Models.UserDataModel _loggedOnUser = null;
        public ProTemplate.Models.UserDataModel LoggedOnUser { get { return _loggedOnUser; } }
        public void SetLoggedOnUser(ProTemplate.Models.UserDataModel user)
        {
            if (user == null)
                throw new Exception("登录用户不能被设置为空！");
            _loggedOnUser = user;
        }

        // Settings
        private List<SettingDataModel> _settings = new List<SettingDataModel>();
        public List<SettingDataModel> Settings
        {
            get { return _settings; }
        }
        public void UpdateSetting(List<SettingDataModel> lst)
        {
            if (lst == null)
                throw new Exception("系统设置列表不能为空");
            _settings = lst;
        }

        public string ClientIP { get; set; }
    }
}
