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
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Collections;
using System.Collections.Generic;

namespace ProTemplate.Models
{
    public class UserDataModel : DataModel
    {
        private int _index;
        public int Index
        {
            get
            {
                return _index;
            }
            set
            {
                _index = value;
                NotifyPropertyChanged("Index");
            }
        }

        

        //ID
        public int ID { get; set; }

        //Name
        private string _name;
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                NotifyPropertyChanged("Name");
            }
        }

        //Name
        private string _alias;
        public string Alias
        {
            get
            {
                return _alias;
            }
            set
            {
                _alias = value;
                NotifyPropertyChanged("Alias");
            }
        }

        //Name
        private string _pwd;
        public string Password
        {
            get
            {
                return _pwd;
            }
            set
            {
                _pwd = value;
                NotifyPropertyChanged("Password");
            }
        }

        private List<RoleDataModel> _roleList = null;
        public List<RoleDataModel> RoleList
        {
            get { return _roleList; }
            set 
            { 
                _roleList = value;
                NotifyPropertyChanged("DisplayRoles");
            }
        }

        public string DisplayRoles
        {
            get {
                if (RoleList == null || RoleList.Count == 0)
                    return "";
                else
                {
                    string rs = "";
                    foreach (var a in RoleList)
                        rs += a.Name +",";
                    return rs.Trim(',');
                }
            }
        }

        //Group Name
        private string _groupName;
        public string GroupName
        {
            get
            {
                return _groupName;
            }
            set
            {
                Validator.ValidateProperty(value, new ValidationContext(this, null, null) { MemberName = "GroupName" });
                _groupName = value;
                NotifyPropertyChanged("GroupName");
            }
        }

        private bool _isActive = true;
        public bool IsActive
        {
            get
            {
                return _isActive;
            }
            set
            {
                _isActive = value;
                NotifyPropertyChanged("IsActive");
            }
        }
    }
}
