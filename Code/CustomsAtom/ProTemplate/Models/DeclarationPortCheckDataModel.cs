using System;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;

namespace ProTemplate.Models
{
    public class DeclarationPortCheckDataModel : DataModel
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
        private bool _checkItem = true;
        public bool CheckedItem { get { return _checkItem; } set { _checkItem = value; NotifyPropertyChanged("CheckedItem"); } }

        public int ID { get; set; }
        public string CustomerName { get; set; }
        public string DeclarationNumber { get; set; }

        //提运单号
        private string _billNumber;
        public string BillNumber
        {
            get { return _billNumber; }
            set {
                _billNumber = value;
                NotifyPropertyChanged("BillNumber");
            }
        }

        //核销单号
        private string _approvalNumber;
        public string ApprovalNumber
        {
            get { return _approvalNumber; }
            set
            {
                _approvalNumber = value;
                NotifyPropertyChanged("ApprovalNumber");
            }
        }

        // 港区
        private string _dock;
        public string Dock
        {
            get { return _dock; }
            set
            {
                _dock = value;
                NotifyPropertyChanged("Dock");
            }
        }

        //校验港区
        private string _verifyDock;
        public string VerifyDock
        {
            get { return _verifyDock; }
            set
            {
                _verifyDock = value;
                NotifyPropertyChanged("VerifyDock");
            }
        }

        // 预配仓单
        private string _prerecordWarehouseWarrant;
        public string PrerecordWarehouseWarrant
        {
            get { return _prerecordWarehouseWarrant; }
            set
            {
                _prerecordWarehouseWarrant = value;
                NotifyPropertyChanged("PrerecordWarehouseWarrant");
            }
        }

        //二放信息
        private string _erFangInfomation;
        public string ErFangInfomation
        {
            get { return _erFangInfomation; }
            set
            {
                _erFangInfomation = value;
                NotifyPropertyChanged("ErFangInfomation");
            }
        }

        // 校验状态
        private string _verifyStatus;
        public string VerifyStatus
        {
            get { return _verifyStatus; }
            set
            {
                _verifyStatus = value;
                NotifyPropertyChanged("VerifyStatus");
            }
        }

        //失败信息
        private string _failedInformation;
        public string FailedInformation
        {
            get { return _failedInformation; }
            set
            {
                _failedInformation = value;
                NotifyPropertyChanged("FailedInformation");
            }
        }

        //箱号
        private string _boxNumber = "";
        public string BoxNumber
        {
            get { return _boxNumber; }
            set
            {
                _boxNumber = value;
                NotifyPropertyChanged("BoxNumber");
            }
        }

        private string _netBoxNumber = "";
        public string NetBoxNumber
        {
            get { return _netBoxNumber; }
            set
            {
                _netBoxNumber = value;
                NotifyPropertyChanged("NetBoxNumber");
            }
        }

        //件数
        private string _packageNumber = "";
        public string PackageNumber
        {
            get { return _packageNumber; }
            set
            {
                _packageNumber = value;
                NotifyPropertyChanged("PackageNumber");
            }
        }

        private string _netPackageNumber = "";
        public string NetPackageNumber
        {
            get { return _netPackageNumber; }
            set
            {
                _netPackageNumber = value;
                NotifyPropertyChanged("NetPackageNumber");
            }
        }

        //毛重(公斤)
        private string _grossWeight = "";
        public string GrossWeight
        {
            get { return _grossWeight; }
            set
            {
                _grossWeight = value;
                NotifyPropertyChanged("GrossWeight");
            }
        }

        private string _netGrossWeight = "";
        public string NetGrossWeight
        {
            get { return _netGrossWeight; }
            set
            {
                _netGrossWeight = value;
                NotifyPropertyChanged("NetGrossWeight");
            }
        }

        //箱量
        private string _boxCount = "";
        public string BoxCount
        {
            get { return _boxCount; }
            set
            {
                _boxCount = value;
                NotifyPropertyChanged("BoxCount");
            }
        }

        private string _netBoxCount = "";
        public string NetBoxCount
        {
            get { return _netBoxCount; }
            set
            {
                _netBoxCount = value;
                NotifyPropertyChanged("NetBoxCount");
            }
        }

        //运输工具
        private string _conveyance = "";
        public string Conveyance
        {
            get { return _conveyance; }
            set
            {
                _conveyance = value;
                NotifyPropertyChanged("Conveyance");
            }
        }

        private string _netConveyance = "";
        public string NetConveyance
        {
            get { return _netConveyance; }
            set
            {
                _netConveyance = value;
                NotifyPropertyChanged("NetConveyance");
            }
        }
        //航次号
        private string _voyageNumber = "";
        public string VoyageNumber
        {
            get { return _voyageNumber; }
            set
            {
                _voyageNumber = value;
                NotifyPropertyChanged("VoyageNumber");
            }
        }

        private string _netVoyageNumber = "";
        public string NetVoyageNumber
        {
            get { return _netVoyageNumber; }
            set
            {
                _netVoyageNumber = value;
                NotifyPropertyChanged("NetVoyageNumber");
            }
        }

        //离港时间
        private string _leaveTime = "";
        public string LeaveTime
        {
            get { return _leaveTime; }
            set
            {
                _leaveTime = value;
                NotifyPropertyChanged("LeaveTime");
            }
        }

        public void UpdateVerifyStatus()
        {
            string errInfo = "";

            if (!BoxNumber.Equals(NetBoxNumber, StringComparison.OrdinalIgnoreCase))
                errInfo += "箱号,";

            if (!PackageNumber.Equals(NetPackageNumber, StringComparison.OrdinalIgnoreCase))
                errInfo += "件数,";

            if (!GrossWeight.Equals(NetGrossWeight.Contains(".") ? NetGrossWeight.Substring(0, NetGrossWeight.IndexOf('.')) : NetGrossWeight, StringComparison.OrdinalIgnoreCase))
                errInfo += "毛重,";

            if (!BoxCount.Equals(NetBoxCount, StringComparison.OrdinalIgnoreCase))
                errInfo += "箱量,";
            if (!Conveyance.Equals(NetConveyance, StringComparison.OrdinalIgnoreCase))
                errInfo += "运输工具,";

            if (!VoyageNumber.Equals(NetVoyageNumber, StringComparison.OrdinalIgnoreCase))
                errInfo += "航次号,";

            if (string.IsNullOrEmpty(errInfo))
            {
                VerifyStatus = "校验成功";
                FailedInformation = "";
            }
            else
            {
                VerifyStatus = "校验失败";
                FailedInformation = errInfo.TrimEnd(',');
            }
        }
    }
}
