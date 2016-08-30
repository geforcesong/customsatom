using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using System.Net;
using System.Security.Principal;
using ProTemplate.Web.Utility;

namespace ProTemplate.Web
{
    public partial class ReportForm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string report = Request.QueryString["Report"].ToString();
            string content = Request.QueryString["content"].ToString();
            if (!Page.IsPostBack)
            {
                //byte[] reviewData =SevenZip.Compression.LZMA.SevenZipHelper.Decompress(byte[] result);
                //Rijndeal
                rptViewer.ProcessingMode = ProcessingMode.Remote;
                rptViewer.ServerReport.ReportServerUrl = new Uri("http://localhost/reportserver");
                rptViewer.ServerReport.ReportPath = "/CustomsAtom.Report/" + report;
                //if (rptViewer.ServerReport.ReportServerCredentials == null)
                //{
                //    rptViewer.ServerReport.ReportServerCredentials = new MyReportViewerCredential("administrator", "Boss..net");
                //}
                List<ReportParameter> parameters = new List<ReportParameter>();
                parameters.Add(new ReportParameter("Statement", EncryptionUtil.Decrypt(content), false));
                parameters.Add(new ReportParameter("IsValidation", "0", false));
                //More parameters added here... 
                rptViewer.ServerReport.SetParameters(parameters);
                rptViewer.ServerReport.Refresh();
            }
        }
    }
}

