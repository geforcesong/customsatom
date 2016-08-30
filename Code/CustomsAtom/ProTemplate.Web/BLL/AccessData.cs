using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.OleDb;
using System.Data;
using ProTemplate.Web.Utility;

namespace ProTemplate.Web.BLL
{
    public class AccessData
    {
        private string _connectionString;
        public AccessData(string ip)
        {
            if (string.IsNullOrEmpty(ip))
                _connectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=e:\customdata2004.mdb;";
            else
                _connectionString = string.Format(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=\\{0}\Easipass\PlugIn\CUSDECI\Data\customdata2004.mdb",ip);
        }

        public DoubleCheckDeclaration GetDoubleCheckDeclaration(string declarationNumber, string approvalNumber)
        {
            DoubleCheckDeclaration dcd = GetDoubleCheckDeclarationFromAccess(declarationNumber, approvalNumber);
            if (dcd != null)
            {
                List<DoubleCheckDeclarationItem> items = GetDoubleCheckDeclarationItems(dcd.DeclarationNumber);
                if (items != null)
                {
                    foreach (var i in items)
                        dcd.DoubleCheckDeclarationItem.Add(i);
                }
            }
            return dcd;
        }

        private DoubleCheckDeclaration GetDoubleCheckDeclarationFromAccess(string declarationNumber, string approvalNumber)
        {
            OleDbConnection connection = new OleDbConnection(_connectionString);
            OleDbCommand command = connection.CreateCommand();
            command.CommandText = "SELECT Top 1 pre_entry_id as DeclarationNumber, "
                                 + " appr_no as ApprovalNumber, "
                                 + " trans_mode as TransactionCode, "
                                 + " district_code as DistrictCode, "
                                 + " trade_mode as TradeCode, "
                                 + " fee_rate as FreightFeeRate, "
                                 + " fee_curr as FreightFeeCurrency, "
                                 + " insur_rate as InsuranceFeeRate, "
                                 + " insur_curr as InsuranceFeeCurrency, "
                                 + " i_e_port as CustomhouseCode,"
                                 + " trade_country as CountryName, "
                                 + "manual_no as ManualNumber, "
                                 + "lisence_no as LicenseNumber, "
                                 + "pack_no as PackageAmount,"
                                 + "wrap_type as WrapName, "
                                 + "pay_way as PayCode, "
                                 + "contr_no as ContractNumber, "
                                 + "gross_wt as GrossWeight, "
                                 + "net_wt as NetWeight"
                //ContainerNumbers
                //DocumentCodes
                                 + " FROM Form_Head"
                                 + " Where pre_entry_id = '" + declarationNumber + "'"
                                 + " ORDER By Create_date DESC, pre_entry_id";
            OleDbDataReader odr = null;
            try
            {
                connection.Open();
                odr = command.ExecuteReader();
                if (odr.Read())
                {
                    DoubleCheckDeclaration dcd = new DoubleCheckDeclaration();
                    dcd.DeclarationNumber = odr["DeclarationNumber"].ToString().Trim();
                    dcd.ApprovalNumber = odr["ApprovalNumber"].ToString().Trim();
                    dcd.TransactionName = odr["TransactionCode"].ToString().Trim(); // Code ,need to be update to Name from SQLServer
                    dcd.DistrictName = odr["DistrictCode"].ToString().Trim();// Code ,need to be update to Name from SQLServer
                    dcd.TradeName = odr["TradeCode"].ToString().Trim();// Code ,need to be update to Name from SQLServer
                    dcd.FeightFeeRate = odr["FreightFeeRate"].ToString().Trim();
                    dcd.InsuranceFeeRate = odr["InsuranceFeeRate"].ToString().Trim();
                    dcd.CustomhouseName = odr["CustomhouseCode"].ToString().Trim();// Code ,need to be update to Name from SQLServer
                    dcd.CountryName = odr["CountryName"].ToString().Trim();
                    dcd.ManualNumber = odr["ManualNumber"].ToString().Trim();
                    dcd.LicenseNumber = odr["LicenseNumber"].ToString().Trim();
                    dcd.PackageAmount = odr["PackageAmount"].ToString().Trim();
                    dcd.WrapName = odr["WrapName"].ToString().Trim();
                    dcd.GrossWeight = odr["GrossWeight"].ToString().Trim();
                    dcd.NetWeight = odr["NetWeight"].ToString().Trim();
                    dcd.PayName = odr["PayCode"].ToString().Trim();// Code ,need to be update to Name from SQLServer
                    dcd.ContractNumber = odr["ContractNumber"].ToString();
                    dcd.ContractNumber = Encryptor.Decrypt(dcd.ContractNumber);
                    dcd.ContainerNumbers = GetContainerNumbers(dcd.DeclarationNumber);
                    dcd.FreightFeeCurrencyName = odr["FreightFeeCurrency"].ToString(); // Code ,need to be update to Name from SQLServer
                    dcd.InsuranceFeeCurrencyName = odr["InsuranceFeeCurrency"].ToString(); // Code ,need to be update to Name from SQLServer
                    string examinationNumber;
                    dcd.DocumentCodes = GetDocumentCodes(dcd.DeclarationNumber, out examinationNumber);
                    dcd.ExaminationNumber = examinationNumber;
                    return dcd;
                }
                else
                    return null;
            }
            finally
            {
                if (odr != null)
                    odr.Close();
                if (ConnectionState.Closed != connection.State)
                    connection.Close();
            }
        }

        private List<DoubleCheckDeclarationItem> GetDoubleCheckDeclarationItems(string declarationNumber)
        {
            OleDbConnection connection = new OleDbConnection(_connectionString);
            OleDbCommand command = connection.CreateCommand();
            command.CommandText = "SELECT g_no as SortNumber, code_t as HSCode, code_s as HSCodeSub, g_name as Name, qty_conv as FirstQuantity, unit_1 as FirstUnitName, qty_2 as SecondQuantity, "
                                 + "unit_2 as SecondUnitName, qty_1 as DeclaredQuantity, g_unit as DeclaredUnitName, contr_item as ControlNumber, trade_total as TotalAmount, trade_curr as CurrencyName"
                                 + " FROM Form_List"
                                 + " Where pre_entry_id = '" + declarationNumber + "'"
                                 + " ORDER By g_no ASC";
            OleDbDataReader odr = null;
            try
            {
                connection.Open();
                odr = command.ExecuteReader();
                List<DoubleCheckDeclarationItem> lst = new List<DoubleCheckDeclarationItem>();
                while (odr.Read())
                {
                    DoubleCheckDeclarationItem dcd = new DoubleCheckDeclarationItem();
                    dcd.SortOrder = Convert.ToInt32(odr["SortNumber"]);
                    dcd.HSCode = odr["HSCode"].ToString();
                    string hsCodeSub = odr["HSCodeSub"] == DBNull.Value ? "" : odr["HSCodeSub"].ToString();
                    dcd.Name = odr["Name"].ToString();
                    dcd.HSCode = Encryptor.Decrypt(dcd.HSCode);
                    hsCodeSub = Encryptor.Decrypt(hsCodeSub);
                    if (string.IsNullOrEmpty(hsCodeSub))
                        hsCodeSub = "00";
                    dcd.HSCode += hsCodeSub;
                    dcd.Name = Encryptor.Decrypt(dcd.Name);
                    dcd.ControlNumber = odr["ControlNumber"] == DBNull.Value ? "" : odr["ControlNumber"].ToString();
                    dcd.FirstUnitName = odr["FirstUnitName"] == DBNull.Value ? "" : odr["FirstUnitName"].ToString();// Code ,need to be update to Name from SQLServer
                    dcd.FirstQuantity = odr["FirstQuantity"] == DBNull.Value ? "" : odr["FirstQuantity"].ToString();
                    dcd.SecondQuantity = odr["SecondQuantity"] == DBNull.Value ? "" : odr["SecondQuantity"].ToString();
                    dcd.SecondUnitName = odr["SecondUnitName"] == DBNull.Value ? "" : odr["SecondUnitName"].ToString();// Code ,need to be update to Name from SQLServer
                    dcd.DeclaredUnitName = odr["DeclaredUnitName"] == DBNull.Value ? "" : odr["DeclaredUnitName"].ToString();// Code ,need to be update to Name from SQLServer
                    dcd.DeclaredQuantity = odr["DeclaredQuantity"] == DBNull.Value ? "" : odr["DeclaredQuantity"].ToString();
                    dcd.TotalAmount = odr["TotalAmount"] == DBNull.Value ? "" : odr["TotalAmount"].ToString();
                    dcd.CurrencyName = odr["CurrencyName"] == DBNull.Value ? "" : odr["CurrencyName"].ToString(); // Code ,need to be update to Name from SQLServer
                    if (dcd.HSCode != null)
                        dcd.HSCode.Trim();
                    if (dcd.Name != null)
                        dcd.Name.Trim();
                    lst.Add(dcd);
                }
                return lst;
            }
            finally
            {
                if (odr != null)
                    odr.Close();
                if (ConnectionState.Closed != connection.State)
                    connection.Close();
            }
        }

        private string GetContainerNumbers(string declarationNumber)
        {
            OleDbConnection connection = new OleDbConnection(_connectionString);
            OleDbCommand command = connection.CreateCommand();
            command.CommandText = "select container_no from Container_number Where pre_entry_id = '" + declarationNumber + "'";
            OleDbDataReader odr = null;
            try
            {
                System.Text.StringBuilder ret = new System.Text.StringBuilder();
                connection.Open();
                odr = command.ExecuteReader();
                while (odr.Read())
                {
                    string containNum = odr["container_no"] == DBNull.Value ? "" : odr["container_no"].ToString();
                    ret.Append(containNum + ",");
                }
                return ret.ToString().TrimEnd(',');
            }
            catch
            {
                return "";
            }
            finally
            {
                if (odr != null)
                    odr.Close();
                if (ConnectionState.Closed != connection.State)
                    connection.Close();
            }
        }

        private string GetDocumentCodes(string declarationNumber, out string examinationNumber)
        {
            OleDbConnection connection = new OleDbConnection(_connectionString);
            OleDbCommand command = connection.CreateCommand();
            command.CommandText = "select docu_code, cert_code from Cert_List Where pre_entry_id = '" + declarationNumber + "'";
            OleDbDataReader odr = null;
            examinationNumber = "";
            try
            {
                System.Text.StringBuilder ret = new System.Text.StringBuilder();
                connection.Open();
                odr = command.ExecuteReader();
                while (odr.Read())
                {
                    string containNum = odr["docu_code"] == DBNull.Value ? "" : odr["docu_code"].ToString();
                    if(containNum.ToUpper() == "B")
                        examinationNumber = odr["cert_code"] == DBNull.Value ? "" : odr["cert_code"].ToString();
                    ret.Append(containNum + ",");
                }
                return ret.ToString().TrimEnd(',');
            }
            catch
            {
                return "";
            }
            finally
            {
                if (odr != null)
                    odr.Close();
                if (ConnectionState.Closed != connection.State)
                    connection.Close();
            }
        }

    }
}