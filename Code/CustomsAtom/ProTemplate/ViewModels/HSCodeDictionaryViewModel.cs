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
using System.Collections.ObjectModel;
using ProTemplate.Models;
using ProTemplate.Utility;
using System.ServiceModel.DomainServices.Client;
using System.Linq;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace ProTemplate.ViewModels
{
    public class HSCodeDictionaryViewModel
    {
        ObservableCollection<HSCodeDictionaryDataModel> _items = new ObservableCollection<HSCodeDictionaryDataModel>();
        private string _version = "NAN";

        public ObservableCollection<HSCodeDictionaryDataModel> Items
        {
            get { return _items; }
            set
            {
                _items = value;
            }
        }

        public void SaveToIsolatedStorage()
        {
            //现在比较版本是否一样, 现在版本不可能为空
            _version = (from a in SystemConfiguration.Instance.Settings
                        where a.Name == ObjectKeys.HSCodeDictionaryDataKey
                        select a.StringValue).FirstOrDefault();
            //
            System.Diagnostics.Debug.Assert(!string.IsNullOrEmpty(_version));
            if (IsolatedStorageManager.Instance.FileExists(ObjectKeys.HSCodeDictionaryDataKey, _version))
            {
                //找到，说明本地跟数据库中的一样，不用更新
                return;
            }
            else
            {
                // 没找到，需要从数据库更新
                SystemConfiguration.Instance.DataContext.Load(SystemConfiguration.Instance.DataContext.GetHSCodeDictionaryQuery(), delegate(LoadOperation<Web.HSCodeDictionary> lp)
                {
                    if (lp.HasError)
                    {
                        lp.MarkErrorAsHandled();
                    }
                    else
                    {
                        _items.Clear();
                        // 更新ViewModel
                        foreach (var c in lp.Entities)
                        {
                            HSCodeDictionaryDataModel cdm = new HSCodeDictionaryDataModel();
                            cdm.ID = c.ID;
                            cdm.Code = c.Code;
                            cdm.Name = c.Name;
                            cdm.FirstUnitName = c.FirstUnitName;
                            cdm.SecondUnitName = c.SecondUnitName;
                            cdm.ManagementName = c.ManagementName;
                            cdm.DeclarationFactor = c.DeclarationFactor;
                            cdm.ExportRate = c.ExportRate;
                            cdm.DrawbackRate = c.DrawbackRate;

                            Items.Add(cdm);
                        }

                        Serilize(_version);
                        // 删除，释放资源
                        SystemConfiguration.Instance.DataContext.HSCodeDictionaries.Clear();
                    }
                }, null);

            }
        }

        public void Load()
        {
            if (_items != null && Items.Count > 0)
                return;
            else
                DeSerilize(_version);
        }

        private void Serilize(string version)
        {
            // 序列化
            if (_items == null || _items.Count == 0)
                return;
            MemoryStream ms = new MemoryStream();
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(ObservableCollection<HSCodeDictionaryDataModel>));
            ser.WriteObject(ms, _items);
            byte[] array = ms.ToArray();
            ms.Close();
            string _serializeString = Encoding.UTF8.GetString(array, 0, array.Length);
            //保存数据
            IsolatedStorageManager.Instance.Save(ObjectKeys.HSCodeDictionaryDataKey, _serializeString,version);
        }

        private void DeSerilize(string version)
        {
            string dataJson = IsolatedStorageManager.Instance.GetFileContent(ObjectKeys.HSCodeDictionaryDataKey, version);
            if (string.IsNullOrEmpty(dataJson))
            {
                _items = new ObservableCollection<HSCodeDictionaryDataModel>();
                return;
            }
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(dataJson));
            DataContractJsonSerializer ser1 = new DataContractJsonSerializer(typeof(ObservableCollection<HSCodeDictionaryDataModel>));
            var dataObj = ser1.ReadObject(ms);
            _items = dataObj as ObservableCollection<HSCodeDictionaryDataModel>;
        }
    }
}
