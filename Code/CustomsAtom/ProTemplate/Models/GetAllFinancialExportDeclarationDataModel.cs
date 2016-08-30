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
    public class GetAllFinancialExportDeclarationDataModel : DataModel
    {
        public GetAllFinancialExportDeclarationDataModel()
        {
            IsLocked = false;
        }

        public int Index { get; set; }
        public int ID { get; set; }

        private string _customerName;
        public string CustomerName
        {
            get { return _customerName; }
            set
            {
                if (!IsLocked)
                    _customerName = value;
                NotifyPropertyChanged("CustomerName");
            }
        }
        public DateTime? ReceivedDate { get; set; }

        private string _declarationNumber;
        public string DeclarationNumber 
        {
            get { return _declarationNumber; }
            set
            {
                if (!IsLocked)
                    _declarationNumber = value;
                NotifyPropertyChanged("DeclarationNumber");
            }
        }
        public string TraderName { get; set; }
        public string Conveyance { get; set; }
        public string VoyageNumber { get; set; }
        public int? PackageAmount { get; set; }
        public decimal? GrossWeight { get; set; }
        public DateTime? DeclarationDate { get; set; }
        public string Dock { get; set; }
        public string RelatedSystemNumber { get; set; }
        private string _billNumber;
        public string BillNumber 
        {
            get { return _billNumber; }
            set
            {
                if (!IsLocked)
                    _billNumber = value;
                NotifyPropertyChanged("BillNumber");
            }
        }
        private string _approvalNumber;
        public string ApprovalNumber
        {
            get { return _approvalNumber; }
            set
            {
                if (!IsLocked)
                    _approvalNumber = value;
                NotifyPropertyChanged("ApprovalNumber");
            }
        }
        public string DeclarationStatus { get; set; }
        public string DrawbackStatus { get; set; }
        public DateTime? ShipLeaveDate { get; set; }
        public int BillCount { get; set; }
        public string Remark { get; set; }
        public string ExaminationNumber { get; set; }
        public string HasExamination { get; set; }

        private decimal? _declarationFeeAmount;
        public decimal? DeclarationFeeAmount
        {
            get { return _declarationFeeAmount; }
            set
            {
                if (!IsLocked)
                    _declarationFeeAmount = value;
                NotifyPropertyChanged("DeclarationFeeAmount");
            }
        }
        public decimal? DeclarationFeePaid { get; set; }
        private decimal? _declarationFeeCost;
        public decimal? DeclarationFeeCost
        {
            get { return _declarationFeeCost; }
            set
            {
                if (!IsLocked)
                    _declarationFeeCost = value;
                NotifyPropertyChanged("DeclarationFeeCost");
            }
        }
        public string DeclarationFeeStatus { get; set; }
        private decimal? _examinationFeeAmount;
        public decimal? ExaminationFeeAmount
        {
            get { return _examinationFeeAmount; }
            set
            {
                if (!IsLocked)
                    _examinationFeeAmount = value;
                NotifyPropertyChanged("ExaminationFeeAmount");
            }
        }
        public decimal? ExaminationFeePaid { get; set; }
        private decimal? _examinationFeeCost;
        public decimal? ExaminationFeeCost
        {
            get { return _examinationFeeCost; }
            set
            {
                if (!IsLocked)
                    _examinationFeeCost = value;
                NotifyPropertyChanged("ExaminationFeeCost");
            }
        }
        public string ExaminationFeeStatus { get; set; }
        private decimal? _checkFeeAmount;
        public decimal? CheckFeeAmount
        {
            get { return _checkFeeAmount; }
            set
            {
                if (!IsLocked)
                    _checkFeeAmount = value;
                NotifyPropertyChanged("CheckFeeAmount");
            }
        }
        public decimal? CheckFeePaid { get; set; }
        private decimal? _checkFeeCost;
        public decimal? CheckFeeCost
        {
            get { return _checkFeeCost; }
            set
            {
                if (!IsLocked)
                    _checkFeeCost = value;
                NotifyPropertyChanged("CheckFeeCost");
            }
        }
        public string CheckFeeStatus { get; set; }
        public decimal? ModificationFeeAmount { get; set; }
        public decimal? ModificationFeePaid { get; set; }
        public decimal? ModificationFeeCost { get; set; }
        public string ModificationFeeStatus { get; set; }

        private decimal? _commissionFeeAmount;
        public decimal? CommissionFeeAmount
        {
            get { return _commissionFeeAmount; }
            set
            {
                if (!IsLocked)
                    _commissionFeeAmount = value;
                NotifyPropertyChanged("CommissionFeeAmount");
            }
        }
        public decimal? CommissionFeePaid { get; set; }
        private decimal? _commissionFeeCost;
        public decimal? CommissionFeeCost
        {
            get { return _commissionFeeCost; }
            set
            {
                if (!IsLocked)
                    _commissionFeeCost = value;
                NotifyPropertyChanged("CommissionFeeCost");
            }
        }
        public string CommissionFeeStatus { get; set; }
        private decimal? _billFeeAmount;
        public decimal? BillFeeAmount
        {
            get { return _billFeeAmount; }
            set
            {
                if (!IsLocked)
                    _billFeeAmount = value;
                NotifyPropertyChanged("BillFeeAmount");
            }
        }
        public decimal? BillFeePaid { get; set; }
        public decimal? BillFeeCost { get; set; }
        public string BillFeeStatus { get; set; }
        public decimal? OtherFeeAmount { get; set; }
        public decimal? OtherFeePaid { get; set; }
        public decimal? OtherFeeCost { get; set; }
        public string OtherFeeStatus { get; set; }
        public string FinancialRemark { get; set; }
        public bool IsLocked { get; set; }
        public string ContainerNumbers { get; set; }
    }
}
