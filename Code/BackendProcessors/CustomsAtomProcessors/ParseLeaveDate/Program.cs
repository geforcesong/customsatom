using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;
using ProTemplate.Web.DataCrawler;
using ProcessorUtilities;

namespace ParseLeaveDate
{
    class Program
    {
        private static readonly SqlConnection conn = new SqlConnection("Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=CustomsAtom;Data Source=localhost");
        private static List<ExportDelcaration> lstExportDeclaration = new List<ExportDelcaration>();

        static void Main(string[] args)
        {
            conn.Open();
            //ClearOldData();
            ParseRootPage();
        }
        private static void ParseRootPage()
        {
            try
            {
                string sql = string.Format("select Conveyance, VoyageNumber, Dock,DeclarationNumber  from Declaration where (DeclarationStatus != '报关完成' AND DeclarationStatus != '查验' AND DeclarationStatus != '退关' AND DeclarationStatus != '注销' AND DeclarationStatus != '关封' AND DeclarationStatus != '联单' AND DeclarationStatus != '商检') AND (ReceivedDate > '{0}') AND (Conveyance != '') AND (VoyageNumber != '') AND (Dock != '') AND (ShipLeaveDate != '' OR ShipLeaveDate is NULL)", DateTime.Now.AddDays(-5).ToString("yyyy-MM-dd HH:mm:ss"));

                DataSet ds = new DataSet();
                using (SqlDataAdapter adapter = new SqlDataAdapter(sql, conn))
                {
                    adapter.Fill(ds);
                }

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    ExportDelcaration exportDecl = new ExportDelcaration();
                    exportDecl.Dock = row["Dock"].ToString();
                    exportDecl.Conveyance = row["Conveyance"].ToString();
                    exportDecl.DeclarationNumber = row["DeclarationNumber"].ToString();
                    exportDecl.VoyageNumber = row["VoyageNumber"].ToString();

                    lstExportDeclaration.Add(exportDecl);
                }

                foreach (var exportDeclaration in lstExportDeclaration)
                {
                    string leaveDate = LeaveDockDateCrawler.FindExportDate(exportDeclaration.Conveyance, exportDeclaration.VoyageNumber, exportDeclaration.Dock);
                    DateTime dtLeaveDate;
                    if (Information.IsDate(leaveDate))
                    {
                        dtLeaveDate = DateTime.Parse(leaveDate);
                        SqlCommand comm = new SqlCommand(string.Format("Update Declaration set ShipLeaveDate = '{1}' where DeclarationNumber = '{0}'", exportDeclaration.DeclarationNumber, dtLeaveDate), conn);
                        comm.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
    internal class ExportDelcaration
    {
        public string Dock;
        public string Conveyance;
        public string VoyageNumber;
        public string DeclarationNumber;
    }
}
