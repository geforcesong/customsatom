﻿using System;
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
using System.Linq;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.ServiceModel.DomainServices.Client;

namespace ProTemplate.ViewModels
{
    public class FeeMarkViewModel
    {
        ObservableCollection<FeeMarkDataModel> _items = new ObservableCollection<FeeMarkDataModel>();
        private string _version = "NAN";

        public string GetFeeMarkName(string code)
        {
            if (_items == null)
                return "";
            else
            {
                var query = (from c in _items
                             where c.Code == code
                             select c).SingleOrDefault();
                if (query != null)
                    return query.Name;
                else
                    return ""; ;
            }
        }

        public ObservableCollection<FeeMarkDataModel> Items
        {
            get { return _items; }
            set
            {
                _items = value;
            }
        }


        public string GetFeeMarkCode(string name)
        {
            if (string.IsNullOrEmpty(name))
                return "";
            else
            {
                var query = (from c in _items
                             where c.Name == name
                             select c).SingleOrDefault();
                if (query != null)
                    return query.Code;
                else
                    return name;
            }
        }


        public void SaveToIsolatedStorage()
        {
            _version = (from a in SystemConfiguration.Instance.Settings
                        where a.Name == ObjectKeys.FeeMarkDataKey
                        select a.StringValue).FirstOrDefault();
            //
            System.Diagnostics.Debug.Assert(!string.IsNullOrEmpty(_version));
            if (IsolatedStorageManager.Instance.FileExists(ObjectKeys.FeeMarkDataKey, _version))
            {
                //找到，说明本地跟数据库中的一样，不用更新
                return;
            }
            else
            {
                // 没找到，需要从数据库更新
                SystemConfiguration.Instance.DataContext.Load(SystemConfiguration.Instance.DataContext.GetFeeMarkQuery(), delegate(LoadOperation<Web.FeeMark> lp)
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
                            FeeMarkDataModel cdm = new FeeMarkDataModel();
                            cdm.Code = c.Code;
                            cdm.Name = c.Name;
                            cdm.PinYin = c.PinYin;
                            Items.Add(cdm);
                        }

                        Serilize(_version);
                        // 删除，释放资源
                        SystemConfiguration.Instance.DataContext.Countries.Clear();
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
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(ObservableCollection<FeeMarkDataModel>));
            ser.WriteObject(ms, _items);
            byte[] array = ms.ToArray();
            ms.Close();
            string _serializeString = Encoding.UTF8.GetString(array, 0, array.Length);
            //保存数据
            IsolatedStorageManager.Instance.Save(ObjectKeys.FeeMarkDataKey, _serializeString,version);
        }

        private void DeSerilize(string version)
        {
            string dataJson = IsolatedStorageManager.Instance.GetFileContent(ObjectKeys.FeeMarkDataKey, version);
            if (string.IsNullOrEmpty(dataJson))
            {
                _items = new ObservableCollection<FeeMarkDataModel>();
                return;
            }
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(dataJson));
            DataContractJsonSerializer ser1 = new DataContractJsonSerializer(typeof(ObservableCollection<FeeMarkDataModel>));
            var dataObj = ser1.ReadObject(ms);
            _items = dataObj as ObservableCollection<FeeMarkDataModel>;
        }
    }
}
