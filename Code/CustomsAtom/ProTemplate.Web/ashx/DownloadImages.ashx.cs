using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace ProTemplate.Web.ashx
{
    /// <summary>
    /// Summary description for DownloadImages
    /// </summary>
    public class DownloadImages : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string sFileName = context.Request.QueryString["FilePath"];
            int iReading;

            FileStream fStream = null;
            Stream outStream = null;
            try
            {
                string sFilePath = Path.Combine(System.Web.HttpContext.Current.Request.PhysicalApplicationPath, sFileName);
                fStream = new FileStream(sFilePath, FileMode.Open, FileAccess.Read);
                outStream = context.Response.OutputStream;//get output stream
                context.Response.ContentType = "application/Zip";
                context.Response.AppendHeader("Connection", "close");
                context.Response.AppendHeader("Content-Disposition", "  attachment;  filename  = " + sFileName);//default file name when download

                long lngFileSize = fStream.Length;
                byte[] bytBuffer = new byte[(int)lngFileSize];
                while ((iReading = fStream.Read(bytBuffer, 0, (int)lngFileSize)) > 0)
                {
                    outStream.Write(bytBuffer, 0, iReading);
                }
            }
            catch
            {
                context.Response.ContentType = "text/HTML";
                context.Response.AppendHeader("Connection", "close");
                context.Response.Write("File is not existing.");
            }
            finally
            {
                if (fStream != null)
                    fStream.Close();
                if (outStream != null)
                    outStream.Close();
                context.Response.Flush();
                context.Response.End();
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}