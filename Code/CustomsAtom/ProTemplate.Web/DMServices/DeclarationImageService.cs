
namespace ProTemplate.Web.DMServices
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Data;
    using System.Linq;
    using System.ServiceModel.DomainServices.EntityFramework;
    using System.ServiceModel.DomainServices.Hosting;
    using System.ServiceModel.DomainServices.Server;
    using ProTemplate.Web;
    using System.IO;


    // Implements application logic using the CustomsAtomEntities context.
    // TODO: Add your application logic to these methods or in additional methods.
    // TODO: Wire up authentication (Windows/ASP.NET Forms) and uncomment the following to disable anonymous access
    // Also consider adding roles to restrict access as appropriate.
    // [RequiresAuthentication]
    // TODO: add the EnableClientAccessAttribute to this class to expose this DomainService to clients.
    public partial class CustomsAtomService : LinqToEntitiesDomainService<CustomsAtomEntities>
    {

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'DeclarationImage' query.
        public IQueryable<DeclarationImage> GetDeclarationImage()
        {
            return this.ObjectContext.DeclarationImage;
        }

        public void InsertDeclarationImage(DeclarationImage declarationImage)
        {
            if ((declarationImage.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(declarationImage, EntityState.Added);
            }
            else
            {
                this.ObjectContext.DeclarationImage.AddObject(declarationImage);
            }
        }

        public void UpdateDeclarationImage(DeclarationImage currentDeclarationImage)
        {
            this.ObjectContext.DeclarationImage.AttachAsModified(currentDeclarationImage, this.ChangeSet.GetOriginal(currentDeclarationImage));
        }

        public void DeleteDeclarationImage(DeclarationImage declarationImage)
        {
            if ((declarationImage.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(declarationImage, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.DeclarationImage.Attach(declarationImage);
                this.ObjectContext.DeclarationImage.DeleteObject(declarationImage);
            }

            try
            {
                // 获取报关单的时候，检查保存图片文件夹是否存在，如果不存在，创建一个
                string filePath = System.Web.HttpContext.Current.Request.PhysicalApplicationPath + "UserUploads\\" + declarationImage.DeclarationId.ToString() + "\\" + declarationImage.ScanImageName;
                if (File.Exists(filePath))
                    File.Delete(filePath);
            }
            catch { }
        }

        public void ProcessUploadedImages()
        {
            string folderPath = System.Web.HttpContext.Current.Request.PhysicalApplicationPath + "UserUploads\\DeclarationImageTemp\\";
            DirectoryInfo TheFolder = new DirectoryInfo(folderPath);
            FileInfo[] files = TheFolder.GetFiles();
            for (int i = 0; i < files.Length;i++ )
            {
                // 得到declarationNumber 或者 ApprovalNumber
                string declarationNumberOrApprovalNumber = files[i].Name;
                if (declarationNumberOrApprovalNumber.Contains("."))
                    declarationNumberOrApprovalNumber = declarationNumberOrApprovalNumber.Split('.')[0];

                var query = from d in this.ObjectContext.Declaration
                            where d.DeclarationNumber == declarationNumberOrApprovalNumber || d.ApprovalNumber == declarationNumberOrApprovalNumber
                            select d.ID;
                if (query.Count() > 0)
                {
                    foreach (var d in query)
                    {
                        // 找到文件夹
                        string fileFolder = System.Web.HttpContext.Current.Request.PhysicalApplicationPath + "UserUploads\\" + d.ToString();
                        //文件夹如果不存在，创建
                        if (!Directory.Exists(fileFolder))
                            Directory.CreateDirectory(fileFolder);
                        string filePath = fileFolder + "\\" + files[i].Name;
                        if (File.Exists(filePath))
                            File.Delete(filePath);
                        // 创建文件
                        File.Copy(folderPath + files[i].Name, filePath);
                        // 更新数据库
                        string savedFileName = files[i].Name;
                        var queryDB = from im in this.ObjectContext.DeclarationImage
                                      where im.DeclarationId == d && im.ScanImageName == savedFileName
                                      select im;
                        // 不存在，则添加
                        if (queryDB.Count() == 0)
                        {
                            DeclarationImage di = new DeclarationImage();
                            di.DeclarationId = d;
                            di.ScanImageName = files[i].Name;
                            this.ObjectContext.AddToDeclarationImage(di);
                        }
                    }
                }
                File.Delete(folderPath + files[i].Name);
            }
            this.ObjectContext.SaveChanges();
        }
    }
}


