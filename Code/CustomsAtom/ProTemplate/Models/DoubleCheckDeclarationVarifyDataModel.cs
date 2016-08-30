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

namespace ProTemplate.Models
{
    public class DoubleCheckDeclarationVarifyDataModel : DataModel
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
        public int ID { get; set; }
        public int DeclarationId { get; set; }
        public string CustomerName { get; set; }
        public string DeclarationNumber { get; set; }
        public string ApprovalNumber { get; set; }
        public string TransactionName { get; set; }
        public string DistrictName { get; set; }
        public string TradeName { get; set; }
        public string FeightFeeRate { get; set; }
        public string InsuranceFeeRate { get; set; }
        public string CustomhouseName { get; set; }
        public string CountryName { get; set; }
        public string ManualNumber { get; set; }
        public string LicenseNumber { get; set; }
        public string PackageAmount { get; set; }
        public string WrapName { get; set; }
        public string GrossWeight { get; set; }
        public string NetWeight { get; set; }
        public string ContainerNumbers { get; set; }
        public string DocumentCodes { get; set; }
        public string PrimaryColumn { get; set; }

        private string _varifyFlag;
        public string VarifyFlag
        {
            get
            {
                return _varifyFlag;
            }
            set
            {
                _varifyFlag = value;
                NotifyPropertyChanged("VarifyFlag");
            }
        }

        private string _varifyMsg;
        public string VarifyMsg
        {
            get
            {
                return _varifyMsg;
            }
            set
            {
                _varifyMsg = value;
                NotifyPropertyChanged("VarifyMsg");
            }
        }
    }
}
