using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.IO;
using System.Drawing;

namespace ProTemplate.Web.ASMXServices
{
    /// <summary>
    /// Summary description for CustomsAtomWebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class CustomsAtomWebService : System.Web.Services.WebService
    {
        [WebMethod]
        public string HelloWorld(string a)
        {
            return "Hello World";
        }

        [WebMethod]
        public bool UploadImage(string fileName, byte[] imgData)
        {
            CustomsAtomEntities c = new CustomsAtomEntities();

            var query = (from a in c.Declaration
                            where a.DeclarationNumber == fileName || a.ApprovalNumber == fileName
                            select a).FirstOrDefault();
            if (query == null)
                return false;
            else
            {
                string folderPath = HttpContext.Current.Request.PhysicalApplicationPath + "UserUploads\\" + query.ID.ToString();
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);
                if (imgData == null || imgData.Length == 0)
                    return true;
                else
                {
                    string imgName = fileName + ".jpg";
                    MemoryStream ms = new MemoryStream(imgData);
                    Image img = Image.FromStream(ms);
                    img.Save(folderPath + "\\" +imgName );
                    DeclarationImage di = new DeclarationImage();
                    di.DeclarationId = query.ID;
                    di.ScanImageName = imgName;
                    c.DeclarationImage.AddObject(di);
                    c.SaveChanges();
                    return true;
                }
            }
            
            
            return true;
        }
    }
}
