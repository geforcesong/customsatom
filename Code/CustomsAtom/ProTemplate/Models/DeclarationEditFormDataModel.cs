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
    public class DeclarationEditFormDataModel : DataModel
    {
        // 基本
        public int ID { get; set; }
        public int CustomerID { get; set; }
        public string CustomerName { get; set; }
        public DateTime ReceivedDate { get; set; }
        public string DeclarationNumber { get; set; }
        public string PreEntryNumber { get; set; } //预录入编号
        public string BillNumber { get; set; } //提运单号
        public string ApprovalNumber { get; set; } //批准文号
        public string Dock { get; set; } //码头
        public bool IsShangJian { get; set; } //商检
        public int Liandan { get; set; } //联单
        public string RelatedSystemNumber { get; set; } //运编号
        public DateTime? ShipLeaveDate { get; set; } //离港日期
        public string Remark { get; set; } // 注释

        public string DeclarationStatus { get; set; }
        public string DeclarationStatusRemark { get; set; }
        public DateTime? DeclarationDate { get; set; }

        public string DrawbackStatus { get; set; }
        public string DrawbackStatusRemark { get; set; }
        public DateTime? DrawbackDate { get; set; }

        public string VerificationStatus { get; set; }
        public string VerificationStatusDetail { get; set; }

        public int TotalItems { get; set; }// 品名数量
        public int TotalContainers { get; set; }//集装箱数量

        public string AdmissionStatus { get; set; } //放行状态
        public string LadingStatus { get; set; } //进港状态
        public string OnBoardingStatus { get; set; } //上船状态

        //报关
        public string CustomhouseName { get; set; }
        public DateTime? IEDate { get; set; }
        public string TransportName { get; set; }
        public string VoyageNumber { get; set; } //航次号
        public string TradeName { get; set; }
        public string LevyName { get; set; } //征免性质
        public string PayName { get; set; }
        public string CountryName { get; set; }
        public string PortName { get; set; }
        public string DistrictName { get; set; }
        public string TransactionName { get; set; }
        public string FeeMarkTransName { get; set; }
        public string CurrencyTransName { get; set; }
        public decimal FeeTrans { get; set; }
        public string FeeMarkInsureName { get; set; }
        public string CurrencyInsureName { get; set; }
        public decimal FeeInsure { get; set; }
        public string FeeMarkOtherName { get; set; }
        public string CurrencyOtherName { get; set; }
        public decimal FeeOther { get; set; }

        public int PackageAmount { get; set; }
        public decimal GrossWeight { get; set; }
        public decimal NetWeight { get; set; }
        public string ContainerQuantity { get; set; }
        public DateTime? CreateDate { get; set; }
        public string WrapName { get; set; }

        public string ManualNumber { get; set; } //备案号
        public string ControlNumber { get; set; } //合同编号
        public string LicenseNumber { get; set; } //许可证号
        public string Conveyance { get; set; } //运输工具
        public string Note { get; set; } //备注
        public string TraderCode { get; set; } //经营单位编号
        public string TraderName { get; set; } // 经营单位
        public string OwnerCode { get; set; } // 发货单位
        public string OwnerName { get; set; }
        public string AgentCode { get; set; } //申报单位
        public string AgentName { get; set; }

        public string FinancialRemark { get; set; }
    }
}
