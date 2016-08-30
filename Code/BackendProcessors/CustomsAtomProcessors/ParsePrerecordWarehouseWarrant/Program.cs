using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;
using ProcessorUtilities;
using System.Net;
using System.Data;
using System.Threading.Tasks;

namespace ParsePrerecordWarehouseWarrant
{
    class Program
    {

        private static readonly SqlConnection conn = new SqlConnection("Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=CustomsAtom;Data Source=localhost");
        private static List<ExportDelcaration> lstExportDeclaration = new List<ExportDelcaration>();
        private static void Main()
        {
            conn.Open();
            //ClearOldData();
            ParseRootPage();
        }

        private static void ParseRootPage()
        {
            try
            {
                string sql = string.Format("select BillNumber, Conveyance,DeclarationNumber, VoyageNumber  from Declaration where (PrerecordWarehouseWarrant != '成功接收' OR PrerecordWarehouseWarrant IS NULL) AND (BillNumber IS NOT NULL OR BillNumber != '') AND (ReceivedDate > '{0}')", DateTime.Now.AddDays(-3).ToString("yyyy-MM-dd HH:mm:ss"));

                DataSet ds = new DataSet();
                using (SqlDataAdapter adapter = new SqlDataAdapter(sql, conn))
                {
                    adapter.Fill(ds);
                }

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    ExportDelcaration exportDecl = new ExportDelcaration();
                    exportDecl.BillNumber = row["BillNumber"].ToString();
                    exportDecl.Conveyance = row["Conveyance"].ToString();
                    exportDecl.DeclarationNumber = row["DeclarationNumber"].ToString();
                    exportDecl.VoyageNumber = row["VoyageNumber"].ToString();

                    lstExportDeclaration.Add(exportDecl);
                }

                foreach(var exportDeclaration in lstExportDeclaration)
                {
                    if (!string.IsNullOrEmpty(exportDeclaration.BillNumber))
                    {
                        //int i = 1;
                        //if (exportDeclaration.BillNumber == "1SHAKU652B7")
                        //{
                        //    i = 0;   
                        //}
                        //int n = i;
                        string htmlContent = HtmlParseUtils.SaveWebPage(string.Format("http://edi.easipass.com/dataportal/query.do?qn=dp_premanifest_ack_detail&blno={2}&vslname={0}&voyage={1}", exportDeclaration.Conveyance, exportDeclaration.VoyageNumber, exportDeclaration.BillNumber));
                        if (!string.IsNullOrEmpty(htmlContent))
                        {
                            string content = HtmlParseUtils.FormatHtml(htmlContent, false, true);
                            if (!string.IsNullOrEmpty(content))
                            {
                                List<string> headList = HtmlParseUtils.GetSubStrings(content, "<td width=\"40%\">", null, "</td>", "<td width=\"40%\">", null, "</td>");
                                if (headList != null)
                                {
                                    try
                                    {
                                        if (headList.Count > 0)
                                        {
                                            string strText = headList[0].Trim();

                                            if (strText.Contains("成功接收") || strText.Contains("无相应的海关回执"))
                                            {

                                            }
                                            else
                                            {
                                                if (headList[0].Trim().Length + 3 > 32)
                                                {
                                                    strText = string.Format("失败:{0}", headList[0].Trim()).Substring(0, 32);
                                                }
                                                else
                                                {
                                                    strText = string.Format("失败:{0}", headList[0].Trim());
                                                }
                                            }
                                            using (SqlCommand command = new SqlCommand("", conn))
                                            {
                                                command.CommandText =
                                                    @"UPDATE [Declaration]
                                                           SET 
                                                              [PrerecordWarehouseWarrant] = '" + strText + @"'
                                                         WHERE DeclarationNumber = '" + exportDeclaration.DeclarationNumber + @"'";
                                                command.ExecuteNonQuery();
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        ex.HelpLink = "BillNumber:\"" + exportDeclaration.BillNumber + "\"";
                                    }
                                }
                            }
                        }
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
        public string BillNumber;
        public string Conveyance;
        public string VoyageNumber;
        public string DeclarationNumber;
    }
}
