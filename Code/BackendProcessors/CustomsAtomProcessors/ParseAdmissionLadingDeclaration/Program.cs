using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using ProcessorUtilities;
using System.Threading.Tasks;
using System.Xml;
using System.Data.SqlClient;
using System.Data;
using ProTemplate.Web.DataCrawler;

namespace ParseAdmissionLadingDeclaration
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
                string sql = string.Format("select BillNumber,DeclarationNumber  from Declaration where ((DeclarationStatus != '报关完成' AND DeclarationStatus != '查验' AND DeclarationStatus != '退关' AND DeclarationStatus != '注销' AND DeclarationStatus != '关封' AND DeclarationStatus != '联单' AND DeclarationStatus != '商检') OR (declarationnumber like '_________81%')) AND (AdmissionStatus != '已放行' OR AdmissionStatus IS NULL) AND (ReceivedDate > '{0}') AND (BillNumber != '')", DateTime.Now.AddDays(-5).ToString("yyyy-MM-dd HH:mm:ss"));

                DataSet ds = new DataSet();
                using (SqlDataAdapter adapter = new SqlDataAdapter(sql, conn))
                {
                    adapter.Fill(ds);
                }

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    ExportDelcaration exportDecl = new ExportDelcaration();
                    exportDecl.BillNumber = row["BillNumber"].ToString();
                    exportDecl.DeclarationNumber = row["DeclarationNumber"].ToString();

                    lstExportDeclaration.Add(exportDecl);
                }

                foreach (var exportDeclaration in lstExportDeclaration)
                {
                    string admissionStatus = ContainerAdmissionStatusCrawler.RunParseAdmission(exportDeclaration.BillNumber);
                    if (!string.IsNullOrEmpty(admissionStatus))
                    {
                        SqlCommand comm = new SqlCommand(string.Format("Update Declaration set AdmissionStatus = '{1}' where DeclarationNumber = '{0}'", exportDeclaration.DeclarationNumber, admissionStatus), conn);
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
        public string AdmissionStatus;
        public string BillNumber;
        public string DeclarationNumber;
    }
}
