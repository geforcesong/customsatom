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
using System.IO.IsolatedStorage;
using System.IO;

namespace ProTemplate.Utility
{
    public class IsolatedStorageManager
    {
        private IsolatedStorageManager() { }
        private static object _lockObj = new object();

        private static readonly IsolatedStorageManager _instance;
        public static IsolatedStorageManager Instance { get { return _instance; } }

        private const string _folderName = "CustomsAtomClientStorage";
        private const long _availableQuote = 9242880;

        static IsolatedStorageManager()
        {
            _instance = new IsolatedStorageManager();
        }

        public void Save(string key, string val, string version)
        {
            lock (_lockObj)
            {
                using (IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    if (!store.DirectoryExists(_folderName))
                        store.CreateDirectory(_folderName);

                    // 删除老版本的文件
                    string[] files = store.GetFileNames(_folderName + "/");
                    foreach (var file in files)
                    {
                        if (file.Contains(key))
                        {
                            string oldFilePath = System.IO.Path.Combine(_folderName, file);
                            if (store.FileExists(oldFilePath))
                                store.DeleteFile(oldFilePath);
                        }
                    }

                    string filePath = System.IO.Path.Combine(_folderName, key + "-" + version + ".txt");
                    //删除新版本的文件（可有可无，因为上面应该已经全部删除掉了）
                    if (store.FileExists(filePath))
                        store.DeleteFile(filePath);
                    // 创建文件
                    IsolatedStorageFileStream fileStream = store.CreateFile(filePath);
                    using (StreamWriter sw = new StreamWriter(fileStream))
                    {
                        sw.Write(val);
                        sw.Flush();
                    }
                    fileStream.Close();
                }
            }
        }

        //public void Delete(string key)
        //{
        //    lock (_lockObj)
        //    {
        //        if (string.IsNullOrEmpty(key))
        //            return;
        //        if (IsolatedStorageSettings.ApplicationSettings.Contains(key))
        //            IsolatedStorageSettings.ApplicationSettings.Remove(key);
        //    }
        //}

        //public bool Contains(string key)
        //{
        //    lock (_lockObj)
        //    {
        //        return IsolatedStorageSettings.ApplicationSettings.Contains(key);
        //    }
        //}

        public bool FileExists(string name, string version)
        {
            lock (_lockObj)
            {
                string filePath = System.IO.Path.Combine(_folderName, name + "-" + version + ".txt");
                using (IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    return store.FileExists(filePath);
                }
            }
        }

        //public string GetStringValue(string key)
        //{
        //    lock (_lockObj)
        //    {
        //        if (IsolatedStorageSettings.ApplicationSettings.Contains(key) && IsolatedStorageSettings.ApplicationSettings[key] != null)
        //            return IsolatedStorageSettings.ApplicationSettings[key].ToString();
        //        else
        //            return "";
        //    }
        //}

        public string GetFileContent(string name, string version)
        {
            lock (_lockObj)
            {
                string filePath = System.IO.Path.Combine(_folderName, name + "-" + version + ".txt");
                using (IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    if (!store.FileExists(filePath))
                        return null;
                    else
                    {
                        IsolatedStorageFileStream ifs = store.OpenFile(filePath, FileMode.Open, FileAccess.Read);
                        StreamReader sdr = new StreamReader(ifs);
                        string content = sdr.ReadToEnd();
                        sdr.Close();
                        ifs.Close();
                        return content;
                    }
                }
            }
        }

        public bool UpgradeStorageCapacity()
        {
            using (IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForApplication())
            {
                Int64 needSpace = 9242880;

                if (store.Quota < needSpace)
                {
                    if (!store.IncreaseQuotaTo(needSpace))
                        return false;
                }
                return true;
            }
        }

    }
}
