
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
    using ProTemplate.Web.BLL;
    using System.Text;


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
        // To support paging you will need to add ordering to the 'Role' query.
        public IQueryable<DoubleCheckDeclarationVarifyResult> DoubleCheckDeclarationVarifyResults(string data)
        {
            List<DoubleCheckDeclarationVarifyResult> lst = new List<DoubleCheckDeclarationVarifyResult>();
            //DoubleCheckDeclarationVarifyResult a = new DoubleCheckDeclarationVarifyResult();
            //a.DeclarationNumber = "123456780123456789";
            //a.VarifyFlag = true;
            //a.VarifyMsg = "没什么";
            //lst.Add(a);
            //a = new DoubleCheckDeclarationVarifyResult();
            //a.DeclarationNumber = "123456780123456788";
            //a.VarifyFlag = false;
            //a.VarifyMsg = "娜娜不一样";
            //lst.Add(a);
            //return lst.AsQueryable<DoubleCheckDeclarationVarifyResult>();

            string[] info = data.Split(',');
            foreach (var d in info)
            {
                var items = d.Split(':');
                var res = Varify(int.Parse(items[0]), items[1], items[2]);
                if (res != null)
                    lst.Add(res);
            }
            return lst.AsQueryable<DoubleCheckDeclarationVarifyResult>();
        }

        private DoubleCheckDeclarationVarifyResult Varify(int id, string declarationNumber, string approvalNumber)
        {
            if (string.IsNullOrEmpty(declarationNumber) && string.IsNullOrEmpty(approvalNumber))
                return null;
            
            DoubleCheckDeclarationVarifyResult rvr = new DoubleCheckDeclarationVarifyResult();
            rvr.ID = id;
            rvr.DeclarationNumber = declarationNumber;
            rvr.VarifyFlag = true;
            rvr.VarifyMsg = "";
            

            var querySQL = (from q in this.ObjectContext.DoubleCheckDeclaration
                            join p in this.ObjectContext.Declaration on q.DeclarationId equals p.ID
                            where (q.DeclarationNumber == declarationNumber && q.DeclarationNumber != "")
                            orderby p.ReceivedDate descending
                            select q).FirstOrDefault();
            if (querySQL == null)
            {
                rvr.VarifyFlag = false;
                rvr.VarifyMsg = string.Format("海关编号[{0}]或核销单号{1}不存在数据库中！", declarationNumber, approvalNumber);
                return rvr;
            }

            string machineIP = (from ip in this.ObjectContext.MachineNameIPMapping
                                where ip.MachineName == querySQL.MachineName
                                select ip.MachineIP).FirstOrDefault();

            DoubleCheckDeclaration objAccess = null;
            try
            {
                AccessData asd = new AccessData(machineIP);
                objAccess = asd.GetDoubleCheckDeclaration(declarationNumber, approvalNumber);                
            }
            catch
            {
                rvr.VarifyFlag = false;
                rvr.VarifyMsg = string.Format("获取Access中DoubleCheckDeclaration对象异常，请检查Access路径是否正确！");
                return rvr;
            }

            if (objAccess == null)
            {
                rvr.VarifyFlag = false;
                rvr.VarifyMsg = string.Format("海关编号[{0}]或核销单号{1}不存在Access数据库中！", declarationNumber, approvalNumber);
                return rvr;
            }
        
            ObjectContext.LoadProperty<DoubleCheckDeclaration>(querySQL, a => a.DoubleCheckDeclarationItem);


            // 根据code获取name
            objAccess.TransactionName = (from q in ObjectContext.Transaction
                                            where q.Code == objAccess.TransactionName
                                            select q.Name).SingleOrDefault();

            objAccess.DistrictName = (from q in ObjectContext.District
                                        where q.Code == objAccess.DistrictName
                                        select q.Name).SingleOrDefault();

            objAccess.TradeName = (from q in ObjectContext.Trade
                                    where q.Code == objAccess.TradeName
                                    select q.Name).SingleOrDefault();

            objAccess.CustomhouseName = (from q in ObjectContext.Customhouse
                                            where q.Code == objAccess.CustomhouseName
                                            select q.Name).SingleOrDefault();

            if (!string.IsNullOrEmpty(objAccess.WrapName))
            {
                objAccess.WrapName = (from q in ObjectContext.Wrap
                                        where q.Code == objAccess.WrapName
                                                select q.Name).SingleOrDefault();
            }
            // 国家
            if (!string.IsNullOrEmpty(objAccess.CountryName))
            {
                objAccess.CountryName = (from q in ObjectContext.Country
                                        where q.Code == objAccess.CountryName
                                        select q.Name).SingleOrDefault();
            }

            // Pay way
            if (!string.IsNullOrEmpty(objAccess.PayName))
            {
                objAccess.PayName = (from q in ObjectContext.Pay
                                         where q.Code == objAccess.PayName
                                         select q.Name).SingleOrDefault();
            }

            //FreightFeeCurrencyName
            if (!string.IsNullOrEmpty(objAccess.FreightFeeCurrencyName))
            {
                objAccess.FreightFeeCurrencyName = (from q in ObjectContext.Currency
                                                    where q.Code == objAccess.FreightFeeCurrencyName
                                     select q.Name).SingleOrDefault();
            }

            //InsuranceFeeCurrencyName
            if (!string.IsNullOrEmpty(objAccess.InsuranceFeeCurrencyName))
            {
                objAccess.InsuranceFeeCurrencyName = (from q in ObjectContext.Currency
                                                      where q.Code == objAccess.InsuranceFeeCurrencyName
                                                    select q.Name).SingleOrDefault();
            }

            foreach (var item in objAccess.DoubleCheckDeclarationItem)
            {
                if (!string.IsNullOrEmpty(item.FirstUnitName))
                {
                    item.FirstUnitName = (from q in ObjectContext.Unit
                                                where q.Code == item.FirstUnitName
                                                select q.Name).SingleOrDefault();
                }

                if (!string.IsNullOrEmpty(item.SecondUnitName))
                {
                    item.SecondUnitName = (from q in ObjectContext.Unit
                                            where q.Code == item.SecondUnitName
                                            select q.Name).SingleOrDefault();
                }

                if (!string.IsNullOrEmpty(item.DeclaredUnitName))
                {
                    item.DeclaredUnitName = (from q in ObjectContext.Unit
                                                where q.Code == item.DeclaredUnitName
                                            select q.Name).SingleOrDefault();
                }

                if (!string.IsNullOrEmpty(item.CurrencyName))
                {
                    item.CurrencyName = (from q in ObjectContext.Currency
                                            where q.Code == item.CurrencyName
                                            select q.Name).SingleOrDefault();
                }
            }
            //开始校验DoubleCheckDeclaration字段
            //ValidField(querySQL.DeclarationNumber, objAccess.DeclarationNumber, rvr, "海关编号");
            //ValidField(querySQL.ApprovalNumber, objAccess.ApprovalNumber, rvr, "批准文号");
            ValidField(querySQL.TransactionName, objAccess.TransactionName, rvr, "成交方式");
            ValidField(querySQL.DistrictName, objAccess.DistrictName, rvr, "境内货源地");
            ValidField(querySQL.TradeName, objAccess.TradeName, rvr, "贸易方式");
            ValidField(querySQL.FeightFeeRate, objAccess.FeightFeeRate, rvr, "运费");
            ValidField(querySQL.FreightFeeCurrencyName, objAccess.FreightFeeCurrencyName, rvr, "运费币制");
            ValidField(querySQL.InsuranceFeeRate, objAccess.InsuranceFeeRate, rvr, "保费");
            ValidField(querySQL.InsuranceFeeCurrencyName, objAccess.InsuranceFeeCurrencyName, rvr, "保费币制");
            ValidField(querySQL.CustomhouseName, objAccess.CustomhouseName, rvr, "出口口岸");
            ValidField(querySQL.CountryName, objAccess.CountryName, rvr, "国家");
            ValidField(querySQL.ManualNumber, objAccess.ManualNumber, rvr, "备案号");
            ValidField(querySQL.LicenseNumber, objAccess.LicenseNumber, rvr, "许可证号");
            ValidField(querySQL.PackageAmount, objAccess.PackageAmount, rvr, "件数");
            //ValidField(querySQL.WrapName, objAccess.WrapName, rvr, "包装种类");
            ValidField(querySQL.GrossWeight, objAccess.GrossWeight, rvr, "毛重");
            ValidField(querySQL.NetWeight, objAccess.NetWeight, rvr, "净重");
            ValidField(querySQL.ContractNumber, objAccess.ContractNumber, rvr, "合同号");
            ValidField(querySQL.PayName, objAccess.PayName, rvr, "结汇方式");
            ValidCommaSplitField(querySQL.ContainerNumbers, objAccess.ContainerNumbers, rvr, "集装箱");
            ValidCommaSplitField(querySQL.DocumentCodes, objAccess.DocumentCodes, rvr, "随附单证号");
            //if(!string.IsNullOrEmpty(objAccess.ExaminationNumber))
                ValidField(querySQL.ExaminationNumber, objAccess.ExaminationNumber, rvr, "商检编号");

            //开始校验DoubleCheckDeclarationItem
            var sqlItems = from q in querySQL.DoubleCheckDeclarationItem
                            orderby q.SortOrder
                            select q;
            int sqlCount = sqlItems.Count();
            int accessCount = objAccess.DoubleCheckDeclarationItem.Count();

            int loop = sqlCount <= accessCount ? sqlCount : accessCount;
            for (int i = 0; i < loop; i++)
            {
                ValidField(sqlItems.ElementAt(i).ControlNumber, objAccess.DoubleCheckDeclarationItem.ElementAt(i).ControlNumber, rvr, "项号" + (i + 1).ToString());
                ValidField(sqlItems.ElementAt(i).HSCode, objAccess.DoubleCheckDeclarationItem.ElementAt(i).HSCode, rvr, "HSCode" +(i+1).ToString());
                ValidField(sqlItems.ElementAt(i).Name, objAccess.DoubleCheckDeclarationItem.ElementAt(i).Name, rvr, "商品名称" + (i + 1).ToString());
                ValidField(sqlItems.ElementAt(i).FirstUnitName, objAccess.DoubleCheckDeclarationItem.ElementAt(i).FirstUnitName, rvr, "第一计量单位" + (i + 1).ToString());
                ValidFieldForNumber(sqlItems.ElementAt(i).FirstQuantity, objAccess.DoubleCheckDeclarationItem.ElementAt(i).FirstQuantity, rvr, "第一数量" + (i + 1).ToString());
                ValidField(sqlItems.ElementAt(i).SecondUnitName, objAccess.DoubleCheckDeclarationItem.ElementAt(i).SecondUnitName, rvr, "第二计量单位" + (i + 1).ToString());
                ValidFieldForNumber(sqlItems.ElementAt(i).SecondQuantity, objAccess.DoubleCheckDeclarationItem.ElementAt(i).SecondQuantity, rvr, "第二数量" + (i + 1).ToString());
                ValidField(sqlItems.ElementAt(i).DeclaredUnitName, objAccess.DoubleCheckDeclarationItem.ElementAt(i).DeclaredUnitName, rvr, "申报单位" + (i + 1).ToString());
                ValidFieldForNumber(sqlItems.ElementAt(i).DeclaredQuantity, objAccess.DoubleCheckDeclarationItem.ElementAt(i).DeclaredQuantity, rvr, "申报数量" + (i + 1).ToString());
                ValidFieldForNumber(sqlItems.ElementAt(i).TotalAmount, objAccess.DoubleCheckDeclarationItem.ElementAt(i).TotalAmount, rvr, "总价" + (i + 1).ToString());
                ValidField(sqlItems.ElementAt(i).CurrencyName, objAccess.DoubleCheckDeclarationItem.ElementAt(i).CurrencyName, rvr, "币制" + (i + 1).ToString());
            }

            if (sqlCount < accessCount)
            {
                rvr.VarifyFlag = false;
                for (; loop < accessCount; loop++)
                {
                    rvr.VarifyMsg += string.Format("预录Item不存在：{0}，{1}。", objAccess.DoubleCheckDeclarationItem.ElementAt(loop).HSCode, objAccess.DoubleCheckDeclarationItem.ElementAt(loop).Name);
                }
            }
            else if (sqlCount > accessCount)
            {
                rvr.VarifyFlag = false;
                for (; loop < sqlCount; loop++)
                {
                    rvr.VarifyMsg += string.Format("打单Item不存在：{0}，{1}。", sqlItems.ElementAt(loop).HSCode, sqlItems.ElementAt(loop).Name);
                }
            }
                

            return rvr;
            
        }

        private void ValidField(string f1, string f2, DoubleCheckDeclarationVarifyResult rvr, string fieldName)
        {
            if (f1 == null) f1 = string.Empty;
            if (f2 == null) f2 = string.Empty;

            if (f1.Trim() != f2.Trim())
            {
                rvr.VarifyFlag = false;
                rvr.VarifyMsg += string.Format("{0}不同!预录：{1}，打单：{2}。", fieldName, f1, f2);
            }
        }

        private void ValidFieldForNumber(string f1, string f2, DoubleCheckDeclarationVarifyResult rvr, string fieldName)
        {
            if (string.IsNullOrEmpty(f1)) f1 = "0";
            if (string.IsNullOrEmpty(f2)) f2 = "0";

            double v1;
            double v2;
            if (!double.TryParse(f1, out v1))
            {
                rvr.VarifyFlag = false;
                rvr.VarifyMsg += string.Format("{0}不同!预录的值{1}不是数字。", fieldName, f1);
                return;
            }
            if (!double.TryParse(f2, out v2))
            {
                rvr.VarifyFlag = false;
                rvr.VarifyMsg += string.Format("{0}不同!打单中的值{1}不是数字。", fieldName, f2);
                return;
            }

            if (v1 != v2)
            {
                rvr.VarifyFlag = false;
                rvr.VarifyMsg += string.Format("{0}不同!预录：{1}，打单：{2}。", fieldName, f1, f2);
            }
        }


        private void ValidCommaSplitField(string f1, string f2, DoubleCheckDeclarationVarifyResult rvr, string fieldName)
        {
            if (f1 == null) f1 = string.Empty;
            if (f2 == null) f2 = string.Empty;

            var items1 = f1.Split(',');
            //StringBuilder field1 = new StringBuilder();
            //foreach (var a in f1.Split(',').OrderBy((p) => p))
            //{
            //    field1.Append(a.ToString().Trim() + ",");
            //}
            //string fieldItem1 = field1.ToString().TrimEnd(',');

            var items2 = f2.Split(',');
            //StringBuilder field2 = new StringBuilder();
            //foreach (var a in f2.Split(',').OrderBy((p) => p))
            //{
            //    field2.Append(a.ToString().Trim() + ",");
            //}
            //string fieldItem2 = field2.ToString().TrimEnd(',');

            List<string> result1 = new List<string>();

            foreach (string a in items1)
            {
                if (items2.Contains(a.Trim()))
                {
                    continue;
                }
                else
                {
                    result1.Add(a);
                }
            }

            List<string> result2 = new List<string>();

            foreach (string a in items2)
            {
                if (items1.Contains(a.Trim()))
                {
                    continue;
                }
                else
                {
                    result2.Add(a);
                }
            }

            if (result1.Count > 0 || result2.Count > 0)
            {
                rvr.VarifyFlag = false;
                rvr.VarifyMsg += string.Format("{0}不同!预录：{1}，打单：{2}。", fieldName, string.Join(",", result1.OrderBy(p=>p)), string.Join(",", result2.OrderBy(p=>p)));
            }
        }
    }
}


