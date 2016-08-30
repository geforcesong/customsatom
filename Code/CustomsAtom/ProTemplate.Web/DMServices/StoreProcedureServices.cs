
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
    using ProTemplate.Web.Utility;


    public partial class CustomsAtomService : LinqToEntitiesDomainService<CustomsAtomEntities>
    {

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'Role' query.
        public IQueryable<GetAllDeclarationByReceiveDateResult> GetAllDeclarationDuplicatedResultsFunc(int userID, string condition)
        {
            if (this.ObjectContext.Connection.State != ConnectionState.Open)
                this.ObjectContext.Connection.Open();
            return this.ObjectContext.GetAllDeclarationDuplicated(userID, condition).AsQueryable<GetAllDeclarationByReceiveDateResult>();
        }
        public IQueryable<GetAllDeclarationByReceiveDateResult> GetAllDeclarationByReceiveDateResultsFunc(int userID, string condition)
        {
            if (this.ObjectContext.Connection.State != ConnectionState.Open)
                this.ObjectContext.Connection.Open();
            string sqlCondition = EncryptionUtil.Decrypt(condition);
            return this.ObjectContext.GetAllDeclarationByReceiveDate(userID, sqlCondition).AsQueryable<GetAllDeclarationByReceiveDateResult>();
        }

        public IQueryable<GetAllDeclarationByReceiveDateResult> GetAllDeclarationByDeclarationCodeResultsFunc(int userID, string codes)
        {
            if (this.ObjectContext.Connection.State != ConnectionState.Open)
                this.ObjectContext.Connection.Open();
            return this.ObjectContext.GetAllDeclarationByDeclarationCodes(userID, codes).AsQueryable<GetAllDeclarationByReceiveDateResult>();
        }

        public int UpdateDeclarationStatus(string ids, string status, string remark)
        {
            return this.ObjectContext.UpdateDeclarationStatus(ids, status, remark);
        }

        public int UpdateDrawbackStatus(string ids, string status, string remark)
        {
            return this.ObjectContext.UpdateDrawbackStatus(ids, status, remark);
        }

        public int DeleteDeclarations(string ids)
        {
            if (!string.IsNullOrEmpty(ids))
            {
                string[] idArr = ids.Split(',');
                foreach (var id in idArr)
                {
                    try
                    {
                        // 获取报关单的时候，检查保存图片文件夹是否存在，如果存在，全部删除
                        string folderPath = System.Web.HttpContext.Current.Request.PhysicalApplicationPath + "UserUploads\\" + id + "\\";
                        if (Directory.Exists(folderPath))
                            Directory.Delete(folderPath, true);
                    }
                    catch { }
                }
            }
            return this.ObjectContext.DeleteSelectedDeclarations(ids);
        }
    }
}