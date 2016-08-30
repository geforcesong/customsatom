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
using System.Collections.Generic;
using System.Linq;
using System.Collections;

namespace ProTemplate.Models
{
    public class DataModel : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        public event PropertyChangedEventHandler PropertyChanged;

        //存放错误信息，一个Property可能对应多个错误信息        
        private Dictionary<string, List<string>> errors = new Dictionary<string, List<string>>();

        //实现INotifyDataErrorInfo        
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;        
        public IEnumerable GetErrors(string propertyName)        
        {            
            if (string.IsNullOrEmpty(propertyName))            
            {                
                return null;            
            }            
            else            
            {                
                if (errors.ContainsKey(propertyName))                
                {                    
                    return errors[propertyName];                
                }           
            }            
            return null;        
        }        
        public bool HasErrors        
        {            
            get            
            {                
                return (errors.Count > 0);            
            }        
        }
        public void SetErrors(string propertyName, List<string> propertyErrors) 
        { 
            errors.Remove(propertyName); errors.Add(propertyName, propertyErrors); 
            if (ErrorsChanged != null) 
            { 
                ErrorsChanged(this, new DataErrorsChangedEventArgs(propertyName)); 
            } 
        }        
        public void ClearErrors(string propertyName) 
        { 
            errors.Remove(propertyName); 
            if (ErrorsChanged != null) 
            { 
                ErrorsChanged(this, new DataErrorsChangedEventArgs(propertyName)); 
            } 
        }

        public void NotifyErrorsChanged(string propertyName)
        {
            if (ErrorsChanged != null)
                ErrorsChanged(this, new DataErrorsChangedEventArgs(propertyName));
        }


        protected void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class ValidationErrorInfo
    {
        public int ErrorCode { get; set; }

        public string ErrorMessage { get; set; }

        public override string ToString()
        {
            return ErrorMessage;
        }
    }
}
