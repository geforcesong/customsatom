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
using ProTemplate.Utility;
using System.ServiceModel.DomainServices.Client;
using ProTemplate.Models;

namespace ProTemplate.ViewModels
{
    public class ViewModelManager
    {
        #region ViewModelInstance
        public static CustomerViewModel CustomerViewModelInstance {
            get
            {
                return App.Current.Resources["CustomerViewModel"] as CustomerViewModel;
            }
        }

        public static FeeTypeViewModel FeeTypeViewModelInstance
        {
            get
            {
                return App.Current.Resources["FeeTypeViewModel"] as FeeTypeViewModel;
            }
        }

        public static CustomhouseViewModel CustomhouseViewModelInstance { get { return App.Current.Resources["CustomhouseViewModel"] as CustomhouseViewModel; } }
        public static CountryViewModel CountryViewModelInstance { get { return App.Current.Resources["CountryViewModel"] as CountryViewModel; } }
        public static CurrencyViewModel CurrencyViewModelInstance { get { return App.Current.Resources["CurrencyViewModel"] as CurrencyViewModel; } }
        public static DistrictViewModel DistrictViewModelInstance { get { return App.Current.Resources["DistrictViewModel"] as DistrictViewModel; } }
        public static DocumentViewModel DocumentViewModelInstance { get { return App.Current.Resources["DocumentViewModel"] as DocumentViewModel; } }
        public static DocumentTypeViewModel DocumentTypeViewModelInstance { get { return App.Current.Resources["DocumentTypeViewModel"] as DocumentTypeViewModel; } }
        public static DutyViewModel DutyViewModelInstance { get { return App.Current.Resources["DutyViewModel"] as DutyViewModel; } }
        public static FeeMarkViewModel FeeMarkViewModelInstance { get { return App.Current.Resources["FeeMarkViewModel"] as FeeMarkViewModel; } }
        public static LevyViewModel LevyViewModelInstance { get { return App.Current.Resources["LevyViewModel"] as LevyViewModel; } }
        public static PayViewModel PayViewModelInstance { get { return App.Current.Resources["PayViewModel"] as PayViewModel; } }
        public static PortViewModel PortViewModelInstance { get { return App.Current.Resources["PortViewModel"] as PortViewModel; } }
        public static PurposeViewModel PurposeViewModelInstance { get { return App.Current.Resources["PurposeViewModel"] as PurposeViewModel; } }
        public static TradeViewModel TradeViewModelInstance { get { return App.Current.Resources["TradeViewModel"] as TradeViewModel; } }
        public static TransactionViewModel TransactionViewModelInstance { get { return App.Current.Resources["TransactionViewModel"] as TransactionViewModel; } }
        public static TransportViewModel TransportViewModelInstance { get { return App.Current.Resources["TransportViewModel"] as TransportViewModel; } }
        public static UnitViewModel UnitViewModelInstance { get { return App.Current.Resources["UnitViewModel"] as UnitViewModel; } }
        public static WrapViewModel WrapViewModelInstance { get { return App.Current.Resources["WrapViewModel"] as WrapViewModel; } }
        public static DeclarationItemViewModel DeclarationItemViewModelInstance { get { return App.Current.Resources["DeclarationItemViewModel"] as DeclarationItemViewModel; } }
        public static DeclarationDocumentViewModel DeclarationDocumentViewModelInstance { get { return App.Current.Resources["DeclarationDocumentViewModel"] as DeclarationDocumentViewModel; } }
        public static DeclarationContainerViewModel DeclarationContainerViewModelInstance { get { return App.Current.Resources["DeclarationContainerViewModel"] as DeclarationContainerViewModel; } }
        public static GetAllDeclarationByReceiveDateResultViewModel GetAllDeclarationByReceiveDateResultViewModelInstance { get { return App.Current.Resources["GetAllDeclarationByReceiveDateResultViewModel"] as GetAllDeclarationByReceiveDateResultViewModel; } }
        public static GetAllFinancialExportDeclarationViewModel GetAllFinancialExportDeclarationViewModelInstance { get { return App.Current.Resources["GetAllFinancialExportDeclarationViewModel"] as GetAllFinancialExportDeclarationViewModel; } }
        public static ExaminationViewModel ExaminationViewModelInstance { get { return App.Current.Resources["ExaminationViewModel"] as ExaminationViewModel; } }
        public static DoubleCheckDeclarationVarifyViewModel DoubleCheckDeclarationVarifyViewModelInstance { get { return App.Current.Resources["DoubleCheckDeclarationVarifyViewModel"] as DoubleCheckDeclarationVarifyViewModel; } }
        public static CustomsUserQueryViewModel CustomsUserQueryViewModelInstance { get { return App.Current.Resources["CustomsUserQueryViewModel"] as CustomsUserQueryViewModel; } }
        public static YSExaminationDataViewModel YSExaminationDataViewModelInstance { get { return App.Current.Resources["YSExaminationDataViewModel"] as YSExaminationDataViewModel; } }
        public static DeclarationPortCheckViewModel DeclarationPortCheckViewModelInstance { get { return App.Current.Resources["DeclarationPortCheckViewModel"] as DeclarationPortCheckViewModel; } }
        public static FinancialExportDeclarationViewModel FinancialExportDeclarationViewModelInstance { get { return App.Current.Resources["FinancialExportDeclarationViewModel"] as FinancialExportDeclarationViewModel; } }
        public static HSCodeDictionaryViewModel HSCodeDictionaryViewModelInstance { get { return App.Current.Resources["HSCodeDictionaryViewModel"] as HSCodeDictionaryViewModel; } }
        public static MachineViewModel MachineViewModelInstance { get { return App.Current.Resources["MachineViewModel"] as MachineViewModel; } }
        #endregion

        public static void ResetCustomerViewModel()
        {
            SystemConfiguration.Instance.DataContext.Customers.Clear();
            SystemConfiguration.Instance.DataContext.Load(SystemConfiguration.Instance.DataContext.GetCustomerQuery(), delegate(LoadOperation<Web.Customer> lp)
            {
                CustomerViewModel cvm = CustomerViewModelInstance;
                if (cvm == null)
                    return;
                cvm.Items.Clear();
                foreach (var q in lp.Entities)
                {
                    CustomerDataModel customerMD = new CustomerDataModel();
                    customerMD.ID = q.ID;
                    customerMD.Name = q.Name;
                    customerMD.PinYin = q.PinYin;
                    customerMD.PhoneNumber = q.PhoneNumber;
                    customerMD.Address = q.Address;
                    customerMD.BossName = q.Boss.Name;
                    cvm.Items.Add(customerMD);
                }
                cvm.UpdateIndex();
                SystemConfiguration.Instance.DataContext.Customers.Clear();
            }, null);
        }

        public static void ResetFeeTypeViewModel()
        {
            SystemConfiguration.Instance.DataContext.FeeTypes.Clear();
            SystemConfiguration.Instance.DataContext.Load(SystemConfiguration.Instance.DataContext.GetFeeTypeQuery(), delegate(LoadOperation<Web.FeeType> lp)
            {
                FeeTypeViewModel cvm = FeeTypeViewModelInstance;
                if (cvm == null)
                    return;
                cvm.Items.Clear();
                foreach (var q in lp.Entities)
                {
                    FeeTypeDataModel feetypeMD = new FeeTypeDataModel();
                    feetypeMD.Name = q.Name;
                    feetypeMD.Amount = q.Amount;
                    feetypeMD.Code = q.Code;
                    feetypeMD.Cost = q.Cost;
                    cvm.Items.Add(feetypeMD);
                }
                SystemConfiguration.Instance.DataContext.FeeTypes.Clear();
            }, null);
        }

        public static void ResetCountryViewModel()
        {
            CountryViewModel cvm = App.Current.Resources["CountryViewModel"] as CountryViewModel;
            if (cvm != null)
            {
                cvm.Load();
            }
        }

        public static void ResetCurrencyViewModel()
        {
            CurrencyViewModel cvm = App.Current.Resources["CurrencyViewModel"] as CurrencyViewModel;
            if (cvm != null)
            {
                cvm.Load();
            }
        }

        public static void ResetCustomhouseViewModel()
        {
            CustomhouseViewModel cvm = App.Current.Resources["CustomhouseViewModel"] as CustomhouseViewModel;
            if (cvm != null)
            {
                cvm.Load();
            }
        }

        public static void ResetDistrictViewModel()
        {
            DistrictViewModel cvm = App.Current.Resources["DistrictViewModel"] as DistrictViewModel;
            if (cvm != null)
            {
                cvm.Load();
            }
        }

        public static void ResetDocumentViewModel()
        {
            DocumentViewModel cvm = App.Current.Resources["DocumentViewModel"] as DocumentViewModel;
            if (cvm != null)
            {
                cvm.Load();
            }
        }

        public static void ResetDocumentTypeViewModel()
        {
            DocumentTypeViewModel cvm = App.Current.Resources["DocumentTypeViewModel"] as DocumentTypeViewModel;
            if (cvm != null)
            {
                cvm.Load();
            }
        }

        public static void ResetDutyViewModel()
        {
            DutyViewModel cvm = App.Current.Resources["DutyViewModel"] as DutyViewModel;
            if (cvm != null)
            {
                cvm.Load();
            }
        }

        public static void ResetFeeMarkViewModel()
        {
            FeeMarkViewModel cvm = App.Current.Resources["FeeMarkViewModel"] as FeeMarkViewModel;
            if (cvm != null)
            {
                cvm.Load();
            }
        }

        public static void ResetLevyViewModel()
        {
            LevyViewModel cvm = App.Current.Resources["LevyViewModel"] as LevyViewModel;
            if (cvm != null)
            {
                cvm.Load();
            }
        }

        public static void ResetPayViewModel()
        {
            PayViewModel cvm = App.Current.Resources["PayViewModel"] as PayViewModel;
            if (cvm != null)
            {
                cvm.Load();
            }
        }

        public static void ResetPortViewModel()
        {
            PortViewModel cvm = App.Current.Resources["PortViewModel"] as PortViewModel;
            if (cvm != null)
            {
                cvm.Load();
            }
        }

        public static void ResetPurposeViewModel()
        {
            PurposeViewModel cvm = App.Current.Resources["PurposeViewModel"] as PurposeViewModel;
            if (cvm != null)
            {
                cvm.Load();
            }
        }

        public static void ResetTradeViewModel()
        {
            TradeViewModel cvm = App.Current.Resources["TradeViewModel"] as TradeViewModel;
            if (cvm != null)
            {
                cvm.Load();
            }
        }

        public static void ResetTransportViewModel()
        {
            TransportViewModel cvm = App.Current.Resources["TransportViewModel"] as TransportViewModel;
            if (cvm != null)
            {
                cvm.Load();
            }
        }

        public static void ResetUnitViewModel()
        {
            UnitViewModel cvm = App.Current.Resources["UnitViewModel"] as UnitViewModel;
            if (cvm != null)
            {
                cvm.Load();
            }
        }

        public static void ResetWrapViewModel()
        {
            WrapViewModel cvm = App.Current.Resources["WrapViewModel"] as WrapViewModel;
            if (cvm != null)
            {
                cvm.Load();
            }
        }

        public static void ResetHSCodeDictionaryViewModelViewModel()
        {
            HSCodeDictionaryViewModel cvm = App.Current.Resources["HSCodeDictionaryViewModel"] as HSCodeDictionaryViewModel;
            if (cvm != null)
            {
                cvm.Load();
            }
        }

        public static void ResetTransactionViewModel()
        {
            TransactionViewModel cvm = App.Current.Resources["TransactionViewModel"] as TransactionViewModel;
            if (cvm != null)
            {
                cvm.Load();
            }
        }
    }
}
