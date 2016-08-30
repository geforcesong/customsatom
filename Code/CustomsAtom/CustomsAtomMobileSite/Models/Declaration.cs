using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;

namespace CustomsAtomMobileSite.Models
{
    public class Declaration : ModelBase
    {
        public int Index { get; set; }
        public int ID { get; set; }
        public string DeclarationNumber { get; set; }
        public DateTime? ReceivedDate { get; set; }
        public string ReceivedDateStr { get; set; }
        public string CustomerName{ get; set; }
        public string ManualNumber{ get; set; }
        public string TraderName{ get; set; }
        public string Conveyance{ get; set; }
        public string VoyageNumber { get; set; }
        public string BillNumber{ get; set; }
        public string TradeName{ get; set; }
        public string ApprovalNumber{ get; set; }
        public int? PackageAmount { get; set; }
        public decimal? GrossWeight { get; set; }
        public string PrerecordWarehouseWarrant { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string DeclarationStatus{ get; set; }
        public string DeclarationStatusRemark { get; set; }
        public DateTime? DeclarationDate { get; set; }
        public string DrawbackStatus { get; set; }
        public string DrawbackStatusRemark { get; set; }
        public DateTime? DrawbackDate { get; set; }
        public string VerificationStatus { get; set; }
        public string VerificationStatusDetail { get; set; }
        public int TotalItems { get; set; }
        public int TotalContainers { get; set; }
        public string Dock { get; set; }
        public DateTime? ShipLeaveDate { get; set; }
        public string RelatedSystemNumber { get; set; }
        public string Remark { get; set; }
        public string LadingStatus { get; set; }
        public string AdmissionStatus { get; set; }
        public string HasExamination { get; set; }
        public string ExaminationNumber { get; set; }
        public int BillCount { get; set; }

        public static List<Declaration> GetDeclarations(int userID, string filter)
        {
            SqlConnection connection = new SqlConnection(ConnectionString);
            SqlCommand selectCMD = connection.CreateCommand();
            selectCMD.CommandText = "GetAllDeclarationByReceiveDate";
            selectCMD.CommandType = System.Data.CommandType.StoredProcedure;
            SqlParameter[] param = new SqlParameter[2];
            param[0] = new SqlParameter("@UserId", System.Data.SqlDbType.Int);
            param[0].Value = userID;
            param[1] = new SqlParameter("@Condition", System.Data.SqlDbType.NVarChar);
            param[1].Value = filter;

            for (int i = 0; i < param.Length; i++)
                selectCMD.Parameters.Add(param[i]);

            SqlDataReader sdr = null;
            try
            {
                connection.Open();
                sdr = selectCMD.ExecuteReader();
                return GetValidDeclarationList(sdr);
            }
            catch(Exception exp)
            {
                return null;
            }
            finally
            {
                if (sdr != null && !sdr.IsClosed)
                    sdr.Close();
                if (connection.State != ConnectionState.Closed)
                    connection.Close();
            }
        }

        public static List<Declaration> GetDeclarations(int userID, DateTime start, DateTime end)
        {
            SqlConnection connection = new SqlConnection(ConnectionString);
            SqlCommand selectCMD = connection.CreateCommand();
            selectCMD.CommandText = "GetAllDeclarationByReceiveDate";
            selectCMD.CommandType = System.Data.CommandType.StoredProcedure;
            SqlParameter[] param = new SqlParameter[3];
            param[0] = new SqlParameter("@UserId", System.Data.SqlDbType.Int);
            param[0].Value = userID;
            param[1] = new SqlParameter("@From", System.Data.SqlDbType.DateTime);
            param[1].Value = start;
            param[2] = new SqlParameter("@To", System.Data.SqlDbType.DateTime);
            param[2].Value = end;

            for (int i = 0; i < param.Length; i++)
                selectCMD.Parameters.Add(param[i]);

            SqlDataReader sdr = null;
            try
            {
                connection.Open();
                sdr = selectCMD.ExecuteReader();
                return GetValidDeclarationList(sdr);
            }
            catch
            {
                return null;
            }
            finally
            {
                if (sdr != null && !sdr.IsClosed)
                    sdr.Close();
                if (connection.State != ConnectionState.Closed)
                    connection.Close();
            }
        }

        private static List<Declaration> GetValidDeclarationList(SqlDataReader sdr)
        {
            if (sdr == null)
                return null;
            List<Declaration> lst = new List<Declaration>();
            int count = 0;
            while (sdr.Read())
            {
                count++;
                Declaration d = new Declaration();
                d.Index = count;
                d.ID = Convert.ToInt32(sdr["ID"]);
                d.ReceivedDate = sdr["ReceivedDate"] == DBNull.Value ? (DateTime?)null : DateTime.Parse(sdr["ReceivedDate"].ToString());
                d.ReceivedDateStr = "";
                if (d.ReceivedDate.HasValue)
                {
                    DateTime dt = d.ReceivedDate.Value;
                    d.ReceivedDateStr = string.Format("{0}年{1:D2}月{2:D2}日 {3:D2}:{4:D2}:{5:D2}", dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second);
                }
                d.DeclarationNumber = sdr["DeclarationNumber"] == DBNull.Value ? "" : sdr["DeclarationNumber"].ToString();
                d.CustomerName = sdr["CustomerName"] == DBNull.Value ? "" : sdr["CustomerName"].ToString();
                d.ManualNumber = sdr["ManualNumber"] == DBNull.Value ? "" : sdr["ManualNumber"].ToString();
                d.TraderName = sdr["TraderName"] == DBNull.Value ? "" : sdr["TraderName"].ToString();
                d.Conveyance = sdr["Conveyance"] == DBNull.Value ? "" : sdr["Conveyance"].ToString();
                d.VoyageNumber = sdr["VoyageNumber"] == DBNull.Value ? "" : sdr["VoyageNumber"].ToString();
                d.BillNumber = sdr["BillNumber"] == DBNull.Value ? "" : sdr["BillNumber"].ToString();
                d.TradeName = sdr["TradeName"] == DBNull.Value ? "" : sdr["TradeName"].ToString();
                d.ApprovalNumber = sdr["ApprovalNumber"] == DBNull.Value ? "" : sdr["ApprovalNumber"].ToString();
                d.PackageAmount = sdr["PackageAmount"] == DBNull.Value ? 0 : Convert.ToInt32(sdr["PackageAmount"]);
                d.GrossWeight = sdr["GrossWeight"] == DBNull.Value ? 0 : Convert.ToDecimal(sdr["GrossWeight"]);
                d.PrerecordWarehouseWarrant = sdr["PrerecordWarehouseWarrant"] == DBNull.Value ? "" : sdr["PrerecordWarehouseWarrant"].ToString();
                d.CreatedDate = sdr["CreatedDate"] == DBNull.Value ? (DateTime?)null : DateTime.Parse(sdr["CreatedDate"].ToString());
                d.DeclarationStatus = sdr["DeclarationStatus"] == DBNull.Value ? "" : sdr["DeclarationStatus"].ToString();
                d.DeclarationStatusRemark = sdr["DeclarationStatusRemark"] == DBNull.Value ? "" : sdr["DeclarationStatusRemark"].ToString();
                d.DeclarationDate = sdr["DeclarationDate"] == DBNull.Value ? (DateTime?)null : DateTime.Parse(sdr["DeclarationDate"].ToString());
                d.DrawbackStatus = sdr["DrawbackStatus"] == DBNull.Value ? "" : sdr["DrawbackStatus"].ToString();
                d.DrawbackStatusRemark = sdr["DrawbackStatusRemark"] == DBNull.Value ? "" : sdr["DrawbackStatusRemark"].ToString();
                d.DrawbackDate = sdr["DrawbackDate"] == DBNull.Value ? (DateTime?)null : DateTime.Parse(sdr["DrawbackDate"].ToString());
                d.VerificationStatus = sdr["VerificationStatus"] == DBNull.Value ? "" : sdr["VerificationStatus"].ToString();
                d.VerificationStatusDetail = sdr["VerificationStatusDetail"] == DBNull.Value ? "" : sdr["VerificationStatusDetail"].ToString();
                d.TotalItems = sdr["TotalItems"] == DBNull.Value ? 0 : Convert.ToInt32(sdr["TotalItems"]);
                //d.TotalContainers = sdr["TotalContainers"] == DBNull.Value ? 0 : Convert.ToInt32(sdr["TotalContainers"]);
                d.Dock = sdr["Dock"] == DBNull.Value ? "" : sdr["Dock"].ToString();
                d.ShipLeaveDate = sdr["ShipLeaveDate"] == DBNull.Value ? (DateTime?)null : DateTime.Parse(sdr["ShipLeaveDate"].ToString());
                //d.RelatedSystemNumber = sdr["RelatedSystemNumber"] == DBNull.Value ? "" : sdr["RelatedSystemNumber"].ToString();
                //d.Remark = sdr["Remark"] == DBNull.Value ? "" : sdr["Remark"].ToString();
                d.LadingStatus = sdr["LadingStatus"] == DBNull.Value ? "" : sdr["LadingStatus"].ToString();
                d.AdmissionStatus = sdr["AdmissionStatus"] == DBNull.Value ? "" : sdr["AdmissionStatus"].ToString();
                //d.HasExamination = sdr["HasExamination"] == DBNull.Value ? "" : sdr["HasExamination"].ToString();
                d.ExaminationNumber = sdr["ExaminationNumber"] == DBNull.Value ? "" : sdr["ExaminationNumber"].ToString();
                //d.BillCount = sdr["BillCount"] == DBNull.Value ? 0 : Convert.ToInt32(sdr["BillCount"]);
                lst.Add(d);
            }
            return lst;
        }

    }
}