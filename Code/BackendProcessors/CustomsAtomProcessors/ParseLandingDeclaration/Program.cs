using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data;

namespace ParseLandingDeclaration
{
    class Program
    {
        private static readonly SqlConnection conn = new SqlConnection("Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=CustomsAtom;Data Source=localhost");
        private static List<LandingNetInfo> lstExportDeclaration = new List<LandingNetInfo>();

        static void Main()
        {
            conn.Open();
            //ClearOldData();
            ParseRootPage();
        }

        private static void ParseRootPage()
        {
            try
            {
                string sql = string.Format("select BillNumber,DeclarationNumber  from Declaration where ((DeclarationStatus != '报关完成' AND DeclarationStatus != '查验' AND DeclarationStatus != '退关' AND DeclarationStatus != '注销' AND DeclarationStatus != '关封' AND DeclarationStatus != '联单' AND DeclarationStatus != '商检') OR (declarationnumber like '_________81%')) AND (LadingStatus != '已进港' OR LadingStatus IS NULL) AND (ReceivedDate > '{0}') AND (BillNumber != '')", DateTime.Now.AddDays(-5).ToString("yyyy-MM-dd HH:mm:ss"));

                DataSet ds = new DataSet();
                using (SqlDataAdapter adapter = new SqlDataAdapter(sql, conn))
                {
                    adapter.Fill(ds);
                }

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    LandingNetInfo exportDecl = new LandingNetInfo();
                    exportDecl.BillNumber = row["BillNumber"].ToString();
                    exportDecl.DeclarationNumber = row["DeclarationNumber"].ToString();

                    lstExportDeclaration.Add(exportDecl);
                }

                foreach (LandingNetInfo exportDeclaration in lstExportDeclaration)
                {
                    LandingNetInfo onlineResult = LandingCrawler.QueryLading(exportDeclaration);
                    if (onlineResult != null)
                    {
                        if (!string.IsNullOrEmpty(onlineResult.OnlineContainerNumber.Trim(',')))
                        {
                            SqlCommand comm = new SqlCommand(string.Format("Update Declaration set LadingStatus = '{1}' where DeclarationNumber = '{0}'", exportDeclaration.DeclarationNumber, "已进港"), conn);
                            comm.ExecuteNonQuery();
                        }
                        else
                        {
                            SqlCommand comm = new SqlCommand(string.Format("Update Declaration set LadingStatus = '{1}' where DeclarationNumber = '{0}'", exportDeclaration.DeclarationNumber, "未进港"), conn);
                            comm.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        SqlCommand comm = new SqlCommand(string.Format("Update Declaration set LadingStatus = '{1}' where DeclarationNumber = '{0}'", exportDeclaration.DeclarationNumber, "未进港"), conn);
                        comm.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}
